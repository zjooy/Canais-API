namespace Canais.Application.Models;

public class ReclamacaoRequest
{
    public string Nome { get; set; }
    public string Cpf { get; set; }
    public string Texto { get; set; }
    public string? Canal { get; set; }
    public bool ReclamacaoAtendida { get; private set; } = false;
    //public List<string>? Anexos { get; private set; } = new List<string>();
}
