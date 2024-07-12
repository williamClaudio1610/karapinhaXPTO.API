using KarapinhaXpto.DTO;
using KarapinhaXpto.Model;
using KarapinhaXpto.Shared.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.DAL.Repositories
{
    public class MarcacoesRepositorio<T> : IMarcacaoRepositorio<T> where T : class 
    {

        private readonly KarapinhaXptoDbContext _karapinhaXptoDbContext;
        private readonly DbSet<T> _dbSet;

        public MarcacoesRepositorio(KarapinhaXptoDbContext karapinhaXptoDbContext)
        {
            _karapinhaXptoDbContext = karapinhaXptoDbContext;
            _dbSet = _karapinhaXptoDbContext.Set<T>();

        }

        public async Task<Marcacao> GetByIdAsyncConfirmar(int id)
        {
                    return await _karapinhaXptoDbContext.Marcacaos
                .Include(m => m.ServicosMarcados)      // Inclui os serviços marcados
                .ThenInclude(sm => sm.Servico)
                .ThenInclude(sm => sm.Categoria)
                .Include(m => m.Utilizador)            // Inclui os dados do utilizador associado
                .FirstOrDefaultAsync(m => m.Id == id); // Busca a marcação pelo ID
        }

        public async Task<List<Marcacao>> GetAllWithServicosMarcadosAsync()
        {
            return await _karapinhaXptoDbContext.Marcacaos
                .Include(m => m.ServicosMarcados)
                .ToListAsync();
        }
       

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _karapinhaXptoDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _karapinhaXptoDbContext.Entry(entity).State = EntityState.Modified;
            await _karapinhaXptoDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Marcação com ID {id} não encontrada.");
            }
            _dbSet.Remove(entity);
            await _karapinhaXptoDbContext.SaveChangesAsync();
        }


        public IEnumerable<Marcacao> MarcacoesPorProfissional(int profissionalId)
        {
            return _karapinhaXptoDbContext.Marcacaos
                .Where(m => m.ServicosMarcados.Any(sm => sm.ProfissionalId == profissionalId))
                .ToList();
        }
        public async Task UpdateServicoMarcacaoAsync(ServicoMarcacao servicoMarcacao)
        {
            _karapinhaXptoDbContext.ServicoMarcacaos.Update(servicoMarcacao);
            await _karapinhaXptoDbContext.SaveChangesAsync();
        }
        public async Task<ServicoMarcacao> GetByIdServicoMarcacaoAsync(int id)
        {
            return await _karapinhaXptoDbContext.ServicoMarcacaos.FindAsync(id);
        }
        public async Task UpdateAsync(Marcacao marcacao)
        {
            _karapinhaXptoDbContext.Marcacaos.Update(marcacao);
            await _karapinhaXptoDbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateServicoMarcacao1Async(ServicoMarcacaoUpdateDTO dto)
        {
            var servicoMarcacao = await _karapinhaXptoDbContext.ServicoMarcacaos
                .Include(sm => sm.Marcacao) 
                .FirstOrDefaultAsync(sm => sm.Id == dto.ServicoMarcacaoId);

            if (servicoMarcacao == null) return false;
            servicoMarcacao.DataAnterior = servicoMarcacao.Data;
            servicoMarcacao.HoraAnterior = servicoMarcacao.Hora;


            var novaData = DateOnly.FromDateTime(DateTime.Parse(dto.NovaData));
            var novaHora = TimeOnly.FromDateTime(DateTime.Parse(dto.NovaHora));

            servicoMarcacao.Data = novaData;
            servicoMarcacao.Hora = novaHora;

            servicoMarcacao.Marcacao.DataRegistro = DateTime.UtcNow;

            await _karapinhaXptoDbContext.SaveChangesAsync();

            return true;
        }
    }



}

