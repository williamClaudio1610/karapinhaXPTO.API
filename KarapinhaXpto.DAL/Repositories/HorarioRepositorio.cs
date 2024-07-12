using KarapinhaXpto.DTO;

using KarapinhaXpto.Shared.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.DAL.Repositories
{
    public class HorarioRepositorio: IHorarioRepositorio
    {

        private readonly KarapinhaXptoDbContext _karapinhaXptoDbContext;

        public HorarioRepositorio(KarapinhaXptoDbContext karapinhaXptoDbContext)
        {
            _karapinhaXptoDbContext = karapinhaXptoDbContext;
        }

        public List<HorarioDTO> ObterTodosHorarios()
        {
            var horarios = _karapinhaXptoDbContext.Horarios
                                   .Select(h => new HorarioDTO
                                   {
                                       Id = h.Id,   
                                       Hora = h.Hora
                                   })
                                   .ToList();

            return horarios;
        }
    }
}

