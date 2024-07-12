using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.Model
{
    public class ProfissionalCategoria
    {
        
            // Chaves estrangeiras para Profissional e Servico
            [Key]
            public int ProfissionalId { get; set; }

            [Key]
            public int CategoriaID { get; set; }

            // Propriedades de navegação
            public Profissional Profissional { get; set; }
            public Categoria Categoria { get; set; }
        
    }
}
