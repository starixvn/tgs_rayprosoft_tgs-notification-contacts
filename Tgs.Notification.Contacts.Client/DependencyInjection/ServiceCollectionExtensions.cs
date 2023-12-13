using Microsoft.Extensions.Options;
using Tgs.Notification.Contacts.Client;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNotificationContactsClient(
        this IServiceCollection services,
        Action<NotificationContactsClientOptions, IServiceProvider> configure)
        =>
        services
        .AddOptions<NotificationContactsClientOptions>()
        .Configure(configure)
        .Services
        .AddHttpClient<INotificationContactsClient, NotificationContactsClient>()
        .ConfigureHttpClient((sp, httpClient) =>
        {
            var options = sp.GetRequiredService<IOptions<NotificationContactsClientOptions>>().Value;
            httpClient.BaseAddress = options.BaseUrl;
        })
        .AddHttpMessageHandler<LoggingHandler>()
        .Services
        .AddTransient<LoggingHandler>();
}
