using DomainModel;
using LayerData;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using SvcEncryptionExceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Services
{
    public class MasterPassword
    {
        private readonly EncryptionData _encryptionData;
        private readonly AppInit _appInit;
        private readonly byte[] _;
        private readonly GCHandle _handle;
        private readonly ILogger<MasterPassword> _logger;

        public MasterPassword(ILogger<MasterPassword> logger, IServiceProvider serviceProvider)
        {
            using IServiceScope scope = serviceProvider.CreateScope();
            _encryptionData = scope.ServiceProvider.GetService<EncryptionData>();
            _appInit = IsSingleRow();
            _logger = logger;
            try
            {
                CngKey keystore = CngKey.Open(_appInit.id.ToString());
                if (_appInit != null)
                {
                    _ = GenerateAesKey(keystore, _appInit);
                    _handle = GCHandle.Alloc(_, GCHandleType.Pinned);

                }
                else
                {
                    _logger.LogCritical("Invalid Application Initialization");
                    throw new InvalidAppInitializationException("Invalid Application Initialization");
                }
            }
            catch (Exception ex) {
                _logger.LogCritical($"Master password can't be generated\n\n{ex.Message}");
                throw new MasterPasswordException($"Master password can't be generated\n\n{ex.Message}");
            }
            



        }

        ~MasterPassword()
        {
            _handle.Free();
        }
        private byte[] GenerateAesKey(CngKey keystore, AppInit appInit)
        {

            RSACng rsa = new RSACng(keystore);

            byte[] _salt = new byte[64];
            byte[] _interations = new byte[appInit.mk.Length - 64];

            Buffer.BlockCopy(appInit.mk, 0, _salt, 0, _salt.Length);
            Buffer.BlockCopy(appInit.mk, _salt.Length, _interations, 0, _interations.Length);

            return Rfc2898DeriveBytes.Pbkdf2(rsa.ExportRSAPrivateKey(), _salt, BitConverter.ToInt32(_interations, 0), HashAlgorithmName.SHA512, 32);
        }



        private AppInit IsSingleRow()
        {
            List<AppInit> init = this._encryptionData.Read<AppInit>().ToList();
            if (init.Count == 1)
            {
                return init[0];
            }
            return null;
        }

        public byte[] GetAesKey()
        {
            return _;
        }
    }
}
