using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.Model
{
    public class Profissional
    {
        [Key]
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public string Foto { get; set; }

        public string BI { get; set; }

        public string Telemovel { get; set; }

        // Coleção de Horarios do Profissional
        public ICollection<ProfissionalHorario> Horarios { get; set; }

        // Coleção de Servicos do Profissional (many-to-many)
        public ICollection<ProfissionalCategoria> ProfissionalCategorias { get; set; }



    }
   
}
