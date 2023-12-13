using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Tgs.Notification.Contacts.Core;
using Tgs.Notification.Contacts.Repositories;
using Tgs.Notification.Contacts.Repositories.Entities;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollecitonExtensions
{
    public static IServiceCollection AddNotificationContactsRepositories(
        this IServiceCollection services,
        Action<IServiceProvider, DbContextOptionsBuilder> tgsConfigure,
        Func<IServiceProvider, IDatabase> redisDatabaseResolver)
        => services
        .AddDbContext<TgsContext>(tgsConfigure)
        .AddTransient<IResellerRepository, ResellerRepository>()
        .AddTransient<ILicenseeRepository, LicenseeRepository>()
        .AddTransient<IBrandRepository, BrandRepository>()
        .AddTransient<INotificationContactRepository, NotificationContactRepository>()
        .Decorate<INotificationContactRepository>((innerRepository, sp) => new NotificationContactCacheRepository(innerRepository, redisDatabaseResolver(sp), TimeSpan.FromMinutes(60)));
}
