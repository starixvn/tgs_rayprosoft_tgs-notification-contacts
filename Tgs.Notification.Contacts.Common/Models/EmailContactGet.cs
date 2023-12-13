namespace Tgs.Notification.Contacts.Common.Models;

public record EmailContactGet(
    Guid ResellerId,
    string ResellerCode,
    Guid LicenseeId,
    string LicenseeCode,
    Guid BrandId,
    string BrandCode,
    IEnumerable<EmailContact> Contacts);
