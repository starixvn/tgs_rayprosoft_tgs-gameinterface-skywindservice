using Tgs.GameInterface.SkywindService.Protos;

namespace Tgs.GameInterface.SkywindService.Client;

public interface IGameInterfaceSkywindClient
{
    ValueTask<FundReply> FundInAsync(FundRequest request, CancellationToken cancellationToken = default);

    ValueTask<FundReply> FundOutAsync(FundRequest request, CancellationToken cancellationToken = default);

    ValueTask<FundStatusReply> FundStatusAsync(FundStatusRequest request, CancellationToken cancellationToken = default);

    ValueTask<GameRecordReply> GameRecordBySettleAsync(GameRecordRequest request, CancellationToken cancellationToken = default);

    ValueTask<GameRecordReply> GameRecordByUnSettleAsync(GameRecordRequest request, CancellationToken cancellationToken = default);

    ValueTask<GameURLReply> GameURLByLiveAsync(GameURLByLiveRequest request, CancellationToken cancellationToken = default);

    ValueTask<GameURLReply> GameURLBySlotAsync(GameURLBySlotRequest request, CancellationToken cancellationToken = default);

    ValueTask<PlayerBalanceReply> PlayerBalanceAsync(PlayerBalanceRequest request, CancellationToken cancellationToken = default);

    ValueTask<BaseReply> PlayerCreateAsync(PlayerCreateRequest request, CancellationToken cancellationToken = default);
}
