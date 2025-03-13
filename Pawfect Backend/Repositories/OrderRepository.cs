using Microsoft.EntityFrameworkCore;
using Pawfect_Backend.Context;
using Pawfect_Backend.Models;

namespace Pawfect_Backend.Repositories
{
    public interface IOrderRepository
    {
        public Task <Order> CreateOrder(Order order);

        public Task<List<Order>> GetOrderByUserByid(int userId);
        public Task<List<Order>> GetAllOrder();
        public Task<List<OrderItem>> GetOrderItems();
        public  Task<Cart> GetCartByUserId(int userId);
        public Task<Address>GetAddressById(int Addressid,int userId);

        public Task RemoveCart(Cart cart);

        public Task updateQuantity(Product product);



    }
    public class OrderRepository:IOrderRepository
    {
      private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context) { 
        
            _context = context;
        }
        public async Task<Order> CreateOrder(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;

        }
        public async Task<List<Order>> GetOrderByUserByid(int userId)
        {
            return await _context.Orders.Include(x=>x.Address).Include(x=>x.OrderItems)
                .ThenInclude(x=>x.Product).Where(x=>x.userId == userId).ToListAsync();


        }
        public async Task<List<Order>> GetAllOrder()
        {
            return await _context.Orders
                .Include(O=>O.Address)
                .Include(O=>O.OrderItems)
                .ThenInclude(oi=>oi.Product)
                .ToListAsync();
        }  

        public async Task<Cart> GetCartByUserId(int userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<List<OrderItem>> GetOrderItems()
        {
            return await _context.OrderItems.Include(x=>x.Product).ToListAsync();
        }

        public async Task<Address> GetAddressById(int Addressid, int userId)
        {
          return  await _context.Address.FirstOrDefaultAsync(x => x.AddressId == Addressid && userId == userId);
        }
        public async Task RemoveCart(Cart cart)
        {
            _context.Carts.Remove(cart);
           await _context.SaveChangesAsync();
        }    

        public async Task updateQuantity(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }                       
    }          
}     
