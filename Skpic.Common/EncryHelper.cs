using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Skpic.Common
{
    /// <summary>
    /// 加密类
    /// </summary>
    public static class EncryHelper
    {
        private const string Key = "Guz(%&hj7x89H$yuBI0456FtmaT5&fvHUFCy76*h%(HilJ$lhj!y6&(*jkP87jH7";
        private static readonly SymmetricAlgorithm MobjCryptoService = new RijndaelManaged();

        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="source">待解密的串</param>
        /// <returns>经过解密的串</returns>
        public static string DecryptString(this string source)
        {
            byte[] buffer = Convert.FromBase64String(source);
            var stream = new MemoryStream(buffer, 0, buffer.Length);
            MobjCryptoService.Key = GetLegalKey();
            MobjCryptoService.IV = GetLegalIV();
            var transform = MobjCryptoService.CreateDecryptor();
            var stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read);
            var reader = new StreamReader(stream2);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="source"> 待加密的串</param>
        /// <returns> 经过加密的串</returns>
        public static string EncryptString(this string source)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(source);
            var stream = new MemoryStream();
            MobjCryptoService.Key = GetLegalKey();
            MobjCryptoService.IV = GetLegalIV();
            var transform = MobjCryptoService.CreateEncryptor();
            var stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write);
            stream2.Write(bytes, 0, bytes.Length);
            stream2.FlushFinalBlock();
            stream.Close();
            return Convert.ToBase64String(stream.ToArray());
        }

        private static byte[] GetLegalIV()
        {
            string s = "E4ghj*Ghg7!rNIfb&95GUY86GfghUb#er57HBh(u%g6HJ($jhWk7&!hg4ui%$hjk";
            MobjCryptoService.GenerateIV();
            int length = MobjCryptoService.IV.Length;
            if (s.Length > length)
            {
                s = s.Substring(0, length);
            }
            else if (s.Length < length)
            {
                s = s.PadRight(length, ' ');
            }
            return Encoding.ASCII.GetBytes(s);
        }

        private static byte[] GetLegalKey()
        {
            var key = Key;
            MobjCryptoService.GenerateKey();
            var length = MobjCryptoService.Key.Length;
            if (key.Length > length)
            {
                key = key.Substring(0, length);
            }
            else if (key.Length < length)
            {
                key = key.PadRight(length, ' ');
            }
            return Encoding.ASCII.GetBytes(key);
        }
 
    }
}