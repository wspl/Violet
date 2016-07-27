using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Internal.Http;

using MiddlewareDelegate = Violet.Middlewares.MiddlewareDelegate;

namespace Violet
{
    public class Application
    {
        internal Middlewares Middlewares { get; set; }
        internal Router Router { get; set; }

        public Application()
        {
            Middlewares = new Middlewares(this);
            Router = new Router(this);
        }

        public Application Use(MiddlewareDelegate middleware)
        {
            Middlewares.Add(middleware);
            return this;
        }

        private void RunKestrel(string host, int port)
        {
            var server = new WebHostBuilder()
                .UseKestrel()
                .UseUrls($"http://{host}:{port}")
                .UseWebRoot("public")
                .Configure(app => app.Run(Middlewares.RequestCallback))
                .Build();

            server.Run();
        }

        public void Listen(int port)
        {
            RunKestrel("*", port);
        }

        public void Get(string path, RequestHandlerDelegate handler) => Router.Bind(HttpMethods.Get, path, handler);
        public void Post(string path, RequestHandlerDelegate handler) => Router.Bind(HttpMethods.Post, path, handler);
        public void Put(string path, RequestHandlerDelegate handler) => Router.Bind(HttpMethods.Put, path, handler);
        public void Delete(string path, RequestHandlerDelegate handler) => Router.Bind(HttpMethods.Delete, path, handler);
        public void Patch(string path, RequestHandlerDelegate handler) => Router.Bind(HttpMethods.Patch, path, handler);
    }
}
