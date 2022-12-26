using Azure.Communication.Email;
using Azure.Communication.Email.Models;
using System.Security.Claims;

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
    }
}
