namespace Tgs.GameInterface.SkywindService.Core.ApplicationServices;

public record GameURLBySlotQuery(
    Uri ApiUrl,
    string SecretKey,
    string Username,
    string Password,
    string PlayerCode,
    string GameCode,
    string Language);
