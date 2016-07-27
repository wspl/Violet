using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Violet
{
    public class Program

    {
        public static void Main(string[] args)
        {
            var app = new Application();

            app.Use(async (ctx, next) =>
            {
                var start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                await next();
                var ms = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - start;
                Debug.WriteLine($"{ctx.Request.Method} {ctx.Request.Path} - {ms}ms");
            });

            app.Get("/", async ctx =>
            {
                await Task.Delay(200);
                ctx.Body = $"{ctx.Request.Method} {ctx.Request.Path}";
            });

            app.Get("/est/:asdf/:fsdaf", async ctx => 
            {
                await Task.Delay(200);
                ctx.Body = $"Routing Successful! {ctx.Request.Method} {ctx.Request.Path}";
            });

            app.Get("/test", async ctx => 
            {
                await Task.Delay(200);
                ctx.Body = $"Routing Successful! {ctx.Request.Method} {ctx.Request.Path}";
            });

            app.Listen(5000);
        }
    }
}
