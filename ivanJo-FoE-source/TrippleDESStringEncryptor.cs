using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace ForgeBot
{
    public class TripleDESStringEncryptor
    {
        private byte[] _key;
        private byte[] _iv;
        private TripleDESCryptoServiceProvider _provider;

        public TripleDESStringEncryptor()
        {
            var asm = Assembly.GetExecutingAssembly();
            var guid = asm.GetType().GUID.ToString().Replace("-", string.Empty);
            _key = Encoding.ASCII.GetBytes(guid.Substring(0,24));
            _iv = Encoding.ASCII.GetBytes(guid.Substring(24, 8));
            _provider = new TripleDESCryptoServiceProvider();
        }

        #region IStringEncryptor Members

        public string EncryptString(string plainText)
        {
            return Transform(plainText, _provider.CreateEncryptor(_key, _iv));
        }

        public string DecryptString(string encryptedText)
        {
            return Transform(encryptedText, _provider.CreateDecryptor(_key, _iv));
        }

        #endregion

        private string Transform(string text, ICryptoTransform transform)
        {
            if (text == null)
            {
                return null;
            }
            using (var stream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Write))
                {
                    byte[] input = Encoding.Default.GetBytes(text);
                    cryptoStream.Write(input, 0, input.Length);
                    cryptoStream.FlushFinalBlock();

                    return Encoding.Default.GetString(stream.ToArray());
                }
            }
        }
    }
}
