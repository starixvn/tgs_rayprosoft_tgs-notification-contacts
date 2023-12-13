using Tgs.Notification.Contacts.Core.Models;

namespace Tgs.Notification.Contacts.Core;

public interface IResellerRepository
{
    ValueTask<ResellerInfo> GetAsync(string code, CancellationToken cancellationToken = default);

    ValueTask<ResellerInfo> GetAsync(Guid id, CancellationToken cancellationToken = default);
}
