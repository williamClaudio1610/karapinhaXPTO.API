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
    public class MarcacaoController : ControllerBase
    {
        private readonly IMarcacoesServices _marcacaoService;
        public MarcacaoController(IMarcacoesServices marcacaoService)
        {
            _marcacaoService = marcacaoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MarcacaoDTO>>> GetAll()
        {
            var marcacoes = await _marcacaoService.GetAllMarcacoesAsync();
            return Ok(marcacoes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MarcacaoDTO>> GetById(int id)
        {
            var marcacao = await _marcacaoService.GetMarcacaoByIdAsync(id);
            if (marcacao == null) return NotFound();

            return Ok(marcacao);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] MarcacaoDTO marcacaoDto)
        {
            // 1. Validar o modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // 2. Adicionar a marcação (supondo que `AddMarcacaoAsync` retorne a entidade com o Id preenchido)
                await _marcacaoService.AddMarcacaoAsync(marcacaoDto);

                // 3. Retornar CreatedAtAction com o ID da nova marcação
                return CreatedAtAction(nameof(GetById), new { id = marcacaoDto.Id }, marcacaoDto);
            }
            catch (Exception ex)
            {
                // 4. Logar o erro (caso tenha um serviço de logging configurado)
                // _logger.LogError(ex, "Erro ao criar a marcação.");

                // 5. Retornar um status de erro genérico (ajustar conforme necessário)
                return StatusCode(500, "Um erro ocorreu enquanto a marcação estava sendo criada. Por favor, tente novamente.");
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, MarcacaoDTO marcacaoDto)
        {
            if (id != marcacaoDto.Id) return BadRequest();

            await _marcacaoService.UpdateMarcacaoAsync(marcacaoDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _marcacaoService.DeleteMarcacaoAsync(id);
                return Ok(new { mensagem = "Marcação excluída com sucesso." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { erro = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = "Ocorreu um erro ao excluir a entidade.", detalhes = ex.Message });
            }
        }

        [HttpPost("{id}/confirmar")]
        public async Task<IActionResult> ConfirmarMarcacao(int id)
        {
            var result = await _marcacaoService.ConfirmarMarcacaoAsync(id);
            if (result)
            {
                return Ok(new { message = "Marcação confirmada com sucesso." });
            }
            return BadRequest(new { message = "Falha ao confirmar a marcação." });
        }

        [HttpPut("atualizar")]
        public async Task<IActionResult> AtualizarServicoMarcacao([FromBody] ServicoMarcacaoUpdateDTO dto)
        {
            var resultado = await _marcacaoService.AtualizarServicoMarcacaoAsync(dto);

            if (!resultado)
            {
                return NotFound(new { mensagem = "Serviço de marcação não encontrado." });
            }

            return Ok(new { mensagem = "Serviço de marcação atualizado com sucesso." });
        }
    }

}

