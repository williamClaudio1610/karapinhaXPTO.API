using KarapinhaXpto.DTO;
using KarapinhaXpto.Shared.IRepositories;
using KarapinhaXpto.Shared.IServices;
using KarapinhaXpto.Shared.Responses;
using Microsoft.AspNetCore.Hosting;
using KarapinhaXpto.Model;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using Microsoft.AspNetCore.Identity;
using KarapinhaXpto.DAL;
using Microsoft.AspNetCore.Http;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;


namespace KarapinhaXpto.Services
{
    public class UtilizadorService : IUtilizadorServices
    {
        private readonly IUtilizadorRepositorio _utilizadorRepositorio;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly EmailService _emailService;
        private readonly IPasswordHasher<Utilizador> _passwordHasher;
        private readonly ITokenServices _tokenService;

        public UtilizadorService(IUtilizadorRepositorio utilizadorRepositorio, IWebHostEnvironment webHostEnvironment, EmailService emailService, IPasswordHasher<Utilizador> passwordHasher, ITokenServices tokenService)
        {
            _utilizadorRepositorio = utilizadorRepositorio ?? throw new ArgumentNullException(nameof(utilizadorRepositorio));
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _passwordHasher = passwordHasher;
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));


        }

        public async Task<ServiceResponse> RegisterUserAsync(UtilizadorRegistradoDTO utilizadorRegistradoDTO)
        {
            if (utilizadorRegistradoDTO == null)
            {
                throw new ArgumentNullException(nameof(utilizadorRegistradoDTO));
            }

            
              // Verificar se o email já está em uso
            var existingUserByEmail = await _utilizadorRepositorio.GetUserByEmailAsync(utilizadorRegistradoDTO.Email);
            if (existingUserByEmail != null)
            {
                return new ServiceResponse { Success = false, Message = "Email já está em uso." };
            }

            // Verificar se o nome de usuário já está em uso
            var existingUserByUsernmae = await _utilizadorRepositorio.GetUserByUsernameAsync(utilizadorRegistradoDTO.Username);
            if (existingUserByUsernmae != null)
            {
                return new ServiceResponse { Success = false, Message = "Nome de Usuário já está em uso." };
            }

            // Verificar se o BI já está em uso
            var existingUserByBi = await _utilizadorRepositorio.GetUserByBiAsync(utilizadorRegistradoDTO.Bi);
            if (existingUserByBi != null)
            {
                return new ServiceResponse { Success = false, Message = "Número de BI já está em uso." };
            }

            var utilizador = new Utilizador
            {
                NomeCompleto = utilizadorRegistradoDTO.NomeCompleto,
                Email = utilizadorRegistradoDTO.Email,
                Telefone = utilizadorRegistradoDTO.Telefone,
                Bi = utilizadorRegistradoDTO.Bi,
                UserName = utilizadorRegistradoDTO.Username,
                Status = false,
                Foto = "default/path/to/photo.jpg", // Valor padrão para a foto
                Role = "Registado"
            };

            //senha encriptada 
			// utilizador.Password = _passwordHasher.HashPassword(utilizador, utilizadorRegistradoDTO.Password);
			utilizador.Password = utilizadorRegistradoDTO.Password;

			if (utilizadorRegistradoDTO.Foto != null)
            {
                // Define o caminho completo para a pasta Imagens
                var imagesFolderPath = Path.Combine("C:\\Users\\Admin\\Documents\\ISPTEC - Universidade\\ISPTEC- 3º ano - 2023-2024\\2º Semestre\\Aplicações Web (AW)\\AAA_PROJECTO_FINAL_ KARAPINHA_XPTO\\Karapinha-Xpto\\src\\assets\\images\\Utilizador");

				// Garante que o diretório de imagens existe
				if (!Directory.Exists(imagesFolderPath))
                {
                    Directory.CreateDirectory(imagesFolderPath);
                }

                var filePath = Path.Combine(imagesFolderPath, utilizadorRegistradoDTO.Foto.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await utilizadorRegistradoDTO.Foto.CopyToAsync(stream);
                }
                utilizador.Foto = Path.Combine("images\\Utilizador", utilizadorRegistradoDTO.Foto.FileName); // Caminho relativo salvo no banco de dados
            }

            await _utilizadorRepositorio.AddUserAsync(utilizador);
            // Enviar notificação por email ao administrador
            await _emailService.SendNewUserNotificationAsync(utilizador.Email);

            return new ServiceResponse { Success = true, Message = "Usuário registrado com sucesso Aguarde a ativação da sua conta." };
        }


        public async Task<ServiceResponseLogin<UserResponseDTO>> LoginUserAsync(UtilizadorLoginDTO loginDto)
        {
            if (loginDto == null)
            {
                throw new ArgumentNullException(nameof(loginDto));
            }

            // Buscar usuário pelo nome de usuário
            var user = await _utilizadorRepositorio.GetUserByUsernameAsync(loginDto.username);
            if (user == null)
            {
                return new ServiceResponseLogin<UserResponseDTO> { Success = false, Message = "Usuário não encontrado." };
            }

            // Verificar se a senha está correta
            // var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, loginDto.Password);
            var senhaLogin = loginDto.Password;
            var senhaBaseDados = user.Password;
			if (senhaLogin != senhaBaseDados)
            {
                return new ServiceResponseLogin<UserResponseDTO> { Success = false, Message = "Senha incorreta." };
            }

           

          

            // Gerar token JWT
            var token = _tokenService.GenerateToken(user);

            // Verificar se a conta do usuário está ativa ou se precisa alterar a senha
            if (user.Role == "Administrativo")
            {
                if (!user.Status)
                {
                    return new ServiceResponseLogin<UserResponseDTO>
                    {
                        Success = true,
                        Message = "Primeiro login detectado. Por favor, altere sua senha.",
                        Data = new UserResponseDTO { Token = token }, 
                        RequiresPasswordChange = true // Indica que a senha precisa ser alterada
                    };
                }
            }
          
                // Verificar se a conta do usuário está ativa
                if (!user.Status)
                {
                    return new ServiceResponseLogin<UserResponseDTO> { Success = false, Message = "Conta não ativada. Por favor, contacte o administrador." };
                }

            



            // Retornar os dados básicos do usuário e o token JWT
            return new ServiceResponseLogin<UserResponseDTO>
                {
                    Success = true,
                    Message = "Login bem-sucedido.",
                    Data = new UserResponseDTO
                    {
                        Token = token
                    }
                };


            

          
        }

        // Novo método para obter todos os usuários
        public async Task<IEnumerable<Utilizador>> GetAllUsersAsync()
        {
            return await _utilizadorRepositorio.GetAllUsersAsync();
        }

        public async Task ToggleActivationAsync(int userId)
        {
            var user = await _utilizadorRepositorio.GetUserByIdAsync(userId);
            if (user != null)
            {
                user.Status = !user.Status; // Alternar o estado de ativação
                await _utilizadorRepositorio.SaveChangesAsync();

                var status = user.Status ? "Atividado" : "desactivado";
                await _emailService.SendEmailAsync(user.Email, $"Karapinha-XPTO: Conta {status}", $"Sua conta da Karapinha-XPTO foi {status}.");
            }
        }

       


        

        // Registrar Adminstrativo
        public async Task<ServiceResponse> RegisterAdministrativoAsync(UtilizadorRegistradoDTO utilizadorRegistradoDTO)
        {
            if (utilizadorRegistradoDTO == null)
            {
                throw new ArgumentNullException(nameof(utilizadorRegistradoDTO));
            }


            // Verificar se o email já está em uso
            var existingUserByEmail = await _utilizadorRepositorio.GetUserByEmailAsync(utilizadorRegistradoDTO.Email);
            if (existingUserByEmail != null)
            {
                return new ServiceResponse { Success = false, Message = "Email já está em uso." };
            }

            // Verificar se o nome de usuário já está em uso
            var existingUserByUsernmae = await _utilizadorRepositorio.GetUserByUsernameAsync(utilizadorRegistradoDTO.Username);
            if (existingUserByUsernmae != null)
            {
                return new ServiceResponse { Success = false, Message = "Nome de Usuário já está em uso." };
            }

            // Verificar se o BI já está em uso
            var existingUserByBi = await _utilizadorRepositorio.GetUserByBiAsync(utilizadorRegistradoDTO.Bi);
            if (existingUserByBi != null)
            {
                return new ServiceResponse { Success = false, Message = "Número de BI já está em uso." };
            }

            var utilizador = new Utilizador
            {
                NomeCompleto = utilizadorRegistradoDTO.NomeCompleto,
                Email = utilizadorRegistradoDTO.Email,
                Telefone = utilizadorRegistradoDTO.Telefone,
                Bi = utilizadorRegistradoDTO.Bi,
                UserName = utilizadorRegistradoDTO.Username,
                Status = false, // Só fica verdadeiro quando ele altera a senha 
                Foto = "default/path/to/photo.jpg", // Valor padrão para a foto
                Role = "Administrativo"
            };

            // utilizador.Password = _passwordHasher.HashPassword(utilizador, utilizadorRegistradoDTO.Password);
			utilizador.Password = utilizadorRegistradoDTO.Password;

			if (utilizadorRegistradoDTO.Foto != null)
            {
                // Define o caminho completo para a pasta Imagens
                var imagesFolderPath = Path.Combine("C:\\Users\\Admin\\Documents\\ISPTEC - Universidade\\ISPTEC- 3º ano - 2023-2024\\2º Semestre\\Aplicações Web (AW)\\AAA_PROJECTO_FINAL_ KARAPINHA_XPTO\\Karapinha-Xpto\\src\\assets\\images\\Utilizador");

				// Garante que o diretório de imagens existe
				if (!Directory.Exists(imagesFolderPath))
                {
                    Directory.CreateDirectory(imagesFolderPath);
                }

                var filePath = Path.Combine(imagesFolderPath, utilizadorRegistradoDTO.Foto.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await utilizadorRegistradoDTO.Foto.CopyToAsync(stream);
                }
                utilizador.Foto = Path.Combine("images\\Utilizador", utilizadorRegistradoDTO.Foto.FileName); // Caminho relativo salvo no banco de dados
            }

            await _utilizadorRepositorio.AddUserAsync(utilizador);
      
          
            // Enviar email com os dados de autenticação
            var emailBody = $"Olá {utilizador.NomeCompleto},\n\n" +
                            $"Sua conta no Salão Karapinha XPTO foi criada com sucesso, para acessar:\n" +
                            $"Username: {utilizador.UserName}\n" +
                            $"Password: {utilizadorRegistradoDTO.Password}\n\n" +
                            "Por favor, altere sua senha no primeiro login.";

            await _emailService.SendEmailAsync(utilizador.Email, "Dados de Autenticação", emailBody);


            return new ServiceResponse { Success = true, Message = "Administrativo registrado com sucesso conta Ativada." };
        }


        public async Task<ServiceResponse> AlterarSenhaAsync(AlterarSenhaDTO alterarSenhaDTO)
        {
            var utilizador = await _utilizadorRepositorio.GetUserByIdAsync(alterarSenhaDTO.UserId);
            if (utilizador == null)
            {
                return new ServiceResponse { Success = false, Message = "Usuário não encontrado." };
            }

            // Opcional: Verificar a senha antiga antes de permitir a mudança
            if (!string.IsNullOrEmpty(alterarSenhaDTO.SenhaAntiga))
            {
				//var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(utilizador, utilizador.Password, alterarSenhaDTO.SenhaAntiga);

				var senhaLogin = utilizador.Password;
				var senhaBaseDados = alterarSenhaDTO.SenhaAntiga;

				if (senhaLogin != senhaBaseDados)
                {
                    return new ServiceResponse { Success = false, Message = "Senha antiga está incorreta." };
                }
            }

            utilizador.Password = alterarSenhaDTO.NovaSenha;

			// Se o usuário for "Administrativo", ativar a conta após a alteração da senha
			if (utilizador.Role == "Administrativo")
            {
                utilizador.Status = true;
            }

            await _utilizadorRepositorio.UpdateUserAsync(utilizador);

            return new ServiceResponse { Success = true, Message = "Senha alterada com sucesso." };
        }

        public async Task<ServiceResponse<Utilizador>> AtualizarUtilizadorAsync(UtilizadorAtualizadoDTO utilizadorAtualizado, IFormFile foto)
        {
            var response = new ServiceResponse<Utilizador>();
            try
            {
                var utilizadorExistente = await _utilizadorRepositorio.GetUserByIdAsync(utilizadorAtualizado.Id);

                if (utilizadorExistente == null)
                {
                    response.Success = false;
                    response.Message = "Utilizador não encontrado.";
                    return response;
                }

                // Verificar duplicidade de BI, excluindo o próprio utilizador
                if (!string.IsNullOrEmpty(utilizadorAtualizado.Bi) && utilizadorAtualizado.Bi != utilizadorExistente.Bi)
                {
                    var biExistente = await _utilizadorRepositorio.GetByBiExcludingIdAsync(utilizadorAtualizado.Bi, utilizadorExistente.Id);
                    if (biExistente != null)
                    {
                        response.Success = false;
                        response.Message = "Nº de BI já está se encontra em uso.";
                        return response;
                    }
                    string biPattern = @"^\d{9}[A-Za-z0-9]{2}\d{3}$";
                    if (!System.Text.RegularExpressions.Regex.IsMatch(utilizadorAtualizado.Bi, biPattern))
                    {
                        response.Success = false;
                        response.Message = "Nº de BI inválido";
                        return response;

                    }
                    utilizadorExistente.Bi = utilizadorAtualizado.Bi;
                }

                // Verificar duplicidade de Email, excluindo o próprio utilizador
                if (!string.IsNullOrEmpty(utilizadorAtualizado.Email) && utilizadorAtualizado.Email != utilizadorExistente.Email)
                {
                    var emailExistente = await _utilizadorRepositorio.GetByEmailExcludingIdAsync(utilizadorAtualizado.Email, utilizadorExistente.Id);
                    if (emailExistente != null)
                    {
                        response.Success = false;
                        response.Message = "O endereço de email já está em uso por outro utilizador.";
                        return response;
                    }
                    utilizadorExistente.Email = utilizadorAtualizado.Email;
                }

                // Verificar duplicidade de Username, excluindo o próprio utilizador
                if (!string.IsNullOrEmpty(utilizadorAtualizado.Username) && utilizadorAtualizado.Username != utilizadorExistente.UserName)
                {
                    var usernameExistente = await _utilizadorRepositorio.GetByUsernameExcludingIdAsync(utilizadorAtualizado.Username, utilizadorExistente.Id);
                    if (usernameExistente != null)
                    {
                        response.Success = false;
                        response.Message = "Username já em uso";
                        return response;
                    }
                    utilizadorExistente.UserName = utilizadorAtualizado.Username;
                }

                if (!string.IsNullOrEmpty(utilizadorAtualizado.NomeCompleto))
                {
                    utilizadorExistente.NomeCompleto = utilizadorAtualizado.NomeCompleto;
                }

                if (!string.IsNullOrEmpty(utilizadorAtualizado.Telefone))
                {
                    if (Regex.IsMatch(utilizadorAtualizado.Telefone, @"^\d{9}$"))
                    {
                        utilizadorExistente.Telefone = utilizadorAtualizado.Telefone;
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Número de telefone deve ter exatamente 9 dígitos.";
                        return response;

                    }
                }

                if (!string.IsNullOrEmpty(utilizadorAtualizado.Password))
                {

                    if (utilizadorAtualizado.Password == utilizadorAtualizado.ConfirmPassword) {
                        utilizadorExistente.Password = _passwordHasher.HashPassword(utilizadorExistente, utilizadorAtualizado.Password);
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "O Confirmar Senha não coincide com a nova senha";
                        return response;
                    }
                }

                if (foto != null)
                {
                    utilizadorExistente.Foto = Path.Combine("images\\Utilizador", await SalvarFotoAsync(foto));
                }

                await _utilizadorRepositorio.UpdateUserAsync(utilizadorExistente);

                response.Success = true;
                response.Message = "Dados atualizados com sucesso.";
                response.Data = utilizadorExistente;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Erro ao atualizar os dados do utilizador: {ex.Message}";
            }

            return response;
        }





        private async Task<string> SalvarFotoAsync(IFormFile foto)
            {
            /*
                    if (foto == null || foto.Length == 0)
                    {
                        throw new ArgumentException("Imagem inválida.");
                    }
            */
                    // Define o caminho completo para a pasta Imagens
                    var imagesFolderPath = Path.Combine("C:\\Users\\Admin\\Documents\\ISPTEC - Universidade\\ISPTEC- 3º ano - 2023-2024\\2º Semestre\\Aplicações Web (AW)\\AAA_PROJECTO_FINAL_ KARAPINHA_XPTO\\Karapinha-Xpto\\src\\assets\\images\\Utilizador");

			// Garante que o diretório de imagens existe
			if (!Directory.Exists(imagesFolderPath))
                    {
                        Directory.CreateDirectory(imagesFolderPath);
                    }

                    // Gera um nome de arquivo único
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(foto.FileName);
                    var filePath = Path.Combine(imagesFolderPath, fileName);

                    // Salva o arquivo
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await foto.CopyToAsync(stream);
                    }

                    // Retorna o caminho relativo do arquivo
                    return fileName;
            }

        /*
            private string HashPassword(string password)
            {

                return BCrypt.Net.BCrypt.HashPassword(password);
            }
        */




    }
}
