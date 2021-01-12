using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace SaeedrezayiWebsite.Api.WebApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddSwaggerExtension(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                //c.IncludeXmlComments(string.Format(@"{0}\SaeedrezayiWebsite.Api.WebApi.xml", AppDomain.CurrentDomain.BaseDirectory));
                var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory,
                               "*.xml", SearchOption.TopDirectoryOnly).ToList();
                xmlFiles.ForEach(xmlFile => c.IncludeXmlComments(xmlFile));

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "SaeedRezayi Blog V1.0 API",
                    Description = "Through this API you can access blog and all it's content's.",
                    Contact = new OpenApiContact()
                    {
                        Email = "saeedrezayi@saeedrezayi.ir",
                        Name = "Saeed Rezayi",
                        Url = new Uri("http://www.saeedrezayi.ir")
                    },
                    License = new OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });
                c.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        //BearerFormat = "Bearer "
                        Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
                    });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            },
                            //Scheme = "oauth2",
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                            BearerFormat = "Bearer "
                        }, new List<string>()
                    },
                });
                //c.DescribeAllEnumsAsStrings();
            });
        }
        public static void AddApiVersioningExtension(this IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                // Specify the default API Version as 1.0
                config.DefaultApiVersion = new ApiVersion(1, 0);
                // If the client hasn't specified the API version in the request, use the default API version number 
                config.AssumeDefaultVersionWhenUnspecified = true;
                // Advertise the API versions supported for the particular endpoint
                config.ReportApiVersions = true;
            });
        }
    }
}
