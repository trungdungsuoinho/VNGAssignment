using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VNGAssignment.Helpers;
using VNGAssignment.Models;
using VNGAssignment.Services;

namespace VNGAssignment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IUserService userService, IOptions<JwtSettings> jwtSettings) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly IOptions<JwtSettings> _jwtSettings = jwtSettings;

        [HttpPost("token")]
        public async Task<IActionResult> GenerateToken([FromBody] LoginModel model)
        {
            if (await _userService.VerifyPassword(model))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSettings.Value.SecretKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity([new Claim(ClaimTypes.Name, model.Username)]),
                    Expires = DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings.Value.ExpiryMinutes)),
                    Issuer = jwtSettings.Value.Issuer,
                    Audience = jwtSettings.Value.Audience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);
                return Ok(new { Token = tokenString });
            }
            return Unauthorized();
        }
    }
}
