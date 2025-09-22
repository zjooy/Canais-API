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
            return BadRequest("Não foi possível cadastrar a reclamação. Verifique se os dados informados estão corretos.");
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

    /// <summary>
    /// Faz o upload de reclamações fisicas.
    /// </summary>
    /// <param name="arquivo"></param>
    /// <returns>Mensagem De Arquivo enviado com sucesso</returns>
    /// <response code="200">Reclamações enviadas com sucesso.</response>
    /// <response code="400">Não foi possível cadastrar a reclamação.</response>
    [Authorize]
    [HttpPost("reclamacoes-fisicas/anexo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadReclamacoesFisicasAsync(IFormFile arquivo)
    {
        if (arquivo.ContentType != "application/pdf")
            return BadRequest("Somente arquivos PDF são permitidos.");

        var arquivoEnviado = await _reclamacaoService.UploadReclamacoesFisicasAsync(arquivo);

        if (!arquivoEnviado)
        {
            return BadRequest("Não foi possível enviar a reclamação.");
        }

        return Ok("Reclamação fisica enviada com sucesso.");
    }

    /// <summary>
    /// Obtém o histórico de um cliente específico.
    /// </summary>
    /// <param name="cpf"></param>
    /// <returns>Histórico do cliente com o banco</returns>
    /// <response code="200">Reclamações enviadas com sucesso.</response>
    /// <response code="404">Não foi possível localizar histórico do cliente com o banco.</response>
    [HttpGet("reclamacoes/{cpf}/detalhes")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterDetalhesReclamacaoAsync(string cpf)
    {
        var detalhes = await _reclamacaoService.ObterHistoricoClienteAsync(cpf);

        if (detalhes == null)
            return NotFound("Nenhum histórico com o cliente encontrado.");

        return Ok(detalhes);
    }

    /// <summary>
    /// Envia a reclamação para o sistema legado.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Mensagem De Reclamação enviada com sucesso</returns>
    [HttpPost("reclamacoes/{id}/enviar-legado")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EnviarParaSistemaLegadoAsync(int id)
    {
        var sucesso = await _reclamacaoService.EnviarParaSistemaLegadoAsync(id);

        if (!sucesso)
            return BadRequest("Falha ao enviar reclamação para o sistema legado.");

        return Ok("Reclamação enviada com sucesso.");
    }

}
