using System.Text.Json;
using NSubstitute;
using Tgs.Notification.Contacts.Common;
using Tgs.Notification.Contacts.Common.Models;
using Tgs.Notification.Contacts.Core.ApplicationServices;
using Tgs.Notification.Contacts.Core.Models;

namespace Tgs.Notification.Contacts.Core.UnitTest.ApplicationServices;

public class EmailResellerGetServiceUnitTest
{
    [Fact]
    public async void Normal_Process()
    {
        var fakeResellerRepository = Substitute.For<IResellerRepository>();
        var fakeNotificationContactRepository = Substitute.For<INotificationContactRepository>();

        var query = new EmailResellerGetQuery(
            Code: "TGS");

        var resellerInfo = new ResellerInfo(
            ResellerId: Guid.Parse("b5a2c708-ab70-4316-9f4d-5d2ad0e77460"),
            ResellerCode: "TGS");
        _ = fakeResellerRepository.GetAsync(
            Arg.Is(query.Code),
            Arg.Any<CancellationToken>())
            .Returns(resellerInfo);

        var contacts = new EmailContact[]
        {
            new() {
                Name="tst",
                Email="tst@rayprosoft.com",
                Remark="tst"
            }
        };
        var contactInfo = new NotificationContactInfo(
            ResellerId: Guid.Parse("b5a2c708-ab70-4316-9f4d-5d2ad0e77460"),
            LicenseeId: Guid.Empty,
            BrandId: Guid.Empty,
            Type: ContactTypes.Email,
            Contacts: JsonSerializer.Serialize(contacts),
            CreatedOn: DateTimeOffset.Parse("2023-12-06T09:19:50.010516+00:00"),
            UpdatedOn: DateTimeOffset.Parse("2023-12-07T10:38:10.350982+00:00"));
        _ = fakeNotificationContactRepository.GetAsync(
            resellerId: Arg.Is(resellerInfo.ResellerId),
            licenseeId: Arg.Is(Guid.Empty),
            brandId: Arg.Is(Guid.Empty),
            type: Arg.Is(ContactTypes.Email),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(contactInfo);

        var service = new EmailResellerGetService(
            fakeResellerRepository,
            fakeNotificationContactRepository);

        var actual = await service.GetAsync(query);

        Assert.NotNull(actual);
        Assert.Equal(Guid.Parse("b5a2c708-ab70-4316-9f4d-5d2ad0e77460"), actual.ResellerId);
        Assert.Equal("TGS", actual.ResellerCode);
        Assert.Equal(Guid.Empty, actual.LicenseeId);
        Assert.Equal(string.Empty, actual.LicenseeCode);
        Assert.Equal(Guid.Empty, actual.BrandId);
        Assert.Equal(string.Empty, actual.BrandCode);
        Assert.Equal(contacts, actual.Contacts);
    }
}
