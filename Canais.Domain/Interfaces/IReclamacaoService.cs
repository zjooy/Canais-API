using Canais.Application.Models;
using Canais.Domain.Request;
using Canais.Domain.Response;
using Microsoft.AspNetCore.Http;

namespace Canais.Application.Service;

public interface IReclamacaoService
{
    Task EnviarReclamacoesParaFilaAsync(ReclamacaoRequest reclamacao, List<IFormFile> arquivos);
    Task<ResultResponse<List<ReclamacoesClassificadasResponse>>> ObterReclamacoesClassificadasAsync(FiltroReclamacoesRequest filtro);
}
