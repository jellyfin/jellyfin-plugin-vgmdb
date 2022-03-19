using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.Vgmdb.Models;

public class SearchResponseResults
{
    [JsonPropertyName("albums")]
    public IReadOnlyList<SearchResponseResultsAlbum> Albums { get; set; } = Array.Empty<SearchResponseResultsAlbum>();

    [JsonPropertyName("artists")]
    public IReadOnlyList<SearchResponseResultsArtist> Artists { get; set; } = Array.Empty<SearchResponseResultsArtist>();
}
