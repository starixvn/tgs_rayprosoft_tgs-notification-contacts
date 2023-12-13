using System.Text.Json.Nodes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Tgs.ServiceUtility;
using Tgs.Notification.Contacts.Common.Models;
using Tgs.Notification.Contacts.IntegrationUnitTest;
using Tgs.Notification.Contacts.Client;
using Tgs.Notification.Contacts.Core.ApplicationServices;

namespace Tgs.Notification.Contacts.IntegrationUnitTest;

public partial class NotificationContactsClientUnitTest
{
    [Fact]
    public async Task Email_Brand_Get()
    {
        var code = "GPI";

        var web = new TestNotificationContactsApplication(builder =>
        {
            _ = builder.ConfigureServices(services =>
            {
                var fakeService = Substitute.For<IGetService<EmailBrandGetQuery, EmailGetResult?>>();

                var query = new EmailBrandGetQuery(code);

                _ = fakeService.GetAsync(
                    query: Arg.Is(query),
                    cancellationToken: Arg.Any<CancellationToken>())
                .Returns(new EmailGetResult(
                        ResellerId: Guid.Parse("b5a2c708-ab70-4316-9f4d-5d2ad0e77460"),
                        ResellerCode: "TGS",
                        LicenseeId: Guid.Parse("66c5fef5-217b-4929-9a08-72c2479a8f4e"),
                        LicenseeCode: "TGS",
                        BrandId: Guid.Parse("4e86719f-243f-46f6-b010-5c758b7285af"),
                        BrandCode: "GPI",
                        Contacts: new EmailContact[]
                        {
                            new() {
                                Name="tst",
                                Email="tst@rayprosoft.com",
                                Remark="tst"
                            }
                        }));

                _ = services.AddTransient(_ => fakeService);
            });
        });

        var fakeLoggingHandler = new LoggingHandler(web.Services.GetRequiredService<ILogger<LoggingHandler>>());

        var httpClient = web.CreateDefaultClient(fakeLoggingHandler);

        var client = new NotificationContactsClient(httpClient);

        var actual = await client.EmailBrandGetAsync(code: code);

        Assert.NotNull(actual);
        Assert.Equal(Guid.Parse("b5a2c708-ab70-4316-9f4d-5d2ad0e77460"), actual.ResellerId);
        Assert.Equal("TGS", actual.ResellerCode);
        Assert.Equal(Guid.Parse("66c5fef5-217b-4929-9a08-72c2479a8f4e"), actual.LicenseeId);
        Assert.Equal("TGS", actual.LicenseeCode);
        Assert.Equal(Guid.Parse("4e86719f-243f-46f6-b010-5c758b7285af"), actual.BrandId);
        Assert.Equal("GPI", actual.BrandCode);
        Assert.True(actual.Contacts.Any());
    }

    [Fact]
    public async Task Email_Brand_Get_No_Data()
    {
        var code = "GPI";

        var web = new TestNotificationContactsApplication(builder =>
        {
            _ = builder.ConfigureServices(services =>
            {
                var fakeService = Substitute.For<IGetService<EmailBrandGetQuery, EmailGetResult?>>();

                var query = new EmailBrandGetQuery(code);

                _ = fakeService.GetAsync(
                    query: Arg.Is(query),
                    cancellationToken: Arg.Any<CancellationToken>())
                .Returns((EmailGetResult?)null);

                _ = services.AddTransient(_ => fakeService);
            });
        });

        var fakeLoggingHandler = new LoggingHandler(web.Services.GetRequiredService<ILogger<LoggingHandler>>());

        var httpClient = web.CreateDefaultClient(fakeLoggingHandler);

        var client = new NotificationContactsClient(httpClient);

        var actual = await client.EmailBrandGetAsync(code: code);

        Assert.Null(actual);
    }

    [Fact]
    public async Task Email_Brand_Merge()
    {
        var code = "GPI";
        var contacts = new EmailContact[]
        {
            new() {
                Name="tst",
                Email="tst@rayprosoft.com",
                Remark="tst"
            }
        };

        var web = new TestNotificationContactsApplication(builder =>
        {
            _ = builder.ConfigureServices(async services =>
            {
                var fakeService = Substitute.For<ICommandService<EmailBrandMergeCommand>>();

                var command = new EmailBrandMergeCommand(
                    Code: code,
                    Contacts: contacts);

                await fakeService
                .ExecuteAsync(
                    command: Arg.Is(command),
                    cancellationToken: Arg.Any<CancellationToken>());

                _ = services.AddTransient(_ => fakeService);
            });
        });

        var fakeLoggingHandler = new LoggingHandler(web.Services.GetRequiredService<ILogger<LoggingHandler>>());

        var httpClient = web.CreateDefaultClient(fakeLoggingHandler);

        var client = new NotificationContactsClient(httpClient);

        await client.EmailBrandMergeAsync(
            code: code,
            contacts: contacts);
    }
}
