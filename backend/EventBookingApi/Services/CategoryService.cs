using EventBookingApi.Interfaces;
using EventBookingApi.Models;

namespace EventBookingApi.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _categoryRepository.GetAll();
        }

        public async Task<Category> GetByIdAsync(Guid id)
        {
            return await _categoryRepository.Get(id);
        }

        public async Task<Category?> GetByNameAsync(string name)
        {
            return await _categoryRepository.GetByNameAsync(name);
        }
    }
}
