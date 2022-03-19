using System;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.Vgmdb.Models;

public class SearchResponseResultsAlbum
{
    [JsonPropertyName("catalog")]
    public string Catalog { get; set; }

    [JsonPropertyName("link")]
    public string Link { get; set; }

    [JsonPropertyName("release_date")]
    public string ReleaseDate { get; set; }

    [JsonPropertyName("titles")]
    public LocalizedString Titles { get; set; }

    public int Id => int.Parse(Link.Replace("album/", string.Empty, StringComparison.OrdinalIgnoreCase), CultureInfo.InvariantCulture);
}
