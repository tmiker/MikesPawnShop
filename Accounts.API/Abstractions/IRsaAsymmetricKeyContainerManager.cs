namespace Accounts.API.Abstractions
{
    public interface IRsaAsymmetricKeyContainerManager
    {
        // void GenKeys_SaveInContainer(string containerName);
        string GetPublicKeyForContainerWithName(string containerName);
        string GetPublicAndPrivateKeyForContainerWithName(string containerName);
        void DeleteKeyFromContainer(string containerName);

    }
}
