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

        //AES加密轉16進制
        //public string AesEncryptHex(string input, string key, string iv)
        //{
        //    string encryptHexResult = string.Empty;
        //    if (!string.IsNullOrEmpty(input))
        //    {
        //        byte[] b_Input = Encoding.UTF8.GetBytes(input);
        //        var encryptHexValue = Encoding.UTF8.GetBytes(AesEncrypt(b_Input, key, iv));

        //        encryptHexResult = BitConverter.ToString(encryptHexValue)?.Replace("-", string.Empty)?.ToLower();
        //    }
        //    return encryptHexResult;
        //}

        // 去除PKCS7的Padding???
        public byte[] RemovePKCS7Padding(byte[] data)
        {
            int indexLength = data[data.Length - 1];
            var outputData = new byte[data.Length - indexLength];
            Buffer.BlockCopy(data, 0, outputData, 0, outputData.Length);
            return outputData;
        }
        //AES解密
        public string AesDecrypt(byte[] input, string key, string iv)
        {
            var aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.None;
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = Encoding.UTF8.GetBytes(iv);

            var decryptor = aes.CreateDecryptor();

            byte[] decryptResult = decryptor.TransformFinalBlock(input, 0, input.Length);
            return Encoding.UTF8.GetString(RemovePKCS7Padding(decryptResult));
        }
        public string DecryptAESHex(string source, string cryptoKey, string cryptoIV)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(source))
            {
                // 將 16 進制字串 轉為 byte[] 後
                byte[] sourceBytes = ToByteArray(source);

                if (sourceBytes != null)
                {
                    // 使用金鑰解密後，轉回 加密前 value
                    result = Encoding.UTF8.GetString(DecryptAES(sourceBytes, cryptoKey, cryptoIV)).Trim();
                }
            }

            return result;
        }

        public byte[] ToByteArray(string source)
        {
            byte[] result = null;

            if (!string.IsNullOrWhiteSpace(source))
            {
                var outputLength = source.Length / 2;
                var output = new byte[outputLength];

                for (var i = 0; i < outputLength; i++)
                {
                    output[i] = Convert.ToByte(source.Substring(i * 2, 2), 16);
                }
                result = output;
            }

            return result;
        }
        public static byte[] DecryptAES(byte[] source, string cryptoKey, string cryptoIV)
        {
            byte[] dataKey = Encoding.UTF8.GetBytes(cryptoKey);
            byte[] dataIV = Encoding.UTF8.GetBytes(cryptoIV);

            using (var aes = System.Security.Cryptography.Aes.Create())
            {
                aes.Mode = System.Security.Cryptography.CipherMode.CBC;
                // 智付通無法直接用PaddingMode.PKCS7，會跳"填補無效，而且無法移除。"
                // 所以改為PaddingMode.None並搭配RemovePKCS7Padding
                aes.Padding = System.Security.Cryptography.PaddingMode.None;
                aes.Key = dataKey;
                aes.IV = dataIV;

                using (var decryptor = aes.CreateDecryptor())
                {
                    byte[] data = decryptor.TransformFinalBlock(source, 0, source.Length);
                    int iLength = data[data.Length - 1];
                    var output = new byte[data.Length - iLength];
                    Buffer.BlockCopy(data, 0, output, 0, output.Length);
                    return output;
                }
            }
        }

    }
}
