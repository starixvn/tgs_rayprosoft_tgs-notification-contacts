using Microsoft.EntityFrameworkCore;
using Tgs.Notification.Contacts;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNotificationContacts(
        this IServiceCollection services,
        Action<IServiceProvider, DbContextOptionsBuilder> tgsConfigure,
        Action<RedisOptions, IServiceProvider> redisOptions)
        => services
        .AddOptions<RedisOptions>()
        .Configure(redisOptions)
        .Services
        .AddSingleton<RedisConnectionManager>()
        .AddNotificationContactsCore()
        .AddNotificationContactsRepositories(
            tgsConfigure: tgsConfigure,
            redisDatabaseResolver: (sp) => sp.GetRequiredService<RedisConnectionManager>().Database);
}
