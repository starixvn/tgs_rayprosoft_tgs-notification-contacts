using System.Text.Json;

using NSubstitute;
using Tgs.Notification.Contacts.Common;
using Tgs.Notification.Contacts.Common.Models;
using Tgs.Notification.Contacts.Core.ApplicationServices;
using Tgs.Notification.Contacts.Core.Models;

namespace Tgs.Notification.Contacts.Core.UnitTest.ApplicationServices;

public class EmailLicenseeMergeCommandServiceUnitTest
{
    [Fact]
    public async void Add()
    {
        var fakeLicenseeRepository = Substitute.For<ILicenseeRepository>();
        var fakeNotificationContactRepository = Substitute.For<INotificationContactRepository>();

        var command = new EmailLicenseeMergeCommand(
            Code: "GPI",
            Contacts: new EmailContact[]
            {
                new() {
                    Name="tst",
                    Email="tst@rayprosoft.com",
                    Remark="tst"
                }
            });

        var licenseeInfo = new LicenseeInfo(
            ResellerId: Guid.Parse("b5a2c708-ab70-4316-9f4d-5d2ad0e77460"),
            ResellerCode: "TGS",
            LicenseeId: Guid.Parse("66c5fef5-217b-4929-9a08-72c2479a8f4e"),
            LicenseeCode: "TGS");
        _ = fakeLicenseeRepository.GetAsync(
            Arg.Is(command.Code),
            Arg.Any<CancellationToken>())
            .Returns(licenseeInfo);

        _ = fakeNotificationContactRepository.GetAsync(
            resellerId: Arg.Is(licenseeInfo.ResellerId),
            licenseeId: Arg.Is(licenseeInfo.LicenseeId),
            brandId: Arg.Is(Guid.Empty),
            type: Arg.Is(ContactTypes.Email),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns((NotificationContactInfo?)null);

        var service = new EmailLicenseeMergeCommandService(
            fakeLicenseeRepository,
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
        var fakeLicenseeRepository = Substitute.For<ILicenseeRepository>();
        var fakeNotificationContactRepository = Substitute.For<INotificationContactRepository>();

        var command = new EmailLicenseeMergeCommand(
            Code: "GPI",
            Contacts: new EmailContact[]
            {
                new() {
                    Name="tst",
                    Email="tst@rayprosoft.com",
                    Remark="tst"
                }
            });

        var licenseeInfo = new LicenseeInfo(
            ResellerId: Guid.Parse("b5a2c708-ab70-4316-9f4d-5d2ad0e77460"),
            ResellerCode: "TGS",
            LicenseeId: Guid.Parse("66c5fef5-217b-4929-9a08-72c2479a8f4e"),
            LicenseeCode: "TGS");
        _ = fakeLicenseeRepository.GetAsync(
            Arg.Is(command.Code),
            Arg.Any<CancellationToken>())
            .Returns(licenseeInfo);

        _ = fakeNotificationContactRepository.GetAsync(
            resellerId: Arg.Is(licenseeInfo.ResellerId),
            licenseeId: Arg.Is(licenseeInfo.LicenseeId),
            brandId: Arg.Is(Guid.Empty),
            type: Arg.Is(ContactTypes.Email),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(
            new NotificationContactInfo(
                ResellerId: Guid.Parse("b5a2c708-ab70-4316-9f4d-5d2ad0e77460"),
                LicenseeId: Guid.Parse("66c5fef5-217b-4929-9a08-72c2479a8f4e"),
                BrandId: Guid.Empty,
                Type: ContactTypes.Email,
                Contacts: JsonSerializer.Serialize(Enumerable.Empty<EmailContact>()),
                CreatedOn: DateTimeOffset.Parse("2023-12-06T09:19:50.010516+00:00"),
                UpdatedOn: DateTimeOffset.Parse("2023-12-07T10:38:10.350982+00:00")));

        var service = new EmailLicenseeMergeCommandService(
            fakeLicenseeRepository,
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
