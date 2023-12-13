using System.Text.Json;
using System.Text.Json.Nodes;
using Tgs.ServiceUtility;
using Tgs.Notification.Contacts.Common;
using Tgs.Notification.Contacts.Core;
using Tgs.Notification.Contacts.Core.ApplicationServices;
using Tgs.Notification.Contacts.Core.Expansions;
using Tgs.Notification.Contacts.Core.Models;

namespace Tgs.Notification.Contacts.Core.ApplicationServices;

internal class EmailResellerMergeCommandService(
    IResellerRepository resellerRepository,
    INotificationContactRepository repository) : ICommandService<EmailResellerMergeCommand>
{
    public async ValueTask ExecuteAsync(EmailResellerMergeCommand command, CancellationToken cancellationToken = default)
    {
        if (!command.Contacts.Any())
        {
            throw new ArgumentNullException("command.Contacts");
        }

        var info = await resellerRepository.GetAsync(
            code: command.Code,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (await repository.GetAsync(
            resellerId: info.ResellerId,
            licenseeId: Guid.Empty,
            brandId: Guid.Empty,
            type: ContactTypes.Email,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false) is null)
        {
            await repository.AddAsync(
                source: new NotificationContactAdd(
                    ResellerId: info.ResellerId,
                    LicenseeId: Guid.Empty,
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
                    LicenseeId: Guid.Empty,
                    BrandId: Guid.Empty,
                    Type: ContactTypes.Email,
                    Contacts: command.Contacts.ToJsonNode()),
                cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
