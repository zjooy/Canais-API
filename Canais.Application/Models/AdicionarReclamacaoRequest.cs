namespace Canais.Application.Models
{
    public class AdicionarReclamacaoRequest
    {
        public string Nome { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Texto { get; set; } = string.Empty;
        public string? Canal { get; set; } = "Site";
        public bool ReclamacaoAtendida { get; private set; } = false;
    }
}
