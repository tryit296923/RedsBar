using System.Drawing.Imaging;
using System.Security.Cryptography;
using System.Text;

namespace Alcoholic.Services
{
    public class AesService
    {
        //AES加密
        public string AesEncrypt (byte[] input, string key, string iv)
        {
            var aes = Aes.Create();
            byte[] infoKey = Encoding.UTF8.GetBytes (key);
            byte[] infoIv = Encoding.UTF8.GetBytes (iv);

            aes.Mode = CipherMode.CBC;  //加密模式
            aes.Padding = PaddingMode.PKCS7;  //填充方式
            aes.Key = infoKey;
            aes.IV = infoIv;
            
            var encryptor = aes.CreateEncryptor ();
            
            byte[] encryptResult = encryptor.TransformFinalBlock(input, 0, input.Length);
            return BitConverter.ToString(encryptResult)?.Replace("-", "")?.ToLower();
            
        }

        //加密轉16進制
        //public string AesEncryptHex(string input, string key, string iv)
        //{
        //    string hexResult = string.Empty;
        //    if (!string.IsNullOrEmpty(input))
        //    {
        //        byte[] bInput = Encoding.UTF8.GetBytes (input);
        //        var encryptHexValue = AesEncrypt(bInput, key, iv);
        //        if (encryptHexValue != null)
        //        {
        //            hexResult = BitConverter.ToString(encryptHexValue).Replace("-", string.Empty).ToLower();
        //        }
        //    }
        //    return hexResult;
        //}

        //AES解密
        //public byte[] AesDecrypt(byte[] input, string key, string iv)
        //{
        //    var aes = Aes.Create();
        //    aes.Mode = CipherMode.CBC;
        //    aes.Padding = PaddingMode.PKCS7;
        //    aes.Key = Encoding.UTF8.GetBytes(key);
        //    aes.IV = Encoding.UTF8.GetBytes(iv);

        //    var decryptor = aes.CreateDecryptor();

        //    byte[] decryptResult = decryptor.TransformFinalBlock(input, 0, input.Length);

        //}
    }
}
