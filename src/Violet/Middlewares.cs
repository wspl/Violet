using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Violet
{
    public class Middlewares
    {
        public Application App { get; set; }

        public delegate Task MiddlewareNext();
        public delegate Task MiddlewareDelegate(ApplicationContext context, MiddlewareNext next);
        public delegate Task EndMiddlewareDelegate(ApplicationContext context);

        private List<MiddlewareDelegate> MiddlewaresList { get; } = new List<MiddlewareDelegate>();

        public Middlewares(Application app)
        {
            App = app;
        }

        public void Add(MiddlewareDelegate middleware)
        {
            MiddlewaresList.Add(middleware);
        }

        internal async Task RequestCallback(HttpContext context)
        {
            var appContext = new ApplicationContext(context);
            await Toway(appContext, 0);
        }

        internal async Task Toway(ApplicationContext context, int i)
        {
            if (i < MiddlewaresList.Count)
            {
                await MiddlewaresList[i](context, async () => {
                    await Toway(context, i + 1);
                });
            }
            else if (i == MiddlewaresList.Count)
            {
                await App.Router.HandleRequest(context);
            }
        }
    }
}
