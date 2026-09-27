using DataLayer;
using DomainModel;
using LayerData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace Services
{
    public class StartUpEncryptionValidation
    {
        private readonly EncryptionData _data;
        private readonly ILogger<StartUpEncryptionValidation> _logger;

        public StartUpEncryptionValidation(EncryptionData data, ILogger<StartUpEncryptionValidation> logger)
        {
            _data = data;
            _logger = logger;

        }

        private (bool, AppInit) IsSingleRow()
        {
            List<AppInit> init = this._data.Read<AppInit>().ToList();
            switch (init.Count)
            {
                case 0:
                    return (false, null);
                case 1:
                    return (true, init[0]);
                case > 1:
                    return (false, init[0]);
                default:
                    return (false, null);
            }
        }
        public byte[] GetAesKey(CngKey keystore,AppInit appInit)
        {

            RSACng rsa = new RSACng(keystore);

            byte[] _salt = new byte[64];
            byte[] _interations = new byte[appInit.mk.Length - 64];

            Buffer.BlockCopy(appInit.mk, 0, _salt, 0, _salt.Length);
            Buffer.BlockCopy(appInit.mk, _salt.Length, _interations, 0, _interations.Length);

            return Rfc2898DeriveBytes.Pbkdf2(rsa.ExportRSAPrivateKey(), _salt, BitConverter.ToInt32(_interations, 0), HashAlgorithmName.SHA512, 32);
        }

        public byte[] Decrypt(byte[] cipher, AppInit appInit)
        {
            CngKey keystore = CngKey.Open(appInit.id.ToString());
            AesGcm aes = new AesGcm(GetAesKey(keystore, appInit));
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
        public bool IsPrivateKeyValid()
        {
            (bool _isSingleRow, AppInit initData) = IsSingleRow();
            if(_isSingleRow)
            {
                if (!initData.initialization)
                {
                    return false;
                }
                try
                {
                    CngKey keystore = CngKey.Open(initData.id.ToString());
                    if (Encoding.UTF8.GetString(Decrypt(initData.ValidationMessage, initData)) != initData.initializationTime.ToString())
                    {
                        _logger.LogCritical($"Private key used is not valid to; check if keyname={initData.id} with command\ncertutil -key -csp KSP -user (runs this commad as user under which the application is working)");
                        return false;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogCritical($"Private key used is not valid to; check if keyname={initData.id} with command\ncertutil -key -csp KSP -user (runs this commad as user under which the application is working)");
                    return false;
                }
            }
            else
            {
                _logger.LogCritical($"There are multipe rows in the AppInit table, it must be only one");
                return false;
            }
            
        }

    }
}
