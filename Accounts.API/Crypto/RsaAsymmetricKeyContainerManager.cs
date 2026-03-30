using Accounts.API.Abstractions;
using System.Security.Cryptography;

namespace Accounts.API.Crypto
{
    public class RsaAsymmetricKeyContainerManager : IRsaAsymmetricKeyContainerManager
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        //public void GenKeys_SaveInContainer(string containerName)
        //{
        //    // Create the CspParameters object and set the key container name used to store the RSA key pair.
        //    var parameters = new CspParameters
        //    {
        //        KeyContainerName = containerName
        //    };

        //    // Create a new instance of RSACryptoServiceProvider that accesses the key container MyKeyContainerName.
        //    using var rsa = new RSACryptoServiceProvider(parameters);

        //    // Display the key information to the console.
        //    Console.WriteLine($"Key added to container: \n  {rsa.ToXmlString(true)}");
        //}

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        public string GetPublicKeyForContainerWithName(string containerName)
        {
            // NOTE THIS WILL CREATE A KEY CONTAINER WITH NAME IF IT DOES NOT YET EXIST
            // Create the CspParameters object and set the key container name used to store the RSA key pair.
            var parameters = new CspParameters
            {
                KeyContainerName = containerName
            };

            // Create a new instance of RSACryptoServiceProvider that accesses the key container MyKeyContainerName.
            using var rsa = new RSACryptoServiceProvider(parameters);

            // Display the key information to the console.
            Console.WriteLine($"Public Key retrieved from container : \n {rsa.ToXmlString(false)}");
            return rsa.ToXmlString(false);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        public string GetPublicAndPrivateKeyForContainerWithName(string containerName)
        {
            // Create the CspParameters object and set the key container
            // name used to store the RSA key pair.
            var parameters = new CspParameters
            {
                KeyContainerName = containerName
            };

            // Create a new instance of RSACryptoServiceProvider that accesses
            // the key container MyKeyContainerName.
            using var rsa = new RSACryptoServiceProvider(parameters);

            // Display the key information to the console.
            Console.WriteLine($"Public and Private Key retrieved from container : \n {rsa.ToXmlString(true)}");
            return rsa.ToXmlString(true);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        public void DeleteKeyFromContainer(string containerName)
        {
            // Create the CspParameters object and set the key container name used to store the RSA key pair.
            var parameters = new CspParameters
            {
                KeyContainerName = containerName
            };

            // Create a new instance of RSACryptoServiceProvider that accesses the key container.
            using var rsa = new RSACryptoServiceProvider(parameters)
            {
                // Delete the key entry in the container.
                PersistKeyInCsp = false
            };

            // Call Clear to release resources and delete the key from the container.  Note, this should cause the key container to be deleted when RSA instance is released or garbage collected.
            rsa.Clear();
            Console.WriteLine("Key deleted.");
        }

    }
}
