using HappyWay.Domain;
using HappyWay.Infrastructure;
using HappyWay.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;




namespace HappyWay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var existingUser =
                (await _unitOfWork.Users.GetAllAsync()).
                FirstOrDefault(u => u.UserName == request.UserName || u.Email == request.Email);
            if (existingUser !=null )
            {
                return BadRequest("User with this username or email already exists.");
            }
            var newUser = new AppUser
            {
                UserName = request.UserName,
                Email = request.Email,
                Role = "Customer", // Default role
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _unitOfWork.Users.AddAsync(newUser);
           await _unitOfWork.SaveAsync();

            return Ok("Dang Ky Thanh Cong");

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
         
            var user = (await _unitOfWork.Users.GetAllAsync())
                .FirstOrDefault(u => u.UserName == request.UserName);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid username or password.");
            }

            // Tạo JWT Token
            var token = GenerateJwtToken(user);
            return Ok(new LoginResponse
            {
                Token = token,
                UserName = user.UserName,
                Role = user.Role
            });
        }
        private string GenerateJwtToken(AppUser user)
        {
            var claims = new[]
          {
                new Claim(ClaimTypes.Role,string.Join(";",user.Role)),
                new Claim(ClaimTypes.Name,user.UserName)
            };
            //Ma Hoa Bang Thu Vien SymmerTric
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_configuration["Tokens:Issuer"],
                _configuration["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(7),
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
            /* var claims = new[]
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName,user.FirstName),
                new Claim(ClaimTypes.Role, string.Join(";",roles)),
                new Claim(ClaimTypes.Name, request.UserName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);*/

        }

    }
}
