using System.Diagnostics.CodeAnalysis;

namespace Umbrella.WebApi.Commons.SwaggerManagement
{
    /// <summary>
    /// Attribute to add header param called trxId
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SwaggerBusinessTrxIdHeader : SwaggerHeaderAttribute
    {
        public const string ParameterName = "businessTrxId";

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SwaggerBusinessTrxIdHeader() 
            : base(ParameterName, description: "Set the Business Transaction Id for the call",  isRequired: false)
        {
        }
    }
}
