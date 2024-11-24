using System.Security.Cryptography;
using System.Text;

public static class KeyGenerator
{
    public static string ExportPrivateKey(RSA rsa)
    {
        var privateKey = rsa.ExportRSAPrivateKey();
        var builder = new StringBuilder();
        builder.AppendLine("-----BEGIN PRIVATE KEY-----");
        builder.AppendLine(Convert.ToBase64String(privateKey));
        builder.AppendLine("-----END PRIVATE KEY-----");
        return builder.ToString();
    }

    public static string ExportPublicKey(RSA rsa)
    {
        var publicKey = rsa.ExportSubjectPublicKeyInfo();
        var builder = new StringBuilder();
        builder.AppendLine("-----BEGIN PUBLIC KEY-----");
        builder.AppendLine(Convert.ToBase64String(publicKey));
        builder.AppendLine("-----END PUBLIC KEY-----");
        return builder.ToString();
    }
}