using AutoMapper;
using Pawfect_Backend.Dto;
using Pawfect_Backend.Models;
using Pawfect_Backend.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pawfect_Backend.Services
{
    public interface ICartServices
    {
        Task<Responses<CartViewDto>> GetCartItems(int userId);
        Task<Responses<object>> AddToCart(int userId, int productId);
        Task<Responses<string>> RemoveFromCart(int userId, int productId);
        Task<Responses<CartItem>> IncreaseQuantity(int userId, int productId);
        Task<Responses<CartItem>> DecreaseQuantity(int userId, int productId);
    }

    public class CartServices : ICartServices
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public CartServices(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<Responses<CartViewDto>> GetCartItems(int userId)
        {
            var cart = await _cartRepository.GetCartByUserId(userId);
            if (cart != null)
            {
                var cartItemsDto = cart.CartItems.Select(ci => new CartItemViewDto
                {
                    ProductId = ci.ProductId,
                    ProductName = ci.Product.ProductName,
                    Url = ci.Product.Url,
                    Quantity = ci.Quantity,
                    Price = ci.Product.Price * ci.Quantity,
                }).ToList();

                var cartDto = _mapper.Map<CartViewDto>(cart);
                cartDto.Items = cartItemsDto;
                cartDto.ItemCount = cartItemsDto.Count;
                cartDto.TotalAmount = cartItemsDto.Sum(ci => ci.Price);

                return new Responses<CartViewDto> { StatusCode = 200, Data = cartDto, Message = "Cart Retrieved Successfully" };
            }

            return new Responses<CartViewDto> { StatusCode = 200, Data = null, Message = "Cart is Empty" };
        }

        public async Task<Responses<object>> AddToCart(int userId, int productId)
        {
            try
            {
                var user = await _cartRepository.GetUserWithCart(userId);
                if (user == null)
                {
                    return new Responses<object> { StatusCode = 404, Message = "User not found" };
                }

                var product = await _cartRepository.GetProductById(productId);
                if (product == null)
                {
                    return new Responses<object> { StatusCode = 404, Message = "Product not found" };
                }

                if (product.Quantity == 0)
                {
                    return new Responses<object> { Message = "Product is out of stock", StatusCode = 200 };
                }

                if (user.Cart == null)
                {
                    user.Cart = new Cart
                    {
                        UserId = userId,
                        CartItems = new List<CartItem>()
                    };
                    await _cartRepository.AddCart(user.Cart);
                    await _cartRepository.SaveChanges();
                }

                var existingItem = user.Cart.CartItems.FirstOrDefault(i => i.ProductId == productId);
                if (existingItem != null)
                {
                    if (existingItem.Quantity < product.Quantity)
                    {
                        existingItem.Quantity++;
                        await _cartRepository.SaveChanges();
                        return new Responses<object> { Message = "Quantity increased", StatusCode = 200 };
                    }
                    return new Responses<object> { Message = "Stock limit exceeded", StatusCode = 400 };
                }

                var newItem = new CartItem
                {
                    ProductId = productId,
                    Quantity = 1,
                    CartId = user.Cart.CartId
                };

                await _cartRepository.AddCartItem(newItem);
                await _cartRepository.SaveChanges();
                return new Responses<object> { StatusCode = 200, Message = "Product added to cart" };
            }
            catch (Exception ex)
            {
                return new Responses<object>
                {
                    StatusCode = 500,
                    Message =  ex.Message
                };
            }
        }

        public async Task<Responses<string>> RemoveFromCart(int userId, int productId)
        {
            try
            {
                var user = await _cartRepository.GetUserWithCart(userId);
                if (user == null)
                {
                    return new Responses<string> { StatusCode = 404, Message = "User not found" };
                }

                var deleteItem = user.Cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
                if (deleteItem == null)
                {
                    return new Responses<string> { StatusCode = 404, Message = "Product not found in cart" };
                }

                await _cartRepository.RemoveCartItem(deleteItem);
                await _cartRepository.SaveChanges();
                return new Responses<string> { StatusCode = 200, Message = "Product removed from cart" };
            }
            catch (Exception ex)
            {
                return new Responses<string>
                {
                    StatusCode = 500,
                    Message = ex.Message  
                };
            }
        }

        public async Task<Responses<CartItem>> IncreaseQuantity(int userId, int productId)
        {
            try
            {
                var user = await _cartRepository.GetUserWithCart(userId);
                if (user == null)
                {
                    return new Responses<CartItem> { StatusCode = 404, Message = "User Not Found" };
                }

                var product = await _cartRepository.GetProductById(productId);
                if (product == null)
                {
                    return new Responses<CartItem> { StatusCode = 404, Message = "Product Not Found" };
                }

                var item = user.Cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
                if (item == null)
                {
                    return new Responses<CartItem> { StatusCode = 404, Message = "Product Not Found In The Cart" };
                }

                if (item.Quantity >= 10)
                {
                    return new Responses<CartItem> { StatusCode = 400, Message = "Maximum Quantity reached (10 items)" };
                }

                if (product.Quantity <= item.Quantity)
                {
                    return new Responses<CartItem> { StatusCode = 400, Message = "Out Of Stock" };
                }

                item.Quantity++;
                await _cartRepository.SaveChanges();
                return new Responses<CartItem> { StatusCode = 200, Message = "Quantity Increased Successfully" };
            }
            catch (Exception ex)
            {
                return new Responses<CartItem> { StatusCode = 500, Message = "Internal Server Error" };
            }
        }

        public async Task<Responses<CartItem>> DecreaseQuantity(int userId, int productId)
        {
            try
            {
                var user = await _cartRepository.GetUserWithCart(userId);
                if (user == null)
                {
                    return new Responses<CartItem> { StatusCode = 404, Message = "User Not Found" };
                }

                var product = await _cartRepository.GetProductById(productId);
                if (product == null)
                {
                    return new Responses<CartItem> { StatusCode = 404, Message = "Product is not found" };
                }

                var item = user.Cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
                if (item == null)
                {
                    return new Responses<CartItem> { StatusCode = 404, Message = "Product Not Found in The Cart" };
                }

                if (item.Quantity <= 1)
                {
                    await _cartRepository.RemoveCartItem(item);
                    await _cartRepository.SaveChanges();
                    return new Responses<CartItem> { StatusCode = 200, Message = "Product removed from the cart as quantity reached zero" };
                }

                item.Quantity--;
                await _cartRepository.SaveChanges();
                return new Responses<CartItem> { StatusCode = 200, Message = "Quantity Decreased Successfully" };
            }
            catch (Exception ex)
            {
                return new Responses<CartItem> { StatusCode = 500, Message = "Internal Server Error" };
            }
        }
    }
}