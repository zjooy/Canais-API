using Canais.Domain.Entities;

namespace Canais.Domain.Contracts.Repositories;

public interface IReclamacaoRepository
{
    Task<IEnumerable<ReclamacoesEntity>> ListarReclamacoesClassificadasAsync(FiltroReclamacoesEntity request);
    Task<ReclamacoesEntity> CadastrarReclamacaoAsync(ReclamacoesEntity reclamacao);
    Task AtualizarAnexosAsync(int id, List<string> anexos);
    Task<ReclamacoesEntity> ObterPorIdAsync(int id);
}
