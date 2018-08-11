using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
                KeySize = 1024,
                BlockSize = 512
            };
            _aesProvider.GenerateIV();
            _aesProvider.GenerateKey();
            _aesDecryptor = _aesProvider.CreateDecryptor();
            _aesEncryptor = _aesProvider.CreateEncryptor();
        }

        public byte[] GetKeyAES()
        {
            return _aesProvider.Key;
        }

        public byte[] DecryptDataAES(byte[] data)
        {
            return _aesDecryptor.TransformFinalBlock(data, 0, data.Length);
        }

        public byte[] EncryptDataAES(byte[] data)
        {
            return _aesEncryptor.TransformFinalBlock(data, 0, data.Length);
        }
    }
}
