using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.Model
{
    public class ProfissionalHorario
    {
        [Key]
        public int Id { get; set; }

        public int ProfissionalID { get; set; }
         [ForeignKey(nameof(ProfissionalID))]

         public Profissional Profissional { get; set; }

        public int HorarioID { get; set; }
        [ForeignKey(nameof(HorarioID))]


        public Horario Horario { get; set; }    
    }
}
