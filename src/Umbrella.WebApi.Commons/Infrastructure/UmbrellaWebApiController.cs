using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Umbrella.WebApi.Commons.Infrastructure.ErrorManagement;
using Umbrella.WebApi.Commons.SwaggerManagement;

namespace Umbrella.WebApi.Commons.Infrastructure
{
    /// <summary>
    /// Base controller for API exposed by API
    /// </summary>
    [Route("v{v:apiversion}/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public abstract class UmbrellaWebApiController : ControllerBase
    {
        /// <summary>
        /// Logger of the controller
        /// </summary>
        protected ILogger _Logger;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected UmbrellaWebApiController(ILogger logger)
        {
            this._Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region Protected methods

        /// <summary>
        /// gets the value for channel, taken from http headers
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        protected string? ExtractChannelFromHeaders(HttpRequest req)
        {
            return ExtractHeader(req, SwaggerChannelHeaderRequired.ParameterName);
        }
        /// <summary>
        /// Process the request, validating the headers too
        /// </summary>
        /// <param name="req"></param>
        /// <param name="action"></param>
        /// <param name="errorAction">if set, it generates the InternalServer error repsonse; otherwise it is built by default</param>
        /// <returns></returns>
        protected IActionResult ProcessRequest(HttpRequest req, 
                                                Func<string, string, string, IActionResult> action, 
                                                Func<string, Exception, InternalServerErrorActionResult>? errorAction = null)
        {
            this._Logger.LogInformation("Verifying request {url}", req.GetDisplayUrl());
            var headerEx = ValidateHeaders(req, out string channel, out string trxId, out string businessTrxId);
            if (headerEx != null)
            {
                this._Logger.LogError("Bad Request: {errorMessage}", headerEx.Message);
                return new BadRequestActionResult(headerEx.Message, trxId);
            }

            // craeting the conttext to wrap the logs
            using (this._Logger.BeginScope("Context: {context}", new
            {
                Channel = channel,
                TransactionId = trxId,
                BusinessTransactionId = businessTrxId,
                url = req.GetDisplayUrl()
            }))
            {
                try
                {
                    this._Logger.LogInformation("Start processing Request");
                    return action.Invoke(channel, trxId, businessTrxId);
                }
                catch (Exception ex)
                {
                    this._Logger.LogError(ex, "Unexpected error during invoke of controller {controlelrType}", this.GetType().Name);
                    if (errorAction != null)
                        return errorAction.Invoke(trxId, ex);
                    else
                    {
                        if(ex.GetType() == typeof(ArgumentNullException))
                            return new BadRequestActionResult(ex.Message, trxId);
                        else
                            return new InternalServerErrorActionResult(ex.Message, trxId);
                    }
                }
                finally
                {
                    this._Logger.LogInformation("End processing Request");
                }
            }

        }

        #endregion

        #region Private methods

        static Exception? ValidateHeaders(HttpRequest req, out string channel, out string trxId, out string businessTrxId)
        {
            channel = "";
            trxId = "";
            businessTrxId = "";

            try
            {
                channel = ExtractHeader(req, SwaggerChannelHeaderRequired.ParameterName) ?? "";
                if (String.IsNullOrEmpty(channel))
                    throw new InvalidOperationException($"Header {SwaggerChannelHeaderRequired.ParameterName} is missing");

                trxId = ExtractHeader(req, SwaggerTrxIdHeaderRequired.ParameterName) ?? "";
                if (String.IsNullOrEmpty(trxId))
                    throw new InvalidOperationException($"Header {SwaggerTrxIdHeaderRequired.ParameterName} is missing");

                businessTrxId = ExtractHeader(req, SwaggerBusinessTrxIdHeader.ParameterName) ?? "";
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        static string? ExtractHeader(HttpRequest req, string key)
        {
            if (req == null)
                throw new ArgumentNullException(nameof(req));
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var headers = req.Headers;
            if (headers == null)
                throw new NullReferenceException($"Expected to get Headers from Request");
            if (headers.ContainsKey(key))
            {
                return headers[key];
            }
            else
                return "";
        }

        #endregion

        /// <summary>
        /// Health-check endpoint
        /// </summary>        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("healthcheck")]
        public virtual IActionResult HealthCheck()
        {
            return Ok("The service is up and running");
        }
    }
}
