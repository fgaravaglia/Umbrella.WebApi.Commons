using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace Umbrella.WebApi.Commons.SwaggerManagement
{
    /// <summary>
    /// Extensions to startup Swagger UI
    /// </summary>
    public static class SwaggerExtensions
    {
        /// <summary>
        /// Registers dependencies and configuration
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="executingApiXmlFile">xml file that contains comments on executing APi assembly</param>
        /// <param name="entitiesFile">xml file that contains comments of assembly that includes the entities used in swagger</param>
        public static void AddSwaggerServices(this WebApplicationBuilder builder, string executingApiXmlFile, string entitiesFile = "")
        {
            if(builder ==null)
                throw new ArgumentNullException(nameof(builder));
            if(String.IsNullOrEmpty(executingApiXmlFile))
                throw new ArgumentNullException(nameof(executingApiXmlFile)); 

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
               {
                   options.EnableAnnotations(true, true);
                   options.UseAllOfForInheritance();
                   options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                   {
                       Description = "JWT Authorization header using the bearer scheme",
                       Name = "Authorization",
                       In = ParameterLocation.Header,
                       Type = SecuritySchemeType.ApiKey
                   });
                   //options.SwaggerDoc("MyHealth-Headache-api", new OpenApiInfo { Title = "__DOMAINNAME api", Version = "v1" });

                   // format numbers
                   options.MapType<decimal>(() => new OpenApiSchema { Type = "number", Format = "decimal" });
                   options.MapType<decimal?>(() => new OpenApiSchema { Type = "number", Format = "decimal" });
                   options.IgnoreObsoleteActions();

                   // injects the filter to add predefined headers into each request
                   options.OperationFilter<SwaggerHeaderFilter>();

                   // Set the comments path for the Swagger JSON and UI.
                   var xmlPath = Path.Combine(AppContext.BaseDirectory, executingApiXmlFile);
                   options.IncludeXmlComments(xmlPath);
                   if (!string.IsNullOrEmpty(entitiesFile))
                   {
                       var entitiesPath = Path.Combine(AppContext.BaseDirectory, entitiesFile);
                       options.IncludeXmlComments(entitiesPath);
                   }
               });
            builder.Services.AddSwaggerGenNewtonsoftSupport();
            builder.Services.AddApiVersioning( options =>
               {
                   options.ReportApiVersions = true;
                   options.AssumeDefaultVersionWhenUnspecified = true;
                   options.DefaultApiVersion = ApiVersion.Default;
                   // combine header and/or media type version check
                   options.ApiVersionReader = ApiVersionReader.Combine(new HeaderApiVersionReader("X-Version"), new MediaTypeApiVersionReader("X-Version"));
               });
        }
        /// <summary>
        /// Startup UI
        /// </summary>
        /// <param name="app"></param>
        public static void UseUmbrellaSwaggerUI(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}
