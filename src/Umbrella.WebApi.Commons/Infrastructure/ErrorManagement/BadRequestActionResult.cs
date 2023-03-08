using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Umbrella.WebApi.Commons.Infrastructure.ErrorManagement
{
    /// <summary>
    /// Action Result to push back in case of Bad Request
    /// </summary>
    public class BadRequestActionResult : ObjectResult
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
        public BadRequestActionResult(string message, string trxId) : base(new ErrorResponse(message, trxId))
        {
            this.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}
