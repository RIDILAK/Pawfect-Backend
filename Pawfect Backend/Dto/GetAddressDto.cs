using System.ComponentModel.DataAnnotations;

namespace Pawfect_Backend.Dto
{
    public class GetAddressDto
    {
        public int AddressId { get; set; }

        public string Name { get; set; }
      
        public string Email { get; set; }
      
        public string Phone { get; set; }

        public string HouseName { get; set; }

        public string Place { get; set; }

        public string PinCode { get; set; }

        public string City { get; set; }

        public string State { get; set; }
    }
}
