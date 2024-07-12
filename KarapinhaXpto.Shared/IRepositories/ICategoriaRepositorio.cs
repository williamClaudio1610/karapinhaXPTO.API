using KarapinhaXpto.Model;
using KarapinhaXpto.Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.Shared.IRepositories
{
  public interface ICategoriaRepositorio: IGeralRepositorio<Categoria>    {


        Task AddCategoriaAsync(Categoria categoria);
       

    }
}
