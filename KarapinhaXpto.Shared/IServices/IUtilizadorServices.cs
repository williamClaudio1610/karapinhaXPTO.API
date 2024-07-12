using KarapinhaXpto.DTO;
using KarapinhaXpto.Model;
using KarapinhaXpto.Shared.IRepositories;
using KarapinhaXpto.Shared.Responses;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.Shared.IServices
{
    public interface IUtilizadorServices
    {
        Task<ServiceResponse> RegisterUserAsync(UtilizadorRegistradoDTO utilizadorRegistradoDTO);
        Task<ServiceResponse> RegisterAdministrativoAsync(UtilizadorRegistradoDTO utilizadorRegistradoDTO);
        Task<ServiceResponse> AlterarSenhaAsync(AlterarSenhaDTO alterarSenhaDTO);
        Task<ServiceResponse<Utilizador>> AtualizarUtilizadorAsync(UtilizadorAtualizadoDTO utilizadorAtualizado, IFormFile foto);


        Task<ServiceResponseLogin<UserResponseDTO>> LoginUserAsync(UtilizadorLoginDTO loginDto);

        Task<IEnumerable<Utilizador>> GetAllUsersAsync();

        Task ToggleActivationAsync(int userId);

    }
}
