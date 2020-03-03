namespace Api
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Server.Kestrel.Core;
    using Microsoft.Extensions.Hosting;
    using Prometheus.DotNetRuntime;
    using Serilog;
    using System;
    using System.Net;

    public class Program
    {
        public static void Main(string[] args)
        {
            DotNetRuntimeStatsBuilder
                .Customize()
                .WithThreadPoolSchedulingStats()
                .WithContentionStats()
                .WithGcStats()
                .WithJitStats()
                .WithThreadPoolStats()
                .WithErrorHandler(ex => Console.WriteLine("ERROR: " + ex.ToString()))
                //.WithDebuggingMetrics(true);
                .StartCollecting();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .ConfigureKestrel(options =>
                            {
                                options.Listen(IPAddress.Any, 80, listenOptions =>
                                {
                                    listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                                });

                                options.Listen(IPAddress.Any, 5001, listenOptions =>
                                {
                                    listenOptions.Protocols = HttpProtocols.Http2;
                                });
                            });
                });
    }
}
