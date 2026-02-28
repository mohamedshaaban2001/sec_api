using Contracts.interfaces.Repository;
using Entities.Models;
using Entities.Models.Databases.OracleDb;
using Entities.Models.Databases.PostgresDb;
using IdentityServer4.AccessTokenValidation;
using LoggerService;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repositories.Repositories;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Service_API;
using Service_API.Services;
namespace Service_API.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            services.AddMapster();
            //services.AddAutoMapper(typeof(MappingProfile));
        }
        public static void ConfigureLogger(this IServiceCollection services, IConfiguration Configuration)
        {
            switch (Configuration["DatabaseProvider"])
            {

                case "Oracle":
                    services.AddSingleton<ILoggerManager>(logger => new LoggerManager(Configuration["DatabaseProvider"],
                        Configuration.GetConnectionString("OracleConnection")));
                    break;
                case "Postgres":
                    services.AddSingleton<ILoggerManager>(logger => new LoggerManager(Configuration["DatabaseProvider"],
                        Configuration.GetConnectionString("PostgresConnection")));
                    break;

                default:
                    throw new Exception("Unsupported database provider.");
            }            
        }
        public static void ConfigureHttpContext(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
 
        public static void ConfigureDataBase(this IServiceCollection services, IConfiguration Configuration)
        {
            switch (Configuration["DatabaseProvider"])
            {
                case "Oracle":
                    services.AddDbContext<RepositoryContext, OracleContext>(options =>
                        options.UseOracle(Configuration.GetConnectionString("OracleConnection")));
                    break;
                case "Postgres":
                    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                    services.AddDbContext<RepositoryContext, PostgresContext>(options =>
                        options.UseNpgsql(Configuration.GetConnectionString("PostgresConnection")));
                    break;

                default:
                    throw new Exception("Unsupported database provider.");
            }
        }

        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }

        //public static void ConfigureJwtBearer(this IServiceCollection services, IConfiguration config)
        //{
        //    services.AddAuthentication(options =>
        //    {
        //        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        //        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //    })
        //    .AddJwtBearer(options =>
        //    {
        //        options.SaveToken = true;
        //        options.RequireHttpsMetadata = false;
        //        options.TokenValidationParameters = new TokenValidationParameters()
        //        {
        //            ValidateIssuer = true,
        //            ValidateAudience = true,
        //            ValidIssuer = config["JWT:ValidIssuer"],
        //            ValidAudience = config["JWT:ValidAudience"],
        //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Secret"]))
        //        };
        //    });
        //}
        public static void ConfigureJwtBearer(this IServiceCollection Services, IConfiguration Configuration)
        {
            // Authentication: Keycloak (JWT bearer)
            Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var authority = Configuration["Keycloak:Authority"];
                    options.Authority = authority;
                    options.Audience = Configuration["Keycloak:Audience"];
                    options.RequireHttpsMetadata = Configuration.GetValue<bool?>("Keycloak:RequireHttpsMetadata") ?? false;
                    options.MetadataAddress = $"{authority}/.well-known/openid-configuration";
                    options.BackchannelHttpHandler = new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    };
                    options.SaveToken = true;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false, // Keycloak adds multiple audiences; API is realm-protected
                        ValidIssuer = Configuration["Keycloak:Authority"],
                        ClockSkew = TimeSpan.FromMinutes(1)
                    };

                    // Map custom claims so repositories continue to read EMP_SERIAL
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            // Log or inspect token issues during debugging
                            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                            logger.LogWarning(context.Exception, "JWT authentication failed");
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = async context =>
                        {
                            var identity = context.Principal?.Identity as ClaimsIdentity;
                            if (identity != null)
                            {
                                var empSerial = identity.FindFirst("EMP_SERIAL")?.Value
                                                ?? identity.FindFirst("emp_serial")?.Value
                                                ?? identity.FindFirst("USER_CODE")?.Value
                                                ?? identity.FindFirst("user_code")?.Value
                                                ?? identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                                if (!string.IsNullOrEmpty(empSerial) && identity.FindFirst("EMP_SERIAL") == null)
                                {
                                    identity.AddClaim(new Claim("EMP_SERIAL", empSerial));
                                }

                                // Ensure USER_CODE is present (Keycloak mapper may emit USER_CODE or user_code)
                                var userCode = identity.FindFirst("USER_CODE")?.Value
                                                ?? identity.FindFirst("user_code")?.Value
                                                ?? empSerial;
                                if (!string.IsNullOrEmpty(userCode) && identity.FindFirst("USER_CODE") == null)
                                {
                                    identity.AddClaim(new Claim("USER_CODE", userCode));
                                }

                                var universities = identity.FindFirst("UNIVERSITIES")?.Value
                                                  ?? identity.FindFirst("universities")?.Value;
                                if (!string.IsNullOrEmpty(universities) && identity.FindFirst("UNIVERSITIES") == null)
                                {
                                    identity.AddClaim(new Claim("UNIVERSITIES", universities));
                                }

                                // Fetch and add User Groups
                                try 
                                {
                                    var userId = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value; // 'sub' claim
                                    if (!string.IsNullOrEmpty(userId))
                                    {
                                        var keycloakService = context.HttpContext.RequestServices.GetRequiredService<Service_API.Services.IKeycloakService>();
                                        var groups = await keycloakService.GetUserGroupsAsync(userId);
                                        foreach (var group in groups)
                                        {
                                            // Add as 'groups' claim or 'role' depending on requirement. Using 'groups' for now.
                                            identity.AddClaim(new Claim("groups", group));
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                                    logger.LogError(ex, "Failed to fetch user groups from Keycloak");
                                }

                            }
                        }
                    };
                });

            // Ensure you also configure the appropriate services and middleware to use authentication:
            Services.AddAuthorization(options =>
            {
                // Require authentication for all endpoints unless explicitly allowed
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });
        }
        public static void ConfigureSwaggerGen(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen();

            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Description = "Please enter your token with this format: ''Bearer YOUR_TOKEN''",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Name = "Bearer",
                In = ParameterLocation.Header,
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
            });

        }

    }
}
