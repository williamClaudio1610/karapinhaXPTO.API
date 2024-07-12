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
    public class ServicoController : ControllerBase
    {
        private readonly IServicoServices _servicoService;

        public ServicoController(IServicoServices servicoService)
        {
            _servicoService = servicoService ?? throw new ArgumentNullException(nameof(servicoService));

        }

        [HttpPost("registrarServicos")]
        public async Task<IActionResult> RegisterServicoAsync([FromForm] ServicoDTO servicoDTO)
        {

            var result = await _servicoService.RegisterServicoAsync(servicoDTO);

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("listarServicos")]
        public async Task<IActionResult> GetAllServicos()
        {
            var servicos = await _servicoService.GetAllServicosAsync();
            return Ok(servicos);
        }

        [HttpDelete("eliminarServico/{id}")]
        public async Task<IActionResult> EliminarServico(int id)
        {
            var resultado = await _servicoService.EliminarServicoAsync(id);
            if (resultado.Success)
            {
                return Ok(resultado);
            }
            else
            {
                return BadRequest(resultado); 
            }
        }

        [HttpPut("atualizarServico")]
        public async Task<IActionResult> AtualizarServico([FromForm] ServicoDTOActulizar servicoAtualizado)
        {
            var resultado = await _servicoService.AtualizarServicoAsync(servicoAtualizado, Request.Form.Files["imagem"]);

            if (resultado == null)
            {
                return NotFound("Serviço não encontrado.");
            }

            return Ok(resultado);
        }

    }
}
