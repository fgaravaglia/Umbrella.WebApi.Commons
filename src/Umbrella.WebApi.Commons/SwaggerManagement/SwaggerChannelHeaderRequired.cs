using System.Diagnostics.CodeAnalysis;

namespace Umbrella.WebApi.Commons.SwaggerManagement
{
    /// <summary>
    /// Attribute to add header param called channel
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SwaggerChannelHeaderRequired : SwaggerHeaderAttribute
    {
        public const string ParameterName = "channel";

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SwaggerChannelHeaderRequired() 
            : base(ParameterName, description: "Set the channel that is consuming the endpoint", isRequired: true)
        {
        }
    }
}
