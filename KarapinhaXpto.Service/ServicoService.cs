using KarapinhaXpto.DAL.Repositories;
using KarapinhaXpto.DTO;
using KarapinhaXpto.Model;
using KarapinhaXpto.Shared.IRepositories;
using KarapinhaXpto.Shared.IServices;
using KarapinhaXpto.Shared.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace KarapinhaXpto.Services
{
    public class ServicoService: IServicoServices
    {

        private readonly IServicoRepositorio _servicoRepositorio;
        private readonly ICategoriaRepositorio _categoriaRepositorio;

        public ServicoService(IServicoRepositorio servicoRepositorio)
        {
            _servicoRepositorio = servicoRepositorio;
        }


        public async Task<ServiceResponse> RegisterServicoAsync(ServicoDTO servicoDTO)
        {
            if (servicoDTO == null)
            {
                throw new ArgumentNullException(nameof(servicoDTO));
            }

            // Verificar se o serviço já existe pelo nome
            var existingServicoByNome = await _servicoRepositorio.GetServicoByNomeAsync(servicoDTO.Nome);
            if (existingServicoByNome != null)
            {
                return new ServiceResponse { Success = false, Message = "Serviço com este nome já existe." };
            }

            // Verificar se a categoria existe
            var existingCategoria = await _servicoRepositorio.GetCategoriaByIdAsync(servicoDTO.CategoriaId);
            if (existingCategoria == null)
            {
                return new ServiceResponse { Success = false, Message = "Categoria não encontrada." };
            }


            if (servicoDTO.Imagem == null)
            {
                return new ServiceResponse { Success = false, Message = "A foto da categoria é obrigatória." };
            }

            // Define o caminho completo para a pasta Imagens
            var imagesFolderPath = Path.Combine("C:\\Users\\Admin\\Documents\\ISPTEC - Universidade\\ISPTEC- 3º ano - 2023-2024\\2º Semestre\\Aplicações Web (AW)\\AAA_PROJECTO_FINAL_ KARAPINHA_XPTO\\Karapinha-Xpto\\src\\assets\\images\\Servicos");

            // Garante que o diretório de imagens existe
            if (!Directory.Exists(imagesFolderPath))
            {
                Directory.CreateDirectory(imagesFolderPath);
            }

            // Gera um nome de arquivo único
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(servicoDTO.Imagem.FileName);
            var filePath = Path.Combine(imagesFolderPath, fileName);

            // Salva o arquivo
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await servicoDTO.Imagem.CopyToAsync(stream);
            }


            var servico = new Servico
            {
                Nome = servicoDTO.Nome,
                Descricao = servicoDTO.Descricao,
                CategoriaId = servicoDTO.CategoriaId,
                Preco = servicoDTO.Preco,
                Imagem = Path.Combine("images\\Servicos", fileName) // Caminho relativo salvo no banco de dados

            };


            // Salvamento do serviço
            await _servicoRepositorio.AddServicoAsync(servico);

            return new ServiceResponse { Success = true, Message = "Serviço registrado com sucesso." };
        }

        public async Task<IEnumerable<ServicoDTOListar>> GetAllServicosAsync()
        {
            var servicos = await _servicoRepositorio.GetAllServicosAsync();

            // Transformar os dados da entidade em DTO
            return servicos.Select(s => new ServicoDTOListar
            {
                Id = s.Id,
                Nome = s.Nome,
                Descricao = s.Descricao,
                CategoriaId = s.CategoriaId,
                Preco = s.Preco,
                Imagem = s.Imagem
            }).ToList();
        }
        public async Task<ServiceResponse> EliminarServicoAsync(int id)
        {
            var servico = await _servicoRepositorio.GetByIdAsync(id);

            if (servico == null)
            {
                return new ServiceResponse { Success = false, Message = "Serviço não encontrado." };
            }

            var deleted = await _servicoRepositorio.DeleteAsync(id);

            if (deleted)
            {
                return new ServiceResponse { Success = true, Message = "Serviço eliminado com sucesso." };
            }
            else
            {
                return new ServiceResponse { Success = false, Message = "Erro ao eliminar o serviço." };
            }
        }



        public async Task<Servico> AtualizarServicoAsync(ServicoDTOActulizar servicoAtualizado, IFormFile imagem)
        {
            var servicoExistente = await _servicoRepositorio.GetByIdAsync(servicoAtualizado.Id);

            if (servicoExistente == null)
            {
                return null; // Serviço não encontrado
            }

            // Atualiza apenas os campos que foram fornecidos
            if (!string.IsNullOrEmpty(servicoAtualizado.Nome))
            {
                servicoExistente.Nome = servicoAtualizado.Nome;
            }

            if (!string.IsNullOrEmpty(servicoAtualizado.Descricao))
            {
                servicoExistente.Descricao = servicoAtualizado.Descricao;
            }

            if (imagem != null)
            {
                servicoExistente.Imagem = Path.Combine("images\\Servicos", await SalvarImagemAsync(imagem)) ;
            }

            if (servicoAtualizado.CategoriaId.HasValue)
            {
                servicoExistente.CategoriaId = servicoAtualizado.CategoriaId.Value;
            }

            if (servicoAtualizado.Preco.HasValue)
            {
                servicoExistente.Preco = servicoAtualizado.Preco.Value;
            }

            await _servicoRepositorio.UpdateAsync(servicoExistente);

            return servicoExistente;
        }

        private async Task<string> SalvarImagemAsync(IFormFile imagem)
        {
            if (imagem == null || imagem.Length == 0)
            {
                throw new ArgumentException("Imagem inválida.");
            }

            // Define o caminho completo para a pasta Imagens
            var imagesFolderPath = Path.Combine("C:\\Users\\Admin\\Documents\\ISPTEC - Universidade\\ISPTEC- 3º ano - 2023-2024\\2º Semestre\\Aplicações Web (AW)\\AAA_PROJECTO_FINAL_ KARAPINHA_XPTO\\Karapinha-Xpto\\src\\assets\\images\\Servicos");

            // Garante que o diretório de imagens existe
            if (!Directory.Exists(imagesFolderPath))
            {
                Directory.CreateDirectory(imagesFolderPath);
            }

            // Gera um nome de arquivo único
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imagem.FileName);
            var filePath = Path.Combine(imagesFolderPath, fileName);

            // Salva o arquivo
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imagem.CopyToAsync(stream);
            }

            // Retorna o caminho relativo do arquivo
            return fileName;
        }



    }

}



