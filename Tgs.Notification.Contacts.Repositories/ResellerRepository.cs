using Microsoft.EntityFrameworkCore;
using Tgs.Notification.Contacts.Core;
using Tgs.Notification.Contacts.Core.Models;
using Tgs.Notification.Contacts.Repositories.Entities;

namespace Tgs.Notification.Contacts.Repositories;

internal class ResellerRepository(TgsContext context) : IResellerRepository
{
    public async ValueTask<ResellerInfo> GetAsync(string code, CancellationToken cancellationToken = default)
#pragma warning disable CA1862 // 使用 'StringComparison' 方法多載來執行不區分大小寫的字串比較
        => await context.Resellers.AsQueryable()
        .Where(predicate => predicate.Code.ToLower() == code.ToLower())
        .Select(selector: selector => new ResellerInfo(
            selector.Id,
            selector.Code
            ))
        .FirstOrDefaultAsync(
            cancellationToken: cancellationToken)
        .ConfigureAwait(false)
         ?? throw new NullReferenceException($"This Reseller does not exist.{code}");

    public async ValueTask<ResellerInfo> GetAsync(Guid id, CancellationToken cancellationToken = default)
        => await context.Resellers.AsQueryable()
        .Where(predicate => predicate.Id == id)
        .Select(selector: selector => new ResellerInfo(
            selector.Id,
            selector.Code
            ))
        .FirstOrDefaultAsync(
            cancellationToken: cancellationToken)
        .ConfigureAwait(false)
         ?? throw new NullReferenceException($"This Reseller does not exist.{id}");
}
