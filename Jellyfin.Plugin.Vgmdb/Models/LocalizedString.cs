using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.Vgmdb.Models;

public class LocalizedString
{
    [JsonPropertyName("en")]
    public string En { get; set; }

    [JsonPropertyName("ja")]
    public string Ja { get; set; }

    [JsonPropertyName("jaLatn")]
    public string JaLatn { get; set; }

    public string GetPreferred()
    {
        return JaLatn ?? En ?? Ja;
    }
}
