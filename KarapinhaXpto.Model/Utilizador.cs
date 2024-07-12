using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.Model
{
    public class Utilizador
    {
        [Key]
        public  int Id { get; set; }

        public string NomeCompleto { get; set; }

        public string Email { get; set; }


        public string Telefone { get; set; }


        public string Foto { get; set; }

        public string Bi { get; set; }
        public string UserName { get; set; }

        public string Password { get; set; }

        public bool Status {  get; set; }=false;

        public string Role { get; set; }  // Pode ser "NaoRegistado", "Registado", "Administrador", "Administrativo"

    }
}
