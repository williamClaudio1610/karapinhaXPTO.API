using KarapinhaXpto.DTO;
using KarapinhaXpto.Model;
using KarapinhaXpto.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.Shared.IServices
{
    public interface ICategoriaServices
    {

        Task<Categoria> ListarPorId(int id);

        Task<List<Categoria>> ListarTodos();

        Task<bool> Actualizar(CategoriaUpdateDTO categoriaUpdateDTO);

       Task<ServiceResponse> RegisterCategoriaAsync(CategoriaAddDTO categoriaAddDTO);
        Task<ServiceResponse> Delete(int id);



    }
}
