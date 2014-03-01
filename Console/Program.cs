using System;
using System.IO;
using System.Net.Http;
using System.Web.Http;
using System.Xml.Linq;
using System.Web.Http.SelfHost;

namespace NessusConsole
{
    class Program
    {
        static readonly Uri _baseAddress = new Uri("http://localhost:28763/");

        static void Main(string[] args)
        {
            // Set up server configuration
            HttpSelfHostConfiguration config = new HttpSelfHostConfiguration(_baseAddress);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Create server
            var server = new HttpSelfHostServer(config);

            // Start listening
            server.OpenAsync().Wait();
            Console.WriteLine("Listening on " + _baseAddress + " Hit ENTER to exit...");
            Console.ReadLine();
            server.CloseAsync().Wait();
        }
    }
}
