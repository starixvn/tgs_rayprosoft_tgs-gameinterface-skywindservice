using Microsoft.AspNetCore.Mvc;
using Tgs.GameInterface.SkywindService.Core;
using Tgs.GameInterface.SkywindService.Core.ApplicationServices;
using Tgs.GameInterface.SkywindService.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SkywindService.Controllers;

[Route("[controller]")]
[ApiController]
public class SkywindController : ControllerBase
{
    /// <summary>
    /// Gets the access token asynchronous.
    /// </summary>
    /// <param name="client">The client.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///     {
    ///         "apiUrl": "https://api.gcpstg.m27613.com/",
    ///         "secretKey": "66a44892-54dc-488b-b755-1d1da4f8ad61",
    ///         "username": "PGT_brand_api_stg",
    ///         "password": "ArSg5e6CR2g1wIgB"
    ///     }
    /// </remarks>
    [HttpPost("getaccesstoken")]
    public ValueTask<string> GetAccessTokenAsync(
        [FromServices] IGameApiClient client,
        GetAccessTokenViewModel source)
        => client.GetAccessTokenAsync(
                source.ApiUrl,
                source.SecretKey,
                source.Username,
                source.Password);

    /// <summary>
    /// Players the balance asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///     {
    ///         "apiUrl": "https://api.gcpstg.m27613.com/",
    ///         "secretKey": "66a44892-54dc-488b-b755-1d1da4f8ad61",
    ///         "username": "PGT_brand_api_stg",
    ///         "password": "ArSg5e6CR2g1wIgB",
    ///         "playerCode": "4ucmqplxb029"
    ///     }
    /// </remarks>
    [HttpPost("players/balance")]
    public async ValueTask<PlayerBalanceResultViewModel> PlayerBalanceAsync(
        [FromServices] IGetService<PlayerBalanceQuery, PlayerBalanceResult> service,
        PlayerBalanceViewModel source)
    {
        var query = new PlayerBalanceQuery(
            source.ApiUrl,
            source.SecretKey,
            source.Username,
            source.Password,
            source.PlayerCode);

        var balanceResult = await service
                .GetAsync(
                    query,
                    HttpContext.RequestAborted)
                .ConfigureAwait(false);

        return new PlayerBalanceResultViewModel(
            balanceResult.IsPlayer,
            balanceResult.Balance);
    }

    /// <summary>
    /// Funds the out asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///     {
    ///         "apiUrl": "https://api.gcpstg.m27613.com/",
    ///         "secretKey": "66a44892-54dc-488b-b755-1d1da4f8ad61",
    ///         "username": "PGT_brand_api_stg",
    ///         "password": "ArSg5e6CR2g1wIgB",
    ///         "extTrxId": "20220824143155",
    ///         "playerCode": "4ucmqplxb029",
    ///         "currency": "CNY",
    ///         "amount": 1
    ///     }
    /// </remarks>
    [HttpPost("fund/out")]
    public async ValueTask<FundResultViewModel> FundOutAsync(
        [FromServices] IGetService<FundOutQuery, FundOutResult> service,
        FundViewModel source)
    {
        var query = new FundOutQuery(
            source.ApiUrl,
            source.SecretKey,
            source.Username,
            source.Password,
            source.ExtTrxId,
            source.PlayerCode,
            source.Currency,
            source.Amount);

        var result = await service
                .GetAsync(
                    query,
                    HttpContext.RequestAborted)
                .ConfigureAwait(false);

        return new FundResultViewModel(
             result.Status,
             result.TrxId,
             result.ExtTrxId,
             result.OrderStatus,
             result.Amount,
             result.Balance);
    }

    /// <summary>
    /// Funds the out asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///     {
    ///         "apiUrl": "https://api.gcpstg.m27613.com/",
    ///         "secretKey": "66a44892-54dc-488b-b755-1d1da4f8ad61",
    ///         "username": "PGT_brand_api_stg",
    ///         "password": "ArSg5e6CR2g1wIgB",
    ///         "transactionId": "20220824143155"
    ///     }
    /// </remarks>
    [HttpPost("fund/status")]
    public async ValueTask<FundStatusResultViewModel> FundStatusAsync(
        [FromServices] IGetService<FundStatusQuery, FundStatusResult> service,
        FundStatusViewModel source)
    {
        var query = new FundStatusQuery(
            source.ApiUrl,
            source.SecretKey,
            source.Username,
            source.Password,
            source.ExtTrxId);

        var result = await service
                .GetAsync(
                    query,
                    HttpContext.RequestAborted)
                .ConfigureAwait(false);

        return new FundStatusResultViewModel(
             result.IsTransaction,
             result.Status);
    }

