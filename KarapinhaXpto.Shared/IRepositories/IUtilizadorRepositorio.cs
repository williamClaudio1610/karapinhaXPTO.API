using KarapinhaXpto.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.Shared.IRepositories
{
    public interface IUtilizadorRepositorio
    {
        Task AddUserAsync(Utilizador utilizador);
        Task<Utilizador> GetUserByEmailAsync(string email);

        Task<Utilizador> GetUserByBiAsync(string bi);

        Task<Utilizador> GetUserByUsernameAsync(string username);

        Task SaveChangesAsync();

        Task<Utilizador> GetUserByIdAsync(int userId);

        Task<IEnumerable<Utilizador>> GetAllUsersAsync();


        Task UpdateUserAsync(Utilizador utilizador);

        Task<Utilizador> GetByBiExcludingIdAsync(string bi, int id);


        Task<Utilizador> GetByEmailExcludingIdAsync(string email, int id);


        Task<Utilizador> GetByUsernameExcludingIdAsync(string username, int id);
       



    }
}
