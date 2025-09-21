using Canais.Application.Interfaces;
using Canais.Application.Models;
using Canais.Application.Service;
using Canais.Domain.Contracts.Repositories;
using Canais.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace Canais.Tests.Unit.Application.Services;

public class ReclamacaoServiceTest
{
    private readonly Mock<ISqsService> _mockSqs;
    private readonly Mock<ILogger<ReclamacaoService>> _mockLogger;
    private readonly Mock<IReclamacaoRepository> _mockRepo;
    private readonly Mock<IBucketService> _mockBucket;

    private readonly ReclamacaoService _service;

    public ReclamacaoServiceTest()
    {
        _mockSqs = new Mock<ISqsService>();
        _mockLogger = new Mock<ILogger<ReclamacaoService>>();
        _mockRepo = new Mock<IReclamacaoRepository>();
        _mockBucket = new Mock<IBucketService>();

        _service = new ReclamacaoService(
            _mockSqs.Object,
            _mockLogger.Object,
            _mockRepo.Object,
            _mockBucket.Object
        );
    }

    #region CadastrarReclamacaoAsync

    [Fact]
    public async Task CadastrarReclamacaoAsync_DeveRetornarTrue_QuandoCadastroForBemSucedido()
    {
        // Arrange
        var request = new AdicionarReclamacaoRequest
        {
            Nome = "Joyce",
            Cpf = "12345678900",
            Texto = "Problema com atendimento",
            Canal = "fisico"
        };

        var arquivos = new List<IFormFile>();

        var entidade = new ReclamacoesEntity("Joyce", "12345678900", "Problema com atendimento", "fisico", false, new List<string>(), DateTime.UtcNow)
        {
            Id = Guid.NewGuid()
        };

        _mockRepo.Setup(r => r.CadastrarReclamacaoAsync(It.IsAny<ReclamacoesEntity>()))
                 .ReturnsAsync(entidade);

        _mockBucket.Setup(b => b.SalvarArquivoAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), It.IsAny<string>()))
                   .ReturnsAsync("arquivo.jpg");

        _mockSqs.Setup(s => s.EnviarReclamacaoAsync(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

        // Act
        var resultado = await _service.CadastrarReclamacaoAsync(request, arquivos);

        // Assert
        Assert.True(resultado);
    }

    [Fact]
    public async Task CadastrarReclamacaoAsync_DeveRetornarFalse_QuandoRequestForNulo()
    {
        // Act
        var resultado = await _service.CadastrarReclamacaoAsync(null, new List<IFormFile>());

        // Assert
        Assert.False(resultado);
    }

    [Fact]
    public async Task CadastrarReclamacaoAsync_DeveRetornarFalse_QuandoRepositoryLancarException()
    {
        // Arrange
        var request = new AdicionarReclamacaoRequest
        {
            Nome = "Joyce",
            Cpf = "12345678900",
            Texto = "Erro simulado",
            Canal = "fisico"
        };

        var arquivos = new List<IFormFile>();

        _mockRepo.Setup(r => r.CadastrarReclamacaoAsync(It.IsAny<ReclamacoesEntity>()))
                 .ThrowsAsync(new Exception("Erro de banco"));

        // Act
        var resultado = await _service.CadastrarReclamacaoAsync(request, arquivos);

        // Assert
        Assert.False(resultado);
    }

    #endregion

    #region ObterReclamacoesClassificadasAsync

    [Fact]
    public async Task ObterReclamacoesClassificadasAsync_DeveRetornarSucesso_QuandoEncontrarReclamacoes()
    {
        // Arrange
        var lista = new List<ReclamacoesEntity>
        {
            new ReclamacoesEntity("Joyce", "123", "Texto", "fisico", false, new List<string>(), DateTime.UtcNow)
        };

        _mockRepo.Setup(r => r.ListarReclamacoesClassificadasAsync())
                 .ReturnsAsync(lista);

        var filtro = new FiltroReclamacoesRequest { Canal = "fisico" };

        // Act
        var resultado = await _service.ObterReclamacoesClassificadasAsync(filtro);

        // Assert
        Assert.True(resultado.Sucesso);
        Assert.NotEmpty(resultado.Dados);
        Assert.Equal("Busca realizada com sucesso.", resultado.Mensagem);
    }

    [Fact]
    public async Task ObterReclamacoesClassificadasAsync_DeveRetornarSucessoFalse_QuandoListaEstiverVazia()
    {
        // Arrange
        _mockRepo.Setup(r => r.ListarReclamacoesClassificadasAsync())
                 .ReturnsAsync(new List<ReclamacoesEntity>());

        var filtro = new FiltroReclamacoesRequest { Canal = "fisico" };

        // Act
        var resultado = await _service.ObterReclamacoesClassificadasAsync(filtro);

        // Assert
        Assert.False(resultado.Sucesso);
        Assert.Empty(resultado.Dados);
        Assert.Equal("Nenhum registro foi localizado.", resultado.Mensagem);
    }

    [Fact]
    public async Task ObterReclamacoesClassificadasAsync_DeveRetornarSucessoFalse_QuandoRepositoryLancarException()
    {
        // Arrange
        _mockRepo.Setup(r => r.ListarReclamacoesClassificadasAsync())
                 .ThrowsAsync(new Exception("Erro inesperado"));

        var filtro = new FiltroReclamacoesRequest { Canal = "fisico" };

        // Act
        var resultado = await _service.ObterReclamacoesClassificadasAsync(filtro);

        // Assert
        Assert.False(resultado.Sucesso);
        Assert.Equal("Erro inesperado", resultado.Mensagem);
    }

    #endregion

}
