using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace TaskList.Core
{
    /// <summary>
    /// Provides utility methods for accessing secrets in Azure Key Vaults.
    /// </summary>
    public class AzureKeyVaultUtil
    {
        /// <summary>
        /// Gets the password for the Database Connection String
        /// </summary>
        public static string DatabaseConnectionStringPassword
        {
            get
            {
                return GetSecretByName(nameof(DatabaseConnectionStringPassword));
            }
        }

        private static ConcurrentDictionary<string, string> secretsDictionary = new ConcurrentDictionary<string, string>();
        private static ClientAssertionCertificate AssertionCert { get; set; }

        /// <summary>
        /// Get the most recent secrets from the key vault.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetSecretsFromKeyVault()
        {
            //Get the certificate and authenticate vault access
            GetCert();
            var keyVault = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetAccessToken));

            //Get the Uris of every secret unfortunately, there doesn't seem to be a way to just grab every secret.
            var secs = keyVault.GetSecretsAsync(AppSettings.AzureKeyVaultBaseUri).GetAwaiter().GetResult();

            //Go through each Uri and retrieve from the vault.
            var secrets = new Dictionary<string, string>();

            foreach (Microsoft.Azure.KeyVault.Models.SecretItem secret in secs)
            {
                var lastVersion = keyVault.GetSecretAsync(secret.Id).GetAwaiter().GetResult();
                secrets.Add(secret.Identifier.Name, lastVersion.Value);
            }

            return secrets;
        }

        /// <summary>
        /// Get a secret from the keyvault/cache given a name
        /// </summary>
        /// <param name="secretName">The name of the secret to retrieve.</param>
        /// <returns></returns>
        public static string GetSecretByName(string secretName)
        {
            string result = secretsDictionary.GetOrAdd(secretName, (key) =>
            {
                var secrets = GetSecretsFromKeyVault();

                foreach (KeyValuePair<string, string> entry in secrets)
                {
                    secretsDictionary.TryAdd(entry.Key, entry.Value);
                }

                return secretsDictionary[secretName] ?? null;
            });

            return result;
        }

        #region Private       

        private static async Task<string> GetAccessToken(string authority, string resource, string scope)
        {
            var context = new AuthenticationContext(authority, TokenCache.DefaultShared);
            var result = await context.AcquireTokenAsync(resource, AssertionCert);
            return result.AccessToken;
        }

        private static void GetCert()
        {
            var clientAssertionCertPfx = FindCertificateByThumbprint(AppSettings.AzureKeyVaultCertificateThumbprint);
            AssertionCert = new ClientAssertionCertificate(AppSettings.AzureKeyVaultADApplicationID, clientAssertionCertPfx);
        }

        private static X509Certificate2 FindCertificateByThumbprint(string findValue)
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            try
            {
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection col = store.Certificates.Find(X509FindType.FindByThumbprint, findValue, false);
                if (col == null || col.Count == 0)
                {
                    return null;
                }
                else
                {
                    return col[0];
                }
            }
            finally
            {
                store.Close();
            }
        }

        #endregion
    }
}
