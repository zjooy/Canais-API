using Microsoft.AspNetCore.Http;

namespace Canais.Application.Interfaces;

public interface IBucketService
{
    Task<string> SalvarArquivoAsync(IFormFile arquivo, string nomeReclamante, int identificacaoReclamacao);
    Task<bool> EnviarArquivosFisicosAsync(IFormFile arquivo, string caminhoCompleto);
}
