using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.DTO
{
    public class MarcacaoDTO
    {
        public int Id { get; set; }
        public DateTime DataRegistro { get; set; }
        public double TotalPagar { get; set; }
        public bool Status { get; set; }
        public int? UtilizadorId { get; set; }
        public ICollection<ServicoMarcacaoDTO> ServicosMarcados { get; set; }
    }
    public class MarcacaoUpdateDTO
    {
        public DateTime DataRegistro { get; set; }
        public List<ServicoMarcacaoUpdateDTO> ServicosMarcados { get; set; }
    }

}
