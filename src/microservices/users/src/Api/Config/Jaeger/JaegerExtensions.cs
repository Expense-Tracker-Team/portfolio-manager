namespace Api.Config.Jaeger
{
    using Api.Constants;
    using global::Jaeger;
    using global::Jaeger.Reporters;
    using global::Jaeger.Samplers;
    using global::Jaeger.Senders;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using OpenTracing;
    using OpenTracing.Util;

    public static class JaegerExtensions
    {
        public static IServiceCollection ConfigureTracingServices(this IServiceCollection services)
        {
            JaegerOptions jaegerOptions = GetJaegerOptions(services);

            if (!jaegerOptions.Enabled)
            {
                return services;
            }

            services.AddSingleton<ITracer>(serviceProvider =>
            {
                ILoggerFactory loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

                RemoteReporter reporter = new RemoteReporter
                        .Builder()
                    .WithSender(new UdpSender(jaegerOptions.UdpHost, jaegerOptions.UdpPort, jaegerOptions.MaxPacketSize))
                    .WithLoggerFactory(loggerFactory)
                    .Build();

                ISampler sampler = GetJaegerSampler(jaegerOptions);

                Tracer tracer = new Tracer
                        .Builder(jaegerOptions.ServiceName)
                    .WithReporter(reporter)
                    .WithSampler(sampler)
                    .Build();

                GlobalTracer.Register(tracer);

                return tracer;
            });

            services.AddOpenTracing();

            return services;
        }

        private static JaegerOptions GetJaegerOptions(IServiceCollection services)
        {
            using ServiceProvider serviceProvider = services.BuildServiceProvider();
            IConfiguration configuration = serviceProvider.GetService<IConfiguration>();
            services.Configure<JaegerOptions>(configuration.GetSection(ApplicationSettingKeys.JaegerKey));

            return services.BuildServiceProvider().GetRequiredService<IOptions<JaegerOptions>>().Value;
        }

        private static ISampler GetJaegerSampler(JaegerOptions options)
        {
            return options.Sampler switch
            {
                "const" => new ConstSampler(true),
                "rate" => new RateLimitingSampler(options.MaxTracesPerSecond),
                "probabilistic" => new ProbabilisticSampler(options.SamplingRate),
                _ => new ConstSampler(true),
            };
        }

    }
}
