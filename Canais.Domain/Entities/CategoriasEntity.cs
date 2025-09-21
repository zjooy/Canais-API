using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canais.Domain.Entities
{
    public class CategoriasEntity
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("nome")]
        public string Nome { get; set; }

        [Column("palavras")]
        public List<string> Palavras { get; set; }

        public ICollection<ReclamacaoCategoriasEntity> ReclamacaoCategorias { get; set; }
    }
}
