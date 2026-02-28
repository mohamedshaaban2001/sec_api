using Scalar.AspNetCore;
using Service_API.Extensions;
using System.Xml.XPath;


namespace Service_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //
            // Add services to the container.
            //
            builder.Services.AddControllers();

            builder.Services.ConfigureSwaggerGen();

            builder.Services.ConfigureAutoMapper();

            builder.Services.ConfigureLogger(builder.Configuration);

            builder.Services.ConfigureHttpContext();

            builder.Services.ConfigureDataBase(builder.Configuration);

            //builder.Services.ConfigureJwtBearer(builder.Configuration);

            builder.Services.ConfigureRepositoryWrapper();
            builder.Services.AddHttpClient<Service_API.Services.IKeycloakService, Service_API.Services.KeycloakService>()
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                });
            builder.Services.AddCors(options =>
                        options.AddDefaultPolicy(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

            var app = builder.Build();

            // Add Swagger and OpenAPI services
            // app.MapOpenApi();
            // app.MapScalarApiReference();
            //1. Add SwaggerUI

            //app.UsePathBase("/sec_api");
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}
            //else
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI(c =>
            //    {
            //        c.RoutePrefix = "";
            //        c.SwaggerEndpoint("/swagger/v1/swagger.json", "docs");
            //    });
            //}
            //2. Set BasePath
            app.UsePathBase("/sec_api");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseSwaggerUI(c =>
                {
                    c.RoutePrefix = "sec_api";
                    c.SwaggerEndpoint("swagger/v1/swagger.json", "docs");
                });

                //3. Add Swagger
                app.UseSwagger();
            }


            // Configure the HTTP request pipeline.
            app.UseHttpsRedirection();
            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();
            
            app.MapControllers();
           
            app.Run();
        }
    }
}
