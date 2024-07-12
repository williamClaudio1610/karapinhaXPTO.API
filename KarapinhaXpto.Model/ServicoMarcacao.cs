using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.Model
{
    public class ServicoMarcacao
    {
        [Key]

        public int Id { get; set; }

        public int ServicoId { get; set; }
        [ForeignKey(nameof(ServicoId))]

        public Servico Servico { get; set; }

        public int MarcacaoId { get; set; }
        [ForeignKey(nameof(MarcacaoId))]

        public Marcacao Marcacao { get; set; }

        public int ProfissionalId { get; set; }
        [ForeignKey(nameof(ProfissionalId))]
        public Profissional Profissional { get; set; }

        public DateOnly Data { get; set; }

        public TimeOnly Hora { get; set; }

        public DateOnly? DataAnterior { get; set; }
        public TimeOnly? HoraAnterior { get; set; }


    }
}
