using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Violet
{
    public delegate Task RequestHandlerDelegate(ApplicationContext context);

    public class RouterItem
    {
        public string Path { get; set; }
        public HttpMethods Method { get; set; }
        public RequestHandlerDelegate RequestHandler { get; set; }

        public Dictionary<string, int> Params { get; set; }
        public string MatchRegular { get; set; }
    }
}
