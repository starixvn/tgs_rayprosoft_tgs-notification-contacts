using Tgs.Notification.Contacts.Core.Models;

namespace Tgs.Notification.Contacts.Core;

public interface IBrandRepository
{
    ValueTask<BrandInfo> GetAsync(string code, CancellationToken cancellationToken = default);

    ValueTask<BrandInfo> GetAsync(Guid id, CancellationToken cancellationToken = default);
}
