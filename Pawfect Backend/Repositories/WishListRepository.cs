using Microsoft.EntityFrameworkCore;
using Pawfect_Backend.Context;
using Pawfect_Backend.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pawfect_Backend.Repositories
{
    public interface IWishListRepository
    {
        Task<bool> ProductExistsAsync(int productId);
        Task<WishList> GetWishListItemAsync(int productId, int userId);
        Task AddWishListItemAsync(WishList wishList);
        Task RemoveWishListItemAsync(WishList wishList);
        Task<List<WishList>> GetWishListByUserIdAsync(int userId);
    }

    public class WishListRepository : IWishListRepository
    {
        private readonly ApplicationDbContext _context;

        public WishListRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ProductExistsAsync(int productId)
        {
            return await _context.Products.AnyAsync(x => x.Id == productId);
        }

        public async Task<WishList> GetWishListItemAsync(int productId, int userId)
        {
            return await _context.WishList.FirstOrDefaultAsync(x => x.ProductId == productId && x.UserId == userId);
        }

        public async Task AddWishListItemAsync(WishList wishList)
        {
            await _context.WishList.AddAsync(wishList);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveWishListItemAsync(WishList wishList)
        {
            _context.WishList.Remove(wishList);
            await _context.SaveChangesAsync();
        }

        public async Task<List<WishList>> GetWishListByUserIdAsync(int userId)
        {
            return await _context.WishList.Include(x => x.Product).Where(x => x.UserId == userId).ToListAsync();
        }
    }
}