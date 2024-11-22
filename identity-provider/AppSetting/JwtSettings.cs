namespace IdentityProvider.AppSetting;

public class JwtSettings
{
    public string SecretKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public int ExpiredTimeInMinutes { get; set; } = 5;
}