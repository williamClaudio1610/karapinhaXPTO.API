using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.DTO
{
    public class UtilizadorLoginDTO
    {
        public string username { get; set; }
        public string Password { get; set; }

    }
    // DTO para a resposta após o login do usuário
    public class UserResponseDTO
    {
       // public int Id { get; set; }
     //   public string FullName { get; set; }
       // public string Email { get; set; }
      //  public string Telefone { get; set; }
      //  public string Username { get; set; }
        public string Token { get; set; } // Token JWT para autenticação
    }



    public class AlterarSenhaDTO
    {
        public int UserId { get; set; }
        public string NovaSenha { get; set; }
        public string SenhaAntiga { get; set; } // Opcional, dependendo da sua lógica de negócio
    }



}
