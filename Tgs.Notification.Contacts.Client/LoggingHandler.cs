using Microsoft.Extensions.Logging;
using Tgs.Notification.Contacts.Client;

namespace Tgs.Notification.Contacts.Client;

internal class LoggingHandler(ILogger<LoggingHandler> logger) : DelegatingHandler()
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        logger.LogInformation("LogOn:{DateTime.Now.ToString(\"o\")} Request: {request}", DateTime.Now.ToString("o"), request);

        if (request.Content != null)
        {
            logger.LogInformation("LogOn:{DateTime.Now.ToString(\"o\")} Request Body: {await request.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)}", DateTime.Now.ToString("o"), await request.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false));
        }

        var response = await base
            .SendAsync(request, cancellationToken)
            .ConfigureAwait(false);

        logger.LogInformation("LogOn:{DateTime.Now.ToString(\"o\")} Response: {response}", DateTime.Now.ToString("o"), response);

        if (response.Content != null)
        {
            logger.LogInformation("LogOn:{DateTime.Now.ToString(\"o\")} Response Body: {await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)}", DateTime.Now.ToString("o"), await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false));
        }

        return response;
    }
}
