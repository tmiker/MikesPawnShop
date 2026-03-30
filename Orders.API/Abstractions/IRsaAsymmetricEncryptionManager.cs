namespace Orders.API.Abstractions
{
    public interface IRsaAsymmetricEncryptionManager
    {
        string EncryptUsingPublicKeyXmlString(string data, string rsaXmlString);          // used by public api http client to encrypt owner id using the public key
        string DecryptUsingRsaXmlString(string cipherText, string rsaXmlString);          // used by private api to decrypt owner id using rsa key pair
    }
}
