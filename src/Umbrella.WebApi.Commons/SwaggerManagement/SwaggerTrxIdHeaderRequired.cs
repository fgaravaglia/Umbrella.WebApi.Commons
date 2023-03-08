using System.Diagnostics.CodeAnalysis;

namespace Umbrella.WebApi.Commons.SwaggerManagement
{
    /// <summary>
    /// Attribute to add header param called trxId
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SwaggerTrxIdHeaderRequired : SwaggerHeaderAttribute
    {
        public const string ParameterName = "trxId";

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SwaggerTrxIdHeaderRequired() 
            : base(ParameterName, description: "Set the Transaction Id for the call",  isRequired: true)
        {
        }
    }
}
