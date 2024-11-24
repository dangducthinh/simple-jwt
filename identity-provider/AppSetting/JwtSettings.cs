namespace IdentityProvider.AppSetting;

public class JwtSettings
{
    public string Issuer { get; set; } = string.Empty;
    public int ExpiredTimeInMinutes { get; set; } = 5;
}