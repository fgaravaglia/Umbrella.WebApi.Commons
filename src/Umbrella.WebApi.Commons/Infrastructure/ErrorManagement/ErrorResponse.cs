using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Umbrella.WebApi.Commons.Infrastructure.ErrorManagement
{
    /// <summary>
    /// Simple object to model an Error response for API
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// error message to display inside API response
        /// </summary>
        [JsonPropertyName("errorMessage")]
        public string? ErrorMessage { get; private set; }
        /// <summary>
        /// Transaction ID of request (useful for trouble shooting)
        /// </summary>
        [JsonPropertyName("transactionId")]
        public string? TransactionId { get; private set; }

        /// <summary>
        /// Default Constr
        /// </summary>
        /// <param name="message"></param>
        /// <param name="trxId"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ErrorResponse(string message, string trxId)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message));
            this.ErrorMessage = message;
            this.TransactionId = trxId;
        }
        /// <summary>
        /// converts the current object into string representation
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
