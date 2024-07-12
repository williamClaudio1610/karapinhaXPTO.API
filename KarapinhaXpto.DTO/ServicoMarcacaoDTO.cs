using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.DTO
{
    public class ServicoMarcacaoDTO
    {
        public int Id { get; set; }
        public int ServicoId { get; set; }
        public int MarcacaoId { get; set; }
        public int ProfissionalId { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan Hora { get; set; }
        public DateOnly? DataAnterior { get; set; }
        public TimeOnly? HoraAnterior { get; set; }
    }

    public class ServicoMarcacaoUpdateDTO
    {
        public int ServicoMarcacaoId { get; set; }
        public string NovaData { get; set; }
        public string NovaHora { get; set; }
    }
}
