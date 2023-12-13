namespace Tgs.Notification.Contacts.Core.Models;

public record LicenseeInfo(
    Guid ResellerId,
    string ResellerCode,
    Guid LicenseeId,
    string LicenseeCode);
