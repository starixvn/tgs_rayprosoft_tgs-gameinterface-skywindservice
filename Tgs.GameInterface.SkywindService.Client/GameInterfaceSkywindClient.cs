using Tgs.GameInterface.SkywindService.Protos;

namespace Tgs.GameInterface.SkywindService.Client;

internal class GameInterfaceSkywindClient : IGameInterfaceSkywindClient
{
    private readonly GameInterfaceSkywindService.GameInterfaceSkywindServiceClient _client;

    public GameInterfaceSkywindClient(GameInterfaceSkywindService.GameInterfaceSkywindServiceClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<string> CheckingServiceAsync(string machineName, CancellationToken cancellationToken = default)
    {
        var request = new CheckingServiceRequest
        {
            MachineName = machineName,
        };

        var reply = await _client
            .CheckingServiceAsync(
                request: request,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return reply.MachineName;
    }

    public async ValueTask<FundReply> FundInAsync(FundRequest request, CancellationToken cancellationToken = default)
        => await _client
        .FundInAsync(
            request: request,
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask<FundReply> FundOutAsync(FundRequest request, CancellationToken cancellationToken = default)
        => await _client
        .FundOutAsync(
            request: request,
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask<FundStatusReply> FundStatusAsync(FundStatusRequest request, CancellationToken cancellationToken = default)
        => await _client
        .FundStatusAsync(
            request: request,
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask<GameRecordReply> GameRecord3rdPartyAsync(GameRecordRequest request, CancellationToken cancellationToken = default)
        => await _client
        .GameRecord3rdPartyAsync(
            request: request,
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask<GameRecordReply> GameRecordBySettleAsync(GameRecordRequest request, CancellationToken cancellationToken = default)
        => await _client
        .GameRecordBySettleAsync(
            request: request,
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask<GameUrlReply> GameUrlAsync(GameUrlRequest request, CancellationToken cancellationToken = default)
        => await _client
        .GameUrlAsync(
            request: request,
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask<PlayerBalanceReply> PlayerBalanceAsync(PlayerBalanceRequest request, CancellationToken cancellationToken = default)
        => await _client
        .PlayerBalanceAsync(
            request: request,
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask<BaseReply> PlayerCreateAsync(PlayerCreateRequest request, CancellationToken cancellationToken = default)
        => await _client
        .PlayerCreateAsync(
            request: request,
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);
}
