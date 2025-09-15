using Canais.Application.Models;
using Canais.Application.Service;
using Canais.Domain.Request;
using Microsoft.AspNetCore.Mvc;

namespace Canais.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReclamacoesController : ControllerBase
{
    private readonly IReclamacaoService _reclamacaoService;

    public ReclamacoesController(IReclamacaoService reclamacaoService)
    {
        _reclamacaoService = reclamacaoService;
    }


    [HttpPost]
    public async Task<IActionResult> EnviarReclamacoesParaFilaAsync([FromForm] ReclamacaoRequest reclamacaoModel, List<IFormFile> arquivos)
    {
        await _reclamacaoService.EnviarReclamacoesParaFilaAsync(reclamacaoModel, arquivos);

        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> ObterReclamacoesClassificadasAsync([FromQuery] FiltroReclamacoesRequest filtro)
    {
        var reclamacoes = await _reclamacaoService.ObterReclamacoesClassificadasAsync(filtro);

        return Ok(reclamacoes);
    }
}
