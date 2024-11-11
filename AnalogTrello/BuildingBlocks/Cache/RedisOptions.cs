using System.Text.Json;
using System.Text.Json.Serialization;

namespace BuildingBlocks;

public class RedisOptions
{
    public JsonSerializerOptions JsonSerializerOptions { get; set; } = new JsonSerializerOptions
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        WriteIndented = true
    };
    public TimeSpan DefaultExpiration { get; set; } = TimeSpan.FromMinutes(5);
    public int DefaultChunkSize { get; set; } = 10_000;

    public string ConnectionString { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
}