using System.Text.Json;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Tgs.GameInterface.SkywindService.Core.ApplicationServices;
using Tgs.GameInterface.SkywindService.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SkywindService.GrpcServices;

public class GameInterfaceSkywindGrpcService : GameInterfaceSkywindService.GameInterfaceSkywindServiceBase
{
    private readonly ILogger<GameInterfaceSkywindGrpcService> _logger;
    private readonly IGetService<FundInQuery, FundInResult> _fundInService;
    private readonly IGetService<FundOutQuery, FundOutResult> _fundOutService;
    private readonly IGetService<FundStatusQuery, FundStatusResult> _fundStatusService;
    private readonly IQueryService<GameHistory3rdPartyQuery, GameHistoryResult> _gameHistory3RdPartyService;
    private readonly IQueryService<GameHistoryBySettleQuery, GameHistoryResult> _gameHistoryBySettleService;
    private readonly IGetService<GameUrlQuery, GameUrlResult> _gameUrlService;
    private readonly IGetService<PlayerBalanceQuery, PlayerBalanceResult> _playerBalanceService;
    private readonly ICommandService<PlayerCreateCommand> _playerCreateService;

    public GameInterfaceSkywindGrpcService(
        ILogger<GameInterfaceSkywindGrpcService> logger,
        IGetService<FundInQuery, FundInResult> fundInService,
        IGetService<FundOutQuery, FundOutResult> fundOutService,
        IGetService<FundStatusQuery, FundStatusResult> fundStatusService,
        IQueryService<GameHistory3rdPartyQuery, GameHistoryResult> gameHistory3rdPartyService,
        IQueryService<GameHistoryBySettleQuery, GameHistoryResult> gameHistoryBySettleService,
        IGetService<GameUrlQuery, GameUrlResult> gameUrlService,
        IGetService<PlayerBalanceQuery, PlayerBalanceResult> playerBalanceService,
        ICommandService<PlayerCreateCommand> playerCreateService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fundInService = fundInService ?? throw new ArgumentNullException(nameof(fundInService));
        _fundOutService = fundOutService ?? throw new ArgumentNullException(nameof(fundOutService));
        _fundStatusService = fundStatusService ?? throw new ArgumentNullException(nameof(fundStatusService));
        _gameHistory3RdPartyService = gameHistory3rdPartyService ?? throw new ArgumentNullException(nameof(gameHistory3rdPartyService));
        _gameHistoryBySettleService = gameHistoryBySettleService ?? throw new ArgumentNullException(nameof(gameHistoryBySettleService));
        _gameUrlService = gameUrlService ?? throw new ArgumentNullException(nameof(gameUrlService));
        _playerBalanceService = playerBalanceService ?? throw new ArgumentNullException(nameof(playerBalanceService));
        _playerCreateService = playerCreateService ?? throw new ArgumentNullException(nameof(playerCreateService));
    }

    public override Task<CheckingServiceReply> CheckingService(CheckingServiceRequest request, ServerCallContext context)
    {
        _logger.LogWarning("LogOn:{logOn} Message: Create a check connection.({machineName})", DateTime.Now.ToString("o"), request.MachineName);

        return Task.FromResult(new CheckingServiceReply
        {
            MachineName = Environment.MachineName
        });
    }

    public override async Task<FundReply> FundIn(FundRequest request, ServerCallContext context)
    {
        var query = new FundInQuery(
            new Uri(request.ApiUrl),
            request.SecretKey,
            request.Username,
            request.Password,
            request.ExtTrxId,
            request.PlayerCode,
            request.Currency,
            request.Amount);

        var result = await _fundInService
                    .GetAsync(
                        query,
                        context.CancellationToken)
                    .ConfigureAwait(false);

        return new FundReply
        {
            Status = result.Status,
            TrxId = result.TrxId,
            ExtTrxId = result.ExtTrxId,
            OrderStatus = result.OrderStatus,
            Amount = result.Amount,
            Balance = result.Balance
        };
    }

    public override async Task<FundReply> FundOut(FundRequest request, ServerCallContext context)
    {
        var query = new FundOutQuery(
            new Uri(request.ApiUrl),
            request.SecretKey,
            request.Username,
            request.Password,
            request.ExtTrxId,
            request.PlayerCode,
            request.Currency,
            request.Amount);

        var result = await _fundOutService
                    .GetAsync(
                        query,
                        context.CancellationToken)
                    .ConfigureAwait(false);

        return new FundReply
        {
            Status = result.Status,
            TrxId = result.TrxId,
            ExtTrxId = result.ExtTrxId,
            OrderStatus = result.OrderStatus,
            Amount = result.Amount,
            Balance = result.Balance
        };
    }

