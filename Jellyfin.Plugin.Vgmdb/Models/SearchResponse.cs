using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.Vgmdb.Models;

public class SearchResponse
{
    [JsonPropertyName("results")]
    public SearchResponseResults Results { get; set; }
}
