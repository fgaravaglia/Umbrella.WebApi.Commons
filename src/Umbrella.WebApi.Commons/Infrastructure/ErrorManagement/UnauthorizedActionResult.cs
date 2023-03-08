using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Umbrella.WebApi.Commons.Infrastructure.ErrorManagement
{


    /// <summary>
    /// Action Result to push back in case of Internal server error
    /// </summary>
    public class UnauthorizedActionResult : ObjectResult
    {
        /// <summary>
        /// Error repsonse
        /// </summary>
        [JsonProperty]
        public ErrorResponse? Error { get { return this.Value != null ? (ErrorResponse)this.Value : null; } }

        /// <summary>
        /// default constr
        /// </summary>
        /// <param name="message"></param>
        /// <param name="trxId"></param>
        public UnauthorizedActionResult(string message, string trxId) : base(new ErrorResponse(message, trxId))
        {
            this.StatusCode = StatusCodes.Status401Unauthorized;
        }
    }
}
