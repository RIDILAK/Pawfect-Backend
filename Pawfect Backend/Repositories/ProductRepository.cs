using Microsoft.EntityFrameworkCore;
using Pawfect_Backend.Context;
using Pawfect_Backend.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pawfect_Backend.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProducts();
        Task<Product> GetProductById(int id);
        Task<List<Product>> GetProductsByCategory(string category);
        Task<List<Product>> GetPaginatedProducts(int pageNumber, int pageSize);
        Task<List<Product>> SearchProducts(string search);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _context.Products.Include(e => e.category).ToListAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _context.Products.Include(e => e.category).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Product>> GetProductsByCategory(string category)
        {
            return await _context.Products.Include(e => e.category).Where(x => x.category.CategoryName == category).ToListAsync();
        }

        public async Task<List<Product>> GetPaginatedProducts(int pageNumber, int pageSize)
        {
            return await _context.Products.Include(e => e.category)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Product>> SearchProducts(string search)
        {
            return await _context.Products.Include(x => x.category)

                .Where(x => x.ProductName.Contains(search) || x.Description.Contains(search)) 
                .ToListAsync();
        }
    }
}