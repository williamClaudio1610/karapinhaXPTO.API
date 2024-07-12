using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.Model
{
    public class Categoria
    {

        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }

        public String ?Descricao { get; set; }

        public string Foto { get; set; }

    }
}
