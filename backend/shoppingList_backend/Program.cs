using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using shoppingList_backend;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();



var app = builder.Build();

app.UsePathBase(new PathString("/api/v2"));

app.UseAuthentication();

app.UseAuthorization();



builder.WebHost.UseSentry(o =>
{
    o.Dsn = "https://6e12bacaa20142429f7edad1b420c9c7@o4511156682620928.ingest.de.sentry.io/4511156698939472";
    o.Debug = true;
});



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

AddMigrations.ApplyMigrations();

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();
