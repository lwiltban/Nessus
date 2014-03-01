using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Nessus.Utils
{
    /// <summary>
    /// Symmetric Encryption helper class to encrypt/decrypt Nessus tokens
    /// </summary>
    public class SymmetricEncryption
    {
		private static byte[] KEY = new byte[24] { 81, 104, 117, 96, 31, 80, 109, 104, 117, 104, 99, 39, 95, 110, 100, 32, 86, 105, 111, 108, 97, 32, 32, 32 };
		private static byte[] IV = new byte[8] { 72, 75, 84, 86, 81, 72, 85, 64 };
		
        public static string Encrypt(string original)
		{
            if (original == null || original == string.Empty || original.Length == 0)
                return string.Empty;

			using (var key = new TripleDESCryptoServiceProvider { Key = KEY, IV = IV })
			{
				byte[] buffer = Encrypt(original, key);
				return ToHexString(buffer);
			}
		}

        public static string Decrypt(string s)
        {
            if (s == null || s == string.Empty || s.Length == 0)
                return string.Empty;

            using (var key = new TripleDESCryptoServiceProvider { Key = KEY, IV = IV })
            {
                var buffer = ToByteArray(s);
                return Decrypt(buffer, key);
            }
        }

        private static byte[] Encrypt(string PlainText, SymmetricAlgorithm key)
        {
            MemoryStream ms = new MemoryStream();
            CryptoStream encStream = new CryptoStream(ms, key.CreateEncryptor(), CryptoStreamMode.Write);
            StreamWriter sw = new StreamWriter(encStream);
            sw.WriteLine(PlainText);
            sw.Close();
            byte[] buffer = ms.ToArray();
            return buffer;
        }

        private static string ToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 2);
            for (int i = 0; i < data.Length; i++)
                sb.Append(data[i].ToString("X2"));
            return sb.ToString();
        }

        private static byte[] ToByteArray(string hexString)
        {
            if (hexString == null)
                throw new ArgumentNullException("hexString");
            if ((hexString.Length & 1) != 0)
                throw new ArgumentException("hexString must contain an even number of characters.");
            byte[] result = new byte[hexString.Length / 2];
            for (int i = 0; i < hexString.Length; i += 2)
                result[i / 2] = byte.Parse(hexString.Substring(i, 2), NumberStyles.HexNumber);
            return result;
        }

        private static string Decrypt(byte[] CypherText, SymmetricAlgorithm key)
        {
            string val = string.Empty;
            MemoryStream ms = new MemoryStream(CypherText);
            CryptoStream encStream = new CryptoStream(ms, key.CreateDecryptor(), CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(encStream);
            val = sr.ReadLine();
            sr.Close();
            return val;
        }
    }
}