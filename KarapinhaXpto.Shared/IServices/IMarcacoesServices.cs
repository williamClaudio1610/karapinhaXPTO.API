using KarapinhaXpto.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.Shared.IServices
{
    public interface IMarcacoesServices
    {
        Task<bool> ConfirmarMarcacaoAsync(int id);

        Task<IEnumerable<MarcacaoDTO>> GetAllMarcacoesAsync();
        Task<MarcacaoDTO> GetMarcacaoByIdAsync(int id);
        Task AddMarcacaoAsync(MarcacaoDTO marcacaoDto);
        Task UpdateMarcacaoAsync(MarcacaoDTO marcacaoDto);
        Task DeleteMarcacaoAsync(int id);
        double CalcularTotalPreco(IEnumerable<ServicoMarcacaoDTO> servicosMarcados);
        Task<bool> AtualizarServicoMarcacaoAsync(ServicoMarcacaoUpdateDTO dto);





    }
}
