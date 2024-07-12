using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarapinhaXpto.Shared.Responses
{
    public class ServiceResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; } 
        public ServiceResponse()
        {
            Success = false;
        }

    }

    public class ServiceResponseLogin<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public bool RequiresPasswordChange { get; set; } 



    }
    public class ServiceResponse<T> : ServiceResponse
    {
        public T Data { get; set; }
    }
   
}
