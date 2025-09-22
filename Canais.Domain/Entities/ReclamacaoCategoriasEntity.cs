using System.ComponentModel.DataAnnotations.Schema;

namespace Canais.Domain.Entities
{
    public class ReclamacaoCategoriasEntity
    {
        [Column("idreclamacao")]
        public int IdReclamacao { get; set; }

        [Column("categoriaid")]
        public int CategoriaId { get; set; }

        public ReclamacoesEntity Reclamacao { get; set; }
        public CategoriasEntity Categorias { get; set; }
    }
}
