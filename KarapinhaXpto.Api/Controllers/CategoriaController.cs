using KarapinhaXpto.DTO;
using KarapinhaXpto.Services;
using KarapinhaXpto.Shared.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KarapinhaXpto.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaServices _categoriaServices;

        public CategoriaController( ICategoriaServices categoriaServices)
        {
            _categoriaServices = categoriaServices ?? throw new ArgumentNullException(nameof(categoriaServices));

        }

        [HttpGet]
        public async Task<IActionResult> ListarCategorias()
        {

            return Ok(await _categoriaServices.ListarTodos());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ListarCategoriaPorI(int id)
        {

            return Ok(await _categoriaServices.ListarPorId(id));
        }

        [HttpDelete("{id}")]
        public async Task <IActionResult> EliminarCategoria(int id)
        {
            return Ok(await _categoriaServices.Delete(id));
        }



        [HttpPost("registrarCategoria")]
        public async Task<IActionResult> RegisterCategoria([FromForm] CategoriaAddDTO categoriaAddDTO)
        {

            var result = await _categoriaServices.RegisterCategoriaAsync(categoriaAddDTO);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);


        }


        [HttpPut]
        public async Task<IActionResult> ActualizarCategoria(CategoriaUpdateDTO categoriaUpdateDTO)
        {
            return Ok(await _categoriaServices.Actualizar(categoriaUpdateDTO));
        }



    }
}
