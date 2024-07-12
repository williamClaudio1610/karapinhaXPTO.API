using KarapinhaXpto.DAL;
using KarapinhaXpto.DTO;
using KarapinhaXpto.Model;
using KarapinhaXpto.Shared.IRepositories;
using KarapinhaXpto.Shared.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.Services
{
    public class MarcacaoService : IMarcacoesServices
    {
        private readonly IMarcacaoRepositorio<Marcacao> _marcacaoRepository;
        private readonly IMarcacaoRepositorio<ServicoMarcacao> _servicoMarcacaoRepository;
        private readonly EmailService _emailService;

        public MarcacaoService(IMarcacaoRepositorio<Marcacao> marcacaoRepository, IMarcacaoRepositorio<ServicoMarcacao> servicoMarcacaoRepository, EmailService emailService)
        {
            _marcacaoRepository = marcacaoRepository;
            _servicoMarcacaoRepository = servicoMarcacaoRepository;
            _emailService = emailService;
        }

        public async Task<IEnumerable<MarcacaoDTO>> GetAllMarcacoesAsync()
        {
            var marcacoes = await _marcacaoRepository.GetAllWithServicosMarcadosAsync();
            

            if (marcacoes == null)
            {
                return Enumerable.Empty<MarcacaoDTO>();
            }

            return marcacoes.Select(m => new MarcacaoDTO
            {
                Id = m.Id,
                DataRegistro = m.DataRegistro,
                TotalPagar = m.TotalPagar,
                Status = m.Status,
                UtilizadorId = m.UtilizadorId,

                ServicosMarcados = m.ServicosMarcados.Where(sm => sm.MarcacaoId == m.Id)
                .Select(sm => new ServicoMarcacaoDTO
                {
                    Id = sm.Id,
                    ServicoId = sm.ServicoId,
                    MarcacaoId = sm.MarcacaoId,
                    ProfissionalId = sm.ProfissionalId,
                    Data = sm.Data.ToDateTime(TimeOnly.MinValue),
                    Hora = sm.Hora.ToTimeSpan(),
                    DataAnterior = sm.DataAnterior,
                    HoraAnterior = sm.HoraAnterior
                }).ToList() 


            }).ToList();
        }



        public async Task<MarcacaoDTO> GetMarcacaoByIdAsync(int id)
        {
            var marcacao = await _marcacaoRepository.GetByIdAsync(id);
            if (marcacao == null) return null;

            return new MarcacaoDTO
            {
                Id = marcacao.Id,
                DataRegistro = marcacao.DataRegistro,
                TotalPagar = marcacao.TotalPagar,
                Status = marcacao.Status,
                UtilizadorId = marcacao.UtilizadorId,
                ServicosMarcados = marcacao.ServicosMarcados.Select(sm => new ServicoMarcacaoDTO
                {
                    Id = sm.Id,
                    ServicoId = sm.ServicoId,
                    MarcacaoId = sm.MarcacaoId,
                    ProfissionalId = sm.ProfissionalId,
                    Data = sm.Data.ToDateTime(TimeOnly.MinValue),
                    Hora = sm.Hora.ToTimeSpan()
                }).ToList()
            };
        }

        public async Task AddMarcacaoAsync(MarcacaoDTO marcacaoDto)
        {
            var marcacao = new Marcacao
            {
                DataRegistro = marcacaoDto.DataRegistro,
                TotalPagar = marcacaoDto.TotalPagar,
                Status = marcacaoDto.Status,
                UtilizadorId = marcacaoDto.UtilizadorId,
                ServicosMarcados = marcacaoDto.ServicosMarcados.Select(sm => new ServicoMarcacao
                {
                    ServicoId = sm.ServicoId,
                    ProfissionalId = sm.ProfissionalId,
                    Data = DateOnly.FromDateTime(sm.Data),
                    Hora = TimeOnly.FromTimeSpan(sm.Hora)
                }).ToList()
            };

            await _marcacaoRepository.AddAsync(marcacao);
        }

        public async Task UpdateMarcacaoAsync(MarcacaoDTO marcacaoDto)
        {
            var marcacao = await _marcacaoRepository.GetByIdAsync(marcacaoDto.Id);
            if (marcacao == null) return;

            marcacao.DataRegistro = marcacaoDto.DataRegistro;
            marcacao.TotalPagar = marcacaoDto.TotalPagar;
            marcacao.Status = marcacaoDto.Status;
            marcacao.UtilizadorId = marcacaoDto.UtilizadorId;
            // Update de `ServicosMarcados` pode ser complexo e precisa de um tratamento adequado
            // dependendo da política de atualização.

            await _marcacaoRepository.UpdateAsync(marcacao);
        }

        public async Task DeleteMarcacaoAsync(int id)
        {
            await _marcacaoRepository.DeleteAsync(id);
        }



        public double CalcularTotalPreco(IEnumerable<ServicoMarcacaoDTO> servicosMarcados)
        {
            // Verifique se servicosMarcados não é nulo e possui itens
            if (servicosMarcados == null || !servicosMarcados.Any())
            {
                return 0;

            }
            return 1;
            /*
                    // Calcula a soma dos preços dos serviços marcados
                    decimal totalPreco = 0;
                    foreach (var sm in servicosMarcados)
                    {
                      // Aqui você precisa obter o preço do serviço com base no ServicoId
                    var servico = _marcacaoRepository.Servico.FirstOrDefault(s => s.Id == sm.ServicoId);
                      if (servico != null)
                      {
                          totalPreco += servico.Preco;
                      }
                    }

                    return (double)totalPreco;*/
        }

        public async Task<bool> ConfirmarMarcacaoAsync(int id)
        {
            var marcacao = await _marcacaoRepository.GetByIdAsyncConfirmar(id);

            if (marcacao == null || marcacao.Status)
            {
                return false; // Marcação não existe ou já está confirmada
            }

            // Atualizar o status para confirmado
            marcacao.Status = true;
            await _marcacaoRepository.UpdateAsync(marcacao);

            // Enviar e-mail de confirmação
            await _emailService.SendConfirmationEmailAsync(marcacao);
            return true;
        }

        public async Task<bool> AtualizarServicoMarcacaoAsync(ServicoMarcacaoUpdateDTO dto)
        {

            return await _marcacaoRepository.UpdateServicoMarcacao1Async(dto);
        }
    }
}
