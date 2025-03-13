using AutoMapper;
using Pawfect_Backend.Dto;
using Pawfect_Backend.Models;
using Pawfect_Backend.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pawfect_Backend.Services
{
    public interface IProductServices
    {
        Task<Responses<List<GetProductsDto>>> GetProducts();
        Task<Responses<GetProductsDto>> GetById(int id);
        Task<Responses<List<GetProductsDto>>> GetByCategory(string category);
        Task<Responses<List<GetProductsDto>>> GetPaginatedProduct(int pageNumber, int pageSize);
        Task<List<GetProductsDto>> SearchProducts(string search);
    }

    public class ProductServices : IProductServices
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductServices(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Responses<List<GetProductsDto>>> GetProducts()
        {
            var products = await _productRepository.GetAllProducts();
            return new Responses<List<GetProductsDto>> { Data = _mapper.Map<List<GetProductsDto>>(products), StatusCode = 200 };
        }

        public async Task<Responses<GetProductsDto>> GetById(int id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null)
            {
                return new Responses<GetProductsDto> { StatusCode = 404, Message = "Product is not Found" };
            }
            return new Responses<GetProductsDto> { Data = _mapper.Map<GetProductsDto>(product), StatusCode = 200 };
        }

        public async Task<Responses<List<GetProductsDto>>> GetByCategory(string category)
        {
            var products = await _productRepository.GetProductsByCategory(category);
            if (products.Count == 0)
            {
                return new Responses<List<GetProductsDto>> { StatusCode = 404, Message = "Category is Not Found" };
            }
            return new Responses<List<GetProductsDto>> { Data = _mapper.Map<List<GetProductsDto>>(products), StatusCode = 200 };
        }

        public async Task<Responses<List<GetProductsDto>>> GetPaginatedProduct(int pageNumber, int pageSize)
        {
            var products = await _productRepository.GetPaginatedProducts(pageNumber, pageSize);
            if (products == null || products.Count == 0)
            {
                return new Responses<List<GetProductsDto>> { StatusCode = 404, Message = "Products are Not Found" };
            }
            return new Responses<List<GetProductsDto>> { Data = _mapper.Map<List<GetProductsDto>>(products), Message = "Paginated Successfully", StatusCode = 200 };
        }

        public async Task<List<GetProductsDto>> SearchProducts(string search)
        {
            var products = await _productRepository.SearchProducts(search);
            if (products == null || products.Count == 0)
            {
                return new List<GetProductsDto>();
            }
            return _mapper.Map<List<GetProductsDto>>(products);
        }
    }
}