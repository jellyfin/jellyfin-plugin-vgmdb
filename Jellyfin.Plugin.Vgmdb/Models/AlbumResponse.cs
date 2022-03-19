using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.Vgmdb.Models;

public class AlbumResponse
{
    [JsonPropertyName("names")]
    public LocalizedString Names { get; set; }

    [JsonPropertyName("picture_full")]
    public string PictureFull { get; set; }

    [JsonPropertyName("picture_small")]
    public string PictureSmall { get; set; }

    [JsonPropertyName("picture_thumb")]
    public string PictureThumb { get; set; }

    [JsonPropertyName("link")]
    public string Link { get; set; }

    [JsonPropertyName("notes")]
    public string Notes { get; set; }

    [JsonPropertyName("release_date")]
    public string ReleaseDate { get; set; }

    [JsonPropertyName("categories")]
    public IReadOnlyList<string> Categories { get; set; } = Array.Empty<string>();

    [JsonPropertyName("organizations")]
    public IReadOnlyList<Organization> Organizations { get; set; } = Array.Empty<Organization>();

    public int Id => int.Parse(Link.Replace("album/", string.Empty, StringComparison.OrdinalIgnoreCase), CultureInfo.InvariantCulture);
}
