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
            KeyVaultSecret COSMOS_PRIMARY_CONNECTION_STRING = client.GetSecret("COSMOS-PRIMARY-CONNECTION-STRING").Value;
            KeyVaultSecret FILES_KEY =  client.GetSecret("files-key").Value;

            string sqlPassword = SQL_PASSWORD.Value;
            string cosmosPrimaryConnectionString = COSMOS_PRIMARY_CONNECTION_STRING.Value;
             string filesKey = FILES_KEY.Value;

            System.Environment.SetEnvironmentVariable("SQL_PASSWORD",sqlPassword);
            System.Environment.SetEnvironmentVariable("COSMOS_PRIMARY_CONNECTION_STRING",cosmosPrimaryConnectionString);
            System.Environment.SetEnvironmentVariable("FILES_KEY",filesKey);
        }
    }
}
