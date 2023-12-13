using System.Text.Json;
using System.Text.Json.Nodes;
using Tgs.ServiceUtility;
using Tgs.Notification.Contacts.Common;
using Tgs.Notification.Contacts.Core;
using Tgs.Notification.Contacts.Core.ApplicationServices;
using Tgs.Notification.Contacts.Core.Expansions;
using Tgs.Notification.Contacts.Core.Models;

namespace Tgs.Notification.Contacts.Core.ApplicationServices;

internal class EmailLicenseeMergeCommandService(
    ILicenseeRepository licenseeRepository,
    INotificationContactRepository repository) : ICommandService<EmailLicenseeMergeCommand>
{
    public async ValueTask ExecuteAsync(EmailLicenseeMergeCommand command, CancellationToken cancellationToken = default)
    {
        if (!command.Contacts.Any())
        {
            throw new ArgumentNullException("command.Contacts");
        }

        var info = await licenseeRepository.GetAsync(
            code: command.Code,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (await repository.GetAsync(
            resellerId: info.ResellerId,
            licenseeId: info.LicenseeId,
            brandId: Guid.Empty,
            type: ContactTypes.Email,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false) is null)
        {
            await repository.AddAsync(
                source: new NotificationContactAdd(
                    ResellerId: info.ResellerId,
                    LicenseeId: info.LicenseeId,
                    BrandId: Guid.Empty,
                    Type: ContactTypes.Email,
                    Contacts: command.Contacts.ToJsonNode()),
                cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
        else
        {
            await repository.UpdateAsync(
                source: new NotificationContactUpdate(
                    ResellerId: info.ResellerId,
                    LicenseeId: info.LicenseeId,
                    BrandId: Guid.Empty,
                    Type: ContactTypes.Email,
                    Contacts: command.Contacts.ToJsonNode()),
                cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
