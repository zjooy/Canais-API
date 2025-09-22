namespace Canais.Application.Models;

public class ReclamacaoLegado
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Cpf { get; set; }
    public string Texto { get; set; }
    public string Canal { get; set; }
    public string Classificacao { get; set; }
    public DateTime Data { get; set; }
}
