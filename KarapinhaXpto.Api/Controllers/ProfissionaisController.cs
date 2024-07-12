using KarapinhaXpto.DTO;
using KarapinhaXpto.Model;
using KarapinhaXpto.Services;
using KarapinhaXpto.Shared.IRepositories;
using KarapinhaXpto.Shared.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KarapinhaXpto.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfissionaisController : ControllerBase
    {
        private readonly IProfissionaisServices _profissionaisServices;


        public ProfissionaisController(IProfissionaisServices profissionaisServices)
        {
            _profissionaisServices = profissionaisServices ?? throw new ArgumentNullException(nameof(profissionaisServices));

        }

        [HttpPost("registrarProfissional")]
        public async Task<IActionResult> RegistrarProfissional([FromForm] ProfissionalDTO registroProfissionalDTO)
        {
            var result = await _profissionaisServices.RegisterProfissionalAsync(registroProfissionalDTO);
            if (result.Success)
            {
                return Ok(result);

            }
            return BadRequest(result);

        }

        [HttpGet("listarProfissionais")]
        public ActionResult<IEnumerable<ProfissionalDTO>> GetProfissionais()
        {
            var profissionais = _profissionaisServices.GetAllProfissionais();
            return Ok(profissionais);
        }

        [HttpDelete("eliminarProfissional/{id}")]
        public IActionResult DeleteProfissional(int id, [FromQuery] bool forceDelete = false)
        {
            var response = _profissionaisServices.DeleteProfissional(id, forceDelete);

            if (response.Success)
            {
                return Ok(new { Success = true, Message = response.Message });
            }
            else if (!response.Success && response.Message.Contains("marcações"))
            {
                // Indicar que a exclusão requer confirmação adicional
                return Ok(new { Success = false, Message = response.Message, RequiresConfirmation = true });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Success = false, Message = response.Message });
            }
        }



    }
}
