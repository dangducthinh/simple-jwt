// See https://aka.ms/new-console-template for more information
using System.Security.Cryptography;

var keyPath = @"C:\RSA_key";
var privateKeyName = "issuesKey.pem";
var publicKeyName = "verifyKey.pem";
try
{
    using (var rsa = RSA.Create(2048))
    {
        await File.WriteAllTextAsync($"{keyPath}/{privateKeyName}", KeyGenerator.ExportPrivateKey(rsa));
        await File.WriteAllTextAsync($"{keyPath}/{publicKeyName}", KeyGenerator.ExportPublicKey(rsa));
    }
    Console.WriteLine("Key generated");

}
catch (System.Exception ex)
{
    Console.WriteLine("Key generate error: {0}" , ex);
}

