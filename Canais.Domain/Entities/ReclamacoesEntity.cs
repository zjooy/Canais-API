using Amazon.DynamoDBv2.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace Canais.Domain.Entities;

public class ReclamacoesEntity
{
    public ReclamacoesEntity()
    {
        ReclamacaoCategorias = new List<ReclamacaoCategoriasEntity>();
        Anexos = new List<string>();
    }

    // Construtor para uso manual (não usado pelo EF)
    public ReclamacoesEntity(string nome, string cpf, string texto, string canal, bool atendida,
        List<string> anexos, DateTime dataAbertura)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Cpf = cpf;
        Texto = texto;
        Canal = canal;
        Atendida = atendida;
        Anexos = anexos ?? new List<string>();
        DataAbertura = dataAbertura;
        ReclamacaoCategorias = new List<ReclamacaoCategoriasEntity>();
    }

    [Column("id")]
    public Guid Id { get; set; }

    [Column("nome")]
    public string? Nome { get; private set; }

    [Column("cpf")]
    public string? Cpf { get; private set; }

    [Column("texto")]
    public string? Texto { get; private set; }

    [Column("canal")]
    public string? Canal { get; private set; }

    [Column("atendida")]
    public bool? Atendida { get; private set; }

    [Column("anexos")]
    public List<string> Anexos { get; private set; }

    [Column("dataabertura")]
    public DateTime DataAbertura { get; private set; }
    public ICollection<ReclamacaoCategoriasEntity> ReclamacaoCategorias { get; set; }
}