    /// <summary>
    /// Withdrawals the asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///     {
    ///         "apiUrl": "https://api.gcpstg.m27613.com/",
    ///         "secretKey": "66a44892-54dc-488b-b755-1d1da4f8ad61",
    ///         "username": "PGT_brand_api_stg",
    ///         "password": "ArSg5e6CR2g1wIgB",
    ///         "extTrxId": "20220824143155",
    ///         "playerCode": "4ucmqplxb029",
    ///         "currency": "CNY",
    ///         "amount": 1
    ///     }
    /// </remarks>
    [HttpPost("fund/in")]
    public async ValueTask<FundResultViewModel?> FundInAsync(
        [FromServices] IGetService<FundInQuery, FundInResult> service,
        FundViewModel source)
    {
        var query = new FundInQuery(
            source.ApiUrl,
           source.SecretKey,
           source.Username,
           source.Password,
           source.ExtTrxId,
           source.PlayerCode,
           source.Currency,
           source.Amount);

        var result = await service
                .GetAsync(
                    query,
                    HttpContext.RequestAborted)
                .ConfigureAwait(false);

        return new FundResultViewModel(
             result.Status,
             result.TrxId,
             result.ExtTrxId,
             result.OrderStatus,
             result.Amount,
             result.Balance);
    }

    /// <summary>
    /// Gets the list countries asynchronous.
    /// </summary>
    /// <param name="client">The client.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Get
    ///
    ///     {
    ///         "apiUrl": "https://api.gcpstg.m27613.com/",
    ///         "accessToken": {accessToken}
    ///     }
    /// </remarks>
    [HttpGet("countries")]
    public ValueTask<string> GetListCountriesAsync(
        [FromServices] IGameApiClient client,
        [FromQuery] ListCountriesViewModel source)
        => client.GetListCountriesAsync(
                source.ApiUrl,
                source.AccessToken);

    /// <summary>
    /// Gets the list currencies asynchronous.
    /// </summary>
    /// <param name="client">The client.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Get
    ///
    ///     {
    ///         "apiUrl": "https://api.gcpstg.m27613.com/",
    ///         "accessToken": {accessToken}
    ///     }
    /// </remarks>
    [HttpGet("currencies")]
    public ValueTask<string> GetListCurrenciesAsync(
        [FromServices] IGameApiClient client,
        [FromQuery] ListCurrenciesViewModel source)
        => client.GetListCurrenciesAsync(
                source.ApiUrl,
                source.AccessToken);

    /// <summary>
    /// Gets the list languages asynchronous.
    /// </summary>
    /// <param name="client">The client.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Get
    ///
    ///     {
    ///         "apiUrl": "https://api.gcpstg.m27613.com/",
    ///         "accessToken": {accessToken}
    ///     }
    /// </remarks>
    [HttpGet("languages")]
    public ValueTask<string> GetListLanguagesAsync(
        [FromServices] IGameApiClient client,
        [FromQuery] ListLanguagesViewModel source)
        => client.GetListLanguagesAsync(
                source.ApiUrl,
                source.AccessToken);

    /// <summary>
    /// Players the create asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///     {
    ///         "apiUrl": "https://api.gcpstg.m27613.com/",
    ///         "secretKey": "66a44892-54dc-488b-b755-1d1da4f8ad61",
    ///         "username": "PGT_brand_api_stg",
    ///         "password": "ArSg5e6CR2g1wIgB",
    ///         "playerCode": "4ucmqplxb029",
    ///         "currency": "CNY"
    ///     }
    /// </remarks>
    [HttpPost("players/create")]
    public ValueTask PlayerCreateAsync(
        [FromServices] ICommandService<PlayerCreateCommand> service,
        PlayerCreateViewModel source)
    {
        var command = new PlayerCreateCommand(
            source.ApiUrl,
            source.SecretKey,
            source.Username,
            source.Password,
            source.PlayerCode,
            source.Currency);

        return service.ExecuteAsync(
               command,
               HttpContext.RequestAborted);
    }

    /// <summary>
    /// Players the kick line asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///     {
    ///         "apiUrl": "https://api.gcpstg.m27613.com/",
    ///         "secretKey": "66a44892-54dc-488b-b755-1d1da4f8ad61",
    ///         "username": "PGT_brand_api_stg",
    ///         "password": "ArSg5e6CR2g1wIgB",
    ///         "playerCode": "4ucmqplxb029"
    ///     }
    /// </remarks>
    [HttpPost("players/kick")]
    public ValueTask PlayerKickLineAsync(
        [FromServices] ICommandService<PlayerKickCommand> service,
        PlayerKickViewModel source)
    {
        var command = new PlayerKickCommand(
            source.ApiUrl,
            source.SecretKey,
            source.Username,
            source.Password,
            source.PlayerCode);

        return service.ExecuteAsync(
               command,
               HttpContext.RequestAborted);
    }


