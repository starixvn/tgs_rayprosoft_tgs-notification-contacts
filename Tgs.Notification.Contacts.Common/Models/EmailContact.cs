using System.ComponentModel.DataAnnotations;

namespace Tgs.Notification.Contacts.Common.Models;

public record EmailContact
{
    [Required]
    public string Name { get; init; } = default!;
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; init; } = default!;
    public string Remark { get; init; } = default!;
};
