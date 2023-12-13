using Microsoft.AspNetCore.Mvc;
using Tgs.Notification.Contacts.Common.Models;
using Tgs.Notification.Contacts.Core.ApplicationServices;

using Tgs.ServiceUtility;

namespace Tgs.Notification.Contacts.Controllers;

[Route("[controller]")]
[ApiController]
public class EmailController : ControllerBase
{
    /// <summary>
    /// Brands the get asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="code">The code.</param>
    /// <returns></returns>
    [HttpGet("brand/{code}")]
    public async ValueTask<EmailContactGet?> BrandGetAsync(
        [FromServices] IGetService<EmailBrandGetQuery, EmailGetResult?> service,
        string code)
    {
        var result = await service.GetAsync(
            query: new EmailBrandGetQuery(
                Code: code),
            cancellationToken: HttpContext.RequestAborted)
            .ConfigureAwait(false);
        return result is null
            ? null
            : new EmailContactGet(
                ResellerId: result.ResellerId,
                ResellerCode: result.ResellerCode,
                LicenseeId: result.LicenseeId,
                LicenseeCode: result.LicenseeCode,
                BrandId: result.BrandId,
                BrandCode: result.BrandCode,
                Contacts: result.Contacts);
    }

    /// <summary>
    /// Brands the merge asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///         {
    ///             "code": "GPI",
    ///             "contacts": [
    ///                 {
    ///                     "name": "gordon.hung",
    ///                     "email": "gordon.hung@rayprosoft.com",
    ///                     "remark": "gordon"
    ///                 },
    ///                 {
    ///                     "name": "tst",
    ///                     "email": "tst@rayprosoft.com",
    ///                     "remark": "tst"
    ///                 },
    ///                 {
    ///                     "name": "christine.jen",
    ///                     "email": "christine.jen@rayprosoft.com",
    ///                     "remark": "christine"
    ///                 }
    ///             ]
    ///         }
    /// </remarks>
    [HttpPost("brand")]
    public ValueTask BrandMergeAsync(
        [FromServices] ICommandService<EmailBrandMergeCommand> service,
        EmailContactMerge source)
            => service.ExecuteAsync(
                command: new EmailBrandMergeCommand(
                    Code: source.Code,
                    Contacts: source.Contacts),
                cancellationToken: HttpContext.RequestAborted);

    /// <summary>
    /// Licensees the get asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="code">The code.</param>
    /// <returns></returns>
    [HttpGet("licensee/{code}")]
    public async ValueTask<EmailContactGet?> LicenseeGetAsync(
        [FromServices] IGetService<EmailLicenseeGetQuery, EmailGetResult?> service,
        string code)
    {
        var result = await service.GetAsync(
            query: new EmailLicenseeGetQuery(
                Code: code),
            cancellationToken: HttpContext.RequestAborted)
            .ConfigureAwait(false);
        return result is null
            ? null
            : new EmailContactGet(
                ResellerId: result.ResellerId,
                ResellerCode: result.ResellerCode,
                LicenseeId: result.LicenseeId,
                LicenseeCode: result.LicenseeCode,
                BrandId: result.BrandId,
                BrandCode: result.BrandCode,
                Contacts: result.Contacts);
    }

    /// <summary>
    /// Licenseeds the merge asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///         {
    ///             "code": "TGS",
    ///             "contacts": [
    ///                 {
    ///                     "name": "gordon.hung",
    ///                     "email": "gordon.hung@rayprosoft.com",
    ///                     "remark": "gordon"
    ///                 },
    ///                 {
    ///                     "name": "tst",
    ///                     "email": "tst@rayprosoft.com",
    ///                     "remark": "tst"
    ///                 },
    ///                 {
    ///                     "name": "christine.jen",
    ///                     "email": "christine.jen@rayprosoft.com",
    ///                     "remark": "christine"
    ///                 }
    ///             ]
    ///         }
    /// </remarks>
    [HttpPost("licensee")]
    public ValueTask LicenseedMergeAsync(
        [FromServices] ICommandService<EmailLicenseeMergeCommand> service,
        EmailContactMerge source)
        => service.ExecuteAsync(
            command: new EmailLicenseeMergeCommand(
                Code: source.Code,
                Contacts: source.Contacts),
            cancellationToken: HttpContext.RequestAborted);

    /// <summary>
    /// Resellers the get asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="code">The code.</param>
    /// <returns></returns>
    [HttpGet("reseller/{code}")]
    public async ValueTask<EmailContactGet?> ResellerGetAsync(
        [FromServices] IGetService<EmailResellerGetQuery, EmailGetResult?> service,
        string code)
    {
        var result = await service.GetAsync(
            query: new EmailResellerGetQuery(
                Code: code),
            cancellationToken: HttpContext.RequestAborted)
            .ConfigureAwait(false);
        return result is null
            ? null
            : new EmailContactGet(
                ResellerId: result.ResellerId,
                ResellerCode: result.ResellerCode,
                LicenseeId: result.LicenseeId,
                LicenseeCode: result.LicenseeCode,
                BrandId: result.BrandId,
                BrandCode: result.BrandCode,
                Contacts: result.Contacts);
    }

    /// <summary>
    /// Resellers the merge asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///         {
    ///             "code": "TGS",
    ///             "contacts": [
    ///                 {
    ///                     "name": "gordon.hung",
    ///                     "email": "gordon.hung@rayprosoft.com",
    ///                     "remark": "gordon"
    ///                 },
    ///                 {
    ///                     "name": "tst",
    ///                     "email": "tst@rayprosoft.com",
    ///                     "remark": "tst"
    ///                 },
    ///                 {
    ///                     "name": "christine.jen",
    ///                     "email": "christine.jen@rayprosoft.com",
    ///                     "remark": "christine"
    ///                 }
    ///             ]
    ///         }
    /// </remarks>
    [HttpPost("reseller")]
    public ValueTask ResellerMergeAsync(
        [FromServices] ICommandService<EmailResellerMergeCommand> service,
        EmailContactMerge source)
        => service.ExecuteAsync(
            command: new EmailResellerMergeCommand(
                Code: source.Code,
                Contacts: source.Contacts),
            cancellationToken: HttpContext.RequestAborted);
}
