using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canais.Domain.Entities
{
    public class ReclamacaoCategoriasEntity
    {
        [Column("reclamacaoid")]
        public Guid ReclamacaoId { get; set; }

        [Column("categoriaid")]
        public int CategoriaId { get; set; }

        public ReclamacoesEntity Reclamacao { get; set; }
        public CategoriasEntity Categorias { get; set; }
    }
}
