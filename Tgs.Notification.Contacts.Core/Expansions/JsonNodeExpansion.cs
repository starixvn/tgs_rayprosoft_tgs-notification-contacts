using System.Text.Json;
using System.Text.Json.Nodes;
using Tgs.Notification.Contacts.Common.Models;

namespace Tgs.Notification.Contacts.Core.Expansions;

public static class JsonNodeExpansion
{
    public static JsonNode ToJsonNode(this IEnumerable<EmailContact> contacts)
        => JsonNode.Parse(JsonSerializer.Serialize(contacts))!;
}
