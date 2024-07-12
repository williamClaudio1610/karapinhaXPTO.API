using KarapinhaXpto.Model;
using KarapinhaXpto.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.DAL.Repositories
{
    public class CategoriaRepositorio<T> : IGeralRepositorio<T> where T : class
    {

        private readonly KarapinhaXptoDbContext _karapinhaXptoDbContext;

        public CategoriaRepositorio(KarapinhaXptoDbContext karapinhaXptoDbContext)
        {
            _karapinhaXptoDbContext = karapinhaXptoDbContext;
        }

        public async Task <bool> Actualizar(T entity)
        {

            _karapinhaXptoDbContext.Entry(entity).State = EntityState.Modified;
            return await _karapinhaXptoDbContext.SaveChangesAsync() >0;
        }

        public async Task <bool> Eliminar(T entity)
        {

            _karapinhaXptoDbContext.Set<T>().Remove(entity);
            return await _karapinhaXptoDbContext.SaveChangesAsync() > 0;


        }

        public async Task< List<T>> Listar()
        {

            return await _karapinhaXptoDbContext.Set<T>().ToListAsync();    
        }

        public async Task <T> ListarPorId(int id)
        {
            return await _karapinhaXptoDbContext.Set<T>().FindAsync(id);


        }

        public async Task<Categoria> GetCategoriaByNomeAsync(string nome)
        {
            return await _karapinhaXptoDbContext.Categorias.FirstOrDefaultAsync(c => c.Nome == nome);
        }
        public async Task AddCategoriaAsync(Categoria categoria)
        {
            await _karapinhaXptoDbContext.Categorias.AddAsync(categoria);
            await _karapinhaXptoDbContext.SaveChangesAsync();
        }
    }
}
