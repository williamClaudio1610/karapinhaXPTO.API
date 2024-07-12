using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.DTO
{
    public class CategoriaAddDTO
    {
        public string Nome { get; set; }

        public String? Descricao { get; set; }

        public IFormFile Foto { get; set; }

    }
}
