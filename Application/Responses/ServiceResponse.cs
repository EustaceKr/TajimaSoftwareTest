using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public class ServiceResponse<T> : BaseServiceResponse
    {
        public T? Data { get; set; }
        
        public ServiceResponse(HttpStatusCode response ,string? error ,T? data) : base(response, error)
        {
            Data = data;
        }
    }
}
