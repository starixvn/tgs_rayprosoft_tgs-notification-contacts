using Tgs.Notification.Contacts.Common;
using Tgs.Notification.Contacts.Core.Models;

namespace Tgs.Notification.Contacts.Core;

public interface INotificationContactRepository
{
    public ValueTask<NotificationContactInfo?> GetAsync(Guid resellerId, Guid licenseeId, Guid brandId, ContactTypes type, CancellationToken cancellationToken = default);

    public ValueTask AddAsync(NotificationContactAdd source, CancellationToken cancellationToken = default);

    public ValueTask UpdateAsync(NotificationContactUpdate source, CancellationToken cancellationToken = default);
}
