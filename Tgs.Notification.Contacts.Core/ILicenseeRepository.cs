using Tgs.Notification.Contacts.Core.Models;

namespace Tgs.Notification.Contacts.Core;

public interface ILicenseeRepository
{
    ValueTask<LicenseeInfo> GetAsync(string code, CancellationToken cancellationToken = default);

    ValueTask<LicenseeInfo> GetAsync(Guid id, CancellationToken cancellationToken = default);
}
