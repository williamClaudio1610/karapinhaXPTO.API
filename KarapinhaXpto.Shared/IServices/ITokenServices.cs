using KarapinhaXpto.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.Shared.IServices
{
    public interface ITokenServices
    {
         string GenerateToken(Utilizador user);

    }
}
