using AutoMapper;
using Pawfect_Backend.Dto;
using Pawfect_Backend.Models;
using Pawfect_Backend.Repositories;

namespace Pawfect_Backend.Services
{
    public interface IOrderServices
    {
       
        public Task<Responses<string>> Create(int userId, CreateOrderDto createOrderDto);
       public Task <Responses<List<ViewUserOrderDetailsDto>>> GetOrderDetails(int userId);
        public Task<Responses<List<ViewUserOrderDetailsDto>>> GetAllOrders();
        public Task<Responses<TotalRevenueDto>>GetRevenue();
        public Task<Responses<AddStatusDto>> ChangeStatus(int OrderId,string status);
       

    }
    public class OrderServices : IOrderServices
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderServices(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<Responses<string>> Create(int userId, CreateOrderDto createOrderDto)
        {
            var address = await _orderRepository.GetAddressById(createOrderDto.AddressId, userId);
            if (address == null)
            {

                return new Responses<string> { StatusCode = 400, Message = "Address is wrong" };

            }

            var cart = await _orderRepository.GetCartByUserId(userId);
            if (cart == null)
            {
                return new Responses<string> { StatusCode = 404, Message = "Cart Is Empty" };
            }
                var order = new Orders
                {
                    userId = userId,
                    OrderTime = DateTime.Now,
                    AddressId = createOrderDto.AddressId,
                    TotalPrice = createOrderDto.TotalAmount,
                    OrderStatus="Pending",
                    TransactionId = createOrderDto.TransactionId,
                    OrderItems = cart.CartItems.Select(c => new OrderItem
                    {
                        ProductId = c.ProductId,
                        Quantity = c.Quantity,
                        TotalPrice = c.Quantity * c.Product.Price,

                    }).ToList()
                };
                foreach (var cartItem in cart.CartItems)
                {

                    var product = cartItem.Product;
                    if (product.Quantity < cartItem.Quantity)
                    {
                        return new Responses<string> { StatusCode = 400, Message = "Out ofStock" };
                    }

                product.Quantity = cartItem.Quantity;
                await _orderRepository.updateQuantity(product);

                 }

          await _orderRepository.CreateOrder(order);
            await _orderRepository.RemoveCart(cart);
            return new Responses<string> { StatusCode = 200, Message = "Order  create Succesfully" };


        }

        public async Task<Responses<List<ViewUserOrderDetailsDto>>> GetOrderDetails(int userId)
        {
            var orders=await _orderRepository.GetOrderByUserByid(userId);
            if(orders == null)
            {
                return new Responses<List<ViewUserOrderDetailsDto>> { StatusCode = 400, Message = "Order Not Found" };
            }

            var orderDetails = orders.Select(o => new ViewUserOrderDetailsDto
            {
                Id = o.OrderId,
                OrderId = o.OrderId,
                TotalPrice = o.OrderItems.Sum(oi => oi.TotalPrice),
                OrderDate = o.OrderTime,
                OrderStatus= o.OrderStatus,
                TransactionId = o.TransactionId,
                Address = _mapper.Map<AddressCreateDto>(o.Address),
                OrderProduct = _mapper.Map<List<OrderViewDto>>(o.OrderItems.ToList()),
            }).ToList();
            return new Responses<List<ViewUserOrderDetailsDto>> { StatusCode = 200, Message = "Order Retrived Succesfully",Data=orderDetails };


        }

        public async Task<Responses<List<ViewUserOrderDetailsDto>>> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllOrder();
            if (orders == null)
            {
                return new Responses<List<ViewUserOrderDetailsDto>> { StatusCode = 404, Message = "No Orders Found" };
            }
            var orderDetails = orders.Select(o => new ViewUserOrderDetailsDto
            {
                Id = o.OrderId,
                OrderId = o.OrderId,
                TotalPrice = o.OrderItems.Sum(oi => oi.TotalPrice),
                OrderDate = o.OrderTime,
                TransactionId = o.TransactionId,
                Address = _mapper.Map<AddressCreateDto>(o.Address),
                OrderProduct = _mapper.Map<List<OrderViewDto>>(o.OrderItems.ToList()),
            }).ToList();
            return new Responses<List<ViewUserOrderDetailsDto>> { StatusCode = 200, Message = "Orders Retrived Succesfully", Data = orderDetails };
        }

        public async Task<Responses<TotalRevenueDto>> GetRevenue()
        {
            var orderItems = await _orderRepository.GetOrderItems();
            var amount = orderItems.Sum(oi => oi.TotalPrice);
            var Items = orderItems.Sum(oi => oi.Quantity);

            return new Responses<TotalRevenueDto> { StatusCode = 200, Message = "Revenue Retrived Succesfully", Data = new TotalRevenueDto {TotalRevenue=amount,TotalItemsSold=Items } };
        }

        public async Task<Responses<AddStatusDto>> ChangeStatus(int OrderId, string status)
        {
            string[] CheckStatus = { "Pending", "Processing", "Shipped", "Delivered", "Cancelled", "Returned" };
            if (CheckStatus.Contains(status)){ 
            var order = await _orderRepository.GetOrderById(OrderId);

            if (order == null)
            {
                return new Responses<AddStatusDto> { StatusCode = 400, Message = "Order not found" };
            }

            await _orderRepository.UpdateOrderStatus(order, status);

                return new Responses<AddStatusDto>
                {
                    StatusCode = 200,
                    Message = "Order status updated successfully",
                    Data = new AddStatusDto { OrderStatus = order.OrderStatus }
                };
            }
            else
            {
                return new Responses<AddStatusDto>
                {
                    StatusCode = 404,
                    Message = "Invalid Status"
                };
            }

        }


    }
}
