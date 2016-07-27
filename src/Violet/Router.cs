using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Violet
{
    public class Router
    {
        public Application App { get; set; }

        private readonly List<RouterItem> _routersList = new List<RouterItem>();

        public Router(Application app)
        {
            App = app;
        }

        public void Bind(HttpMethods method, string path, RequestHandlerDelegate handler)
        {
            var matchRegular = "^" + Regex.Replace(path, @":\w+", @"([^\/]+?)") + "$";

            var paramsName = new Dictionary<string, int>();

            var pathNodes = path.Split('/');

            for (var i = 0; i < pathNodes.Length; i++)
            {
                var pathNode = pathNodes[i];
                if (pathNode.Length > 0 && pathNode[0] == ':')
                {
                    paramsName.Add(pathNode.Substring(1), i);
                }
            }

            _routersList.Add(new RouterItem
            {
                Method = method,
                Path = path,
                RequestHandler = handler,
                Params = paramsName,
                MatchRegular = matchRegular
            });
        }

        public async Task HandleRequest(ApplicationContext context)
        {
            var path = context.Request.Path.Value;

            foreach (var router in _routersList)
            {
                if (!Regex.Match(path, router.MatchRegular).Success) continue;

                var pathNodes = path.Split('/');

                foreach (var node in router.Params)
                {
                    context.Param.Add(node.Key, pathNodes[node.Value]);
                }

                await router.RequestHandler(context);

                break;
            }
        }
    }
}