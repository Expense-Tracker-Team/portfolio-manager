namespace Api
{
    using System;
    using Api.Interceptors;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using Prometheus;

    using Serilog;
    using Serilog.Events;
    using Serilog.Sinks.Elasticsearch;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            // Create Serilog Elasticsearch logger
            Log.Logger = new LoggerConfiguration()
               .Enrich.FromLogContext()
               .MinimumLevel.Debug()
               .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
               {
                   MinimumLogEventLevel = LogEventLevel.Verbose,
                   AutoRegisterTemplate = true
               })
               .WriteTo.Console()
               .CreateLogger();
            

            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

            services.AddGrpc(options =>
            {
                options.Interceptors.Add<ExceptionHandleInterceptor>();
                options.Interceptors.Add<LoggerInterceptor>();

                options.MaxReceiveMessageSize = 1 * 1024 * 1024; // 1 MB
                options.MaxSendMessageSize = 1 * 1024 * 1024; // 1 MB
            });
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
            app.UseHttpMetrics();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<UserHandlerV1>();
                endpoints.MapMetrics();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Users service is up and running!\n" +
                        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
