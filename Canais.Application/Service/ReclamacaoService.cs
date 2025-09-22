using Canais.Application.Interfaces;
using Canais.Application.Models;
using Canais.Application.Response;
using Canais.Domain.Contracts.Repositories;
using Canais.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Canais.Application.Service;

public class ReclamacaoService : IReclamacaoService
{
    private readonly ISqsService _sqsService;
    private readonly ILogger<ReclamacaoService> _logger;
    private readonly IReclamacaoRepository _repository;
    private readonly IBucketService _bucketService;
    private readonly IHistoricoClienteProvider _historicoClienteProvider;
    private readonly HttpClient _httpClient;

    public ReclamacaoService(ISqsService sqsService, ILogger<ReclamacaoService> logger, IReclamacaoRepository repository, IBucketService bucketService, IHistoricoClienteProvider historicoClienteProvider, IHttpClientFactory httpClientFactory)
    {
        _sqsService = sqsService;
        _logger = logger;
        _repository = repository;
        _bucketService = bucketService;
        _historicoClienteProvider = historicoClienteProvider;
        _httpClient = httpClientFactory.CreateClient("LegadoClient");
    }

    public async Task<bool> CadastrarReclamacaoAsync(AdicionarReclamacaoRequest reclamacao, List<IFormFile> arquivos)
    {
        try
        {
            if (reclamacao is null) return false;

            var reclamacaoEntity = ConverterReclamacoes(reclamacao);

            _logger.LogInformation($"Inicio do cadastro da Reclamação");
            var reclamacaoCadastrada = await _repository.CadastrarReclamacaoAsync(reclamacaoEntity);

            if (reclamacaoCadastrada != null)
            {
                _logger.LogInformation($"Salvando anexos no bucket");
                var anexosSalvos = await SalvarAnexosReclamacao(arquivos, reclamacaoCadastrada);

                if (anexosSalvos.Count != 0)
                {
                    _logger.LogInformation($"Atualizando reclamação com anexos");
                    await _repository.AtualizarAnexosAsync(reclamacaoCadastrada.Id, anexosSalvos);
                }

                _logger.LogInformation($"Enviando Reclamação {reclamacaoCadastrada.Id} para fila");
                await EnviarReclamacaoParaFila(reclamacaoCadastrada!);

                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar reclamação.}");
            return false;
        }
    }

    public async Task<ResultResponse<List<ReclamacoesClassificadasResponse>>> ObterReclamacoesClassificadasAsync(FiltroReclamacoesRequest filtro)
    {
        var serviceResponse = new ResultResponse<List<ReclamacoesClassificadasResponse>>();

        try
        {
            var reclamacoes = await _repository.ListarReclamacoesClassificadasAsync();

            var response = reclamacoes.Select(ReclamacoesClassificadasResponse.ToResponse).ToList();

            if (response.Count == 0)
            {
                serviceResponse.Dados = response;
                serviceResponse.Sucesso = false;
                serviceResponse.Mensagem = "Nenhum registro foi localizado.";

                return serviceResponse;
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

    public async Task<bool> UploadReclamacoesFisicasAsync(IFormFile arquivo)
    {
        try
        {
            if (arquivo != null)
            {
                var nomeSemExtensao = Path.GetFileNameWithoutExtension(arquivo.FileName);
                var caminho = $"reclamacoes-aprocesssar/{arquivo.FileName}";
                var arquivoEnviado = await _bucketService.EnviarArquivosFisicosAsync(arquivo, caminho);

                return arquivoEnviado;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao enviar anexos físicos para S3 da reclamação");
            return false;
        }
    }

    public async Task<HistoricoClienteResponse> ObterHistoricoClienteAsync(string cpf)
    {
        try
        {
            var historico = await _historicoClienteProvider.ConsultarPorCpfAsync(cpf);
            return historico;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao buscar historico cliente datamesh");
            return null;
        }
    }


    public async Task<bool> EnviarParaSistemaLegadoAsync(Guid id)
    {
        try
        {
            var reclamacao = await _repository.ObterPorIdAsync(id);
            if (reclamacao == null) return false;

            var payload = new ReclamacaoLegado
            {
                Id = reclamacao.Id,
                Nome = reclamacao.Nome!,
                Cpf = reclamacao.Cpf!,
                Texto = reclamacao.Texto!,
                Canal = reclamacao.Canal!,
                Classificacao = reclamacao.ReclamacaoCategorias.ToString()!,
                Data = reclamacao.DataAbertura
            };

            var response = await _httpClient.PostAsJsonAsync("api/reclamacoes", payload);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Falha ao enviar reclamação {id} para o sistema legado. Status: {response.StatusCode}");
                return false;
            }

            _logger.LogInformation($"Reclamação {id} enviada com sucesso ao sistema legado.");
            //await _repository.MarcarComoEnviadaAoLegadoAsync(id); // Implementar flag para marcar como enviado legado
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao enviar reclamação {id} para o sistema legado.");
            return false;
        }
    }

    private ReclamacoesEntity ConverterReclamacoes(AdicionarReclamacaoRequest request)
    {
        var reclamacaoEntity = new ReclamacoesEntity(
            nome: request.Nome,
            cpf: request.Cpf,
            texto: request.Texto,
            canal: "Fisico",
            atendida: false,
            anexos: new List<string>(),
            dataAbertura: DateTime.Now
        );

        return reclamacaoEntity;
    }

    private async Task EnviarReclamacaoParaFila(ReclamacoesEntity reclamacao)
    {
        var mensagem = new
        {
            reclamacao.Id,
            reclamacao.Texto
        };

        var payload = JsonConvert.SerializeObject(mensagem);

        await _sqsService.EnviarReclamacaoAsync(payload);

        _logger.LogInformation($"Reclamação {reclamacao.Id} enviada para fila.");
    }

    private async Task<List<string>> SalvarAnexosReclamacao(List<IFormFile> arquivos, ReclamacoesEntity reclamacaoCadastrada)
    {
        List<string> anexosSalvos = new();
        if (arquivos?.Any() == true)
        {
            var uploadTasks = arquivos.Select(async arquivo =>
            {
                return await _bucketService.SalvarArquivoAsync(
                    arquivo,
                    reclamacaoCadastrada.Nome!,
                    reclamacaoCadastrada.Id.ToString()
                );
            });

            anexosSalvos = (await Task.WhenAll(uploadTasks)).ToList();
        }

        return anexosSalvos;
    }

}
