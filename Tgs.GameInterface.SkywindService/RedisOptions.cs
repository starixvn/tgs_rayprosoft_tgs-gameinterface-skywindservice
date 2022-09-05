namespace Tgs.GameInterface.SkywindService;

public record RedisOptions
{
    public string ConnectionString { get; set; } = "localhost";
}
