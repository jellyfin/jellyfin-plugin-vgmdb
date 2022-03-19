using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.Vgmdb.Models;

public class SearchResponseResultsArtist
{
    [JsonPropertyName("aliases")]
    public IReadOnlyList<string> Aliases { get; set; } = Array.Empty<string>();

    [JsonPropertyName("link")]
    public string Link { get; set; }

    [JsonPropertyName("names")]
    public LocalizedString Names { get; set; }

    public int Id => int.Parse(Link.Replace("artist/", string.Empty, StringComparison.OrdinalIgnoreCase), CultureInfo.InvariantCulture);
}
