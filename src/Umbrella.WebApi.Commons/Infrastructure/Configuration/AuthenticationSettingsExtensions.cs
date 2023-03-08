using Microsoft.Extensions.Configuration;

namespace Umbrella.WebApi.Commons.Infrastructure.Configuration
{
    /// <summary>
    /// Extensions to read authentication section from appsettings
    /// </summary>
    public static class AuthenticationSettingsExtensions
    {
        /// <summary>
        /// Reads appsettings file
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static AuthenticationSettings GetAuthenticationSettings(this IConfiguration config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            var settings = new AuthenticationSettings();
            config.GetSection("Authentication").Bind(settings);
            return settings;
        }
    }
}