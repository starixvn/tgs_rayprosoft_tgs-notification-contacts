using System.ComponentModel.DataAnnotations;

namespace Tgs.Notification.Contacts.Common.Models;

public record EmailContactMerge
{
    [Required]
    public string Code { get; init; } = default!;
    [Required]
    public IEnumerable<EmailContact> Contacts { get; init; } = default!;
}
