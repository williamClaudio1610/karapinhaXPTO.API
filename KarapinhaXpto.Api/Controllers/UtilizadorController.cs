using KarapinhaXpto.DTO;
using KarapinhaXpto.Model;
using KarapinhaXpto.Services;
using KarapinhaXpto.Shared.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KarapinhaXpto.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilizadorController : ControllerBase
    {
        private readonly IUtilizadorServices _utilizadorServices;
        private readonly ILogger<UtilizadorController> _logger; 

        public UtilizadorController(IUtilizadorServices utilizadorServices, ILogger<UtilizadorController> logger)
        {
            _utilizadorServices = utilizadorServices ?? throw new ArgumentNullException(nameof(utilizadorServices));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); // Validando o logger

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] UtilizadorRegistradoDTO utilizadorRegistradoDTO)
        {
            var result = await _utilizadorServices.RegisterUserAsync(utilizadorRegistradoDTO);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("registerAdministrativo")]
        public async Task<IActionResult> RegisterAdministrativo([FromForm] UtilizadorRegistradoDTO utilizadorRegistradoDTO)
        {
            var result = await _utilizadorServices.RegisterAdministrativoAsync(utilizadorRegistradoDTO);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UtilizadorLoginDTO loginDto)
        {
            if (loginDto == null || string.IsNullOrEmpty(loginDto.username) || string.IsNullOrEmpty(loginDto.Password))
            {
                _logger.LogWarning("Tentativa de login com dados de entrada inválidos.");
                return BadRequest(new { Message = "Username e senha são obrigatórios." });
            }

            try
            {
                var userResponse = await _utilizadorServices.LoginUserAsync(loginDto);

                if (userResponse == null)
                {
                    _logger.LogWarning("Tentativa de login com credenciais inválidas para o usuário: {Username}", loginDto.username);
                    return Unauthorized(new { Message = "Credenciais inválidas." });
                }

                _logger.LogInformation("Usuário {Username} logado com sucesso.", loginDto.username);
                return Ok(userResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao tentar autenticar usuário: {Username}", loginDto.username);
                return StatusCode(500, new { Message = "Ocorreu um erro ao processar sua solicitação." });
            }
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _utilizadorServices.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost("toggle-activation/{userId}")]
        public async Task<IActionResult> ToggleActivation(int userId)
        {
            await _utilizadorServices.ToggleActivationAsync(userId);
            return Ok(new { message = "User activation status toggled successfully" });
        }

    

        [HttpPost("alterar-senha")]
        public async Task<IActionResult> AlterarSenha([FromBody] AlterarSenhaDTO alterarSenhaDTO)
        {
            var response = await _utilizadorServices.AlterarSenhaAsync(alterarSenhaDTO);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
        [HttpPut("atualizarUtilizador")]
        public async Task<IActionResult> AtualizarUtilizador([FromForm] UtilizadorAtualizadoDTO utilizadorAtualizado)
        {
            // Obter a foto do formulário
            var foto = Request.Form.Files["foto"]; // Certifique-se de que a chave "foto" corresponde ao nome usado no formulário de envio

            // Chamar o serviço para atualizar o utilizador
            var response = await _utilizadorServices.AtualizarUtilizadorAsync(utilizadorAtualizado, foto);

            if (response.Success)
            {
                // Se a operação não for bem-sucedida, retornar um BadRequest com a mensagem apropriada
                return Ok(response);
            }

            // Se a operação for bem-sucedida, retornar o utilizador atualizado
            return Ok(response);

        }


    }
}
