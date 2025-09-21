using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Canais.Domain.Contracts.Repositories;
using Canais.Domain.Entities;
using Canais.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Canais.Infrastructure.Repositories;

public class ReclamacaoRepository : IReclamacaoRepository
{
    private readonly IAmazonDynamoDB _client;
    private readonly string _tableName = "ReclamacoesClassificadas";
    private readonly CanaisDbContext _context;

    public ReclamacaoRepository(IAmazonDynamoDB client, CanaisDbContext context)
    {
        _client = client;
        _context = context;
    }

    public async Task<IEnumerable<ReclamacoesEntity>> ListarReclamacoesClassificadasAsync()
    {
        var result = await _context.Reclamacoes
                                   .Include(r => r.ReclamacaoCategorias)      // inclui relação N:N
                                   .ThenInclude(rc => rc.Categorias)      // inclui a categoria
                                   .AsNoTracking()                             // somente leitura
                                   .ToListAsync();

        return result;
    }

    public async Task<ReclamacoesEntity> CadastrarReclamacaoAsync(ReclamacoesEntity reclamacao)
    {
        await _context.Reclamacoes.AddAsync(reclamacao);

        await _context.SaveChangesAsync();

        return reclamacao;
    }

    public async Task AtualizarAnexosAsync(Guid id, List<string> anexos)
    {
        var reclamacao = await _context.Reclamacoes.FindAsync(id);
        if (reclamacao == null) return;

        reclamacao.Anexos.AddRange(anexos);

        _context.Entry(reclamacao).Property(r => r.Anexos).IsModified = true;
        await _context.SaveChangesAsync();
    }

    //private FiltroDynamo MontarFiltros(FiltroReclamacoesEntity filtro)
    //{
    //    var attributeNames = new Dictionary<string, string>();
    //    var attributeValues = new Dictionary<string, AttributeValue>();
    //    var expressions = new List<string>();

    //    if (!string.IsNullOrEmpty(filtro.Canal))
    //    {
    //        attributeNames["#cat"] = "Categorias";
    //        attributeValues[":categoria"] = new AttributeValue { S = filtro.Categoria };
    //        expressions.Add("contains(#cat, :categoria)");
    //    }

    //    if (filtro.DataInicio.HasValue)
    //    {
    //        attributeNames["#data"] = "DataAbertura";
    //        attributeValues[":dataInicio"] = new AttributeValue { S = filtro.DataInicio.Value.ToString("o") };
    //        expressions.Add("#data >= :dataInicio");
    //    }

    //    if (filtro.DataFim.HasValue)
    //    {
    //        attributeNames["#data"] = "DataAbertura";
    //        attributeValues[":dataFim"] = new AttributeValue { S = filtro.DataFim.Value.ToString("o") };
    //        expressions.Add("#data <= :dataFim");
    //    }

    //    if (!string.IsNullOrEmpty(filtro.CpfReclamante))
    //    {
    //        attributeNames["#cpf"] = "Cpf";
    //        attributeValues[":cpf"] = new AttributeValue { S = filtro.CpfReclamante };
    //        expressions.Add("#cpf = :cpf");
    //    }

    //    if (!string.IsNullOrEmpty(filtro.NomeReclamante))
    //    {
    //        attributeNames["#nome"] = "Nome";
    //        attributeValues[":nome"] = new AttributeValue { S = filtro.NomeReclamante };
    //        expressions.Add("#nome = :nome");
    //    }

    //    return new FiltroDynamo
    //    {
    //        AttributeNames = attributeNames,
    //        AttributeValues = attributeValues,
    //        FilterExpression = expressions.Any() ? string.Join(" AND ", expressions) : null
    //    };
    //}
}
