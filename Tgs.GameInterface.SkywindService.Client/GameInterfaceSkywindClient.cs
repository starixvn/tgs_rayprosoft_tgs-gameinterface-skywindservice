using Tgs.GameInterface.SkywindService.Protos;

namespace Tgs.GameInterface.SkywindService.Client;

internal class GameInterfaceSkywindClient(
    GameInterfaceSkywindService.GameInterfaceSkywindServiceClient client) : IGameInterfaceSkywindClient
{
    public async ValueTask<string> CheckingServiceAsync(string machineName, CancellationToken cancellationToken = default)
    {
        var request = new CheckingServiceRequest
        {
            MachineName = machineName,
        };

        var reply = await client
            .CheckingServiceAsync(
                request: request,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return reply.MachineName;
    }

    public async ValueTask<FundReply> FundInAsync(FundRequest request, CancellationToken cancellationToken = default)
        => await client
        .FundInAsync(
            request: request,
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask<FundReply> FundOutAsync(FundRequest request, CancellationToken cancellationToken = default)
        => await client
        .FundOutAsync(
            request: request,
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask<FundStatusReply> FundStatusAsync(FundStatusRequest request, CancellationToken cancellationToken = default)
        => await client
        .FundStatusAsync(
            request: request,
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask<GameRecordReply> GameRecord3rdPartyAsync(GameRecordRequest request, CancellationToken cancellationToken = default)
        => await client
        .GameRecord3rdPartyAsync(
            request: request,
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask<GameRecordReply> GameRecordBySettleAsync(GameRecordRequest request, CancellationToken cancellationToken = default)
        => await client
        .GameRecordBySettleAsync(
            request: request,
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask<GameUrlReply> GameUrlAsync(GameUrlRequest request, CancellationToken cancellationToken = default)
        => await client
        .GameUrlAsync(
            request: request,
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask<PlayerBalanceReply> PlayerBalanceAsync(PlayerBalanceRequest request, CancellationToken cancellationToken = default)
        => await client
        .PlayerBalanceAsync(
            request: request,
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask<BaseReply> PlayerCreateAsync(PlayerCreateRequest request, CancellationToken cancellationToken = default)
        => await client
        .PlayerCreateAsync(
            request: request,
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);
}
