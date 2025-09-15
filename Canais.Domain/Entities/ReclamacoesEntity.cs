using Amazon.DynamoDBv2.Model;

namespace Canais.Domain.Entities
{
    public class ReclamacoesEntity
    {
        public string Id { get; set; }
        public string? Nome { get; set; }
        public string? Cpf { get; set; }
        public string? Texto { get; set; }
        public string? Canal { get; set; }
        public bool? ReclamacaoAtendida { get; set; }
        public List<string> Anexos { get; set; }
        public List<string> Categorias { get; set; }
        public DateTime DataAbertura { get; set; }

        public static ReclamacoesEntity FromDynamoDB(Dictionary<string, AttributeValue> model)
        {
            return new ReclamacoesEntity
            {
                Id = model["Id"].S,
                Nome = model["Nome"].S,
                Cpf = model["Cpf"].S,
                Texto = model["Texto"].S,
                ReclamacaoAtendida = model["Atendida"].BOOL,
                Anexos = model["Documentos"].SS.ToList(),
                Categorias = model["Categorias"].SS.ToList(),
                DataAbertura = DateTime.Parse(model["DataAbertura"].S)
            };
        }
    }
}
