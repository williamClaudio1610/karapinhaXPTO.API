using KarapinhaXpto.DTO;
using KarapinhaXpto.Model;
using KarapinhaXpto.Shared.Responses;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.Shared.IServices
{
    public interface IServicoServices
    {
      Task<ServiceResponse> RegisterServicoAsync(ServicoDTO servicoDTO);
        Task<IEnumerable<ServicoDTOListar>> GetAllServicosAsync();
        Task<ServiceResponse> EliminarServicoAsync(int id);
        Task<Servico> AtualizarServicoAsync(ServicoDTOActulizar servicoAtualizado, IFormFile imagem);


    }
}
