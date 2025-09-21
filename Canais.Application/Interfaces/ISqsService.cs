namespace Canais.Application.Interfaces;

public interface ISqsService
{
    Task EnviarReclamacaoAsync(string reclamacao);
}
