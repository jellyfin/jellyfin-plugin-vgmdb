using System;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.Vgmdb.Models;

public class Organization
{
    [JsonPropertyName("link")]
    public string Link { get; set; }

    [JsonPropertyName("names")]
    public LocalizedString Names { get; set; }

    [JsonPropertyName("role")]
    public string Role { get; set; }

    public int Id => int.Parse(Link.Replace("org/", string.Empty, StringComparison.OrdinalIgnoreCase), CultureInfo.InvariantCulture);
}
