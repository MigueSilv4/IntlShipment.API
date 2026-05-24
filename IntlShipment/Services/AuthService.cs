using IntlShipment.Data;
using IntlShipment.Helpers;
using IntlShipment.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IntlShipment.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        public async Task<Response<string>> Login(string email, string password)
        {
            var response = new Response  <string>();
            try
            {
                var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
                if (user is null)
                {
                    response.Message = "Credenciales incorrectas.";
                    response.Success = false;
                    return response;
                }

                response.Data = GenerarToken(user);
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = "Ocurrió un error interno.";
                response.Success = false;
                return response;
            }
        }

        public async Task<Response<User>> Register(User user)
        {
            var response = new Response<User>();
            try
            {
                if (await _db.Users.AnyAsync(u => u.Email == user.Email))
                {
                    response.Message = "El email ya está registrado.";
                    response.Success = false;
                    return response;
                }

                _db.Users.Add(user);
                await _db.SaveChangesAsync();
                response.Data = user;
                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = "Ocurrió un error interno.";
                response.Success = false;
                return response;
            }
        }

        private string GenerarToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email!),
                new Claim(ClaimTypes.Role, user.Rol!)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
