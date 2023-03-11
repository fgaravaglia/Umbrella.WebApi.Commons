using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Umbrella.WebApi.Commons.SwaggerManagement;
using Microsoft.AspNetCore.Http;
using Umbrella.WebApi.Commons.Infrastructure.ErrorManagement;

namespace Umbrella.WebApi.Commons.SwaggerManagement.ControllerFilters
{
    /// <summary>
    /// Filter to apply to each Request to validate mandatory headers
    /// </summary>
    public class ChannelFilter : IActionFilter
    {
        readonly Serilog.ILogger _Logger;
        readonly IConfiguration _Config;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="config"></param>
        public ChannelFilter(Serilog.ILogger logger, IConfiguration config)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _Config = config ?? throw new ArgumentNullException(nameof(config));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                _Logger.Information("Start Channel filtering current Action of controller {controllerType}", context.Controller.GetType());

                //check if the method is auhtenticated or not
                var actionMethod = ExtractMethodFromController(context);
                var attribute = actionMethod.GetCustomAttributes(typeof(SwaggerChannelHeaderRequired), true)
                                                                              .Select(x => (SwaggerChannelHeaderRequired)x).FirstOrDefault();
                if (attribute == null)
                    return;

                // read channel value from header
                string channelValue = "";
                _Logger.Debug("Current Headers {headers}", context.HttpContext.Request.Headers);
                if (context.HttpContext.Request.Headers.Any(p => p.Key.Equals(SwaggerChannelHeaderRequired.ParameterName, StringComparison.CurrentCultureIgnoreCase)))
                    channelValue = context.HttpContext.Request.Headers
                                            .Single(p => p.Key.Equals(SwaggerChannelHeaderRequired.ParameterName, StringComparison.CurrentCultureIgnoreCase))
                                                .Value.ToString();
                else
                    channelValue = "";

                // verify acchannel is set
                if (string.IsNullOrEmpty(channelValue) && attribute.IsRequired)
                    throw new InvalidDataException(SwaggerChannelHeaderRequired.ParameterName + " is null");

                // verify channel is valid
                var settings = _Config.GetSection("Authentication:ValidChannels").Value ?? "";
                var validChannels = settings.Split(';').Select(x => x.ToUpperInvariant().Trim()).ToList();
                if (!validChannels.Contains(channelValue.ToUpperInvariant().Trim()))
                    throw new InvalidDataException(SwaggerChannelHeaderRequired.ParameterName + " is invalid");
            }
            catch (InvalidDataException securityEx)
            {
                _Logger.Error(securityEx, "Unauthorized channel access on controller {controllerType}", context.Controller.GetType());
                context.Result = new UnauthorizedActionResult(securityEx.Message, "");
            }
            catch (Exception ex)
            {
                _Logger.Error(ex, "Unexpected error from filtering action of controller {controllerType}", context.Controller.GetType());
                context.Result = new InternalServerErrorActionResult(ex.Message, "");
            }
            finally
            {
                _Logger.Information("End Channel filtering current Action of controller {controllerType}", context.Controller.GetType());
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        #region Private methods

        private string ExtractUmbrellaAuthTokenFromHeader(HttpRequest req)
        {
            if (req == null)
                throw new ArgumentNullException(nameof(req));

            string umbrellaAuthToken = "";
            _Logger.Debug("Current Headers {headers}", req.Headers);
            if (req.Headers.Any(p => p.Key.Equals(SwaggerUmbrellaAuthHeaderRequired.ParameterName, StringComparison.CurrentCultureIgnoreCase)))
                umbrellaAuthToken = req.Headers
                                        .Single(p => p.Key.Equals(SwaggerUmbrellaAuthHeaderRequired.ParameterName, StringComparison.CurrentCultureIgnoreCase))
                                            .Value.ToString();

            return umbrellaAuthToken;
        }

        private MethodInfo ExtractMethodFromController(ActionExecutingContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (context.ActionDescriptor == null)
                throw new ArgumentNullException(nameof(context), "action descriptor cannot be null");
            if (context.ActionDescriptor.RouteValues == null)
                throw new ArgumentNullException(nameof(context), "ActionDescriptor.RouteValues cannot be noll");

            string actionName = "none";
            var routeValues = context.ActionDescriptor.RouteValues.ToList();
            if (routeValues.Exists(x => x.Key.ToLowerInvariant() == "action"))
                actionName = routeValues.Single(x => x.Key.ToLowerInvariant() == "action").Value ?? "";
            _Logger.Information("ActionName: " + actionName);

            var actionMethod = context.Controller.GetType().GetMethods().SingleOrDefault(x => x.Name == actionName);
            if (actionMethod is null)
                throw new InvalidOperationException($"No Action '{context.ActionDescriptor.DisplayName}' inside controller  {context.Controller.GetType()} has been found");
            return actionMethod;
        }
        #endregion
    }
}
