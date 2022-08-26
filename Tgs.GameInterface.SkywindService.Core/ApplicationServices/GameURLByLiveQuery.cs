namespace Tgs.GameInterface.SkywindService.Core.ApplicationServices;

public record GameURLByLiveQuery(
    Uri ApiUrl,
    string SecretKey,
    string Username,
    string Password,
    string PlayerCode,
    string AuthToken,
    string Language);
