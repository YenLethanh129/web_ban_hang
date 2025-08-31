using System.Security.Cryptography;
using System.Text;

namespace Dashboard.Common.Utitlities;
public class CodeGenerator
{
    private const string Alphabet = "ABCDEFGHJKMNPQRSTVWXYZ23456789";

    /// <summary>
    /// Generates a general code that can be used for various types of objects (customer orders, supplier orders, invoices, etc.)
    /// </summary>
    /// <param name="prefix">Prefix (e.g., "ORD", "SUP", "INV")</param>
    /// <param name="existsAsync">Delegate to check existence (if you have a repository, pass it in to avoid duplicate codes)</param>
    /// <param name="randomLength">Length of the random part (default: 4)</param>
    /// <returns>Unique code following the format: Prefix + yyyyMMdd + Random + CheckChar</returns>
    public static async Task<string> GenerateCodeAsync(
        string prefix,
        Func<string, Task<bool>>? existsAsync = null,
        int randomLength = 4)
    {
        if (string.IsNullOrWhiteSpace(prefix))
            throw new ArgumentException("Prefix cannot be null!", nameof(prefix));

        const int maxRetries = 5;

        for (int attempt = 0; attempt < maxRetries; attempt++)
        {
            string datePart = DateTime.UtcNow.ToString("ddMMyyyy"); 
            string randomPart = RandomString(randomLength);
            string body = $"{prefix}{datePart}{randomPart}"; 
            char check = ComputeCheckChar(body); 
            string code = body + check;

            if (existsAsync == null)
                return code;

            bool exists = await existsAsync(code);
            if (!exists)
                return code;
        }

        throw new InvalidOperationException("Operation can not be performed after serveral attempts, please check it again.");
    }

    private static string RandomString(int length)
    {
        var sb = new StringBuilder(length);
        for (int i = 0; i < length; i++)
        {
            int idx = RandomNumberGenerator.GetInt32(0, Alphabet.Length);
            sb.Append(Alphabet[idx]);
        }
        return sb.ToString();
    }

    private static char ComputeCheckChar(string input)
    {
        int acc = 0;
        foreach (char c in input)
        {
            int pos = Alphabet.IndexOf(c);
            if (pos < 0) pos = c % Alphabet.Length;
            acc = (acc * 31 + pos) % Alphabet.Length;
        }
        return Alphabet[acc];
    }
}
