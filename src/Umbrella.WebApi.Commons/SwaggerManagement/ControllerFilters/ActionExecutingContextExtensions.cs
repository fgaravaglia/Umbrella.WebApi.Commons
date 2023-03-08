using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace Umbrella.WebApi.Commons.SwaggerManagement.ControllerFilters
{
    static class ActionExecutingContextExtensions
    {
        public static MethodInfo ExtractMethodFromController(this ActionExecutingContext context)
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

            var actionMethod = context.Controller.GetType().GetMethods().SingleOrDefault(x => x.Name == actionName);
            if (actionMethod is null)
                throw new InvalidOperationException($"No Action '{context.ActionDescriptor.DisplayName}' inside controller  {context.Controller.GetType()} has been found");
            return actionMethod;
        }
    }
}
