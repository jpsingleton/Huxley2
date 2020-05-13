// © James Singleton. EUPL-1.2 (see the LICENSE file for the full license governing this code).

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Runtime.InteropServices;

namespace Huxley2
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            // CreateDefaultBuilder adds Console, Debug, EventSource and EventLog (Windows only from Core 3+) logging providers
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        // Workaround for HTTP2 bug in .NET Core 3.1 and Windows 8.1 / Server 2012 R2
                        // Missing ciphers when hosting in console (not behind IIS) and using HTTPS
                        webBuilder.UseKestrel(options =>
                            options.ConfigureEndpointDefaults(defaults =>
                                defaults.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1
                            )
                        );
                        // Need to call these again so we can still use IIS if desired
                        webBuilder.UseIIS();
                        webBuilder.UseIISIntegration();
                    }
                });

    }
}
