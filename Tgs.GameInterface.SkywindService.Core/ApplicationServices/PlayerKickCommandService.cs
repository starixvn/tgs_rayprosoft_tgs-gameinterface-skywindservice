using Tgs.ServiceUtility;

namespace Tgs.GameInterface.SkywindService.Core.ApplicationServices
{
    internal class PlayerKickCommandService : ICommandService<PlayerKickCommand>
    {
        private readonly IAccessToken _accessToken;
        private readonly IGameApiClient _client;

        public PlayerKickCommandService(
            IAccessToken accessToken,
            IGameApiClient client)
        {
            _accessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async ValueTask ExecuteAsync(PlayerKickCommand command, CancellationToken cancellationToken = default)
        {

            var accessToken = await _accessToken
                    .GetAsync(
                        command.ApiUrl,
                        command.SecretKey,
                        command.Username,
                        command.Password,
                        cancellationToken)
                    .ConfigureAwait(false);

            await _client
                    .DeleteSessionAsync(
                        command.ApiUrl,
                        accessToken,
                        command.PlayerCode,
                        cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
        }
    }
}