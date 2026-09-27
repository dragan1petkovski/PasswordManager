using DomainModel;
using LayerData;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace Services
{
	public class SvcEncryption
	{
        private readonly ILogger<SvcEncryption> _logger;
        private readonly EncryptionData _encryptionData;
        private readonly MasterPassword _masterPassword;

        public SvcEncryption(ILogger<SvcEncryption> logger, EncryptionData encryptionData, MasterPassword masterPassword)
        {
            _logger = logger;
            _encryptionData = encryptionData;
            _masterPassword = masterPassword;
            

        }


        public byte[] Encrypt(byte[] plainText)
        {

            AesGcm aes = new AesGcm(_masterPassword.GetAesKey());
            Random rand = new Random();
            byte[] tag = new byte[16];
            byte[] nonce = new byte[12];
            byte[] cipherText = new byte[plainText.Length];
            byte[] cipher = new byte[cipherText.Length + 28];
            rand.NextBytes(nonce);
            aes.Encrypt(nonce, plainText, cipherText, tag, null);
            Buffer.BlockCopy(nonce, 0, cipher, 0, nonce.Length);
            Buffer.BlockCopy(tag, 0, cipher, nonce.Length, tag.Length);
            Buffer.BlockCopy(cipherText, 0, cipher, (nonce.Length + tag.Length), cipherText.Length);
            return cipher;
        }


        public byte[] Decrypt(byte[] cipher)
        {
            AesGcm aes = new AesGcm(_masterPassword.GetAesKey());
            byte[] tag = new byte[16];
            byte[] nonce = new byte[12];
            byte[] cipherText = new byte[cipher.Length - 28];
            byte[] plainText = new byte[cipher.Length - 28];
            Buffer.BlockCopy(cipher, 0, nonce, 0, nonce.Length);
            Buffer.BlockCopy(cipher, nonce.Length, tag, 0, tag.Length);
            Buffer.BlockCopy(cipher, (nonce.Length + tag.Length), cipherText, 0, cipherText.Length);

            aes.Decrypt(nonce, cipherText, tag, plainText, null);
            return plainText;

        }

    }
}
