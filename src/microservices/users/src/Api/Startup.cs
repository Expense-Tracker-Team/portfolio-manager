namespace Api
{
    using System;
    using System.Threading.Tasks;
    using System.Reflection;
    using Api.Interceptors;
    using Jaeger;
    using Jaeger.Samplers;
    using Application.Interfaces.Infrastructure.Metrics;
    using Infrastructure.Metrics;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using OpenTracing;
    using OpenTracing.Util;
    using Prometheus;
    using Prometheus.DotNetRuntime;
    using Serilog;
    using Jaeger.Reporters;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            this.ConfigureLoggingServices(services);
            this.ConfigureGrpcServices(services);
            this.ConfigureTracingServices(services);
        }

        // This method configures the logging fo the application using Serilog
        public void ConfigureLoggingServices(IServiceCollection services)
        {
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
        }

        public void ConfigureTracingServices(IServiceCollection services)
        {
            services.AddSingleton<ITracer>(serviceProvider =>
            {
                ILoggerFactory loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                var config = Jaeger.Configuration.FromEnv(loggerFactory);
                ITracer tracer = config.GetTracer();
                GlobalTracer.Register(tracer);

                return tracer;
            });

            services.AddOpenTracing();
        }

        public void ConfigureGrpcServices(IServiceCollection services)
        {
            services.AddGrpc(options =>
            {
                options.Interceptors.Add<ServerInterceptor>();

                options.MaxReceiveMessageSize = 1 * 1024 * 1024; // 1 MB
                options.MaxSendMessageSize = 1 * 1024 * 1024; // 1 MB
            });

            services.AddTransient<IMetricsRegistry, PrometheusMetricsRegistry>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogRequestLogging();
            app.UseRouting();

            // Configures request pipeline to collect Prometheus metrics
            app.UseHttpMetrics();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<UserHandlerV1>();

                // Starts a Prometheus metrics exporter using endpoint routing. The default URL is /metrics,
                endpoints.MapMetrics();

                endpoints.MapGet("/", async context =>
                {
                    // Track test metrics
                    IMetricsRegistry metricsRegistry = app.ApplicationServices.GetRequiredService<IMetricsRegistry>();

                    using (metricsRegistry.HistogramGrpcCallsDuration())
                    {
                        metricsRegistry.CountGrpcCalls();

                        var random = new Random();
                        if (random.Next(0, 2) == 0)
                        {
                            metricsRegistry.CountFailedGrpcCalls();
                        }

                        var delayMilliseconds = random.Next(0, 2000);
                        await Task.Delay(delayMilliseconds);

                        await context.Response.WriteAsync(
                            "Users service is up and running!\n" +
                            "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                    }
                });
            });
        }
    }
}
