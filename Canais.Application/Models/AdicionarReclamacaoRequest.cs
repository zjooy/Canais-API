using System.Text.Json.Serialization;

namespace Canais.Application.Models
{
    public class AdicionarReclamacaoRequest
    {
        public string Nome { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Texto { get; set; } = string.Empty;
    }
}
