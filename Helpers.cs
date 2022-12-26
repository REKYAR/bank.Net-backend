using Azure.Communication.Email;
using Azure.Communication.Email.Models;
using System.Security.Claims;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Bank.NET___backend
{
    public static class Helpers
    {
        public static Claim? GetClaim(List<Claim> claims, string claimType)
        {
            return claims.Find(x => x.Type == claimType);
        }

        public static SendEmailResult sendTextEmail()
        {
            string? commsKey = System.Environment.GetEnvironmentVariable("COMMUNICATION_KEY");
            if (commsKey is null)
            {
                throw new NullReferenceException("COMMUNICATION_KEY is null");
            }
            EmailClient emailClient = new EmailClient($"endpoint=https://dotnet-bank-communication-second.communication.azure.com/;accesskey={commsKey}");
            EmailContent emailContent = new EmailContent("Welcome to Azure Communication Service Email APIs.");
            emailContent.PlainText = "This email message is sent from Azure Communication Service Email using .NET SDK.";
            List<EmailAddress> emailAddresses = new List<EmailAddress> { new EmailAddress("01161439@pw.edu.pl") { DisplayName = "Friendly Display Name" }};
            EmailRecipients emailRecipients = new EmailRecipients(emailAddresses);
            EmailMessage emailMessage = new EmailMessage("dotnetbank@394f8de0-648f-49e2-ae57-fce7523309f0.azurecomm.net", emailContent, emailRecipients);
            SendEmailResult emailResult = emailClient.Send(emailMessage,CancellationToken.None);
            return emailResult; 
        }

        public static SendEmailResult sendUploadEmail(string email)
        {
            string? commsKey = System.Environment.GetEnvironmentVariable("COMMUNICATION_KEY");
            if (commsKey is null)
            {
                throw new NullReferenceException("COMMUNICATION_KEY is null");
            }
            EmailClient emailClient = new EmailClient($"endpoint=https://dotnet-bank-communication-second.communication.azure.com/;accesskey={commsKey}");
            EmailContent emailContent = new EmailContent("DOTNET bank document upload");
            emailContent.PlainText = "You need to upload agreement linked here <link>, and a government document. \n <link to documnent upload>, <link to id upload>";
            List<EmailAddress> emailAddresses = new List<EmailAddress> { new EmailAddress($"{email}") { DisplayName = "Friendly Display Name" }};
            EmailRecipients emailRecipients = new EmailRecipients(emailAddresses);
            EmailMessage emailMessage = new EmailMessage("dotnetbank@394f8de0-648f-49e2-ae57-fce7523309f0.azurecomm.net", emailContent, emailRecipients);
            SendEmailResult emailResult = emailClient.Send(emailMessage,CancellationToken.None);
            return emailResult; 
        }
        public static SendEmailResult sendConfirmationEmail(string email)
        {
            string? commsKey = System.Environment.GetEnvironmentVariable("COMMUNICATION_KEY");
            if (commsKey is null)
            {
                throw new NullReferenceException("COMMUNICATION_KEY is null");
            }
            EmailClient emailClient = new EmailClient($"endpoint=https://dotnet-bank-communication-second.communication.azure.com/;accesskey={commsKey}");
            EmailContent emailContent = new EmailContent("DOTNET bank confirmation email");
            emailContent.PlainText = "In order to confirm your loan selection you need to click this link  <link>";
            List<EmailAddress> emailAddresses = new List<EmailAddress> { new EmailAddress($"{email}") { DisplayName = "Friendly Display Name" }};
            EmailRecipients emailRecipients = new EmailRecipients(emailAddresses);
            EmailMessage emailMessage = new EmailMessage("dotnetbank@394f8de0-648f-49e2-ae57-fce7523309f0.azurecomm.net", emailContent, emailRecipients);
            SendEmailResult emailResult = emailClient.Send(emailMessage,CancellationToken.None);
            return emailResult; 
        }

        public static Response<BlobContentInfo> uploadDocument(string containerName, string path, string fileName)
        {
            string? storageKey = System.Environment.GetEnvironmentVariable("STORAGE_KEY");
            if (storageKey is null)
            {
                throw new NullReferenceException("STORAGE_KEY is null");
            }
            BlobServiceClient blobServiceClient = new BlobServiceClient($"DefaultEndpointsProtocol=https;AccountName=dotnetbankstorage;AccountKey={storageKey};EndpointSuffix=core.windows.net");
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            Response<BlobContentInfo> result = blobClient.Upload(path, true);
            return result;
        }

        public static Response downloadDocument(string containerName, string fileName)
        {
            string? storageKey = System.Environment.GetEnvironmentVariable("STORAGE_KEY");
            if (storageKey is null)
            {
                throw new NullReferenceException("STORAGE_KEY is null");
            }
            BlobServiceClient blobServiceClient = new BlobServiceClient($"DefaultEndpointsProtocol=https;AccountName=dotnetbankstorage;AccountKey={storageKey};EndpointSuffix=core.windows.net");
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            string downloadFilePath = $"./downloads/{fileName}";
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            Response response = blobClient.DownloadTo(downloadFilePath);
            return response; 
        }
    }
}
