using AutoMapper;
using Pawfect_Backend.Dto;
using Pawfect_Backend.Models;
using Pawfect_Backend.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pawfect_Backend.Services
{
    public interface IWishListServices
    {
        Task<Responses<string>> AddOrRemoveWishList(int productId, int userId);
        Task<Responses<List<getWishListDto>>> GetWishList(int userId);
    }

    public class WishListServices : IWishListServices
    {
        private readonly IWishListRepository _wishListRepository;
        private readonly IMapper _mapper;

        public WishListServices(IWishListRepository wishListRepository, IMapper mapper)
        {
            _wishListRepository = wishListRepository;
            _mapper = mapper;
        }

        public async Task<Responses<string>> AddOrRemoveWishList(int productId, int userId)
        {
            try
            {
                var productExists = await _wishListRepository.ProductExistsAsync(productId);
                if (!productExists)
                {
                    return new Responses<string> { StatusCode = 200, Message = "Product does not exist" };
                }

                var wishListItem = await _wishListRepository.GetWishListItemAsync(productId, userId);
                if (wishListItem != null)
                {
                    await _wishListRepository.RemoveWishListItemAsync(wishListItem);
                    return new Responses<string> { StatusCode = 200, Message = "Product removed from wishlist" };
                }
                else
                {
                    var newWishListItem = new WishList
                    {
                        ProductId = productId,
                        UserId = userId,
                    };
                    await _wishListRepository.AddWishListItemAsync(newWishListItem);
                    return new Responses<string> { StatusCode = 200, Message = "Product added to wishlist" };
                }
            }
            catch (Exception ex)
            {
                return new Responses<string> { StatusCode = 500, Message = ex.Message };
            }
        }

        public async Task<Responses<List<getWishListDto>>> GetWishList(int userId)
        {
            try
            {
                if (userId == 0)
                {
                    return new Responses<List<getWishListDto>> { StatusCode = 401, Message = "User not authorized" };
                }

                var wishListItems = await _wishListRepository.GetWishListByUserIdAsync(userId);
                if (wishListItems.Count > 0)
                {
                    var wishListDtos = wishListItems.Select(x => new getWishListDto
                    {
                        WishListId = x.WishListId,
                        ProductName = x.Product.ProductName,
                        Url = x.Product.Url,
                        Price = x.Product.Price,
                        Rating = x.Product.Rating,
                        Description = x.Product.Description
                    }).ToList();

                    return new Responses<List<getWishListDto>> { StatusCode = 200, Message = "Get wishlist success", Data = wishListDtos };
                }

                return new Responses<List<getWishListDto>> { StatusCode = 200, Message = "Wishlist is empty" };
            }
            catch (Exception ex)
            {
                return new Responses<List<getWishListDto>> { StatusCode = 500, Message = ex.Message };
            }
        }
    }
}