using System.Text.Json.Nodes;
using Tgs.Notification.Contacts.Common;

namespace Tgs.Notification.Contacts.Core.Models;

public record NotificationContactAdd(
    Guid ResellerId,
    Guid LicenseeId,
    Guid BrandId,
    ContactTypes Type,
    JsonNode Contacts);
