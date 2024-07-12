using KarapinhaXpto.DAL.Repositories;
using KarapinhaXpto.DTO;
using KarapinhaXpto.Model;
using KarapinhaXpto.Shared.IRepositories;
using KarapinhaXpto.Shared.IServices;
using KarapinhaXpto.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging;


namespace KarapinhaXpto.Services
{
    public class ProfissionaisService:IProfissionaisServices
    {
        private readonly ILogger<ProfissionaisService> _logger;
        private readonly IMarcacaoRepositorio<Marcacao> _marcacaoRepository;

        private readonly IProfissionaisRepositorio _profissionaisRepositorio;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProfissionaisService(IProfissionaisRepositorio profissionalRepository, IMarcacaoRepositorio<Marcacao> marcacaoRepository, IWebHostEnvironment webHostEnvironment, ILogger<ProfissionaisService> logger)
        {
            _profissionaisRepositorio = profissionalRepository;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _marcacaoRepository = marcacaoRepository;

        }
        public async Task<ServiceResponse> RegisterProfissionalAsync(ProfissionalDTO profissionalDTO)
        {
            try
            {
                if (profissionalDTO == null)
                {
                    _logger.LogError("O DTO Profissional está nulo.");
                    throw new ArgumentNullException(nameof(profissionalDTO));
                }

                _logger.LogInformation("Iniciando registro do profissional: {@ProfissionalDTO}", profissionalDTO);

                // Verificar se o e-mail já está em uso
                var existingProfissionalByEmail = await _profissionaisRepositorio.GetProfissionalByEmailAsync(profissionalDTO.Email);
                if (existingProfissionalByEmail != null)
                {
                    _logger.LogWarning("O email já está em uso: {Email}", profissionalDTO.Email);
                    return new ServiceResponse { Success = false, Message = "Email já está em uso." };
                }

                // Verificar se o BI já está em uso
                var existingUserByBi = await _profissionaisRepositorio.GetProfissionalByBiAsync(profissionalDTO.BI);
                if (existingUserByBi != null)
                {
                    _logger.LogWarning("O BI já está em uso: {BI}", profissionalDTO.BI);
                    return new ServiceResponse { Success = false, Message = "Número de BI já está em uso." };
                }

                var profissional = new Profissional
                {
                    Nome = profissionalDTO.Nome,
                    Email = profissionalDTO.Email,
                    Foto = "default/path/to/photo.jpg", // Valor padrão para a foto
                    BI = profissionalDTO.BI,
                    Telemovel = profissionalDTO.Telemovel
                };

                // Associar os horários ao profissional usando ProfissionalHorario
                profissional.Horarios = profissionalDTO.HorariosId.Select(horarioId => new ProfissionalHorario
                {
                    ProfissionalID = profissional.Id,
                    HorarioID = horarioId
                }).ToList();

                // Associando os serviços ao profissional
                profissional.ProfissionalCategorias = profissionalDTO.CategoriasId.Select(categoriaId => new ProfissionalCategoria
                {
                    ProfissionalId = profissional.Id,
                    CategoriaID = categoriaId
                }).ToList();

                if (profissionalDTO.Foto != null)
                {
                    // Define o caminho completo para a pasta Imagens
                    var imagesFolderPath = Path.Combine("C:\\Users\\Admin\\Documents\\ISPTEC - Universidade\\ISPTEC- 3º ano - 2023-2024\\2º Semestre\\Aplicações Web (AW)\\AAA_PROJECTO_FINAL_ KARAPINHA_XPTO\\Karapinha-Xpto\\src\\assets\\images\\Profissionais");

                    // Garante que o diretório de imagens existe
                    if (!Directory.Exists(imagesFolderPath))
                    {
                        Directory.CreateDirectory(imagesFolderPath);
                    }

                    var filePath = Path.Combine(imagesFolderPath, profissionalDTO.Foto.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await profissionalDTO.Foto.CopyToAsync(stream);
                    }
                    profissional.Foto = Path.Combine("images\\Profissionais", profissionalDTO.Foto.FileName); // Caminho relativo salvo no banco de dados
                }

                await _profissionaisRepositorio.AddProfissionalAsync(profissional);

                _logger.LogInformation("Profissional registrado com sucesso: {Profissional}", profissional);
                return new ServiceResponse { Success = true, Message = "Profissional registrado com sucesso." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao registrar profissional: {@ProfissionalDTO}", profissionalDTO);
                return new ServiceResponse { Success = false, Message = "Erro ao registrar profissional." };
            }
        }
        public IEnumerable<ProfissionalDTLISTAR> GetAllProfissionais()
        {
            var profissionais = _profissionaisRepositorio.GetAllProfissionais();

            return profissionais.Select(p => new ProfissionalDTLISTAR
            {
                Id = p.Id,
                Nome = p.Nome,
                Email = p.Email,
                Foto = p.Foto,
                BI = p.BI,
                Telemovel = p.Telemovel,
                Horarios = p.Horarios.Select(h => new ProfissionalHorarioDTO
                {
                    Id = h.Id,
                    HorarioID = h.HorarioID,
                    Horario = new HorarioDTO
                    {
                        Id = h.Horario.Id,
                        Hora = h.Horario.Hora
                    }
                }).ToList(),
                ProfissionalCategorias = p.ProfissionalCategorias.Select(pc => new ProfissionalCategoriaDTO
                {
                    ProfissionalId = pc.ProfissionalId,
                    CategoriaId = pc.CategoriaID,
                    Categoria = new CategoriaDTO
                    {
                        Id = pc.Categoria.Id,
                        Nome = pc.Categoria.Nome
                    }
                }).ToList()
            }).ToList();
        }
        public ServiceResponse DeleteProfissional(int profissionalId, bool forceDelete = false)
        {
            var response = new ServiceResponse();

            try
            {
                var profissional = _profissionaisRepositorio.GetProfissionalById(profissionalId);

                if (profissional != null)
                {
                    var marcacoes = _marcacaoRepository.MarcacoesPorProfissional(profissionalId);

                    if (marcacoes.Any() && !forceDelete)
                    {
                        response.Success = false;
                        response.Message = "O profissional tem marcações associadas. Deseja continuar com a exclusão?";
                    }
                    else
                    {
                        _profissionaisRepositorio.Delete(profissional);
                        _profissionaisRepositorio.SaveChanges();

                        response.Success = true;
                        response.Message = "Profissional excluído com sucesso.";
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = $"Profissional com ID {profissionalId} não encontrado.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Erro ao excluir profissional: {ex.Message}";
            }

            return response;
        }



    }


}




