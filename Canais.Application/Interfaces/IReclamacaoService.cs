using Canais.Application.Models;
using Canais.Application.Response;
using Microsoft.AspNetCore.Http;

namespace Canais.Application.Interfaces;

public interface IReclamacaoService
{
    Task<bool> CadastrarReclamacaoAsync(AdicionarReclamacaoRequest reclamacao, List<IFormFile> arquivos);
    Task<ResultResponse<List<ReclamacoesClassificadasResponse>>> ObterReclamacoesClassificadasAsync(FiltroReclamacoesRequest filtro);
}
