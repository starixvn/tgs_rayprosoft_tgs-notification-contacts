using System.Text.Json;
using NSubstitute;
using Tgs.Notification.Contacts.Common;
using Tgs.Notification.Contacts.Common.Models;
using Tgs.Notification.Contacts.Core.ApplicationServices;
using Tgs.Notification.Contacts.Core.Models;

namespace Tgs.Notification.Contacts.Core.UnitTest.ApplicationServices;

public class EmailBrandGetServiceUnitTest
{
    [Fact]
    public async void Normal_Process()
    {
        var fakeBrandRepository = Substitute.For<IBrandRepository>();
        var fakeNotificationContactRepository = Substitute.For<INotificationContactRepository>();

        var query = new EmailBrandGetQuery(
            Code: "GPI");

        var brandInfo = new BrandInfo(
            ResellerId: Guid.Parse("b5a2c708-ab70-4316-9f4d-5d2ad0e77460"),
            ResellerCode: "TGS",
            LicenseeId: Guid.Parse("66c5fef5-217b-4929-9a08-72c2479a8f4e"),
            LicenseeCode: "TGS",
            BrandId: Guid.Parse("4e86719f-243f-46f6-b010-5c758b7285af"),
            BrandCode: "GPI");
        _ = fakeBrandRepository.GetAsync(
            Arg.Is(query.Code),
            Arg.Any<CancellationToken>())
            .Returns(brandInfo);

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
            BrandId: Guid.Parse("4e86719f-243f-46f6-b010-5c758b7285af"),
            Type: ContactTypes.Email,
            Contacts: JsonSerializer.Serialize(contacts),
            CreatedOn: DateTimeOffset.Parse("2023-12-06T09:19:50.010516+00:00"),
            UpdatedOn: DateTimeOffset.Parse("2023-12-07T10:38:10.350982+00:00"));
        _ = fakeNotificationContactRepository.GetAsync(
            resellerId: Arg.Is(brandInfo.ResellerId),
            licenseeId: Arg.Is(brandInfo.LicenseeId),
            brandId: Arg.Is(brandInfo.BrandId),
            type: Arg.Is(ContactTypes.Email),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(contactInfo);

        var service = new EmailBrandGetService(
            fakeBrandRepository,
            fakeNotificationContactRepository);

        var actual = await service.GetAsync(query);

        Assert.NotNull(actual);
        Assert.Equal(Guid.Parse("b5a2c708-ab70-4316-9f4d-5d2ad0e77460"), actual.ResellerId);
        Assert.Equal("TGS", actual.ResellerCode);
        Assert.Equal(Guid.Parse("66c5fef5-217b-4929-9a08-72c2479a8f4e"), actual.LicenseeId);
        Assert.Equal("TGS", actual.LicenseeCode);
        Assert.Equal(Guid.Parse("4e86719f-243f-46f6-b010-5c758b7285af"), actual.BrandId);
        Assert.Equal("GPI", actual.BrandCode);
        Assert.Equal(contacts, actual.Contacts);
    }
}
