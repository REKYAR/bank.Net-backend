using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace Bank.NET___backend.Prelaunch
{
    public static class Prelaunch
    {
        public static void GetSecrets()
        {
            var client = new SecretClient(
            new Uri("https://Bank-Key-Vault.vault.azure.net/"),
            new DefaultAzureCredential(),
            new SecretClientOptions
            {
                Retry =
                    {
                        Delay = TimeSpan.FromSeconds(2),
                        MaxDelay = TimeSpan.FromSeconds(16),
                        MaxRetries = 5,
                        Mode = RetryMode.Exponential
                    }
            }
            );

            KeyVaultSecret SQL_PASSWORD = client.GetSecret("SQL-PASSWORD").Value;
            KeyVaultSecret FILES_KEY =  client.GetSecret("files-key").Value;
            KeyVaultSecret COMMUNICATION_KEY =  client.GetSecret("COMMUNICATION-KEY").Value;
            KeyVaultSecret STORAGE_KEY =  client.GetSecret("STORAGE-KEY").Value;

            string sqlPassword = SQL_PASSWORD.Value;
            string filesKey = FILES_KEY.Value;
            string communicationKey = COMMUNICATION_KEY.Value;
            string storageKey = STORAGE_KEY.Value;

            System.Environment.SetEnvironmentVariable("SQL_PASSWORD",sqlPassword);
            System.Environment.SetEnvironmentVariable("FILES_KEY",filesKey);
            System.Environment.SetEnvironmentVariable("COMMUNICATION_KEY",communicationKey);
            System.Environment.SetEnvironmentVariable("STORAGE_KEY",storageKey);
        }
    }
}
