using System.Text;
using FastEndpoints;
using FastEndpoints.Swagger;
using JasperFx;
using Marten;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Wolverine;
using Wolverine.Marten;

namespace Billing.Web.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var connString = builder.Configuration.GetConnectionString("DefaultConnection")!;

        builder.Host.UseWolverine().StartAsync();
        builder.Services.AddMarten(opts =>
        {
            opts.Connection(connString);
            opts.DatabaseSchemaName = "db";
            opts.UseSystemTextJsonForSerialization();
            opts.CreateDatabasesForTenants(c =>
            {
                c.MaintenanceDatabase(connString);
                c.ForTenant()
                    .CheckAgainstPgDatabase()
                    .WithOwner("postgres")
                    .WithEncoding("UTF-8")
                    .ConnectionLimit(-1);
            });

            // If we're running in development mode, let Marten just take care
            // of all necessary schema building and patching behind the scenes
            if (builder.Environment.IsDevelopment())
            {
                opts.AutoCreateSchemaObjects = AutoCreate.All;
            }
        }).IntegrateWithWolverine();


        builder.Services.AddAuthorization().AddFastEndpoints().SwaggerDocument(o =>

                o.DocumentSettings = s =>
                {
                    s.DocumentName = "v1";
                    s.Title = "Billing API";
                    s.Version = "v1";
                }
            );
        var jwtSettings = builder.Configuration.GetSection("JwtSettings");

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
     .AddJwtBearer(options =>
     {
         options.Authority = jwtSettings["Authority"];
         options.RequireHttpsMetadata = false;

         options.TokenValidationParameters = new TokenValidationParameters
         {
             ValidateIssuer = true,
             ValidIssuer = jwtSettings["Issuer"],
             ValidateAudience = true,
             ValidAudience = jwtSettings["Audience"],
             ValidateLifetime = true,
             IssuerSigningKey = new SymmetricSecurityKey(
                 Encoding.UTF8.GetBytes(jwtSettings["SigningKey"] ?? throw new InvalidOperationException("JwtSettings:SigningKey is missing from configuration."))
             )
         };
     });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwaggerGen();
        }

        app.UseAuthorization()
            .UseFastEndpoints(c => c.Errors.UseProblemDetails());
        app.Run();
    }
}
