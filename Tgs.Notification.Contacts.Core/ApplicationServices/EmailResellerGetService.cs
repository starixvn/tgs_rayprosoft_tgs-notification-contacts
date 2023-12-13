using System.Text.Json;
using Tgs.ServiceUtility;
using Tgs.Notification.Contacts.Common;
using Tgs.Notification.Contacts.Common.Models;
using Tgs.Notification.Contacts.Core;
using Tgs.Notification.Contacts.Core.ApplicationServices;

namespace Tgs.Notification.Contacts.Core.ApplicationServices;

internal class EmailResellerGetService(
    IResellerRepository resellerRepository,
    INotificationContactRepository repository) : IGetService<EmailResellerGetQuery, EmailGetResult?>
{
    public async ValueTask<EmailGetResult?> GetAsync(EmailResellerGetQuery query, CancellationToken cancellationToken = default)
    {
        var resellerInfo = await resellerRepository.GetAsync(
            code: query.Code,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var contactInfo = await repository.GetAsync(
            resellerId: resellerInfo.ResellerId,
            licenseeId: Guid.Empty,
            brandId: Guid.Empty,
            type: ContactTypes.Email,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return contactInfo is null
            ? null
            : new EmailGetResult(
                ResellerId: contactInfo.ResellerId,
                ResellerCode: resellerInfo.ResellerCode,
                LicenseeId: contactInfo.LicenseeId,
                LicenseeCode: string.Empty,
                BrandId: contactInfo.BrandId,
                BrandCode: string.Empty,
                Contacts: JsonSerializer.Deserialize<IEnumerable<EmailContact>>(contactInfo.Contacts)
                ?? Enumerable.Empty<EmailContact>());
    }
}
