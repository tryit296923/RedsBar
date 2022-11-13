using System.Security.Cryptography;
using System.Text;

namespace Alcoholic.Services
{
    public class HashService
    {
        public string GetHash(string input)
        {
            SHA256 sHA256 = SHA256.Create();
            byte[] bytes = sHA256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(bytes, 0, bytes.Length)?.Replace("-", "")?.ToLower(); ;
        }
        public string GetHashHex(string input)
        {
            SHA256 sHA256 = SHA256.Create();
            byte[] bytes = sHA256.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder builder = new StringBuilder();

            return BitConverter.ToString(bytes, 0, bytes.Length)?.Replace("-", "")?.ToLower(); ;
            
        }
    }
}
