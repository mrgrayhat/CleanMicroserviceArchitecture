using Microsoft.AspNetCore.Builder;
using SaeedrezayiWebsite.Api.WebApi.Middlewares;

namespace SaeedrezayiWebsite.Api.WebApi.Extensions
{
    /// <summary>
    /// The Web Application Extentions
    /// </summary>
    public static class AppExtensions
    {
        public static void UseSwaggerExtension(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CleanArchitecture.SaeedrezayiWebsite.Api.WebApi");
            });
        }
        public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }

}
