using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.Model
{
    public class Marcacao
    {
        [Key]
        public int Id { get; set; }
        public DateTime DataRegistro { get; set; }

        public double  TotalPagar { get; set; }

        public bool Status { get; set; }

       public int ?UtilizadorId { get; set; }
        [ForeignKey(nameof(UtilizadorId))]
       public Utilizador ?Utilizador { get; set; }

      public virtual ICollection<ServicoMarcacao> ServicosMarcados { get; set; } 


    }
}
