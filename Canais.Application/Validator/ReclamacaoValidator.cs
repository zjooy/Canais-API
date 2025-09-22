using Canais.Application.Models;
using System.Text.RegularExpressions;

namespace Canais.Application.Validatorç
{
    public static class ReclamacaoValidator
    {
        public static bool IsValid(AdicionarReclamacaoRequest request, out List<string> erros)
        {
            erros = new List<string>();

            if (request == null)
            {
                erros.Add("Reclamação não pode ser nula.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(request.Nome))
                erros.Add("Nome é obrigatório.");

            if (string.IsNullOrWhiteSpace(request.Cpf))
                erros.Add("CPF é obrigatório.");
            else if (!Regex.IsMatch(request.Cpf, @"^\d{3}\.?\d{3}\.?\d{3}-?\d{2}$"))
                erros.Add("CPF está em formato inválido.");

            if (string.IsNullOrWhiteSpace(request.Texto))
                erros.Add("Texto da reclamação é obrigatório.");

            return erros.Count == 0;
        }
    }
}
