using Bank.NET___backend;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Core;
using Bank.NET___backend.Prelaunch;
using Bank.NET___backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using System.Reflection;

Prelaunch.GetSecrets();

var builder = WebApplication.CreateBuilder(args);

//string COSMOS_PRIMARY_CONNECTION_STRING = Environment.GetEnvironmentVariable("COSMOS_PRIMARY_CONNECTION_STRING");
//string SQL_PASSWORD = Environment.GetEnvironmentVariable("SQL_PASSWORD");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAdB2C"));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger Azure AD Demo", Version = "v1" });
        c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Description = "Oauth2.0 which uses AuthorizationCode flow",
            Name = "oauth2.0",
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new Uri(builder.Configuration["SwaggerAzureAD:AuthorizationUrl"]),
                    TokenUrl = new Uri(builder.Configuration["SwaggerAzureAD:TokenUrl"]),
                    Scopes = new Dictionary<string, string>
                    {
                        {builder.Configuration["SwaggerAzureAD:Scope"], "Access API as a User" }
                    }
                }
            }
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                },
                new[] { builder.Configuration["SwaggerAzureAD:Scope"] }
            }
        });
    });

builder.Services.AddDbContext<SqlContext>(options => options.UseNpgsql());


var app = builder.Build();

//
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.OAuthClientId(builder.Configuration["SwaggerAzureAD:ClientId"]);
        c.OAuthUsePkce();
        c.OAuthScopeSeparator(" ");
    });
}
//get secrets

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
    
app.Run();
