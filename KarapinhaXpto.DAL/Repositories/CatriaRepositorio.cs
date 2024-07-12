using KarapinhaXpto.Model;
using KarapinhaXpto.Shared.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.DAL.Repositories
{
    public class CatriaRepositorio : CategoriaRepositorio<Categoria>, ICategoriaRepositorio
    {
        public CatriaRepositorio(KarapinhaXptoDbContext karapinhaXptoDbContext) : base(karapinhaXptoDbContext)
        {
        }
    }
}
