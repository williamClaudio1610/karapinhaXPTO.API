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
using Microsoft.AspNetCore.Http;

namespace KarapinhaXpto.Services
{
    public class CategoriaService:ICategoriaServices
    {
        private readonly ICategoriaRepositorio _categoriaRepositorio;

        public CategoriaService(ICategoriaRepositorio categoriaRepositorio)
        {
            _categoriaRepositorio = categoriaRepositorio;
        }

        public async Task<bool> Actualizar(CategoriaUpdateDTO categoriaUpdateDTO)
        {

            var categoria = await _categoriaRepositorio.ListarPorId(categoriaUpdateDTO.Id);
            
            if(categoria != null)
            {
                categoria.Descricao = categoriaUpdateDTO.Descricao;  
                return await _categoriaRepositorio.Actualizar(categoria);

            }
            return false;   


        }

        public async Task<ServiceResponse> Delete(int id)
        {
            var response = new ServiceResponse();

            try
            {
                // Verifica se a categoria existe
                var categoria = await _categoriaRepositorio.ListarPorId(id);

                if (categoria == null)
                {
                    response.Success = false;
                    response.Message = "Categoria não encontrada.";
                    return response;
                }

                // Tenta excluir a categoria
                var resultado = await _categoriaRepositorio.Eliminar(categoria);

                if (resultado)
                {
                    response.Success = true;
                    response.Message = "Categoria excluída com sucesso.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Falha ao excluir a categoria.";
                }
            }
            catch (Exception )
            {
              
                response.Success = false;
                response.Message = "Ocorreu um erro inesperado ao tentar excluir a categoria.";
            }

            return response;
        }


        public async Task<Categoria> ListarPorId(int id)
        {
            return await _categoriaRepositorio.ListarPorId(id);
        }

        public async Task<List<Categoria>> ListarTodos()
        {
            return await _categoriaRepositorio.Listar();
        }


        public async Task<ServiceResponse> RegisterCategoriaAsync(CategoriaAddDTO categoriaAddDTO)
        {
            if (categoriaAddDTO == null)
            {
                throw new ArgumentNullException(nameof(categoriaAddDTO));
            }

            // Verificar se a categoria já existe pelo nome
            var existingCategoriaByNome = await _categoriaRepositorio.GetCategoriaByNomeAsync(categoriaAddDTO.Nome);
            if (existingCategoriaByNome != null)
            {
                return new ServiceResponse { Success = false, Message = "Categoria com este nome já existe." };
            }

            if (categoriaAddDTO.Foto == null)
            {
                return new ServiceResponse { Success = false, Message = "A foto da categoria é obrigatória." };
            }

            // Define o caminho completo para a pasta Imagens
            var imagesFolderPath = Path.Combine("C:\\Users\\Admin\\Documents\\ISPTEC - Universidade\\ISPTEC- 3º ano - 2023-2024\\2º Semestre\\Aplicações Web (AW)\\AAA_PROJECTO_FINAL_ KARAPINHA_XPTO\\Karapinha-Xpto\\src\\assets\\images\\Categorias");

            // Garante que o diretório de imagens existe
            if (!Directory.Exists(imagesFolderPath))
            {
                Directory.CreateDirectory(imagesFolderPath);
            }

            // Gera um nome de arquivo único
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(categoriaAddDTO.Foto.FileName);
            var filePath = Path.Combine(imagesFolderPath, fileName);

            // Salva o arquivo
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await categoriaAddDTO.Foto.CopyToAsync(stream);
            }

            var categoria = new Categoria
            {
                Nome = categoriaAddDTO.Nome,
                Descricao = categoriaAddDTO.Descricao,
                Foto = Path.Combine("images\\Categorias", fileName) // Caminho relativo salvo no banco de dados
            };




            // Salvamento da categoria
            await _categoriaRepositorio.AddCategoriaAsync(categoria);

            return new ServiceResponse { Success = true, Message = "Categoria registrada com sucesso." };


        }

    }
}
