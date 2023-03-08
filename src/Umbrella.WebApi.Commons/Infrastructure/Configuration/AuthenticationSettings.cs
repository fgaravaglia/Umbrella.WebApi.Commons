

namespace Umbrella.WebApi.Commons.Infrastructure.Configuration
{
    /// <summary>
    /// Section Authetication from appSettings file
    /// </summary>
    public class AuthenticationSettings
    {
        /// <summary>
        /// List of IDs for enabled applications
        /// </summary>
        /// <value></value>
        public List<ClientSettings> Clients { get; set; }

        /// <summary>
        /// EMpty COnstr
        /// </summary>
        public AuthenticationSettings()
        {
            this.Clients = new List<ClientSettings>();
        }
    }
    /// <summary>
    /// Settings to store couple ClientID - AppId
    /// </summary>
    public class ClientSettings
    {
        /// <summary>
        /// Name of client
        /// </summary>
        /// <value></value>
        public string Name { get; set; }
        /// <summary>
        /// ID of client
        /// </summary>
        /// <value></value>
        public string ClientID { get; set; }
        /// <summary>
        /// ID of application
        /// </summary>
        /// <value></value>
        public string ApplicationID { get; set; }

        /// <summary>
        /// empty COnstr
        /// </summary>
        public ClientSettings()
        {
            this.Name = "";
            this.ApplicationID = "";
            this.ClientID = "";
        }
    }
}