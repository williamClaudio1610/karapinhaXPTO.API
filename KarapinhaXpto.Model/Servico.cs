using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.Model
{
    public class Servico
    {

        [Key]
        public int Id { get; set; }

        public string Nome { get; set; }
        public string Descricao { get; set; }

        public string Imagem { get; set; }

        public int CategoriaId { get; set; }
        [ForeignKey(nameof(CategoriaId))]
        public Categoria Categoria { get; set; }

        public double Preco { get; set; }


    }
}
