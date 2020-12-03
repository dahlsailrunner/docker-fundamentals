using System;
using Microsoft.AspNetCore.Http;

namespace Globomantics.Api.Middleware
{
    public class ApiExceptionOptions
    {       
        public Action<HttpContext, Exception, ApiError> AddResponseDetails { get; set; }        
    }
}
