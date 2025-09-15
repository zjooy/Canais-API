using Canais.Domain.Entities;

namespace Canais.Application.Models;

public class ReclamacoesClassificadasModel
{
    public string NomeReclamante { get; set; }
    public string CpfReclamante { get; set; }
    public string TextoReclamacao { get; set; }
    public string CanalRecebido { get; set; }
    public bool? ReclamacaoAtendida { get; set; }
    public List<string> Categorias { get; set; }
    public List<string> Anexos { get; set; }
    public DateTime DataAbertura { get; set; }
}
