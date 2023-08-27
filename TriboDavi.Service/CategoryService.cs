using Common.DTO;
using TriboDavi.DataAccess.Interface;
using TriboDavi.Domain;
using TriboDavi.DTO;
using TriboDavi.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace TriboDavi.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<ResponseDTO> Create(BasicDTO basicDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var categoryExists = await _categoryRepository.GetEntities().AnyAsync(c => c.Name == basicDTO.Name);
                if (categoryExists)
                {
                    responseDTO.SetBadInput($"A categoria {basicDTO.Name} já existe!");
                    return responseDTO;
                }
                var category = new Category
                {
                    Name = basicDTO.Name,
                };
                category.SetCreatedAt();
                await _categoryRepository.InsertAsync(category);
                await _categoryRepository.SaveChangesAsync();
                responseDTO.Object = category;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> Update(int id, BasicDTO basicDTO)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var category = await _categoryRepository.GetTrackedEntities().FirstOrDefaultAsync(c => c.Id == id);
                if (category == null)
                {
                    responseDTO.SetBadInput($"A categoria {basicDTO.Name} não existe!");
                    return responseDTO;
                }
                category.Name = basicDTO.Name;
                category.SetUpdatedAt();
                await _categoryRepository.SaveChangesAsync();
                responseDTO.Object = category;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> Remove(int id)
        {
            ResponseDTO responseDTO = new();
            try
            {
                var category = await _categoryRepository.GetTrackedEntities().FirstOrDefaultAsync(c => c.Id == id);
                if (category == null)
                {
                    responseDTO.SetBadInput($"A categoria com id: {id} não existe!");
                    return responseDTO;
                }
                _categoryRepository.Delete(category);
                await _categoryRepository.SaveChangesAsync();
                responseDTO.Object = category;
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }

        public async Task<ResponseDTO> GetList()
        {
            ResponseDTO responseDTO = new();
            try
            {
                responseDTO.Object = await _categoryRepository.GetEntities().ToListAsync();
            }
            catch (Exception ex)
            {
                responseDTO.SetError(ex);
            }
            return responseDTO;
        }
    }
}