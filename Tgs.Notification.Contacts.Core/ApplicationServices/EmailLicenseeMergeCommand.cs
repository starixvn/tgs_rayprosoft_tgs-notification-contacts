using System.Text.Json.Nodes;
using Tgs.Notification.Contacts.Common.Models;

namespace Tgs.Notification.Contacts.Core.ApplicationServices;

public record EmailLicenseeMergeCommand(
    string Code,
    IEnumerable<EmailContact> Contacts);
