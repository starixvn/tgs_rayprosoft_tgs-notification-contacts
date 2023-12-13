using Tgs.Notification.Contacts.Common.Models;

namespace Tgs.Notification.Contacts.Client;

public interface INotificationContactsClient
{
    public ValueTask<EmailContactGet?> EmailBrandGetAsync(string code, CancellationToken cancellationToken = default);

    public ValueTask EmailBrandMergeAsync(string code, IEnumerable<EmailContact> contacts, CancellationToken cancellationToken = default);

    public ValueTask<EmailContactGet?> EmailLicenseeGetAsync(string code, CancellationToken cancellationToken = default);

    public ValueTask EmailLicenseeMergeAsync(string code, IEnumerable<EmailContact> contacts, CancellationToken cancellationToken = default);

    public ValueTask<EmailContactGet?> EmailResellerGetAsync(string code, CancellationToken cancellationToken = default);

    public ValueTask EmailResellerMergeAsync(string code, IEnumerable<EmailContact> contacts, CancellationToken cancellationToken = default);
}
