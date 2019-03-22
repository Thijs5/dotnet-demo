using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.Swagger;
using System.Reflection;
using System.IO;
using System;

namespace MyApplication.API
{
    internal static class SwaggerConfiguration
    {
        internal static void Configure(IServiceCollection services)
        {
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My Application API", Version = "v1" });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        internal static void Configure(IApplicationBuilder app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                
                // Our application consists only an API (every route is behind /api/).
                // Setting the RoutePrefix to empty servers Swagger at the root of our application.
                c.RoutePrefix = string.Empty;
            });
        }
    }
}