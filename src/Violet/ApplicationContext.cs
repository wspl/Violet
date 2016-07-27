using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Violet
{
    public class ApplicationContext
    {
        public ApplicationContext(HttpContext rawContext)
        {
            RawContext = rawContext;
        }

        public HttpContext RawContext;

        public string Body
        {
            set { RawContext.Response.WriteAsync(value); }
        }

        public Dictionary<string, string> Param { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> Query { get; set; } = new Dictionary<string, string>();

        public HttpRequest Request => RawContext.Request;

        public HttpResponse Response => RawContext.Response;
    }
}
