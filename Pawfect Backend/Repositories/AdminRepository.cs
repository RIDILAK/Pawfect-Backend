﻿using Microsoft.EntityFrameworkCore;
using Pawfect_Backend.Context;
using Pawfect_Backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pawfect_Backend.Repositories
{
    public interface IAdminRepository
    {
        Task<List<User>> GetAllUsers();
        Task<User> GetUserById(int id);
        Task AddCategory(Category category);

        Task<List<Category>> GetAllCategory();
        Task AddProduct(Products product);
        Task<Products> GetProductById(int id);
        Task UpdateProduct(Products product);
        Task DeleteProduct(Products product);
        Task BlockUser(User user);
    }

    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _context;

        public AdminRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddCategory(Category category)
        {
            await _context.categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }
       public async Task<List<Category>> GetAllCategory()
        {
            return await _context.categories.ToListAsync();
        }
         
        public async Task AddProduct(Products product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Products> GetProductById(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateProduct(Products product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProduct(Products product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task BlockUser(User user)
        {
            user.isBlocked = !user.isBlocked;
            await _context.SaveChangesAsync();
        }
    }
}