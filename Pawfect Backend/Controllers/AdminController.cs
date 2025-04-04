﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pawfect_Backend.Dto;
using Pawfect_Backend.Models;
using Pawfect_Backend.Services;

namespace Pawfect_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminServices _adminServices;
        public AdminController(IAdminServices adminServices)
        {
            _adminServices = adminServices;
        }

        [HttpGet("GetallUsers")]
        [Authorize(Roles = "Admin")]

        public async Task <IActionResult> GetAllUsers()
        {
            var result = await _adminServices.GetallUsers();

            return StatusCode(result.StatusCode,result);
        }

        [HttpGet("GetUserById")]
        [Authorize(Roles = "Admin")]
        public async Task <IActionResult> GetUserId(int id)
        {
            var result =await _adminServices.GetById(id);
            return StatusCode(result.StatusCode,result);
        }
        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUserId()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var result = await _adminServices.GetById(int.Parse(userId));
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("GetAllCategory")]
        [Authorize(Roles ="Admin")]

        public async Task <IActionResult> GetCategory()
        {
            var result = await _adminServices.GetCategories();
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("AddCategory")]
        [Authorize(Roles = "Admin")]

        public async Task <IActionResult> AddCategory(AddCategoryDto category)
        {
            var isCategoryAdded = await _adminServices.AddCategory(category);
            return StatusCode(isCategoryAdded.StatusCode,isCategoryAdded);
                
        }

        [HttpPost("AddProduct")]
        [Authorize(Roles = "Admin")]

        public async Task <IActionResult> AddProduct([FromForm]AddProductDto Product,IFormFile imageFile)
        {
            var isProductAdded = await _adminServices.AddProduct(Product, imageFile);
            return StatusCode(isProductAdded.StatusCode,isProductAdded);
        }

        [HttpPut("UpdateProduct")]
        [Authorize(Roles = "Admin")]

        public async Task <IActionResult> UpdateEmployee(int Id, [FromForm] AddProductDto UpdateProduct, IFormFile imageFile)
        {
                var isProductUpdated=await _adminServices.UpdateProduct(Id, UpdateProduct,imageFile);
            return StatusCode(isProductUpdated.StatusCode,isProductUpdated);
        }

        [HttpDelete("DeleteProduct")]
        [Authorize(Roles = "Admin")]
        public async Task <IActionResult> DeleteProduct(int Id)
        {
            var isDeleted =await _adminServices.DeleteProduct(Id);
            return StatusCode(isDeleted.StatusCode,isDeleted);
        }

        [HttpPatch("BlockUserById")]
        [Authorize(Roles ="Admin")]

        public async Task <IActionResult> BlockUser(int Id)
        {
            var isBlocked= await _adminServices.BlockUser(Id);
            return StatusCode(isBlocked.StatusCode,isBlocked);
        }
        
    }
}
