using System.Text.Json;
using NSubstitute;
using Tgs.Notification.Contacts.Common;
using Tgs.Notification.Contacts.Common.Models;
using Tgs.Notification.Contacts.Core.ApplicationServices;
using Tgs.Notification.Contacts.Core.Models;

namespace Tgs.Notification.Contacts.Core.UnitTest.ApplicationServices;

public class EmailLicenseeGetServiceUnitTest
{
    [Fact]
    public async void Normal_Process()
    {
        var fakeLicenseeRepository = Substitute.For<ILicenseeRepository>();
        var fakeNotificationContactRepository = Substitute.For<INotificationContactRepository>();

        var query = new EmailLicenseeGetQuery(
            Code: "TGS");

        var licenseeInfo = new LicenseeInfo(
            ResellerId: Guid.Parse("b5a2c708-ab70-4316-9f4d-5d2ad0e77460"),
            ResellerCode: "TGS",
            LicenseeId: Guid.Parse("66c5fef5-217b-4929-9a08-72c2479a8f4e"),
            LicenseeCode: "TGS");
        _ = fakeLicenseeRepository.GetAsync(
            Arg.Is(query.Code),
            Arg.Any<CancellationToken>())
            .Returns(licenseeInfo);

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
            LicenseeId: Guid.Parse("66c5fef5-217b-4929-9a08-72c2479a8f4e"),
            BrandId: Guid.Empty,
            Type: ContactTypes.Email,
            Contacts: JsonSerializer.Serialize(contacts),
            CreatedOn: DateTimeOffset.Parse("2023-12-06T09:19:50.010516+00:00"),
            UpdatedOn: DateTimeOffset.Parse("2023-12-07T10:38:10.350982+00:00"));
        _ = fakeNotificationContactRepository.GetAsync(
            resellerId: Arg.Is(licenseeInfo.ResellerId),
            licenseeId: Arg.Is(licenseeInfo.LicenseeId),
            brandId: Arg.Is(Guid.Empty),
            type: Arg.Is(ContactTypes.Email),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(contactInfo);

        var service = new EmailLicenseeGetService(
            fakeLicenseeRepository,
            fakeNotificationContactRepository);

        var actual = await service.GetAsync(query);

        Assert.NotNull(actual);
        Assert.Equal(Guid.Parse("b5a2c708-ab70-4316-9f4d-5d2ad0e77460"), actual.ResellerId);
        Assert.Equal("TGS", actual.ResellerCode);
        Assert.Equal(Guid.Parse("66c5fef5-217b-4929-9a08-72c2479a8f4e"), actual.LicenseeId);
        Assert.Equal("TGS", actual.LicenseeCode);
        Assert.Equal(Guid.Empty, actual.BrandId);
        Assert.Equal(string.Empty, actual.BrandCode);
        Assert.Equal(contacts, actual.Contacts);
    }
}
