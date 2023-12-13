using Microsoft.EntityFrameworkCore;
using Tgs.Notification.Contacts.Core;
using Tgs.Notification.Contacts.Core.Models;
using Tgs.Notification.Contacts.Repositories.Entities;

namespace Tgs.Notification.Contacts.Repositories;

internal class LicenseeRepository(TgsContext context) : ILicenseeRepository
{
    public async ValueTask<LicenseeInfo> GetAsync(string code, CancellationToken cancellationToken = default)
#pragma warning disable CA1862 // 使用 'StringComparison' 方法多載來執行不區分大小寫的字串比較
        => await context.Licensees.AsQueryable()
        .Where(predicate => predicate.Code.ToLower() == code.ToLower())
        .Join(
            context.Resellers,
            inner => inner.ResellerId,
            outer => outer.Id,
            (combined, reseller) => new
            {
                ResellerId = reseller.Id,
                ResellerCode = reseller.Code,
                LicenseeId = combined.Id,
                LicenseeCode = combined.Code,
            })
        .Select(selector: selector => new LicenseeInfo(
            selector.ResellerId,
            selector.ResellerCode,
            selector.LicenseeId,
            selector.LicenseeCode
            ))
        .FirstOrDefaultAsync(
            cancellationToken: cancellationToken)
        .ConfigureAwait(false)
         ?? throw new NullReferenceException($"This Licensee does not exist.{code}");

    public async ValueTask<LicenseeInfo> GetAsync(Guid id, CancellationToken cancellationToken = default)
        => await context.Licensees.AsQueryable()
        .Where(predicate => predicate.Id == id)
        .Join(
            context.Resellers,
            inner => inner.ResellerId,
            outer => outer.Id,
            (combined, reseller) => new
            {
                ResellerId = reseller.Id,
                ResellerCode = reseller.Code,
                LicenseeId = combined.Id,
                LicenseeCode = combined.Code,
            })
        .Select(selector: selector => new LicenseeInfo(
            selector.ResellerId,
            selector.ResellerCode,
            selector.LicenseeId,
            selector.LicenseeCode
            ))
        .FirstOrDefaultAsync(
            cancellationToken: cancellationToken)
        .ConfigureAwait(false)
        ?? throw new NullReferenceException($"This Licensee does not exist.{id}");
}
