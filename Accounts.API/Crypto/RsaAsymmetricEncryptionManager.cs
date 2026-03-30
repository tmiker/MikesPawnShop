using Accounts.API.Abstractions;
using System.Security.Cryptography;
using System.Text;

namespace Accounts.API.Crypto
{
    public class RsaAsymmetricEncryptionManager : IRsaAsymmetricEncryptionManager
    {
        public string EncryptUsingPublicKeyXmlString(string data, string rsaXmlString)              // used by public api http client to encrypt owner id using the public key
        {
            using (var rsaCryptoServiceProvider = new RSACryptoServiceProvider())
            {
                rsaCryptoServiceProvider.FromXmlString(rsaXmlString);
                var byteData = Encoding.UTF8.GetBytes(data);
                var encryptedData = rsaCryptoServiceProvider.Encrypt(byteData, false);
                return Convert.ToBase64String(encryptedData);
            }
        }

        public string DecryptUsingRsaXmlString(string cipherText, string rsaXmlString)              // used by private api to decrypt owner id using rsa key pair
        {
            using (var rsaCryptoServiceProvider = new RSACryptoServiceProvider())
            {
                var cipherDataAsByte = Convert.FromBase64String(cipherText);
                rsaCryptoServiceProvider.FromXmlString(rsaXmlString);
                var decryptedData = rsaCryptoServiceProvider.Decrypt(cipherDataAsByte, false);
                return Encoding.UTF8.GetString(decryptedData);
            }
        }

    }
}
