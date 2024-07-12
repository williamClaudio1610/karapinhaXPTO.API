using KarapinhaXpto.DAL.Repositories;
using KarapinhaXpto.DTO;
using KarapinhaXpto.Shared.IRepositories;
using KarapinhaXpto.Shared.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.Services
{
    public class HorarioService: IHorarioService
    {
        private readonly IHorarioRepositorio _horarioRepository;

        public HorarioService(IHorarioRepositorio horarioRepository)
        {
            _horarioRepository = horarioRepository;
        }

        public List<HorarioDTO> ObterTodosHorarios()
        {
            return _horarioRepository.ObterTodosHorarios();
        }
    }
}
