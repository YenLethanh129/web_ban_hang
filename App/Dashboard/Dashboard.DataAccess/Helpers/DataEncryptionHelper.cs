using Dashboard.Common.Options;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace Dashboard.DataAccess.Helpers;



public class DataEncryptionHelper
{
    private readonly SecurityOptions _options;

    public DataEncryptionHelper(IOptions<SecurityOptions> options)
    {
        _options = options.Value;
    }

    public string HashPassword(string password)
    {
        var salt = _options.PasswordSalt;
        var hashedBytes = SHA256.HashData(Encoding.UTF8.GetBytes(password + salt));
        return Convert.ToBase64String(hashedBytes);
    }

    public bool VerifyPassword(string enteredPassword, string storedHashedPassword)
    {
        var enteredHashed = HashPassword(enteredPassword);
        return enteredHashed == storedHashedPassword;
    }

    public string EncryptString(string plainText)
    {
        var key = Convert.FromBase64String(_options.EncryptionKey);
        var iv = Convert.FromBase64String(_options.EncryptionIV);

        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
            }
            return Convert.ToBase64String(ms.ToArray());
        }
    }
}
