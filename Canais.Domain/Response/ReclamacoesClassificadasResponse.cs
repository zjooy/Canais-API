using Canais.Domain.Entities;

namespace Canais.Domain.Response;

public class ReclamacoesClassificadasResponse
{
    public string NomeReclamante { get; set; }
    public string CpfReclamante { get; set; }
    public string TextoReclamacao { get; set; }
    public string CanalRecebido { get; set; }
    public bool? ReclamacaoAtendida { get; set; }
    public List<string> Categorias { get; set; }
    public List<string> Anexos { get; set; }
    public DateTime DataAbertura { get; set; }

    public static ReclamacoesClassificadasResponse ToResponse(ReclamacoesEntity reclamacao)
    {
        return new ReclamacoesClassificadasResponse
        {
            NomeReclamante = reclamacao.Nome,
            CpfReclamante = reclamacao.Cpf,
            TextoReclamacao = reclamacao.Texto,
            CanalRecebido = reclamacao.Canal,
            ReclamacaoAtendida = reclamacao.ReclamacaoAtendida,
            Categorias = reclamacao.Categorias,
            Anexos = reclamacao.Anexos,
            DataAbertura = reclamacao.DataAbertura
        };
    }
}
