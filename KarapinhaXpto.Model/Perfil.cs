using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.Model
{
    public class Perfil
    {
        [Key]
        public int Id { get; set; }

        public String ?Descricao { get; set; }
    }
}
