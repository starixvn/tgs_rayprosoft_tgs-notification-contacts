using System.Text.Json;
using Tgs.ServiceUtility;
using Tgs.Notification.Contacts.Common;
using Tgs.Notification.Contacts.Common.Models;
using Tgs.Notification.Contacts.Core;
using Tgs.Notification.Contacts.Core.ApplicationServices;

namespace Tgs.Notification.Contacts.Core.ApplicationServices;

internal class EmailBrandGetService(
    IBrandRepository brandRepository,
    INotificationContactRepository repository) : IGetService<EmailBrandGetQuery, EmailGetResult?>
{
    public async ValueTask<EmailGetResult?> GetAsync(EmailBrandGetQuery query, CancellationToken cancellationToken = default)
    {
        var brandInfo = await brandRepository.GetAsync(
            code: query.Code,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var contactInfo = await repository.GetAsync(
            resellerId: brandInfo.ResellerId,
            licenseeId: brandInfo.LicenseeId,
            brandId: brandInfo.BrandId,
            type: ContactTypes.Email,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return contactInfo is null
            ? null
            : new EmailGetResult(
                ResellerId: contactInfo.ResellerId,
                ResellerCode: brandInfo.ResellerCode,
                LicenseeId: contactInfo.LicenseeId,
                LicenseeCode: brandInfo.LicenseeCode,
                BrandId: contactInfo.BrandId,
                BrandCode: brandInfo.BrandCode,
                JsonSerializer.Deserialize<IEnumerable<EmailContact>>(contactInfo.Contacts)
                ?? Enumerable.Empty<EmailContact>());
    }
}
