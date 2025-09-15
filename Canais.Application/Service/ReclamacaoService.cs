using Azure;
using Canais.Application.Models;
using Canais.Domain.Interfaces;
using Canais.Domain.Request;
using Canais.Domain.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Canais.Application.Service;

public class ReclamacaoService : IReclamacaoService
{
    private readonly ISqsService _sqsService;
    private readonly ILogger<ReclamacaoService> _logger;
    private readonly IReclamacaoRepository _repository;
    private readonly IBucketService _bucketService;

    public ReclamacaoService(ISqsService sqsService, ILogger<ReclamacaoService> logger, IReclamacaoRepository repository, IBucketService bucketService)
    {
        _sqsService = sqsService;
        _logger = logger;
        _repository = repository;
        _bucketService = bucketService;
    }

    public async Task EnviarReclamacoesParaFilaAsync(ReclamacaoRequest reclamacao, List<IFormFile> arquivos)
    {
        try
        {
            var identificacaoReclamacao = Guid.NewGuid().ToString();
            var Anexos = new List<string>();

            if (arquivos.Count != 0)
            {
                foreach (var arquivo in arquivos)
                {
                    var nomeArquivo = await _bucketService.SalvarArquivoAsync(arquivo, reclamacao.Nome, identificacaoReclamacao);

                    Anexos.Add(nomeArquivo);
                }
            }

            var mensagem = new
            {
                Id = identificacaoReclamacao,
                reclamacao.Canal,
                reclamacao.Texto,
                reclamacao.Nome,
                reclamacao.Cpf,
                Anexos,
                reclamacao.ReclamacaoAtendida
            };

            var payload = JsonConvert.SerializeObject(mensagem);

            await _sqsService.EnviarReclamacaoAsync(payload);

            _logger.LogInformation($"Reclamação {identificacaoReclamacao} enviada para fila.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar reclamação.}");
            throw;
        }
    }

    public async Task<ResultResponse<List<ReclamacoesClassificadasResponse>>> ObterReclamacoesClassificadasAsync(FiltroReclamacoesRequest filtro)
    {
        var serviceResponse = new ResultResponse<List<ReclamacoesClassificadasResponse>>();

        try
        {
            var reclamacoes = await _repository.ListarReclamacoesClassificadasAsync(filtro);

            var response = reclamacoes.Select(ReclamacoesClassificadasResponse.ToResponse).ToList();

            if (response.Count == 0)
            {
                serviceResponse.Dados = response;
                serviceResponse.Sucesso = false;
                serviceResponse.Mensagem = "Nenhum registro foi localizado.";
            }


            serviceResponse.Dados = response;
            serviceResponse.Mensagem = "Busca realizada com sucesso.";
        }
        catch (Exception ex)
        {
            serviceResponse.Mensagem = ex.Message;
            serviceResponse.Sucesso = false;
        }

        return serviceResponse;
    }
}
