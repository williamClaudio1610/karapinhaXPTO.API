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
    public class UtilizadorRepositorio : IUtilizadorRepositorio
    {


        private readonly KarapinhaXptoDbContext _karapinhaXptoDbContext;

        public UtilizadorRepositorio(KarapinhaXptoDbContext context)
        {
            _karapinhaXptoDbContext = context;
        }

        public async Task AddUserAsync(Utilizador utilizador)
        {
            await _karapinhaXptoDbContext.Utilizadors.AddAsync(utilizador);
            await _karapinhaXptoDbContext.SaveChangesAsync();
        }

        public async Task<Utilizador> GetUserByEmailAsync(string email)
        {
            return await _karapinhaXptoDbContext.Utilizadors.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Utilizador> GetUserByBiAsync(string bi)
        {
            return await _karapinhaXptoDbContext.Utilizadors.FirstOrDefaultAsync(u => u.Bi == bi);
        }

        public async Task<Utilizador> GetUserByUsernameAsync(string username)
        {
            return await _karapinhaXptoDbContext.Utilizadors.FirstOrDefaultAsync(u => u.UserName == username);
        }
        public async Task SaveChangesAsync()
        {
            await _karapinhaXptoDbContext.SaveChangesAsync();
        }
        public async Task<Utilizador> GetUserByIdAsync(int userId)
        {
            return await _karapinhaXptoDbContext.Utilizadors.FindAsync(userId);
        }
        // Novo método para obter todos os usuários
        public async Task<IEnumerable<Utilizador>> GetAllUsersAsync()
        {
            return await _karapinhaXptoDbContext.Utilizadors.ToListAsync();
        }

        public async Task ToggleActivationAsync(int userId)
        {
            var user = await GetUserByIdAsync(userId);
            if (user != null)
            {
                user.Status = !user.Status; // Alternar o estado de ativação
                _karapinhaXptoDbContext.Utilizadors.Update(user);
                await _karapinhaXptoDbContext.SaveChangesAsync();
            }
        }


        public async Task UpdateUser(Utilizador user)
        {
            _karapinhaXptoDbContext.Entry(user).State = EntityState.Modified;
            await _karapinhaXptoDbContext.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(Utilizador utilizador)
        {

            _karapinhaXptoDbContext.Utilizadors.Update(utilizador);
            await _karapinhaXptoDbContext.SaveChangesAsync();
        }


        public async Task<Utilizador> GetByBiExcludingIdAsync(string bi, int id)
        {
            return await _karapinhaXptoDbContext.Utilizadors.SingleOrDefaultAsync(u => u.Bi == bi && u.Id != id);
        }

        public async Task<Utilizador> GetByEmailExcludingIdAsync(string email, int id)
        {
            return await _karapinhaXptoDbContext.Utilizadors.SingleOrDefaultAsync(u => u.Email == email && u.Id != id);
        }

        public async Task<Utilizador> GetByUsernameExcludingIdAsync(string username, int id)
        {
            return await _karapinhaXptoDbContext.Utilizadors.SingleOrDefaultAsync(u => u.UserName == username && u.Id != id);
        }



    }
}
