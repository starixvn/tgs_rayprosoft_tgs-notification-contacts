using Tgs.Notification.Contacts.Common.Models;

namespace Tgs.Notification.Contacts.Core.ApplicationServices;

public record EmailGetResult(
    Guid ResellerId,
    string ResellerCode,
    Guid LicenseeId,
    string LicenseeCode,
    Guid BrandId,
    string BrandCode,
    IEnumerable<EmailContact> Contacts);
