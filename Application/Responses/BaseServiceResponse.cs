using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public class BaseServiceResponse
    {
        public HttpStatusCode Response;
        public string? Error { get; set; }
        public BaseServiceResponse(HttpStatusCode response, string? error)
        {
            Response = response;
            Error = error;
        }
    }
}
