using System;
using System.Net;

namespace PCal
{
    public class HttpResponseException:ApplicationException
    {
        public HttpStatusCode HttpStatusCode { get; }

        public HttpResponseException(HttpStatusCode httpStatusCode)
        {
            HttpStatusCode = httpStatusCode;
        }

        public HttpResponseException(HttpStatusCode httpStatusCode, string message) : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }

        public HttpResponseException(HttpStatusCode httpStatusCode, string message, Exception innerException)
            : base(message, innerException)
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}