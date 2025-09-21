namespace Canais.Application.Models;

public class FiltroReclamacoesRequest
{
    public string? NomeReclamante { get; set; }
    public string? CpfReclamante { get; set; }
    public string? TextoReclamante { get; set; }
    public string? Canal { get; set; }
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public bool? ReclamacaoAtendida { get; set; }
    public string? Categoria { get; set; }
}
