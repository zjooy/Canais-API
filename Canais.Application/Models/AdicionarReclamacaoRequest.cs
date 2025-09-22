using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Canais.Application.Models
{
    public class AdicionarReclamacaoRequest
    {
        [Required]
        public string Nome { get; set; } = string.Empty;

        [Required]
        public string Cpf { get; set; } = string.Empty;

        [Required]
        public string Texto { get; set; } = string.Empty;
    }
}
