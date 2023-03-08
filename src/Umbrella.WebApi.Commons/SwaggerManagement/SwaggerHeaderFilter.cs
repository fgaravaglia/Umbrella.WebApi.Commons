using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace Umbrella.WebApi.Commons.SwaggerManagement
{
    /// <summary>
    /// Filter to be applied to all requests in order to add proper parameters to request also in Swagger file
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SwaggerHeaderFilter : IOperationFilter
    {
        /// <summary>
        /// <inheritdoc cref="IOperationFilter.Apply(OpenApiOperation, OperationFilterContext)"/>
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            var attributes = context.MethodInfo.GetCustomAttributes(typeof(SwaggerHeaderAttribute));

            foreach (var a in attributes.Where(x => x is SwaggerHeaderAttribute))
            {
                SwaggerHeaderAttribute attribute = (SwaggerHeaderAttribute)a;
                var existingParam = operation.Parameters.FirstOrDefault(p => p.In == ParameterLocation.Header && p.Name == attribute.HeaderName);
                if (existingParam != null) // remove description from [FromHeader] argument attribute
                {
                    operation.Parameters.Remove(existingParam);
                }

                // genrate the proper parameter
                var parameter = new OpenApiParameter
                {
                    Name = attribute.HeaderName,
                    In = ParameterLocation.Header,
                    Description = attribute.Description,
                    Required = attribute.IsRequired,
                };

                // settings the schema
                if (!String.IsNullOrEmpty(attribute.EnumType))
                {
                    List<IOpenApiAny> validEnums = new List<IOpenApiAny>();
                    if (attribute.EnumType == SwaggerChannelHeaderRequired.ParameterName)
                    {
                        // get valid channels
                        var channels = new List<string>() { "GARAAPP", "MyHealth" };
                        // validate nput with legal entities
                        validEnums = channels.Select(x => OpenApiAnyFactory.CreateFromJson(JsonSerializer.Serialize(x))).ToList();
                    }
                    else
                        throw new NotImplementedException();

                    parameter.Schema = new OpenApiSchema
                    {
                        Type = "String",
                        Enum = validEnums,
                        ReadOnly = true
                    };
                }
                else
                {
                    parameter.Schema = string.IsNullOrEmpty(attribute.DefaultValue)
                        ? null
                        : new OpenApiSchema
                        {
                            Type = "String",
                            Default = new OpenApiString(attribute.DefaultValue),
                            ReadOnly = true
                        };
                }

                // add it to all requests
                operation.Parameters.Add(parameter);
            }

        }
    }
}
