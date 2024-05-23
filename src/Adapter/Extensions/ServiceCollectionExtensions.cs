using Adapter.Config;
using Application.Config;
using Domain.Services;
using Infra.Clients;
using Infra.Config;
using Infra.Config.PubSub;
using Infra.Models.Messages;
using Infra.Services.DataDog;
using Infra.Services.EasyIngest;
using Infra.Services.Subscriber;
using Infra.Utils;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using Questrade.Library.HealthCheck.AspNetCore.Extensions;
using Questrade.Library.PubSubClientHelper.Extensions;
using Questrade.Library.PubSubClientHelper.HealthCheck;
using Refit;
using Serilog;
using StatsdClient;

namespace Adapter.Extensions;

public static class ServiceCollectionExtensions
{
    internal static WebApplicationBuilder RegisterServices(
        this WebApplicationBuilder builder, AlertsAdapterConfiguration alertsAdapterConfiguration
    )
    {
        builder.AddQuestradeHealthCheck();
        builder.Host.UseSerilog((_, logConfiguration) =>
            logConfiguration.ReadFrom.Configuration(builder.Configuration));

        builder.Services.AddActimizeContext(alertsAdapterConfiguration.ActimizeConfiguration);
        builder.Services.AddControllers();
        builder.Services.AddDataDogMetrics(builder.Configuration);
        builder.Services.AddSubscribers(alertsAdapterConfiguration);
        builder.Services.AddTransient<IMetricService, DataDogMetricService>();

        return builder;
    }

    private static IServiceCollection AddActimizeContext(this IServiceCollection services,
        ActimizeConfiguration actimizeConfiguration)
    {
        var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>()
            .WaitAndRetryAsync(actimizeConfiguration.Retry,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        var timeoutPolicy = Policy
            .TimeoutAsync<HttpResponseMessage>(TimeSpan.FromMilliseconds(actimizeConfiguration.Timeout));

        services.AddTransient<IEasyIngestAuthenticationService, EasyIngestAuthenticationService>();
        services.AddTransient<IWorkItemsService, WorkItemsService>();
        services.TryAddSingleton(actimizeConfiguration);
        services.AddTransient<ActimizeCommonLogs>();
        services.AddRefitClient<IEasyIngestClient>()
            .ConfigureHttpClient(x => x.BaseAddress = new Uri(actimizeConfiguration.EasyIngestBaseUrl))
            .AddPolicyHandler(retryPolicy)
            .AddPolicyHandler(timeoutPolicy);

        return services;
    }

    private static void AddDataDogMetrics(this IServiceCollection services, IConfiguration configuration)
    {
        var dataDogConfiguration = configuration.GetSection("DataDog:StatsD").Get<DataDogConfiguration>();

        services.AddSingleton<IDogStatsd>(_ =>
        {
            var statsdConfig = new StatsdConfig
            {
                Prefix = dataDogConfiguration!.Prefix,
                StatsdServerName = dataDogConfiguration.HostName
            };

            var dogStatsdService = new DogStatsdService();
            dogStatsdService.Configure(statsdConfig);

            return dogStatsdService;
        });
    }

    private static IServiceCollection AddSubscribers(this IServiceCollection services, AlertsAdapterConfiguration configuration)
    {
        if (configuration.AlertsAdapterSubscriberConfiguration.Enable)
        {
            services.RegisterSubscriber<
                AlertsAdapterSubscriberConfiguration,
                AlertsAdapterMessage,
                AlertsAdapterSubscriber,
                SubscriberHealthCheck<
                    AlertsAdapterSubscriberConfiguration,
                    AlertsAdapterMessage
                >
            >(configuration.AlertsAdapterSubscriberConfiguration);
        }

        return services;
    }
}
