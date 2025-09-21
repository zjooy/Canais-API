using Canais.Domain.Entities;

namespace Canais.Domain.Contracts.Repositories;

public interface IReclamacaoRepository
{
    Task<IEnumerable<ReclamacoesEntity>> ListarReclamacoesClassificadasAsync();
    Task<ReclamacoesEntity> CadastrarReclamacaoAsync(ReclamacoesEntity reclamacao);
    Task AtualizarAnexosAsync(Guid id, List<string> anexos);
}
