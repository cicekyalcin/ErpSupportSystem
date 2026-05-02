using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ErpSupport.Api.Models;
using ErpSupport.Api.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ErpSupport.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ErpSupportDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ErpSupportDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            // 1. Kullanıcıyı veritabanında ara
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username && u.IsActive);

            if (user == null)
            {
                return Unauthorized(new { message = "Kullanıcı adı veya şifre hatalı." });
            }

            // Not: Gerçek canlıya alım öncesi BCrypt ile şifreleme ekleyeceğiz. 
            // Şu an prototipleme ve test aşamasında olduğumuz için doğrudan eşleştiriyoruz.
            if (user.PasswordHash != loginDto.Password)
            {
                return Unauthorized(new { message = "Kullanıcı adı veya şifre hatalı." });
            }

            // 2. Kullanıcı doğru, JWT Token (Bilet) Üretimi
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim("UserId", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role), // 'Admin' veya 'SupportStaff'
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Token 8 saat geçerli olacak (Bir tam mesai günü)
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(10),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                role = user.Role,
                fullName = user.FullName
            });
        }
    }
}