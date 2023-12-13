using Microsoft.EntityFrameworkCore;
using Tgs.Notification.Contacts.Common;
using Tgs.Notification.Contacts.Core;
using Tgs.Notification.Contacts.Core.Models;
using Tgs.Notification.Contacts.Repositories.Entities;

namespace Tgs.Notification.Contacts.Repositories;

internal class NotificationContactRepository(TgsContext context) : INotificationContactRepository
{
    public async ValueTask AddAsync(NotificationContactAdd source, CancellationToken cancellationToken = default)
    {
        var entity = new NotificationContact
        {
            ResellerId = source.ResellerId,
            LicenseeId = source.LicenseeId,
            BrandId = source.BrandId,
            Type = (int)source.Type,
            Contacts = source.Contacts.ToJsonString(),
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow
        };

        _ = await context.NotificationContacts.AddAsync(
            entity: entity,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        _ = await context.SaveChangesAsync(
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async ValueTask<NotificationContactInfo?> GetAsync(Guid resellerId, Guid licenseeId, Guid brandId, ContactTypes type, CancellationToken cancellationToken = default)
        => await context.NotificationContacts
        .Where(predicate: predicate => predicate.ResellerId == resellerId
        && predicate.LicenseeId == licenseeId
        && predicate.BrandId == brandId
        && predicate.Type == (int)type)
        .Select(selector: selector => new NotificationContactInfo(
            selector.ResellerId,
            selector.LicenseeId,
            selector.BrandId,
            (ContactTypes)selector.Type,
            selector.Contacts,
            selector.CreatedOn,
            selector.UpdatedOn))
        .FirstOrDefaultAsync(
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

    public async ValueTask UpdateAsync(NotificationContactUpdate source, CancellationToken cancellationToken = default)
    {
        var entity = await context.NotificationContacts
            .SingleAsync(predicate: predicate => predicate.ResellerId == source.ResellerId
            && predicate.LicenseeId == source.LicenseeId
            && predicate.BrandId == source.BrandId
            && predicate.Type == (int)source.Type,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        entity.Contacts = source.Contacts.ToJsonString();
        entity.UpdatedOn = DateTime.UtcNow;

        _ = context.NotificationContacts.Update(entity: entity);

        _ = await context.SaveChangesAsync(
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}
