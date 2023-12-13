using Microsoft.EntityFrameworkCore;
using Tgs.Notification.Contacts.Core;
using Tgs.Notification.Contacts.Core.Models;
using Tgs.Notification.Contacts.Repositories.Entities;

namespace Tgs.Notification.Contacts.Repositories;

internal class BrandRepository(TgsContext context) : IBrandRepository
{
    public async ValueTask<BrandInfo> GetAsync(string code, CancellationToken cancellationToken = default)
#pragma warning disable CA1862 // 使用 'StringComparison' 方法多載來執行不區分大小寫的字串比較
        => await context.Brands.AsQueryable()
        .Where(predicate => predicate.Code.ToLower() == code.ToLower())
        .Join(
           context.Licensees,
           inner => inner.LicenseeId,
           outer => outer.Id,
           (brand, licensee) => new
           {
               licensee.ResellerId,
               LicenseeId = licensee.Id,
               LicenseeCode = licensee.Code,
               BrandId = brand.Id,
               BrandCode = brand.Code,
               brand.TimeSpanMinutes,
           })
        .Join(
            context.Resellers,
            inner => inner.ResellerId,
            outer => outer.Id,
            (combined, reseller) => new
            {
                ResellerId = reseller.Id,
                ResellerCode = reseller.Code,
                combined.LicenseeId,
                combined.LicenseeCode,
                combined.BrandId,
                combined.BrandCode,
                combined.TimeSpanMinutes
            })
        .Select(selector: selector => new BrandInfo(
            selector.ResellerId,
            selector.ResellerCode,
            selector.LicenseeId,
            selector.LicenseeCode,
            selector.BrandId,
            selector.BrandCode
            ))
        .FirstOrDefaultAsync(
            cancellationToken: cancellationToken)
        .ConfigureAwait(false)
         ?? throw new NullReferenceException($"This brand does not exist.{code}");

#pragma warning restore CA1862 // 使用 'StringComparison' 方法多載來執行不區分大小寫的字串比較

    public async ValueTask<BrandInfo> GetAsync(Guid id, CancellationToken cancellationToken = default)
        => await context.Brands.AsQueryable()
        .Where(predicate => predicate.Id == id)
        .Join(
           context.Licensees,
           inner => inner.LicenseeId,
           outer => outer.Id,
           (brand, licensee) => new
           {
               licensee.ResellerId,
               LicenseeId = licensee.Id,
               LicenseeCode = licensee.Code,
               BrandId = brand.Id,
               BrandCode = brand.Code,
               brand.TimeSpanMinutes,
           })
        .Join(
            context.Resellers,
            inner => inner.ResellerId,
            outer => outer.Id,
            (combined, reseller) => new
            {
                ResellerId = reseller.Id,
                ResellerCode = reseller.Code,
                combined.LicenseeId,
                combined.LicenseeCode,
                combined.BrandId,
                combined.BrandCode,
                combined.TimeSpanMinutes
            })
        .Select(selector: selector => new BrandInfo(
            selector.ResellerId,
            selector.ResellerCode,
            selector.LicenseeId,
            selector.LicenseeCode,
            selector.BrandId,
            selector.BrandCode
            ))
        .FirstOrDefaultAsync(
            cancellationToken: cancellationToken)
        .ConfigureAwait(false)
         ?? throw new NullReferenceException($"This brand does not exist.{id}");
}
