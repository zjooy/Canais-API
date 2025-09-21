using Canais.Application.Interfaces;
using Canais.Application.Models;
using Microsoft.AspNetCore.Authorization;
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

    /// <summary>
    /// Cadastra uma nova reclamação com anexos opcionais.
    /// </summary>
    /// <param name="reclamacaoModel"></param>
    /// <param name="arquivos"></param>
    /// <returns>Reclamação recen criada</returns>
    /// <response code="200">Reclamação cadastrada com sucesso.</response>
    /// <response code="400">Não foi possível cadastrar a reclamação.</response>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CadastrarReclamacaoAsync([FromForm] AdicionarReclamacaoRequest reclamacaoModel, List<IFormFile> arquivos)
    {
        var reclamacaoCadastrada = await _reclamacaoService.CadastrarReclamacaoAsync(reclamacaoModel, arquivos);

        if (!reclamacaoCadastrada)
        {
            return BadRequest("Não foi possível cadastrar a reclamação.");
        }

        return Ok("Reclamação cadastrada com sucesso.");
    }

    /// <summary>
    /// Obtém as reclamações classificadas com base nos filtros fornecidos.
    /// </summary>
    /// <param name="filtro"></param>
    /// <returns>Lista de reclamações recebidas</returns>
    /// <response code="200">Reclamações obtidas com sucesso.</response>
    /// <response code="404">Reclamações não encontradas.</response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterReclamacoesClassificadasAsync([FromQuery] FiltroReclamacoesRequest filtro)
    {
        var reclamacoes = await _reclamacaoService.ObterReclamacoesClassificadasAsync(filtro);

        if (!reclamacoes.Sucesso)
        {
            return NotFound(reclamacoes);
        }

        return Ok(reclamacoes);
    }
}
