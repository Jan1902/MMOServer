using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MMOServer.Encryption
{
    class ClientEncryptionInfo
    {
        private RSACryptoServiceProvider _rsaProvider; //RSA is used only once to safely send the AES symetric key to the client for later encryption

        public bool RSAEncryptionEnabled { get; private set; }
        public bool AESEncryptionEnabled { get; set; }

        public byte[] PublicKey { get; private set; }

        public void EnableRSAEncryption(byte[] key)
        {
            PublicKey = key;
            _rsaProvider = new RSACryptoServiceProvider();
            _rsaProvider.ImportCspBlob(key);
            RSAEncryptionEnabled = true;
        }

        public void DisableRSAEncryption()
        {
            _rsaProvider.Dispose();
            RSAEncryptionEnabled = false;
        }

        public byte[] EncryptData(byte[] data)
        {
            return _rsaProvider.Encrypt(data, false);
        }
    }
}
