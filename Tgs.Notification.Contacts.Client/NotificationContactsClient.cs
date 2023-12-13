using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;

using System.Text.Json;

using System.Text.Json.Serialization;
using Tgs.Notification.Contacts.Common.Models;

namespace Tgs.Notification.Contacts.Client;

internal class NotificationContactsClient(HttpClient httpClient) : INotificationContactsClient
{
    private static readonly JsonSerializerOptions _SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    public async ValueTask<EmailContactGet?> EmailBrandGetAsync(string code, CancellationToken cancellationToken = default)
    {
        var path = new Uri(httpClient.BaseAddress!, $"/email/brand/{code}");
        var builder = new UriBuilder(path);
        string url = builder.ToString();

        return await httpClient.GetAsync(url, cancellationToken)
            .ToAsyncEnumerable()
            .Do(response => response.EnsureSuccessStatusCode())
            .Where(response => response.StatusCode == System.Net.HttpStatusCode.OK)
            .SelectMany(response => response.Content
                .ReadFromJsonAsync<EmailContactGet>(_SerializerOptions, cancellationToken)
                .ToAsyncEnumerable())
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async ValueTask EmailBrandMergeAsync(string code, IEnumerable<EmailContact> contacts, CancellationToken cancellationToken = default)
    {
        var add = new EmailContactMerge
        {
            Code = code,
            Contacts = contacts
        };
        var path = new Uri(httpClient.BaseAddress!, $"/email/brand/");
        var builder = new UriBuilder(path);
        string url = builder.ToString();
        var httpContent = new StringContent(JsonSerializer.Serialize(add, _SerializerOptions), Encoding.UTF8, MediaTypeNames.Application.Json);

        _ = await httpClient.PostAsync(url, httpContent, cancellationToken)
            .ToAsyncEnumerable()
           .Do(response => response.EnsureSuccessStatusCode())
           .FirstAsync(cancellationToken: cancellationToken)
           .ConfigureAwait(false);
    }

    public async ValueTask<EmailContactGet?> EmailLicenseeGetAsync(string code, CancellationToken cancellationToken = default)
    {
        var path = new Uri(httpClient.BaseAddress!, $"/email/licensee/{code}");
        var builder = new UriBuilder(path);
        string url = builder.ToString();

        return await httpClient.GetAsync(url, cancellationToken)
            .ToAsyncEnumerable()
            .Do(response => response.EnsureSuccessStatusCode())
            .Where(response => response.StatusCode == System.Net.HttpStatusCode.OK)
            .SelectMany(response => response.Content
                .ReadFromJsonAsync<EmailContactGet>(_SerializerOptions, cancellationToken)
                .ToAsyncEnumerable())
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async ValueTask EmailLicenseeMergeAsync(string code, IEnumerable<EmailContact> contacts, CancellationToken cancellationToken = default)
    {
        var add = new EmailContactMerge
        {
            Code = code,
            Contacts = contacts
        };
        var path = new Uri(httpClient.BaseAddress!, $"/email/licensee/");
        var builder = new UriBuilder(path);
        string url = builder.ToString();
        var httpContent = new StringContent(JsonSerializer.Serialize(add, _SerializerOptions), Encoding.UTF8, MediaTypeNames.Application.Json);

        _ = await httpClient.PostAsync(url, httpContent, cancellationToken)
            .ToAsyncEnumerable()
           .Do(response => response.EnsureSuccessStatusCode())
           .FirstAsync(cancellationToken: cancellationToken)
           .ConfigureAwait(false);
    }

    public async ValueTask<EmailContactGet?> EmailResellerGetAsync(string code, CancellationToken cancellationToken = default)
    {
        var path = new Uri(httpClient.BaseAddress!, $"/email/reseller/{code}");
        var builder = new UriBuilder(path);
        string url = builder.ToString();

        return await httpClient.GetAsync(url, cancellationToken)
            .ToAsyncEnumerable()
            .Do(response => response.EnsureSuccessStatusCode())
            .Where(response => response.StatusCode == System.Net.HttpStatusCode.OK)
            .SelectMany(response => response.Content
                .ReadFromJsonAsync<EmailContactGet>(_SerializerOptions, cancellationToken)
                .ToAsyncEnumerable())
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async ValueTask EmailResellerMergeAsync(string code, IEnumerable<EmailContact> contacts, CancellationToken cancellationToken = default)
    {
        var add = new EmailContactMerge
        {
            Code = code,
            Contacts = contacts
        };
        var path = new Uri(httpClient.BaseAddress!, $"/email/reseller/");
        var builder = new UriBuilder(path);
        string url = builder.ToString();
        var httpContent = new StringContent(JsonSerializer.Serialize(add, _SerializerOptions), Encoding.UTF8, MediaTypeNames.Application.Json);

        _ = await httpClient.PostAsync(url, httpContent, cancellationToken)
            .ToAsyncEnumerable()
           .Do(response => response.EnsureSuccessStatusCode())
           .FirstAsync(cancellationToken: cancellationToken)
           .ConfigureAwait(false);
    }
}
