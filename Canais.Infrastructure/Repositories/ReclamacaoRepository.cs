using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Canais.Application.Models;
using Canais.Domain.Entities;
using Canais.Domain.Interfaces;
using Canais.Domain.Request;
using Canais.Infrastructure.Helpers;

namespace Canais.Infrastructure.Repositories;

public class ReclamacaoRepository : IReclamacaoRepository
{
    private readonly IAmazonDynamoDB _client;
    private readonly string _tableName = "ReclamacoesClassificadas";

    public ReclamacaoRepository(IAmazonDynamoDB client)
    {
        _client = client;
    }

    public async Task<List<ReclamacoesEntity>> ListarReclamacoesClassificadasAsync(FiltroReclamacoesRequest filtro)
    {
        var scanRequest = new ScanRequest
        {
            TableName = _tableName
        };

        var filtros = MontarFiltros(filtro);

        if (!string.IsNullOrEmpty(filtros.FilterExpression))
        {
            scanRequest.ExpressionAttributeNames = filtros.AttributeNames;
            scanRequest.ExpressionAttributeValues = filtros.AttributeValues;
            scanRequest.FilterExpression = filtros.FilterExpression;
        }

        var response = await _client.ScanAsync(scanRequest);
        return response.Items.Select(ReclamacoesEntity.FromDynamoDB).ToList();
    }

    private FiltroDynamo MontarFiltros(FiltroReclamacoesRequest filtro)
    {
        var attributeNames = new Dictionary<string, string>();
        var attributeValues = new Dictionary<string, AttributeValue>();
        var expressions = new List<string>();

        if (!string.IsNullOrEmpty(filtro.Canal))
        {
            attributeNames["#cat"] = "Categorias";
            attributeValues[":categoria"] = new AttributeValue { S = filtro.Categoria };
            expressions.Add("contains(#cat, :categoria)");
        }

        if (filtro.DataInicio.HasValue)
        {
            attributeNames["#data"] = "DataAbertura";
            attributeValues[":dataInicio"] = new AttributeValue { S = filtro.DataInicio.Value.ToString("o") };
            expressions.Add("#data >= :dataInicio");
        }

        if (filtro.DataFim.HasValue)
        {
            attributeNames["#data"] = "DataAbertura";
            attributeValues[":dataFim"] = new AttributeValue { S = filtro.DataFim.Value.ToString("o") };
            expressions.Add("#data <= :dataFim");
        }

        if (!string.IsNullOrEmpty(filtro.CpfReclamante))
        {
            attributeNames["#cpf"] = "Cpf";
            attributeValues[":cpf"] = new AttributeValue { S = filtro.CpfReclamante };
            expressions.Add("#cpf = :cpf");
        }

        if (!string.IsNullOrEmpty(filtro.NomeReclamante))
        {
            attributeNames["#nome"] = "Nome";
            attributeValues[":nome"] = new AttributeValue { S = filtro.NomeReclamante };
            expressions.Add("#nome = :nome");
        }

        return new FiltroDynamo
        {
            AttributeNames = attributeNames,
            AttributeValues = attributeValues,
            FilterExpression = expressions.Any() ? string.Join(" AND ", expressions) : null
        };
    }
}
