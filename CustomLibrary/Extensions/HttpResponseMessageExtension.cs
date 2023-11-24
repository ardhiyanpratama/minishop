using CustomLibrary.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CustomLibrary.Extensions
{
    public static class HttpResponseMessageExtension
    {
        public static async Task<T> MapToObjectResponseAsync<T>(this HttpResponseMessage httpResponse, string serviceName, CancellationToken cancellationToken = default)
            where T : class
        {
            switch (httpResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.Created:
                case HttpStatusCode.Accepted:
                    var responseAsString = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
                    if (typeof(T) == typeof(string))
                    {
                        return responseAsString as T;
                    }
                    else
                    {
                        return JsonConvert.DeserializeObject<T>(responseAsString);
                    }
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedAccessException($"Access Token Tidak Valid {serviceName}");
                default:
                    var errorBodyAsString = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
                    throw new ExternalServiceException(errorBodyAsString, httpResponse.StatusCode, serviceName);
            }
        }
    }
}
