using System.Net.Mime;
using Azure.Communication.Email;
using Azure.Communication.Email.Models;
using System.Security.Claims;
using System.Text;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Bank.NET___backend.ApiStructures.NewFolder.bankapi4dotnet;
using Bank.NET___backend.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

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
        public static SendEmailResult sendInitalStatusUpdateEmail(string email)
        {
            string? commsKey = System.Environment.GetEnvironmentVariable("COMMUNICATION_KEY");
            if (commsKey is null)
            {
                throw new NullReferenceException("COMMUNICATION_KEY is null");
            }
            EmailClient emailClient = new EmailClient($"endpoint=https://dotnet-bank-communication-second.communication.azure.com/;accesskey={commsKey}");
            EmailContent emailContent = new EmailContent("DOTNET bank document upload");
            emailContent.PlainText = "Thank you for uploading both documents, now you need to wait for bank employee to review your request";
            List<EmailAddress> emailAddresses = new List<EmailAddress> { new EmailAddress($"{email}") { DisplayName = "Friendly Display Name" }};
            EmailRecipients emailRecipients = new EmailRecipients(emailAddresses);
            EmailMessage emailMessage = new EmailMessage("dotnetbank@394f8de0-648f-49e2-ae57-fce7523309f0.azurecomm.net", emailContent, emailRecipients);
            SendEmailResult emailResult = emailClient.Send(emailMessage,CancellationToken.None);
            return emailResult; 
        }
        public static SendEmailResult sendUploadEmail(string email, int requestId)
        {
            string? commsKey = System.Environment.GetEnvironmentVariable("COMMUNICATION_KEY");
            if (commsKey is null)
            {
                throw new NullReferenceException("COMMUNICATION_KEY is null");
            }
            EmailClient emailClient = new EmailClient($"endpoint=https://dotnet-bank-communication-second.communication.azure.com/;accesskey={commsKey}");
            EmailContent emailContent = new EmailContent("DOTNET bank document upload");
            emailContent.PlainText = "You need to upload agreement linked here <link>, and a government document. \n <link to document upload>, \n <link to agreement upload>";
            List<EmailAddress> emailAddresses = new List<EmailAddress> { new EmailAddress($"{email}") { DisplayName = "Friendly Display Name" }};
            EmailRecipients emailRecipients = new EmailRecipients(emailAddresses);
            EmailMessage emailMessage = new EmailMessage("dotnetbank@394f8de0-648f-49e2-ae57-fce7523309f0.azurecomm.net", emailContent, emailRecipients);
            SendEmailResult emailResult = emailClient.Send(emailMessage,CancellationToken.None);
            return emailResult; 
        }

        public static SendEmailResult sendLinkToReq(string email, int requestId)
        {
            string? commsKey = System.Environment.GetEnvironmentVariable("COMMUNICATION_KEY");
            if (commsKey is null)
            {
                throw new NullReferenceException("COMMUNICATION_KEY is null");
            }
            EmailClient emailClient = new EmailClient($"endpoint=https://dotnet-bank-communication-second.communication.azure.com/;accesskey={commsKey}");
            EmailContent emailContent = new EmailContent("DOTNET bank document upload");
            emailContent.PlainText = $"Link to offer: https://dotnetfrontend.azurewebsites.net/statusCheck/{requestId} ";
            List<EmailAddress> emailAddresses = new List<EmailAddress> { new EmailAddress($"{email}") { DisplayName = "Friendly Display Name" }};
            EmailRecipients emailRecipients = new EmailRecipients(emailAddresses);
            EmailMessage emailMessage = new EmailMessage("dotnetbank@394f8de0-648f-49e2-ae57-fce7523309f0.azurecomm.net", emailContent, emailRecipients);
            SendEmailResult emailResult = emailClient.Send(emailMessage,CancellationToken.None);
            return emailResult; 
        }

        public static SendEmailResult sendFinal(string email, int requestId, Guid requestGuid)
        {
            string? commsKey = System.Environment.GetEnvironmentVariable("COMMUNICATION_KEY");
            if (commsKey is null)
            {
                throw new NullReferenceException("COMMUNICATION_KEY is null");
            }
            EmailClient emailClient = new EmailClient($"endpoint=https://dotnet-bank-communication-second.communication.azure.com/;accesskey={commsKey}");
            EmailContent emailContent = new EmailContent("DOTNET bank document upload");
            emailContent.PlainText = $" Final confirmation link: https://dotnetfrontend.azurewebsites.net/statusCheck/{requestId} ";
            List<EmailAddress> emailAddresses = new List<EmailAddress> { new EmailAddress($"{email}") { DisplayName = "Friendly Display Name" }};
            EmailRecipients emailRecipients = new EmailRecipients(emailAddresses);
            EmailMessage emailMessage = new EmailMessage("dotnetbank@394f8de0-648f-49e2-ae57-fce7523309f0.azurecomm.net", emailContent, emailRecipients);
            SendEmailResult emailResult = emailClient.Send(emailMessage,CancellationToken.None);
            return emailResult; 
        }

        public static SendEmailResult sendRefusedRequestMail(string email, int requestId)
        {
            string? commsKey = System.Environment.GetEnvironmentVariable("COMMUNICATION_KEY");
            if (commsKey is null)
            {
                throw new NullReferenceException("COMMUNICATION_KEY is null");
            }
            EmailClient emailClient = new EmailClient($"endpoint=https://dotnet-bank-communication-second.communication.azure.com/;accesskey={commsKey}");
            EmailContent emailContent = new EmailContent("DOTNET bank your request was refused");
            emailContent.PlainText = $"Your request {requestId} was refused.";
            List<EmailAddress> emailAddresses = new List<EmailAddress> { new EmailAddress($"{email}") { DisplayName = "Friendly Display Name" } };
            EmailRecipients emailRecipients = new EmailRecipients(emailAddresses);
            EmailMessage emailMessage = new EmailMessage("dotnetbank@394f8de0-648f-49e2-ae57-fce7523309f0.azurecomm.net", emailContent, emailRecipients);
            SendEmailResult emailResult = emailClient.Send(emailMessage, CancellationToken.None);
            return emailResult;
        }

        public static SendEmailResult sendConfirmationEmail(string email, int requestId, Guid requestGuid)
        {
            string? commsKey = System.Environment.GetEnvironmentVariable("COMMUNICATION_KEY");
            if (commsKey is null)
            {
                throw new NullReferenceException("COMMUNICATION_KEY is null");
            }
            EmailClient emailClient = new EmailClient($"endpoint=https://dotnet-bank-communication-second.communication.azure.com/;accesskey={commsKey}");
            EmailContent emailContent = new EmailContent("DOTNET bank confirmation email");
            emailContent.PlainText = "In order to confirm your loan selection you need to click this link <link>";
            List<EmailAddress> emailAddresses = new List<EmailAddress> { new EmailAddress($"{email}") { DisplayName = "Friendly Display Name" }};
            EmailRecipients emailRecipients = new EmailRecipients(emailAddresses);
            EmailMessage emailMessage = new EmailMessage("dotnetbank@394f8de0-648f-49e2-ae57-fce7523309f0.azurecomm.net", emailContent, emailRecipients);
            SendEmailResult emailResult = emailClient.Send(emailMessage,CancellationToken.None);
            return emailResult; 
        }

        public static Response<BlobContentInfo> uploadDocument(string containerName, Stream contentStream, string fileName)
        {
            string? storageKey = System.Environment.GetEnvironmentVariable("STORAGE_KEY");
            if (storageKey is null)
            {
                throw new NullReferenceException("STORAGE_KEY is null");
            }
            BlobServiceClient blobServiceClient = new BlobServiceClient($"DefaultEndpointsProtocol=https;AccountName=dotnetbankstorage;AccountKey={storageKey};EndpointSuffix=core.windows.net");
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            Response<BlobContentInfo> result = blobClient.Upload(contentStream,true);
            return result;
        }

        public static Stream downloadDocument(string containerName, string fileName)
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
            //Response response = blobClient.DownloadTo(downloadFilePath);
            //FileStreamResult resultStream ;
            var resultStream = blobClient.OpenRead();
            //response.GetRawResponse();
            //Response response = blobClient.dow
            //response.GetRawResponse().
            return resultStream;
        }

        public static Stream generateAgreeement(string name, string lastname)
        {
            
            //var document = new PdfDocument();
            //var page = document.AddPage();

            //var gfx = XGraphics.FromPdfPage(page);
            //var font = new XFont("Arial", 20, XFontStyle.Regular);

            //var textColor = XBrushes.Black;
            //var layout = new XRect(20, 20, page.Width, 0);
            //var format = XStringFormats.Default;

            //gfx.DrawString($"I {name} {lastname} agree to this stuff.", font, textColor, layout, format);
            //Stream stream = new MemoryStream();

            //document.Save(stream);
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write($"I {name} {lastname} agree to this stuff.");
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static async Task<int> getJobId(string job)
        {
            
            try
            {
                string uri = System.Environment.GetEnvironmentVariable("API_2");
                HttpClient client = new HttpClient();
                var response = await client.GetAsync($"{uri}/api/v1/Dictionary/jobTypes");
                List<Job> responseDict = new List<Job>();
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    responseDict = JsonConvert.DeserializeObject<List<Job>>(s);
                }

                if (responseDict.Where(x=>x.name == job).Count()!= 0)
                {
                    return responseDict.Where(x => x.name == job).First().id;
                }

                return 0;
            }
            catch (Exception e)
            {
                return -1;
                Console.WriteLine(e);
                throw;
            }
            
        }

        public static async Task<int> getDocId(string doc)
        {
            
            try
            {
                string uri = System.Environment.GetEnvironmentVariable("API_2");
                HttpClient client = new HttpClient();
                var response = await client.GetAsync($"{uri}/api/v1/Dictionary/governmentDocumentTypes");
                List<Doc> responseDict = new List<Doc>();
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    responseDict = JsonConvert.DeserializeObject<List<Doc>>(s);
                }

                if (responseDict.Where(x=>x.name == doc).Count()!= 0)
                {
                    return responseDict.Where(x => x.name == doc).First().id;
                }

                return 0;
            }
             catch (Exception e)
            {
                return -1;
                Console.WriteLine(e);
                throw;
            }

        }

        public static async Task<Data.Response> GetOfferFromApi1(Data.Request req, string apiinfo)
        {
            Data.Response res = new Data.Response();
            res.UserEmail = req.Email;
            res.RequestID = req.RequestID;
            res.State = ResponseStatus.PendingApproval.ToString();
            try
            {
                res.External = true;
                
                res.ApiInfo = apiinfo;
                Dictionary<string, string> responseDict = new Dictionary<string, string>();
                Dictionary<string, string> response2Dict = new Dictionary<string, string>();
                ApiStructures.NewFolder.bankapi4dotnet.Offer of;
            
                HttpClient client = new HttpClient();
                string uri = apiinfo.Split("&&&")[1];
                CreateInquiryRequest cir = new CreateInquiryRequest(req.Amount, req.NumberOfInstallments, req.Name,
                    req.Surname, getDocId(req.GovermentId).Result, req.GovermentId, getJobId(req.JobType).Result, req.IncomeLevel);
                HttpResponseMessage response = await client.PostAsJsonAsync($"{uri}/api/Inquire", cir);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    responseDict = JsonConvert.DeserializeObject<Dictionary<string,string>>(s);
                }

                if (responseDict is null)
                {
                    throw new NullReferenceException();
                }
                else
                {
                    HttpResponseMessage response2 = await client.GetAsync($"{uri}/api/Inquire/{responseDict["inquireId"]}");
                    string s = await response2.Content.ReadAsStringAsync();
                    response2Dict = response2.Content.ReadAsAsync<Dictionary<string,string>>().Result;
                }

                if (response2Dict is null)
                {
                    throw new NullReferenceException();
                }
                else
                {
                    HttpResponseMessage response2 = await client.GetAsync($"{uri}/Offer/{response2Dict["offerId"]}");
                    string s = await response2.Content.ReadAsStringAsync();
                    of = response2.Content.ReadAsAsync<Offer>().Result;
                    res.documentKey = of.DocumentLink.Split('/').Last();
                    res.OfferId = of.Id;
                    res.MonthlyInstallment = of.MonthlyInstallment;
                    //return Ok(of);
                }

                return res;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                res.ApiInfo = null;
                res.External = false;
                res.MonthlyInstallment = Logic.generateOffer(req);
                return res;
                throw;
            }
            
            //OFFER RECIVED
            //HttpResponseMessage response3 = await client.GetAsync($"{uri}/Offer/{of.Id}/document/{of.DocumentLink.Split('/').Last()}");
            //return File(response3.Content.ReadAsStream(), MediaTypeNames.Text.Plain, "File.txt");
        }
    }

    public class Job
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
    public class Doc
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
}
