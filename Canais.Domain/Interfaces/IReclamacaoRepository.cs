using Canais.Application.Models;
using Canais.Domain.Entities;
using Canais.Domain.Request;

namespace Canais.Domain.Interfaces;

public interface IReclamacaoRepository
{
    Task<List<ReclamacoesEntity>> ListarReclamacoesClassificadasAsync(FiltroReclamacoesRequest filtro);
}
