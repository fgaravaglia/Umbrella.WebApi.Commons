using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Umbrella.WebApi.Commons.HealthChecks
{
    /// <summary>
    /// Extensions to setup and configure HealthCheck UI at startup+
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class HealthChecksExtensions
    {
        /// <summary>
        /// Reads appsettings file
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IEnumerable<HealthCheckEnpointSettings> GetHealthCheckEndpointSettings(this IConfiguration config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            var settings = new List<HealthCheckEnpointSettings>();
            config.GetSection("HealthChecksUI:HealthChecks").Bind(settings);
            return settings;
        }
        /// <summary>
        /// Reads appsettings file
        /// </summary>
        /// <param name="config"></param>
        /// <param name="checkName"></param>
        /// <returns></returns>
        public static HealthCheckEnpointSettings? GetCheckEndpointSettings(this IConfiguration config, string checkName)
        {
            if (String.IsNullOrEmpty(checkName))
                throw new ArgumentNullException(nameof(checkName));
            return config.GetHealthCheckEndpointSettings().SingleOrDefault(x => x.Name.Equals(checkName, StringComparison.InvariantCultureIgnoreCase));
        }
        /// <summary>
        /// Checks if UI is enabled
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool IsHealthChecksUIEnabled(this IConfiguration config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            return config.GetSection("HealthChecksUI:Enabled").ToString().ToLowerInvariant() == "true";
        }
    }
}
