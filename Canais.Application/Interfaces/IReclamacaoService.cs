using Canais.Application.Models;
using Canais.Application.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Canais.Application.Interfaces;

public interface IReclamacaoService
{
    Task<bool> CadastrarReclamacaoAsync(AdicionarReclamacaoRequest reclamacao, List<IFormFile> arquivos);
    Task<ResultResponse<List<ReclamacoesClassificadasResponse>>> ObterReclamacoesClassificadasAsync(FiltroReclamacoesRequest filtro);
    Task<bool> UploadReclamacoesFisicasAsync(IFormFile arquivo);
    Task<HistoricoClienteResponse> ObterHistoricoClienteAsync(string cpf);
    Task<bool> EnviarParaSistemaLegadoAsync(int id);
}
