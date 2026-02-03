using Contracts.interfaces.Repository;
using Entities.Models.Databases.OracleDb;
using Entities.Models.Databases.PostgresDb;
using Entities.Models;
using GrpcService1.mapper;
using GrpcService1.Services;
using LoggerService;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Options;
using Repositories.Repositories;
using System.Configuration;
using Microsoft.EntityFrameworkCore;

namespace GrpcService1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //builder.WebHost.ConfigureKestrel(options =>
            //{
            //    options.ListenAnyIP(50051, listenOptions =>
            //    {
            //        listenOptions.Protocols = HttpProtocols.Http2; // Required for gRPC
            //    });

            //    // Disable HTTPS by default
            //    options.ConfigureEndpointDefaults(lo => lo.UseHttps = false);
            //});


            // Add services to the container.
            builder.Services.AddGrpc();

           // builder.Services.AddAutoMapper(typeof(EmployeeProfile));


            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            // Register the appropriate database connection type based on configuration
            string databaseType = builder.Configuration.GetValue<string>("DatabaseType"); // "Postgres" or "Oracle"

            // Configure the DB connection based on the selected type
            if (databaseType == "Postgres")
            {
                builder.Services.AddScoped<IApplicationReadDbConnection, ApplicationReadDbConnection>(provider =>
                {
                    var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");
                    return new ApplicationReadDbConnection(connectionString, "Postgres");
                });
            }
            else if (databaseType == "Oracle")
            {
                builder.Services.AddScoped<IApplicationReadDbConnection, ApplicationReadDbConnection>(provider =>
                {
                    var connectionString = builder.Configuration.GetConnectionString("OracleConnection");
                    return new ApplicationReadDbConnection(connectionString, "Oracle");
                });
            }
            else
            {
                throw new InvalidOperationException("Unsupported database type in configuration.");
            }
            builder.Services.AddHttpContextAccessor();


            switch (databaseType)
            {
                case "Oracle":
                    builder.Services.AddDbContext<RepositoryContext, OracleContext>(options =>
                        options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));
                    break;
                case "Postgres":
                    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                    builder.Services.AddDbContext<RepositoryContext, PostgresContext>(options =>
                        options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));
                    break;

                default:
                    throw new Exception("Unsupported database provider.");
            }


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.MapGrpcService<SecService>();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Run();
        }
    }
}
