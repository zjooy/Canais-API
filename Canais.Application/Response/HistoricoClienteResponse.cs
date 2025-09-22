namespace Canais.Application.Response;

public class HistoricoClienteResponse
{
    public string Nome { get; set; }
    public string Cpf { get; set; }
    public int ScoreCredito { get; set; }
    public string Relacionamento { get; set; }
    public string Status { get; set; }
    public List<string> ProdutosAtivos { get; set; }
}
