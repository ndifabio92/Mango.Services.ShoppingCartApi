using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace ShoppingCartApi.Extensions
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwaggerDefinition(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer()
                    .AddSwaggerGen(option =>
                    {
                        option.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
                        {
                            Name = "Authorization",
                            Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
                            In = ParameterLocation.Header,
                            Type = SecuritySchemeType.ApiKey,
                            Scheme = "Bearer",
                            BearerFormat = "JWT"
                        });
                        option.AddSecurityRequirement(new OpenApiSecurityRequirement
                        {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference= new OpenApiReference
                                    {
                                        Type=ReferenceType.SecurityScheme,
                                        Id=JwtBearerDefaults.AuthenticationScheme
                                    }
                                }, new string[]{}
                            }
                        });
                    });

            return services;
        }
    }
}
