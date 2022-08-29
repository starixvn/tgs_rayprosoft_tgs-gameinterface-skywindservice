using Grpc.Core;
using Tgs.GameInterface.SkywindService.Core.ApplicationServices;
using Tgs.GameInterface.SkywindService.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SkywindService.GrpcServices;

public class GameInterfaceSkywindGrpcService : GameInterfaceSkywindService.GameInterfaceSkywindServiceBase
{
    private readonly IGetService<FundInQuery, FundInResult> _fundInService;
    private readonly IGetService<FundOutQuery, FundOutResult> _fundOutService;
    private readonly IGetService<FundStatusQuery, FundStatusResult> _fundStatusService;
    private readonly IQueryService<GameHistoryBySettleQuery, GameHistoryResult> _gameHistoryBySettleService;
    private readonly IQueryService<GameHistoryByUnSettleQuery, GameHistoryResult> _gameHistoryByUnSettleService;
    private readonly IGetService<GameURLByLiveQuery, GameURLResult> _gameURLByLiveService;
    private readonly IGetService<GameURLBySlotQuery, GameURLResult> _gameURLBySlotService;
    private readonly IGetService<PlayerBalanceQuery, PlayerBalanceResult> _playerBalanceService;
    private readonly ICommandService<PlayerCreateCommand> _playerCreateService;

    public GameInterfaceSkywindGrpcService(
        IGetService<FundInQuery, FundInResult> fundInService,
        IGetService<FundOutQuery, FundOutResult> fundOutService,
        IGetService<FundStatusQuery, FundStatusResult> fundStatusService,
        IQueryService<GameHistoryBySettleQuery, GameHistoryResult> gameHistoryBySettleService,
        IQueryService<GameHistoryByUnSettleQuery, GameHistoryResult> gameHistoryByUnSettleService,
        IGetService<GameURLByLiveQuery, GameURLResult> gameURLByLiveService,
        IGetService<GameURLBySlotQuery, GameURLResult> gameURLBySlotService,
        IGetService<PlayerBalanceQuery, PlayerBalanceResult> playerBalanceService,
        ICommandService<PlayerCreateCommand> playerCreateService)
    {
        _fundInService = fundInService ?? throw new ArgumentNullException(nameof(fundInService));
        _fundOutService = fundOutService ?? throw new ArgumentNullException(nameof(fundOutService));
        _fundStatusService = fundStatusService ?? throw new ArgumentNullException(nameof(fundStatusService));
        _gameHistoryBySettleService = gameHistoryBySettleService ?? throw new ArgumentNullException(nameof(gameHistoryBySettleService));
        _gameHistoryByUnSettleService = gameHistoryByUnSettleService ?? throw new ArgumentNullException(nameof(gameHistoryByUnSettleService));
        _gameURLByLiveService = gameURLByLiveService ?? throw new ArgumentNullException(nameof(gameURLByLiveService));
        _gameURLBySlotService = gameURLBySlotService ?? throw new ArgumentNullException(nameof(gameURLBySlotService));
        _playerBalanceService = playerBalanceService ?? throw new ArgumentNullException(nameof(playerBalanceService));
        _playerCreateService = playerCreateService ?? throw new ArgumentNullException(nameof(playerCreateService));
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
                    Finished = result.Finished,
                    Revenu = result.Revenu ?? 0,
                    Currency = result.Currency,
                    Debit = result.Debit ?? 0,
                    Credit = result.Credit ?? 0,
                    RoundId = result.RoundId,
                    BrandId = result.BrandId,
                    PlayerCode = result.PlayerCode,
                    GameCode = result.GameCode,
                    Device = result.Device,
                    Win = result.Win ?? 0,
                    Bet = result.Bet,
                    BalanceBefore = result.BalanceBefore,
                    BalanceAfter = result.BalanceAfter,
                    TotalEvents = result.TotalEvents,
                    FirstTs = result.FirstTs.UtcDateTime.ToString("O"),
                    Ts = result.Ts.HasValue ? result.Ts.Value.UtcDateTime.ToString("O") : string.Empty,
                    IsTest = result.IsTest,
                    InsertedAt = result.InsertedAt.UtcDateTime.ToString("O"),
                    RecoveryType = result.RecoveryType ?? string.Empty,
                    TotalJpContribution = result.TotalJpContribution,
                    TotalJpWin = result.TotalJpWin ?? 0,
                    AdditionalData = result.AdditionalData
                });
            }
        }

        return reply;
    }

    public override async Task<GameRecordReply> GameRecordByUnSettle(GameRecordRequest request, ServerCallContext context)
    {
        var query = new GameHistoryByUnSettleQuery(
            new Uri(request.ApiUrl),
            request.SecretKey,
            request.Username,
            request.Password,
            request.Start.ToDateTimeOffset(),
            request.End.ToDateTimeOffset(),
            request.Offset,
            request.Limit);

        var results = await _gameHistoryByUnSettleService
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
                    Finished = result.Finished,
                    Revenu = result.Revenu ?? 0,
                    Currency = result.Currency,
                    Debit = result.Debit ?? 0,
                    Credit = result.Credit ?? 0,
                    RoundId = result.RoundId,
                    BrandId = result.BrandId,
                    PlayerCode = result.PlayerCode,
                    GameCode = result.GameCode,
                    Device = result.Device,
                    Win = result.Win ?? 0,
                    Bet = result.Bet,
                    BalanceBefore = result.BalanceBefore,
                    BalanceAfter = result.BalanceAfter,
                    TotalEvents = result.TotalEvents,
                    FirstTs = result.FirstTs.UtcDateTime.ToString("O"),
                    Ts = result.Ts.HasValue ? result.Ts.Value.UtcDateTime.ToString("O") : string.Empty,
                    IsTest = result.IsTest,
                    InsertedAt = result.InsertedAt.UtcDateTime.ToString("O"),
                    RecoveryType = result.RecoveryType,
                    TotalJpContribution = result.TotalJpContribution,
                    TotalJpWin = result.TotalJpWin ?? 0,
                    AdditionalData = result.AdditionalData
                });
            }
        }

        return reply;
    }

    public override async Task<GameURLReply> GameURLByLive(GameURLByLiveRequest request, ServerCallContext context)
    {
        var query = new GameURLByLiveQuery(
            new Uri(request.ApiUrl),
            request.SecretKey,
            request.Username,
            request.Password,
            request.PlayerCode,
            request.AuthToken,
            request.Language);

        var result = await _gameURLByLiveService
                    .GetAsync(
                        query,
                        context.CancellationToken)
                    .ConfigureAwait(false);

        return new GameURLReply
        {
            IsPlayer = result.IsPlayer,
            GameUrl = result.GameUrl
        };
    }

    public override async Task<GameURLReply> GameURLBySlot(GameURLBySlotRequest request, ServerCallContext context)
    {
        var query = new GameURLBySlotQuery(
            new Uri(request.ApiUrl),
            request.SecretKey,
            request.Username,
            request.Password,
            request.PlayerCode,
            request.GameCode,
            request.Language);

        var result = await _gameURLBySlotService
                    .GetAsync(
                        query,
                        context.CancellationToken)
                    .ConfigureAwait(false);

        return new GameURLReply
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
