using System.Security.Cryptography;

namespace MMOServer.Encryption
{
    class ClientEncryptionInfo
    {
        private RSACryptoServiceProvider _rsaProvider;

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
