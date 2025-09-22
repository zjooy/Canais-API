using Canais.Application.Interfaces;
using Canais.Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canais.Application.Service
{
    public class HistoricoClienteProviderService : IHistoricoClienteProvider
    {
        public async Task<HistoricoClienteResponse> ConsultarPorCpfAsync(string cpf)
        {
            return new HistoricoClienteResponse
            {
                Nome = "Maria da Silva",
                Cpf = cpf,
                ScoreCredito = 820,
                Relacionamento = "Cliente desde 2015",
                Status = "Ativo",
                ProdutosAtivos = new List<string> { "Conta Corrente", "Cartão de Crédito" }
            };
        }
    }
}
