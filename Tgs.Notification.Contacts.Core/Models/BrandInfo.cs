namespace Tgs.Notification.Contacts.Core.Models;

public record BrandInfo(
    Guid ResellerId,
    string ResellerCode,
    Guid LicenseeId,
    string LicenseeCode,
    Guid BrandId,
    string BrandCode);
