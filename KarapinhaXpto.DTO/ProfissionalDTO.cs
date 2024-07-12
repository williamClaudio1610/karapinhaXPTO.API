using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.DTO
{
    public class ProfissionalDTO
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O email informado não é válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O BI é obrigatório.")]
        public string BI { get; set; }

        [Required(ErrorMessage = "O telemóvel é obrigatório.")]
        public string Telemovel { get; set; }

        public IFormFile Foto { get; set; }

        [Required(ErrorMessage = "Os horários são obrigatórios.")]
        public int[] HorariosId { get; set; }

        [Required(ErrorMessage = "Os serviços são obrigatórios.")]
        public int[] CategoriasId { get; set; }
    }
    public class ProfissionalDTLISTAR
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Foto { get; set; }
        public string BI { get; set; }
        public string Telemovel { get; set; }
        public ICollection<ProfissionalHorarioDTO> Horarios { get; set; }
        public ICollection<ProfissionalCategoriaDTO> ProfissionalCategorias { get; set; }
    }
    public class ProfissionalHorarioDTO
    {
        public int Id { get; set; }
        public int HorarioID { get; set; }
        public HorarioDTO Horario { get; set; }
    }

   
    public class ProfissionalCategoriaDTO
    {
        public int ProfissionalId { get; set; }
        public int CategoriaId { get; set; }
        public CategoriaDTO Categoria { get; set; }
    }

    public class CategoriaDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }

}
