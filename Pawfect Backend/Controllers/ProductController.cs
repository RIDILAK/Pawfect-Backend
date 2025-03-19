using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pawfect_Backend.Services;

namespace Pawfect_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _services;

        public ProductController(IProductServices services)
        {
            _services = services;
        }

        [HttpGet("GetALl")]
      
        public async Task <IActionResult> GetAll()
        {
            var isGet =await _services.GetProducts();
            return StatusCode(isGet.StatusCode, isGet);
        }

        [HttpGet("GetById")]
        [Authorize]

        public async Task <IActionResult> GetById(int id)
        {
            var isGetById =await _services.GetById(id);
            return StatusCode(isGetById.StatusCode, isGetById);
        }

        [HttpGet("GetByCategory")]
        [Authorize]

        public async Task <IActionResult> GetByCategory(string category)
        {
            var isGetByCategory =await _services.GetByCategory(category);
            return StatusCode(isGetByCategory.StatusCode, isGetByCategory);
        }

        [HttpGet("PaginatedProduct")]
        [Authorize]

        public async Task <IActionResult> PaginatedProduct(int pageNumber, int pageSize)
        {
            var isPaginatedProduct =await _services.GetPaginatedProduct(pageNumber, pageSize);
            return StatusCode(isPaginatedProduct.StatusCode, isPaginatedProduct);
        }

        [HttpGet("Search")]
        [Authorize]
        public async Task<IActionResult> Search(string search)
        {
                var result = await _services.SearchProducts(search);
                if (result == null || result.Count == 0)
                {
                    return NotFound("No products found");
                }
                return Ok(result);
            
            
        }
    }
}
