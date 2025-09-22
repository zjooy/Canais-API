using System.Text.RegularExpressions;

namespace Canais.Domain.Validatorç
{
    public static class ReclamacaoValidator
    {
        public static bool IsValidReclamacao(AdicionarReclamacaoRequest dto, out List<string> erros)
        {
            erros = new List<string>();

            if (string.IsNullOrWhiteSpace(dto.Nome))
                erros.Add("Nome é obrigatório.");

            if (string.IsNullOrWhiteSpace(dto.Cpf))
                erros.Add("CPF é obrigatório.");
            else if (!Regex.IsMatch(dto.Cpf, @"^\d{3}\.?\d{3}\.?\d{3}-?\d{2}$"))
                erros.Add("CPF está em formato inválido.");

            if (string.IsNullOrWhiteSpace(dto.Texto))
                erros.Add("Texto da reclamação é obrigatório.");

            return erros.Count == 0;
        }
    }
}
