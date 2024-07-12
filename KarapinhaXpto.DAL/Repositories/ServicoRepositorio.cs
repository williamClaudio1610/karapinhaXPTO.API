using KarapinhaXpto.Model;
using KarapinhaXpto.Shared.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.DAL.Repositories
{
    public class ServicoRepositorio: IServicoRepositorio
    {
        private readonly KarapinhaXptoDbContext _karapinhaXptoDbContext;

        public ServicoRepositorio(KarapinhaXptoDbContext karapinhaXptoDbContext)
        {
            _karapinhaXptoDbContext = karapinhaXptoDbContext;
        }

        public async Task<Servico> GetServicoByNomeAsync(string nome)
        {
            return await _karapinhaXptoDbContext.Servicos.FirstOrDefaultAsync(s => s.Nome == nome);
        }
        public async Task<Categoria> GetCategoriaByIdAsync(int categoriaId)
        {
            return await _karapinhaXptoDbContext.Categorias.FirstOrDefaultAsync(c => c.Id == categoriaId);
        }
        public async Task AddServicoAsync(Servico servico)
        {

                 await _karapinhaXptoDbContext.Servicos.AddAsync(servico);
                  await _karapinhaXptoDbContext.SaveChangesAsync();

        }
        public async Task<IEnumerable<Servico>> GetAllServicosAsync()
        {
            return await _karapinhaXptoDbContext.Servicos.ToListAsync();
        }
        public async Task<Servico> GetByIdAsync(int id)
        {
            return await _karapinhaXptoDbContext.Servicos.FindAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var servico = await _karapinhaXptoDbContext.Servicos.FindAsync(id);

            if (servico == null)
                return false;

            _karapinhaXptoDbContext.Servicos.Remove(servico);
            await _karapinhaXptoDbContext.SaveChangesAsync();
            return true;
        }
        public async Task UpdateAsync(Servico servico)
        {
            _karapinhaXptoDbContext.Servicos.Update(servico);
            await _karapinhaXptoDbContext.SaveChangesAsync();
        }

    }
}
