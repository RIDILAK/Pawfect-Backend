using Microsoft.EntityFrameworkCore;
using Pawfect_Backend.Context;
using Pawfect_Backend.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pawfect_Backend.Repositories
{
    public interface ICartRepository
    {
        Task<Cart> GetCartByUserId(int userId);
        Task<User> GetUserWithCart(int userId);
        Task<Products> GetProductById(int productId);
        Task AddCart(Cart cart);
        Task AddCartItem(CartItem cartItem);
        Task RemoveCartItem(CartItem cartItem);
        Task SaveChanges();
    }

    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> GetCartByUserId(int userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<User> GetUserWithCart(int userId)
        {
            return await _context.Users
                .Include(u => u.Cart)
                .ThenInclude(c => c.CartItems)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<Products> GetProductById(int productId)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task AddCart(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
        }

        public async Task AddCartItem(CartItem cartItem)
        {
            await _context.CartItems.AddAsync(cartItem);
        }

        public async Task RemoveCartItem(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}