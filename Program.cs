using Bank.NET___backend;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//string COSMOS_PRIMARY_CONNECTION_STRING = Environment.GetEnvironmentVariable("COSMOS_PRIMARY_CONNECTION_STRING");
//string SQL_PASSWORD = Environment.GetEnvironmentVariable("SQL_PASSWORD");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
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

string sqlPassword = SQL_PASSWORD.Value;
string cosmosPrimaryConnectionString = COSMOS_PRIMARY_CONNECTION_STRING.Value;

//
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//get secrets
SecretClientOptions options = new SecretClientOptions()
    {
        Retry =
        {
            Delay= TimeSpan.FromSeconds(2),
            MaxDelay = TimeSpan.FromSeconds(16),
            MaxRetries = 5,
            Mode = RetryMode.Exponential
         }
    };

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
