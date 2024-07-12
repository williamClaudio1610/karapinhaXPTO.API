using KarapinhaXpto.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.Shared.IServices
{
    public interface IHorarioService
    {
        List<HorarioDTO> ObterTodosHorarios();

    }
}
