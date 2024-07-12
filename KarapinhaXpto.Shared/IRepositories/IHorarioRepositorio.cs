using KarapinhaXpto.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.Shared.IRepositories
{
    public interface IHorarioRepositorio
    {
        List<HorarioDTO> ObterTodosHorarios();
    }
}
