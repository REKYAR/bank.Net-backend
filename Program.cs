using Bank.NET___backend;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Core;
using Bank.NET___backend.Prelaunch;
using Bank.NET___backend.Data;
using Microsoft.EntityFrameworkCore;

Prelaunch.GetSecrets();


var builder = WebApplication.CreateBuilder(args);

//string COSMOS_PRIMARY_CONNECTION_STRING = Environment.GetEnvironmentVariable("COSMOS_PRIMARY_CONNECTION_STRING");
//string SQL_PASSWORD = Environment.GetEnvironmentVariable("SQL_PASSWORD");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SqlContext>(options => options.UseNpgsql());


var app = builder.Build();

//
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//get secrets

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
    
app.Run();
