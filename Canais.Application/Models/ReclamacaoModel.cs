namespace Canais.Application.Models;

public class ReclamacaoModel
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public string Cpf { get; private set; }
    public string Texto { get; private set; }
    public string Canal { get; private set; }
    public List<string> Anexos { get; private set; }

    public ReclamacaoModel(string nome, string cpf, string texto, string canal, List<string> anexos)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Cpf = cpf;
        Texto = texto;
        Canal = canal;
        Anexos = anexos;
    }
}
