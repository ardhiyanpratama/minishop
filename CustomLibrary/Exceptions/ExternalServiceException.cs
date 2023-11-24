using System.Net;
using System.Net.Http;

namespace CustomLibrary.Exceptions
{
    public class ExternalServiceException : HttpRequestException
    {
        public string ServiceName { get; init; }

        public ExternalServiceException(string message, HttpStatusCode statusCode) : base(message, null, statusCode)
        {

        }

        public ExternalServiceException(string message, HttpStatusCode statusCode, string serviceName) : base(message, null, statusCode)
        {
            ServiceName = serviceName;
        }
    }
}
