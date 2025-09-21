using Canais.API.Controllers;
using Canais.Application.Interfaces;
using Canais.Application.Models;
using Canais.Application.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Canais.Tests.Unit.Api.Controllers;

public class ReclamacoesControllerTest
{
    private readonly Mock<IReclamacaoService> _mockService;
    private readonly ReclamacoesController _controller;

    public ReclamacoesControllerTest()
    {
        _mockService = new Mock<IReclamacaoService>();
        _controller = new ReclamacoesController(_mockService.Object);
    }

    #region CadastrarReclamacaoAsync

    [Fact]
    public async Task CadastrarReclamacaoAsync_DeveRetornarOk_QuandoCadastroForBemSucedido()
    {
        // Arrange
        var mockService = new Mock<IReclamacaoService>();
        var controller = new ReclamacoesController(mockService.Object);

        var model = new AdicionarReclamacaoRequest
        {
            Nome = "Joyce",
            Cpf = "12345678900",
            Texto = "Minha reclamação"
        };

        var arquivos = new List<IFormFile>();

        mockService
            .Setup(s => s.CadastrarReclamacaoAsync(model, arquivos))
            .ReturnsAsync(true);

        // Act
        var resultado = await controller.CadastrarReclamacaoAsync(model, arquivos);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(resultado);
        Assert.Equal("Reclamação cadastrada com sucesso.", okResult.Value);
    }

    [Fact]
    public async Task CadastrarReclamacaoAsync_DeveRetornarBadRequest_QuandoCadastroFalhar()
    {
        // Arrange
        var mockService = new Mock<IReclamacaoService>();
        var controller = new ReclamacoesController(mockService.Object);

        var model = new AdicionarReclamacaoRequest { Nome = "Joyce", Cpf = "123", Texto = "Erro" };
        var arquivos = new List<IFormFile>();

        mockService
            .Setup(s => s.CadastrarReclamacaoAsync(model, arquivos))
            .ReturnsAsync(false);

        // Act
        var resultado = await controller.CadastrarReclamacaoAsync(model, arquivos);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(resultado);
        Assert.Equal("Não foi possível cadastrar a reclamação.", badRequest.Value);
    }

    #endregion

    #region ObterReclamacoesClassificadasAsync

    [Fact]
    public async Task ObterReclamacoesClassificadasAsync_DeveRetornarOk_QuandoSucesso()
    {
        // Arrange
        var filtro = new FiltroReclamacoesRequest { Canal = "fisico" };

        var resultadoEsperado = new ResultResponse<List<ReclamacoesClassificadasResponse>>
        {
            Sucesso = true,
            Dados = new List<ReclamacoesClassificadasResponse>
            {
                new ReclamacoesClassificadasResponse { NomeReclamante = "Joyce", TextoReclamacao = "Problema", CanalRecebido = "fisico" }
            },
            Mensagem = "Busca realizada com sucesso."
        };

        _mockService.Setup(s => s.ObterReclamacoesClassificadasAsync(filtro))
                    .ReturnsAsync(resultadoEsperado);

        // Act
        var resultado = await _controller.ObterReclamacoesClassificadasAsync(filtro);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(resultado);
        Assert.Equal(resultadoEsperado, okResult.Value);
    }

    [Fact]
    public async Task ObterReclamacoesClassificadasAsync_DeveRetornarNotFound_QuandoSucessoForFalse()
    {
        // Arrange
        var filtro = new FiltroReclamacoesRequest { Canal = "fisico" };

        var resultadoEsperado = new ResultResponse<List<ReclamacoesClassificadasResponse>>
        {
            Sucesso = false,
            Dados = new List<ReclamacoesClassificadasResponse>(),
            Mensagem = "Nenhum registro foi localizado."
        };

        _mockService.Setup(s => s.ObterReclamacoesClassificadasAsync(filtro))
                    .ReturnsAsync(resultadoEsperado);

        // Act
        var resultado = await _controller.ObterReclamacoesClassificadasAsync(filtro);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(resultado);
        Assert.Equal(resultadoEsperado, notFoundResult.Value);
    }

    #endregion
}
