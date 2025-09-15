namespace Canais.Application.Service;

public interface ISqsService
{
    Task EnviarReclamacaoAsync(string reclamacao);
}
