using CustomLibrary.Authorization;
using CustomLibrary.Filters;
using CustomLibrary.Helper;
using CustomLibrary.Services;
using CustomLibrary.Settings;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

namespace CustomLibrary.Extensions
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddMapster(this IServiceCollection services)
        {
            var thisAssembly = Assembly.GetEntryAssembly();
            TypeAdapterConfig.GlobalSettings.Scan(thisAssembly);
            TypeAdapterConfig.GlobalSettings.Default.IgnoreNullValues(true);
            TypeAdapterConfig.GlobalSettings.Default.PreserveReference(true);

            return services;
        }
        public static IServiceCollection AddCustomMvc(this IServiceCollection services)
        {
            services
                .AddControllers(options =>
                {
                    options.Filters.Add(typeof(ModelValidationFilter));
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            services.AddCors();

            return services;
        }
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var identityUrl = configuration.GetValue<string>("IdentityUrl");
            var validIssuers = configuration.GetSection("ValidIssuers").Get<string[]>();

            var key = Encoding.ASCII.GetBytes("b6a318da7213e3ac477abf64cb4ecda9358380d99d72983b46380214c1f7d935");
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "BearerOrToken";
                    options.DefaultChallengeScheme = "BearerOrToken";
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    // asymmetric
                    options.Authority = identityUrl;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        NameClaimType = "name"
                    };
                })
                .AddJwtBearer("Token", options =>
                {
                    // symmetric
                    options.IncludeErrorDetails = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidIssuers = validIssuers,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ClockSkew = TimeSpan.Zero
                    };
                })
                .AddPolicyScheme("BearerOrToken", "BearerOrToken", options =>
                {
                    options.ForwardDefaultSelector = context =>
                    {
                        string authorization = context.Request.Headers[HeaderNames.Authorization].ToString();
                        if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Token "))
                        {
                            var token = authorization.Substring("Token ".Length).Trim();
                            var jwtHandler = new JwtSecurityTokenHandler();

                            var canRead = jwtHandler.CanReadToken(token);
                            var issuerIsValid = validIssuers.Contains(jwtHandler.ReadJwtToken(token).Issuer);

                            if (canRead && issuerIsValid)
                            {
                                context.Request.Headers.Authorization = authorization.Replace("Token", "Bearer");
                                return "Token";
                            }
                        }
                        return JwtBearerDefaults.AuthenticationScheme;
                    };
                });

            return services;
        }

        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrator", policy =>
                {
                    policy.RequireRole(IdentityService.Role.Administrator);
                });
                options.AddPolicy("Customer", policy =>
                {
                    policy.RequireRole(IdentityService.Role.Customer);
                });
            });

            services.AddSingleton<IAuthorizationPolicyProvider, RequirePermissionPolicyProvider>();
            services.AddSingleton<IAuthorizationHandler, RequirePermissionAuthorizationHandler>();

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration, string version)
        {
            var swaggerSetting = new SwaggerSetting();
            configuration.GetSection(nameof(SwaggerSetting)).Bind(swaggerSetting);

            services.AddSwaggerGen(options =>
            {
                options.DescribeAllParametersInCamelCase();

                options.SwaggerDoc(version, new OpenApiInfo
                {
                    Title = swaggerSetting.ServiceName,
                    Version = version,
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Token",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization using Bearer Scheme"
                });

                options.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
            });

            return services;
        }

    }
}