    public override async Task<FundStatusReply> FundStatus(FundStatusRequest request, ServerCallContext context)
    {
        var query = new FundStatusQuery(
            new Uri(request.ApiUrl),
            request.SecretKey,
            request.Username,
            request.Password,
            request.ExtTrxId);

        var result = await _fundStatusService
                    .GetAsync(
                        query,
                        context.CancellationToken)
                    .ConfigureAwait(false);

        return new FundStatusReply
        {
            IsTransaction = result.IsTransaction,
            Status = result.Status
        };
    }

    public override async Task<GameRecordReply> GameRecord3rdParty(GameRecordRequest request, ServerCallContext context)
    {
        var query = new GameHistory3rdPartyQuery(
            new Uri(request.ApiUrl),
            request.SecretKey,
            request.Username,
            request.Password,
            request.Start.ToDateTimeOffset(),
            request.End.ToDateTimeOffset(),
            request.Offset,
            request.Limit);

        var results = await _gameHistory3RdPartyService
                    .QueryAsync(
                        query,
                        context.CancellationToken)
                    .ToArrayAsync(context.CancellationToken)
                    .ConfigureAwait(false);

        var reply = new GameRecordReply();

        if (results.Any())
        {
            foreach (var result in results)
            {
                reply.Items.Add(new GameRecord()
                {
                    PlayerCode = result.PlayerCode,
                    ExternalRoundId = result.ExternalRoundId,
                    ExternalGameId = result.ExternalGameId,
                    Finished = result.Finished,
                    ExternalCreatedOn = result.ExternalCreatedOn.ToTimestamp(),
                    ExternalClosedOn = result.ExternalClosedOn.ToTimestamp(),
                    Bet = result.Bet,
                    ValidBet = result.Bet,
                    PlayerWinLoss = result.PlayerWinLoss,
                    Turnover = result.Bet,
                    Settle = result.Settle,
                    AdditionalData = result.AdditionalData
                });
            }
        }

        return reply;
    }

    public override async Task<GameRecordReply> GameRecordBySettle(GameRecordRequest request, ServerCallContext context)
    {
        var query = new GameHistoryBySettleQuery(
            new Uri(request.ApiUrl),
            request.SecretKey,
            request.Username,
            request.Password,
            request.Start.ToDateTimeOffset(),
            request.End.ToDateTimeOffset(),
            request.Offset,
            request.Limit);

        var results = await _gameHistoryBySettleService
                    .QueryAsync(
                        query,
                        context.CancellationToken)
                    .ToArrayAsync(context.CancellationToken)
                    .ConfigureAwait(false);

        var reply = new GameRecordReply();

        if (results.Any())
        {
            foreach (var result in results)
            {
                reply.Items.Add(new GameRecord()
                {
                    PlayerCode = result.PlayerCode,
                    ExternalRoundId = result.ExternalRoundId,
                    ExternalGameId = result.ExternalGameId,
                    Finished = result.Finished,
                    ExternalCreatedOn = result.ExternalCreatedOn.ToTimestamp(),
                    ExternalClosedOn = result.ExternalClosedOn.ToTimestamp(),
                    Bet = result.Bet,
                    ValidBet = result.Bet,
                    PlayerWinLoss = result.PlayerWinLoss,
                    Turnover = result.Bet,
                    Settle = result.Settle,
                    AdditionalData = result.AdditionalData
                });
            }
        }

        return reply;
    }

    public override async Task<GameUrlReply> GameUrl(GameUrlRequest request, ServerCallContext context)
    {
        var query = new GameUrlQuery(
            new Uri(request.ApiUrl),
            request.SecretKey,
            request.Username,
            request.Password,
            request.PlayerCode,
            request.GameCode,
            request.Language);

        var result = await _gameUrlService
                    .GetAsync(
                        query,
                        context.CancellationToken)
                    .ConfigureAwait(false);

        return new GameUrlReply
        {
            IsPlayer = result.IsPlayer,
            GameUrl = result.GameUrl
        };
    }

    public override async Task<PlayerBalanceReply> PlayerBalance(PlayerBalanceRequest request, ServerCallContext context)
    {
        var query = new PlayerBalanceQuery(
            new Uri(request.ApiUrl),
            request.SecretKey,
            request.Username,
            request.Password,
            request.PlayerCode);

        var result = await _playerBalanceService
                    .GetAsync(
                        query,
                        context.CancellationToken)
                    .ConfigureAwait(false);

        return new PlayerBalanceReply
        {
            IsPlayer = result.IsPlayer,
            Balance = result.Balance
        };
    }

    public override async Task<BaseReply> PlayerCreate(PlayerCreateRequest request, ServerCallContext context)
    {
        var query = new PlayerCreateCommand(
            new Uri(request.ApiUrl),
            request.SecretKey,
            request.Username,
            request.Password,
            request.PlayerCode,
            request.Currency);

        await _playerCreateService
                .ExecuteAsync(
                    query,
                    context.CancellationToken)
                .ConfigureAwait(false);

        return new BaseReply
        {
            Status = true
        };
    }
}
