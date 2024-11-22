using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IdentityProvider.AppSetting;
using IdentityProvider.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace IdentityProvider.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtSettings _jwtSettings;
    public AuthController(JwtSettings jwtSettings)
    {
        _jwtSettings = jwtSettings;
    }

    [HttpGet("HealthCheck")]
    public IActionResult HealthCheck() => Ok("Service ready!");

    [HttpPost("login")]
    public ActionResult<LoginResponse> Login(LoginRequest request)
    {
        // Hardcoded user validation
        if (request.Username == "user" && request.Password == "password")
        {
            var token = GenerateJwtToken(request.Username);
            return Ok(new LoginResponse { AccessToken = token });
        }

        return Unauthorized();
    }

    private string GenerateJwtToken(string username)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiredTimeInMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _jwtSettings.Issuer
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}