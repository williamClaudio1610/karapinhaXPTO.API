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
    public class ProfissionaisRepositorio:IProfissionaisRepositorio
    {
        
        private readonly KarapinhaXptoDbContext _karapinhaXptoDbContext;

        public ProfissionaisRepositorio(KarapinhaXptoDbContext karapinhaXptoDbContext)
        {
            _karapinhaXptoDbContext = karapinhaXptoDbContext;
        }

        public async Task AddProfissionalAsync(Profissional profissional)
        {
            await _karapinhaXptoDbContext.Profissionals.AddAsync(profissional);
            await _karapinhaXptoDbContext.SaveChangesAsync();
        }


        public async Task<Profissional> GetProfissionalByBiAsync(string bi)
        {
            return await _karapinhaXptoDbContext.Profissionals.FirstOrDefaultAsync(p => p.BI == bi);
        }


        public async Task<Profissional> GetProfissionalByEmailAsync(string email)
        {
            return await _karapinhaXptoDbContext.Profissionals
                                 .Include(p => p.Horarios)
                                 .Include(p => p.ProfissionalCategorias)
                                 .ThenInclude(ps => ps.Categoria)
                                 .FirstOrDefaultAsync(p => p.Email == email);
        }

        public async Task UpdateProfissionalAsync(Profissional profissional)
        {
            _karapinhaXptoDbContext.Profissionals.Update(profissional);
            await _karapinhaXptoDbContext.SaveChangesAsync();
        }

        public async Task DeleteProfissionalAsync(Profissional profissional)
        {
            _karapinhaXptoDbContext.Profissionals.Remove(profissional);
            await _karapinhaXptoDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Profissional>> GetAllProfissionaisAsync()
        {
            return await _karapinhaXptoDbContext.Profissionals
                                 .Include(p => p.Horarios)
                                 .Include(p => p.ProfissionalCategorias)
                                 .ThenInclude(ps => ps.Categoria)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Profissional>> SearchProfissionaisAsync(string searchCriteria)
        {
            return await _karapinhaXptoDbContext.Profissionals
                                 .Include(p => p.Horarios)
                                 .Include(p => p.ProfissionalCategorias)
                                 .ThenInclude(ps => ps.Categoria)
                                 .Where(p => p.Nome.Contains(searchCriteria) || p.Email.Contains(searchCriteria))
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Horario>> GetHorariosByIds(IEnumerable<int> horariosId)
        {
            return await _karapinhaXptoDbContext.Horarios
                                 .Where(h => horariosId.Contains(h.Id))
                                 .ToListAsync();
        }

        public  IEnumerable<Profissional> GetAllProfissionais()
        {
            return _karapinhaXptoDbContext.Profissionals
                .Include(p => p.Horarios)
                .ThenInclude(h => h.Horario)
                .Include(p => p.ProfissionalCategorias)
                .ThenInclude(pc => pc.Categoria)
                .ToList();
        }

        public Profissional GetProfissionalById(int id)
        {
            return _karapinhaXptoDbContext.Profissionals.FirstOrDefault(p => p.Id == id);
        }

        public void Delete(Profissional profissional)
        {
            _karapinhaXptoDbContext.Profissionals.Remove(profissional);
        }

        public void SaveChanges()
        {
            _karapinhaXptoDbContext.SaveChanges();
        }



    }

}

