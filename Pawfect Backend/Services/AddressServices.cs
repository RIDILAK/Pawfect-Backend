using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Pawfect_Backend.Dto;
using Pawfect_Backend.Models;
using Pawfect_Backend.Repositories;

namespace Pawfect_Backend.Services
{
    public interface IAddressService
    {
        Task<Responses<string>> AddAddress(int userId, AddressCreateDto newAddress);
        Task<Responses<List<GetAddressDto>>> GetAddresses(int userId);
        Task<Responses<string>> RemoveAddress(int userId, int addressId);
    }

    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;

        public AddressService(IAddressRepository addressRepository, IMapper mapper)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
        }

        public async Task<Responses<string>> AddAddress(int userId, AddressCreateDto newAddress)
        {
            if (newAddress == null)
            {
                return new Responses<string> { StatusCode = 404, Message = "Address Cannot be Null" };
            }

            var userAddresses = await _addressRepository.GetAddressesByUserIdAsync(userId);
            if (userAddresses.Count >= 5)
            {
                return new Responses<string> { StatusCode = 400, Message = "Maximum number of addresses reached" };
            }

            var address = _mapper.Map<Address>(newAddress);
            address.userId = userId;

            await _addressRepository.AddAddress(address);

            return new Responses<string> { StatusCode = 200, Message = "Address Added" };
        }

        public async Task<Responses<List<GetAddressDto>>> GetAddresses(int userId)
        {
            var addresses = await _addressRepository.GetAddressesByUserIdAsync(userId);
            if (addresses.Count == 0)
            {
                return new Responses<List<GetAddressDto>> { StatusCode = 404, Message = "Address not found" };
            }

            var mappedAddresses = _mapper.Map<List<GetAddressDto>>(addresses);
            return new Responses<List<GetAddressDto>> { StatusCode = 200, Message = "Address Retrieved Successfully", Data = mappedAddresses };
        }

        public async Task<Responses<string>> RemoveAddress(int userId, int addressId)
        {
            var address = await _addressRepository.GetAddressById(addressId);
            if (address == null || address.userId != userId)
            {
                return new Responses<string> { StatusCode = 404, Message = "Address Not Found" };
            }

            await _addressRepository.RemoveAddress(address);
            return new Responses<string> { StatusCode = 200, Message = "Address Removed Successfully" };
        }
    }
}