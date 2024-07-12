using KarapinhaXpto.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.Shared.IRepositories
{
    public interface IServicoRepositorio
    {

        Task<Servico> GetServicoByNomeAsync(string nome);
        Task<Categoria> GetCategoriaByIdAsync(int categoriaId);
        Task AddServicoAsync(Servico servico);

        Task<Servico> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<Servico>> GetAllServicosAsync();

        Task UpdateAsync(Servico servico);

    }
}
