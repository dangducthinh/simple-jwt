using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using IdentityProvider.AppSetting;
using IdentityProvider.Dto;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet(nameof(HealthCheck))]
    public IActionResult HealthCheck() => Ok("Service ready!");

    [HttpPost(nameof(Login))]
    [AllowAnonymous]
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

        var rsa = ImportPrivateKey();
        var key = new RsaSecurityKey(rsa);
        var credentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, "UserRole"),
            new Claim("Email", "some_email@groovetechnology.vn"),
            new Claim("Phone", "0906787482"),
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddMinutes(_jwtSettings.ExpiredTimeInMinutes),
            SigningCredentials = credentials,
            Issuer = _jwtSettings.Issuer
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private RSA ImportPrivateKey()
    {
        var issuesKey = System.IO.File.ReadAllText(@"C:\RSA_key\issuesKey.pem");
        var base64 = issuesKey.Replace("-----BEGIN PRIVATE KEY-----", "")
                        .Replace("-----END PRIVATE KEY-----", "")
                        .Replace("\n", "")
                        .Replace("\r", "");
        var keyBytes = Convert.FromBase64String(base64);
        var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(keyBytes, out _);
        return rsa;
    }
}