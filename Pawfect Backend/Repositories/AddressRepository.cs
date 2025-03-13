using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pawfect_Backend.Context;
using Pawfect_Backend.Models;

namespace Pawfect_Backend.Repositories
{
    public interface IAddressRepository
    {
        Task<List<Address>> GetAddressesByUserId(int userId);
        Task<Address> GetAddressById(int addressId);
        Task AddAddress(Address address);
        Task RemoveAddress(Address address);
    }

    public class AddressRepository : IAddressRepository
    {
        private readonly ApplicationDbContext _context;

        public AddressRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Address>> GetAddressesByUserId(int userId)
        {
            return await _context.Address.Where(a => a.userId == userId).ToListAsync();
        }

        public async Task<Address> GetAddressById(int addressId)
        {
            return await _context.Address.FirstOrDefaultAsync(a => a.AddressId == addressId);
        }

        public async Task AddAddress(Address address)
        {
            await _context.Address.AddAsync(address);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAddress(Address address)
        {
            _context.Address.Remove(address);
            await _context.SaveChangesAsync();
        }
    }
}