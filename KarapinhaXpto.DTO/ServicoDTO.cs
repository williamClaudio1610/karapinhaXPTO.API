using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.DTO
{
    public class ServicoDTO
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public IFormFile Imagem { get; set; }
        public int CategoriaId { get; set; }
        public double Preco { get; set; }
    }
    public class ServicoDTOListar
    {
        public int Id { get; set; }
        public string? Nome { get; set; } 
        public string? Descricao { get; set; }
        public string? Imagem { get; set; }
        public int? CategoriaId { get; set; }
        public double? Preco { get; set; }
    }

    public class ServicoDTOActulizar
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public IFormFile? Imagem { get; set; }
        public int? CategoriaId { get; set; }
        public double? Preco { get; set; }
    }
}
