using System.ComponentModel.DataAnnotations;

namespace IdentityProvider.Dto;

public class LoginRequest
{
    [Required]
    public string Username { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}

public class LoginResponse
{
    public string AccessToken { get; set; } = string.Empty;
}
