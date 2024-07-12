using KarapinhaXpto.Model;
using KarapinhaXpto.Shared.IRepositories;
using KarapinhaXpto.Shared.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.DAL.Repositories
{
    public class ServicoMarcacaoRepositorio
    {
        private readonly KarapinhaXptoDbContext _karapinhaXptoDbContext;

        public ServicoMarcacaoRepositorio(KarapinhaXptoDbContext karapinhaXptoDbContext)
        {
            _karapinhaXptoDbContext = karapinhaXptoDbContext;
        }

        public async Task<IEnumerable<ServicoMarcacao>> GetByMarcacaoIdAsync(int profissionalMarcacaoId)
        {
            return await _karapinhaXptoDbContext.ServicoMarcacaos
                                 .Where(sm => sm.ProfissionalId == profissionalMarcacaoId)
                                 .ToListAsync();
        }

       

    }
}
