using KarapinhaXpto.DTO;
using KarapinhaXpto.Model;
using KarapinhaXpto.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.Shared.IRepositories
{
    public interface IProfissionaisRepositorio
    {

        Task AddProfissionalAsync(Profissional profissional);
        Task<Profissional> GetProfissionalByBiAsync(string id);
        Task<Profissional> GetProfissionalByEmailAsync(string email);
        Task UpdateProfissionalAsync(Profissional profissional);
        Task DeleteProfissionalAsync(Profissional profissional);
        Task<IEnumerable<Profissional>> GetAllProfissionaisAsync();
        Task<IEnumerable<Profissional>> SearchProfissionaisAsync(string searchCriteria);
        Task<IEnumerable<Horario>> GetHorariosByIds(IEnumerable<int> horariosId);
        IEnumerable<Profissional> GetAllProfissionais();

        Profissional GetProfissionalById(int id);
        void Delete(Profissional profissional);
        void SaveChanges();


    }
}

