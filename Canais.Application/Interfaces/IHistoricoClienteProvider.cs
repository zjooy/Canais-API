using Canais.Application.Response;

namespace Canais.Application.Interfaces;

public interface IHistoricoClienteProvider
{
    Task<HistoricoClienteResponse> ConsultarPorCpfAsync(string cpf);
}
