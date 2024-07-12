using KarapinhaXpto.DTO;
using KarapinhaXpto.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KarapinhaXpto.Services;
using KarapinhaXpto.Shared.IServices;

namespace KarapinhaXpto.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HorarioController : ControllerBase
    {
        private readonly IHorarioService _horarioService;

        public HorarioController(HorarioService horarioService)
        {
            _horarioService = horarioService;
        }

        [HttpGet("todosHorarios")]
        public IActionResult ObterTodosHorarios()
        {
            List<HorarioDTO> horarios = _horarioService.ObterTodosHorarios();
            return Ok(horarios);
        }
    }
}
