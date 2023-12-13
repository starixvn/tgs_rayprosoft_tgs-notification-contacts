using System.Text.Json;
using Tgs.ServiceUtility;
using Tgs.Notification.Contacts.Common;
using Tgs.Notification.Contacts.Common.Models;
using Tgs.Notification.Contacts.Core;
using Tgs.Notification.Contacts.Core.ApplicationServices;

namespace Tgs.Notification.Contacts.Core.ApplicationServices;

internal class EmailLicenseeGetService(
    ILicenseeRepository licenseeRepository,
    INotificationContactRepository repository) : IGetService<EmailLicenseeGetQuery, EmailGetResult?>
{
    public async ValueTask<EmailGetResult?> GetAsync(EmailLicenseeGetQuery query, CancellationToken cancellationToken = default)
    {
        var licenseeInfo = await licenseeRepository.GetAsync(
            code: query.Code,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var contactInfo = await repository.GetAsync(
            resellerId: licenseeInfo.ResellerId,
            licenseeId: licenseeInfo.LicenseeId,
            brandId: Guid.Empty,
            type: ContactTypes.Email,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return contactInfo is null
            ? null
            : new EmailGetResult(
                ResellerId: contactInfo.ResellerId,
                ResellerCode: licenseeInfo.ResellerCode,
                LicenseeId: contactInfo.LicenseeId,
                LicenseeCode: licenseeInfo.LicenseeCode,
                BrandId: contactInfo.BrandId,
                BrandCode: string.Empty,
                JsonSerializer.Deserialize<IEnumerable<EmailContact>>(contactInfo.Contacts)
                ?? Enumerable.Empty<EmailContact>());
    }
}
