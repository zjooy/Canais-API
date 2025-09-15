using Microsoft.AspNetCore.Http;

namespace Canais.Domain.Interfaces;

public interface IBucketService
{
    Task<string> SalvarArquivoAsync(IFormFile arquivo, string nomeReclamante, string identificacaoReclamacao);
}
