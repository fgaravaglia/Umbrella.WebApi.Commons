namespace Umbrella.WebApi.Commons.HealthChecks
{
    /// <summary>
    /// Settings for a given health check
    /// </summary>
    public class HealthCheckEnpointSettings
    { 
        /// <summary>
        /// Name of check endpoint
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Full URI of endpoint
        /// </summary>
        public string Uri { get; set; }
        /// <summary>
        /// Title of check; it is displayed on Check UI
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// realtive Path of check for self hosted checks
        /// </summary>
        public string RelativePath { get; set; }
        /// <summary>
        /// TRUE if check is self hosted in the same api
        /// </summary>
        public bool IsSelfHosted { get { return !String.IsNullOrEmpty(RelativePath); } }
        /// <summary>
        /// 
        /// </summary>
        public List<string> Tags { get; set; }
        /// <summary>
        /// Empty Constr
        /// </summary>
        public HealthCheckEnpointSettings()
        { 
            this.Name = string.Empty;
            this.Uri = string.Empty;
            this.RelativePath = string.Empty;
            this.Title= string.Empty;   
            this.Tags = new List<string>();
        }
    }
}
