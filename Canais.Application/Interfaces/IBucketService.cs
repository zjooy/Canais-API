using Microsoft.AspNetCore.Http;

namespace Canais.Application.Interfaces;

public interface IBucketService
{
    Task<string> SalvarArquivoAsync(IFormFile arquivo, string nomeReclamante, string identificacaoReclamacao);
    Task<bool> EnviarArquivosFisicosAsync(IFormFile arquivo, string caminhoCompleto);
}
