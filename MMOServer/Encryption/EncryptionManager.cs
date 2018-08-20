using MMOServer.ConsoleStuff;
using MMOServer.Other;
using System.IO;
using System.Security.Cryptography;

namespace MMOServer.Encryption
{
    class EncryptionManager
    {
        private AesCryptoServiceProvider _aesProvider;
        private ICryptoTransform _aesEncryptor;
        private ICryptoTransform _aesDecryptor;

        public EncryptionManager()
        {
            Setup();
        }

        private void Setup()
        {
            _aesProvider = new AesCryptoServiceProvider
            {
                KeySize = 256,
                BlockSize = 128,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
            };
            _aesProvider.GenerateIV();
            _aesProvider.GenerateKey();
            _aesDecryptor = _aesProvider.CreateDecryptor();
            _aesEncryptor = _aesProvider.CreateEncryptor();
            ConsoleUtils.Info("Encryption set up successfully");
        }

        public byte[] GetKeyAES()
        {
            return _aesProvider.Key;
        }

        public byte[] GetIVAes()
        {
            return _aesProvider.IV;
        }

        public byte[] EncryptDataAES(byte[] block)
        {
            return _aesEncryptor.TransformFinalBlock(block, 0, block.Length); //TODO: FIND OUT WHETHER USING THIS IS ANY DIFFERENT FROM USING A STREAM
            //using (MemoryStream memoryStream = new MemoryStream())
            //{
            //    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, _aesEncryptor, CryptoStreamMode.Write))
            //    {
            //        cryptoStream.Write(block, 0, block.Length);
            //        cryptoStream.FlushFinalBlock();
            //        return memoryStream.ToArray();
            //    }
            //}
        }

        public byte[] DecryptDataAES(byte[] block)
        {
            //var data = _aesDecryptor.TransformFinalBlock(block, 0, block.Length);
            //System.Console.WriteLine(Utils.GetHexString(block));
            //System.Console.WriteLine(Utils.GetHexString(data));
            //return data;
            using (MemoryStream memoryStream = new MemoryStream(block))
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, _aesDecryptor, CryptoStreamMode.Read))
                {
                    var output = new byte[block.Length];
                    cryptoStream.Read(output, 0, output.Length);
                    System.Console.WriteLine(Utils.GetHexString(block));
                    System.Console.WriteLine(Utils.GetHexString(output));
                    return output;
                }
            }
        }
    }
}
