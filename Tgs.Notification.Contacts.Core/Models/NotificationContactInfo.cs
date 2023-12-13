using Tgs.Notification.Contacts.Common;

namespace Tgs.Notification.Contacts.Core.Models;

public record NotificationContactInfo(
    Guid ResellerId,
    Guid LicenseeId,
    Guid BrandId,
    ContactTypes Type,
    string Contacts,
    DateTimeOffset CreatedOn,
    DateTimeOffset UpdatedOn);