    /// <summary>
    /// Gets the game URL asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///     Live
    ///     {
    ///         "apiUrl": "https://api.gcpstg.m27613.com/",
    ///         "secretKey": "66a44892-54dc-488b-b755-1d1da4f8ad61",
    ///         "username": "PGT_brand_api_stg",
    ///         "password": "ArSg5e6CR2g1wIgB",
    ///         "playerCode": "4ucmqplxb029",
    ///         "gameCode": "sw_ro_a01bac",
    ///         "language": "en"
    ///     }
    ///
    ///     Slot
    ///     {
    ///         "apiUrl": "https://api.gcpstg.m27613.com/",
    ///         "secretKey": "66a44892-54dc-488b-b755-1d1da4f8ad61",
    ///         "username": "PGT_brand_api_stg",
    ///         "password": "ArSg5e6CR2g1wIgB",
    ///         "playerCode": "4ucmqplxb029",
    ///         "gameCode": "sw_88sf",
    ///         "language": "en"
    ///     }
    /// </remarks>
    [HttpPost("game/url")]
    public async ValueTask<GameUrlResultViewModel> GetGameUrlAsync(
        [FromServices] IGetService<GameUrlQuery, GameUrlResult> service,
        GameUrlViewModel source)
    {
        var query = new GameUrlQuery(
            source.ApiUrl,
            source.SecretKey,
            source.Username,
            source.Password,
            source.PlayerCode,
            source.GameCode,
            source.Language);

        var result = await service
                .GetAsync(
                    query,
                    HttpContext.RequestAborted)
                .ConfigureAwait(false);

        return new GameUrlResultViewModel(
            result.IsPlayer,
            result.GameUrl);
    }

    /// <summary>
    /// Games the history by settle asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///     {
    ///         "apiUrl": "https://api.gcpstg.m27613.com/",
    ///         "secretKey": "66a44892-54dc-488b-b755-1d1da4f8ad61",
    ///         "username": "PGT_brand_api_stg",
    ///         "password": "ArSg5e6CR2g1wIgB",
    ///         "start": "2022-08-25T11:00:00+08:00",
    ///         "end": "2022-08-25T12:00:00+08:00",
    ///         "offset": 1,
    ///         "limit": 20
    ///     }
    /// </remarks>
    [HttpPost("game/history/settle")]
    public async IAsyncEnumerable<RoundResultViewModel> GameHistoryBySettleAsync(
        [FromServices] IQueryService<GameHistoryBySettleQuery, GameHistoryResult> service,
        RoundViewModel source)
    {
        var query = new GameHistoryBySettleQuery(
            source.ApiUrl,
            source.SecretKey,
            source.Username,
            source.Password,
            source.Start,
            source.End,
            source.Offset,
            source.Limit);

        var results = await service
                .QueryAsync(
                    query,
                    HttpContext.RequestAborted)
                .ToArrayAsync(HttpContext.RequestAborted)
                .ConfigureAwait(false);

        if (!results.Any())
        {
            yield break;
        }

        foreach (var result in results)
        {
            yield return new RoundResultViewModel(
                result.Finished,
                result.Revenu,
                result.Currency,
                result.Debit,
                result.Credit,
                result.RoundId,
                result.BrandId,
                result.PlayerCode,
                result.GameCode,
                result.Device,
                result.Win,
                result.Bet,
                result.BalanceBefore,
                result.BalanceAfter,
                result.TotalEvents,
                result.FirstTs,
                result.Ts,
                result.IsTest,
                result.InsertedAt,
                result.RecoveryType,
                result.TotalJpContribution,
                result.TotalJpWin,
                result.AdditionalData);
        }
    }

    /// <summary>
    /// Games the history by un settle asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///     {
    ///         "apiUrl": "https://api.gcpstg.m27613.com/",
    ///         "secretKey": "66a44892-54dc-488b-b755-1d1da4f8ad61",
    ///         "username": "PGT_brand_api_stg",
    ///         "password": "ArSg5e6CR2g1wIgB",
    ///         "start": "2022-08-25T11:00:00+08:00",
    ///         "end": "2022-08-25T12:00:00+08:00",
    ///         "offset": 1,
    ///         "limit": 20
    ///     }
    /// </remarks>
    [HttpPost("game/history/unsettle")]
    public async IAsyncEnumerable<RoundResultViewModel> GameHistoryByUnSettleAsync(
        [FromServices] IQueryService<GameHistoryByUnSettleQuery, GameHistoryResult> service,
        RoundViewModel source)
    {
        var query = new GameHistoryByUnSettleQuery(
            source.ApiUrl,
            source.SecretKey,
            source.Username,
            source.Password,
            source.Start,
            source.End,
            source.Offset,
            source.Limit);

        var results = await service
                .QueryAsync(
                    query,
                    HttpContext.RequestAborted)
                .ToArrayAsync(HttpContext.RequestAborted)
                .ConfigureAwait(false);

        if (!results.Any())
        {
            yield break;
        }

        foreach (var result in results)
        {
            yield return new RoundResultViewModel(
                result.Finished,
                result.Revenu,
                result.Currency,
                result.Debit,
                result.Credit,
                result.RoundId,
                result.BrandId,
                result.PlayerCode,
                result.GameCode,
                result.Device,
                result.Win,
                result.Bet,
                result.BalanceBefore,
                result.BalanceAfter,
                result.TotalEvents,
                result.FirstTs,
                result.Ts,
                result.IsTest,
                result.InsertedAt,
                result.RecoveryType,
                result.TotalJpContribution,
                result.TotalJpWin,
                result.AdditionalData);
        }
    }
}
