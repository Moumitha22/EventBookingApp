using EventBookingApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventBookingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IApiResponseMapper _responseMapper;

        public CategoryController(ICategoryService categoryService, IApiResponseMapper responseMapper)
        {
            _categoryService = categoryService;
            _responseMapper = responseMapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(_responseMapper.MapToOkResponse("Categories retrieved successfully", categories));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            return Ok(_responseMapper.MapToOkResponse("Category retrieved successfully", category));
        }

        [HttpGet("by-name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var category = await _categoryService.GetByNameAsync(name);
            if (category == null)
                return NotFound(_responseMapper.MapToErrorResponse<object>(404, "Category not found"));

            return Ok(_responseMapper.MapToOkResponse("Category found", category));
        }
    }
}
