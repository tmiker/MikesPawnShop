namespace Accounts.API.Abstractions
{
    public interface IAesSymmetricEncryptionManager
    {
        string EncryptSymmetric(string data, string key, string initializationVectorString);     		// used by private internal api to encrypt rsa key before sending to public api
        string DecryptSymmetric(string encryptedText, string key, string initializationVectorString);      	// used by public api to decrypt rsa key received from private internal api 

    }
}
