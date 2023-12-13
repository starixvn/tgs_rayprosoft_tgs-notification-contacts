using System.Text.Json;

using NSubstitute;
using Tgs.Notification.Contacts.Common;
using Tgs.Notification.Contacts.Common.Models;
using Tgs.Notification.Contacts.Core.ApplicationServices;
using Tgs.Notification.Contacts.Core.Models;

namespace Tgs.Notification.Contacts.Core.UnitTest.ApplicationServices;

public class EmailResellerMergeCommandServiceUnitTest
{
    [Fact]
    public async void Add()
    {
        var fakeResellerRepository = Substitute.For<IResellerRepository>();
        var fakeNotificationContactRepository = Substitute.For<INotificationContactRepository>();

        var command = new EmailResellerMergeCommand(
            Code: "GPI",
            Contacts: new EmailContact[]
            {
                new() {
                    Name="tst",
                    Email="tst@rayprosoft.com",
                    Remark="tst"
                }
            });

        var resellerInfo = new ResellerInfo(
            ResellerId: Guid.Parse("b5a2c708-ab70-4316-9f4d-5d2ad0e77460"),
            ResellerCode: "TGS");
        _ = fakeResellerRepository.GetAsync(
            Arg.Is(command.Code),
            Arg.Any<CancellationToken>())
            .Returns(resellerInfo);

        _ = fakeNotificationContactRepository.GetAsync(
            resellerId: Arg.Is(resellerInfo.ResellerId),
            licenseeId: Arg.Is(Guid.Empty),
            brandId: Arg.Is(Guid.Empty),
            type: Arg.Is(ContactTypes.Email),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns((NotificationContactInfo?)null);

        var service = new EmailResellerMergeCommandService(
            fakeResellerRepository,
            fakeNotificationContactRepository);

        await service.ExecuteAsync(command);

        _ = fakeNotificationContactRepository
            .Received(1)
            .AddAsync(
            source: Arg.Any<NotificationContactAdd>(),
            cancellationToken: Arg.Any<CancellationToken>())
            .ConfigureAwait(false);

        _ = fakeNotificationContactRepository
            .Received(0)
            .UpdateAsync(
            source: Arg.Any<NotificationContactUpdate>(),
            cancellationToken: Arg.Any<CancellationToken>())
            .ConfigureAwait(false);
    }

    [Fact]
    public async void Update()
    {
        var fakeResellerRepository = Substitute.For<IResellerRepository>();
        var fakeNotificationContactRepository = Substitute.For<INotificationContactRepository>();

        var command = new EmailResellerMergeCommand(
            Code: "GPI",
            Contacts: new EmailContact[]
            {
                new() {
                    Name="tst",
                    Email="tst@rayprosoft.com",
                    Remark="tst"
                }
            });

        var resellerInfo = new ResellerInfo(
            ResellerId: Guid.Parse("b5a2c708-ab70-4316-9f4d-5d2ad0e77460"),
            ResellerCode: "TGS");
        _ = fakeResellerRepository.GetAsync(
            Arg.Is(command.Code),
            Arg.Any<CancellationToken>())
            .Returns(resellerInfo);

        _ = fakeNotificationContactRepository.GetAsync(
            resellerId: Arg.Is(resellerInfo.ResellerId),
            licenseeId: Arg.Is(Guid.Empty),
            brandId: Arg.Is(Guid.Empty),
            type: Arg.Is(ContactTypes.Email),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(
            new NotificationContactInfo(
                ResellerId: Guid.Parse("b5a2c708-ab70-4316-9f4d-5d2ad0e77460"),
                LicenseeId: Guid.Empty,
                BrandId: Guid.Empty,
                Type: ContactTypes.Email,
                Contacts: JsonSerializer.Serialize(Enumerable.Empty<EmailContact>()),
                CreatedOn: DateTimeOffset.Parse("2023-12-06T09:19:50.010516+00:00"),
                UpdatedOn: DateTimeOffset.Parse("2023-12-07T10:38:10.350982+00:00")));

        var service = new EmailResellerMergeCommandService(
            fakeResellerRepository,
            fakeNotificationContactRepository);

        await service.ExecuteAsync(command);

        _ = fakeNotificationContactRepository
            .Received(0)
            .AddAsync(
            source: Arg.Any<NotificationContactAdd>(),
            cancellationToken: Arg.Any<CancellationToken>())
            .ConfigureAwait(false);

        _ = fakeNotificationContactRepository
            .Received(1)
            .UpdateAsync(
            source: Arg.Any<NotificationContactUpdate>(),
            cancellationToken: Arg.Any<CancellationToken>())
            .ConfigureAwait(false);
    }
}
