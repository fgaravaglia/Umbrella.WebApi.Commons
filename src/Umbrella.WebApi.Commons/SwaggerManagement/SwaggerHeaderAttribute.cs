using System;
using System.Diagnostics.CodeAnalysis;

namespace Umbrella.WebApi.Commons.SwaggerManagement
{
    /// <summary>
    /// Attribute to set the proper headers also in Swagger file
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SwaggerHeaderAttribute : Attribute
    {
        /// <summary>
        /// Name of Parameter defined as header
        /// </summary>
        public string HeaderName { get; }
        /// <summary>
        /// Generid description of the param
        /// </summary>
        public string Description { get; }
        /// <summary>
        /// default value
        /// </summary>
        public string DefaultValue { get; }
        /// <summary>
        /// Madatory (yes/no)
        /// </summary>
        public bool IsRequired { get; }
        /// <summary>
        /// Type of enums to validate the input.
        /// </summary>
        public string EnumType { get; }

        /// <summary>
        /// Defualt Constr
        /// </summary>
        /// <param name="headerName"></param>
        /// <param name="description"></param>
        /// <param name="defaultValue"></param>
        /// <param name="isRequired"></param>
        /// <param name="enumType"></param>
        public SwaggerHeaderAttribute(string headerName, string description = "", string defaultValue = "", bool isRequired = false, string enumType = "")
        {
            HeaderName = headerName;
            Description = description;
            DefaultValue = defaultValue;
            IsRequired = isRequired;
            EnumType = enumType;
        }
    }
}
