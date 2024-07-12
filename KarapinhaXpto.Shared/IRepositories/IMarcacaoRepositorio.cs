using KarapinhaXpto.DTO;
using KarapinhaXpto.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.Shared.IRepositories
{
    public interface IMarcacaoRepositorio<T> where T : class 
    {
        Task<List<Marcacao>> GetAllWithServicosMarcadosAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        IEnumerable<Marcacao> MarcacoesPorProfissional(int profissionalId);
        Task<Marcacao> GetByIdAsyncConfirmar(int id);
        Task UpdateAsync(Marcacao marcacao);
        Task<ServicoMarcacao> GetByIdServicoMarcacaoAsync(int id);
        Task<bool> UpdateServicoMarcacao1Async(ServicoMarcacaoUpdateDTO dto);


       
    }

}
