using System;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.Vgmdb.Models;

public class ArtistResponse
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("picture_full")]
    public string PictureFull { get; set; }

    [JsonPropertyName("picture_small")]
    public string PictureSmall { get; set; }

    [JsonPropertyName("link")]
    public string Link { get; set; }

    [JsonPropertyName("notes")]
    public string Notes { get; set; }

    public int Id => int.Parse(Link.Replace("artist/", string.Empty, StringComparison.OrdinalIgnoreCase), CultureInfo.InvariantCulture);
}
