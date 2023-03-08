
namespace Umbrella.WebApi.Commons.Infrastructure.ErrorManagements
{
    /// <summary>
    /// /Exception raised in case aumbrella token is not valid
    /// </summary>
    public class UmbrellaTokenInvalidException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string ClientID { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public string ApplicationId { get; private set; }
        /// <summary>
        /// Default COnstr
        /// </summary>
        /// <param name="message"></param>
        /// <param name="clientId"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public UmbrellaTokenInvalidException(string message, string clientId, string applicationId) : base(message)
        {
            this.ClientID = clientId;
            this.ApplicationId = applicationId;
        }
    }
}