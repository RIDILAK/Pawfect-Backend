using Microsoft.EntityFrameworkCore;
using Pawfect_Backend.Context;
using Pawfect_Backend.Models;
using Razorpay.Api;

namespace Pawfect_Backend.Repositories
{
    public interface IOrderRepository
    { 
        
        public Task <Orders> CreateOrder(Orders order);

        public Task<List<Orders>> GetOrderByUserByid(int userId);
        public Task<List<Orders>> GetAllOrder();
        public Task<List<OrderItem>> GetOrderItems();
        public  Task<Cart> GetCartByUserId(int userId);
        public Task<Address>GetAddressById(int Addressid,int userId);

        public  Task<Orders> GetOrderById(int Id);
        public  Task UpdateOrderStatus(Orders order, string status);

        public Task RemoveCart(Cart cart);

        public Task updateQuantity(Products product);



    }
    public class OrderRepository:IOrderRepository
    {
      private readonly ApplicationDbContext _context;
        private readonly string _razorpayKey;
        private readonly string _razorpaySecret;
        public OrderRepository(ApplicationDbContext context,IConfiguration configuration) { 
        
            _context = context;
            
        }

      
        public async Task<Orders> CreateOrder(Orders order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;

        }
        public async Task<List<Orders>> GetOrderByUserByid(int userId)
        {
            return await _context.Orders.Include(x=>x.Address).Include(x=>x.OrderItems)
                .ThenInclude(x=>x.Product).Where(x=>x.userId == userId).ToListAsync();


        }
        public async Task<List<Orders>> GetAllOrder()
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

        public async Task updateQuantity(Products product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Orders> GetOrderById(int Id)
        {
            return await _context.Orders.FirstOrDefaultAsync(x => x.OrderId == Id);
        }
        public async Task UpdateOrderStatus(Orders order, string status)
        {
            if (order != null)
            {
                order.OrderStatus = status;
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
            }
        }



    }
}     
