using KarapinhaXpto.DTO;
using KarapinhaXpto.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.Shared.IServices
{
    public interface IProfissionaisServices
    {
        Task<ServiceResponse> RegisterProfissionalAsync(ProfissionalDTO profissionalDTO);
        IEnumerable<ProfissionalDTLISTAR> GetAllProfissionais();

        ServiceResponse DeleteProfissional(int profissionalId, bool forceDelete = false);

    }
}
