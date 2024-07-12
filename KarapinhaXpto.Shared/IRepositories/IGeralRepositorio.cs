using KarapinhaXpto.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.Shared.Repositories
{
    public interface IGeralRepositorio<T>
    {

     Task <T> ListarPorId(int id);

     Task<Categoria> GetCategoriaByNomeAsync(string nome);
     Task<bool>    Eliminar(T entity);

     Task<bool>   Actualizar (T entity);

    Task <List<T> > Listar();    
        


    }
}
