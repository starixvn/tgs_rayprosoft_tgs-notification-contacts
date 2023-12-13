using System.Text.Json;
using StackExchange.Redis;
using Tgs.Notification.Contacts.Common;
using Tgs.Notification.Contacts.Core;
using Tgs.Notification.Contacts.Core.Models;

namespace Tgs.Notification.Contacts.Repositories;

internal class NotificationContactCacheRepository(
    INotificationContactRepository repository,
    IDatabase database,
    TimeSpan Expiry) : INotificationContactRepository
{
    public async ValueTask AddAsync(NotificationContactAdd source, CancellationToken cancellationToken = default)
    {
        await repository.AddAsync(
            source: source,
            cancellationToken: cancellationToken)
             .ConfigureAwait(false);

        _ = await DeleteAsync(
            resellerId: source.ResellerId,
            licenseeId: source.LicenseeId,
            brandId: source.BrandId,
            type: source.Type)
            .ConfigureAwait(false);
    }

    public async ValueTask<NotificationContactInfo?> GetAsync(Guid resellerId, Guid licenseeId, Guid brandId, ContactTypes type, CancellationToken cancellationToken = default)
    {
        var info = await GetAsync(
            resellerId: resellerId,
            licenseeId: licenseeId,
            brandId: brandId,
            type: type)
            .ConfigureAwait(false);

        if (info is null)
        {
            info = await repository.GetAsync(
                resellerId: resellerId,
                licenseeId: licenseeId,
                brandId: brandId,
                type: type,
                cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (info is not null)
            {
                _ = await SetAsync(
                    source: info)
                    .ConfigureAwait(false);
            }
        }

        return info;
    }

    public async ValueTask UpdateAsync(NotificationContactUpdate source, CancellationToken cancellationToken = default)
    {
        await repository.UpdateAsync(
            source: source,
            cancellationToken: cancellationToken)
             .ConfigureAwait(false);

        _ = await DeleteAsync(
            resellerId: source.ResellerId,
            licenseeId: source.LicenseeId,
            brandId: source.BrandId,
            type: source.Type)
            .ConfigureAwait(false);
    }

    private static string GetKey(Guid resellerId, Guid licenseeId, Guid brandId, ContactTypes type)
=> string.Concat("notification:contact:", resellerId, ":", licenseeId, ":", brandId, ":", type.ToString());

    private async ValueTask<NotificationContactInfo?> GetAsync(Guid resellerId, Guid licenseeId, Guid brandId, ContactTypes type)
    {
        var key = GetKey(resellerId, licenseeId, brandId, type);
        if (await database.KeyExistsAsync(key).ConfigureAwait(false))
        {
            var value = await database.StringGetAsync(
                key: key)
                .ConfigureAwait(false);

            if (!value.IsNullOrEmpty)
            {
                return JsonSerializer.Deserialize<NotificationContactInfo>(value.ToString());
            }
        }
        return null;
    }

    private async ValueTask<bool> SetAsync(NotificationContactInfo source)
        => await database.StringSetAsync(
            key: GetKey(source.ResellerId, source.LicenseeId, source.BrandId, source.Type),
            value: JsonSerializer.Serialize(source),
            expiry: Expiry)
        .ConfigureAwait(false);

    private async ValueTask<bool> DeleteAsync(Guid resellerId, Guid licenseeId, Guid brandId, ContactTypes type)
        => await database.KeyDeleteAsync(
            key: GetKey(resellerId, licenseeId, brandId, type),
            flags: CommandFlags.FireAndForget)
        .ConfigureAwait(false);
}
