using AutoMapper;
using Pawfect_Backend.Dto;
using Pawfect_Backend.Models;
using Pawfect_Backend.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pawfect_Backend.Services
{
    public interface IAdminServices
    {
        Task<Responses<List<GetAllUsersDto>>> GetallUsers();
        Task<Responses<GetAllUsersDto>> GetById(int id);
        Task<Responses<string>> AddCategory(AddCategoryDto AddCategory);
        Task<Responses<List<GetCategoryDto>>> GetCategories();
        Task<Responses<string>> AddProduct(AddProductDto AddProduct,IFormFile imageFile);
        Task<Responses<string>> UpdateProduct(int Id, AddProductDto UpdateProduct, IFormFile imageFile);
        Task<Responses<string>> DeleteProduct(int Id);
        Task<Responses<string>> BlockUser(int Id);
    }

    public class AdminServices : IAdminServices
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IMapper _mapper;
        private readonly ICLoudinaryServices _cloudinaryServices;

        public AdminServices(IAdminRepository adminRepository, IMapper mapper, ICLoudinaryServices cloudinaryServices)
        {
            _adminRepository = adminRepository;
            _mapper = mapper;
            _cloudinaryServices = cloudinaryServices;
        }

        public async Task<Responses<List<GetAllUsersDto>>> GetallUsers()
        {
            var users = await _adminRepository.GetAllUsers();
            var mappedUsers = _mapper.Map<List<GetAllUsersDto>>(users);
            return new Responses<List<GetAllUsersDto>> { Data = mappedUsers, StatusCode = 200 };
        }

        public async Task<Responses<GetAllUsersDto>> GetById(int id)
        {
            var user = await _adminRepository.GetUserById(id);
            if (user == null)
            {
                return new Responses<GetAllUsersDto> { StatusCode = 404, Message = "User Not Found" };
            }
            return new Responses<GetAllUsersDto> { Data = _mapper.Map<GetAllUsersDto>(user), StatusCode = 200 };
        }
     public async Task<Responses<List<GetCategoryDto>>> GetCategories()
        {
            var category = await _adminRepository.GetAllCategory();
            var mapperCategory = _mapper.Map<List<GetCategoryDto>>(category);
            return new Responses<List<GetCategoryDto>> { Data = mapperCategory, StatusCode = 200,Message="Category Retrived Succesfully" };
        }

        public async Task<Responses<string>> AddCategory(AddCategoryDto AddCategory)
        {
            var category = _mapper.Map<Category>(AddCategory);
            await _adminRepository.AddCategory(category);
            return new Responses<string> { Message = "Category Added Successfully", StatusCode = 200 };
        }

        public async Task<Responses<string>> AddProduct(AddProductDto AddProduct, IFormFile imageFile)
        {
            var product = _mapper.Map<Product>(AddProduct);
            product.Url = await _cloudinaryServices.UploadImage(imageFile);
            await _adminRepository.AddProduct(product);
            return new Responses<string> { Message = "Product Added Successfully", StatusCode = 200 };
        }

        public async Task<Responses<string>> UpdateProduct(int Id, AddProductDto UpdateProduct,IFormFile imageFile)
        {
            var product = await _adminRepository.GetProductById(Id);

            if (product == null)
            {
                return new Responses<string> { StatusCode = 404, Message = "Product Not Found" };
            }
            if (imageFile != null && imageFile.Length > 0) {

                product.Url = await _cloudinaryServices.UploadImage(imageFile);

            } 

            _mapper.Map(UpdateProduct, product);
            await _adminRepository.UpdateProduct(product);
            return new Responses<string> { Message = "Product Updated Successfully", StatusCode = 200 };
        }

        public async Task<Responses<string>> DeleteProduct(int Id)
        {
            var product = await _adminRepository.GetProductById(Id);
            if (product == null)
            {
                return new Responses<string> { StatusCode = 404, Message = "Product Not Found" };
            }

            await _adminRepository.DeleteProduct(product);
            return new Responses<string> { Message = "Product Deleted Successfully", StatusCode = 200 };
        }

        public async Task<Responses<string>> BlockUser(int Id)
        {
            var user = await _adminRepository.GetUserById(Id);
            if (user == null)
            {
                return new Responses<string> { StatusCode = 404, Message = "User Not Found" };
            }

            await _adminRepository.BlockUser(user);
            var message = user.isBlocked ? "User Blocked Successfully" : "User Unblocked Successfully";
            return new Responses<string> { Message = message, StatusCode = 200 };
        }
    }
}