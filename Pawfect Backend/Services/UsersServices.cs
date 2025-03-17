using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Pawfect_Backend.Dto;
using Pawfect_Backend.Models;
using Pawfect_Backend.Repositories;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Pawfect_Backend.Services
{
    public interface IUserServices
    {
        Task<Responses<string>> SignUpUsers(SignUpDto AddUser);
        Task<Responses<string>> LoginUsers(LoginDto LoginUser);
        string GenerateToken(User user);
    }

    public class UsersServices : IUserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public UsersServices(IUserRepository userRepository, IMapper mapper, IConfiguration config)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _config = config;
        }

        public async Task<Responses<string>> SignUpUsers(SignUpDto AddUser)
        {
            var existingUser = await _userRepository.GetUserByEmail(AddUser.Email);
            if (existingUser != null)
            {
                return new Responses<string> { Message = "Email Already Exists", StatusCode = 409 };
            }

            var user = _mapper.Map<User>(AddUser);
            user.Password = HashPassword(AddUser.Password);
            user.Role = "User";

            try
            {
                await _userRepository.AddUser(user);
                return new Responses<string> { Message = "Registration Successfully Completed", StatusCode = 200 };
            }
            catch (Exception ex)
            {
                return new Responses<string> { Message = "Something went wrong: " + ex.Message, StatusCode = 500 };
            }
        }

        public async Task<Responses<string>> LoginUsers(LoginDto LoginUser)
        {
            var user = await _userRepository.GetUserByEmail(LoginUser.Email);
            if (user == null)
            {
                return new Responses<string> { Message = "Invalid User", StatusCode = 400 };
            }

            if (user.isBlocked)
            {
                return new Responses<string> { Message = "You are blocked by Admin", StatusCode = 403 };
            }

            var verifiedPass = VerifiedPassword(LoginUser.Password, user.Password);
            if (!verifiedPass)
            {
                return new Responses<string> { StatusCode = 401, Message = "Invalid password" };
            }

            return new Responses<string> { Data = GenerateToken(user), StatusCode = 200,Message=user.Role };
        }

        public string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name)  
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifiedPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}