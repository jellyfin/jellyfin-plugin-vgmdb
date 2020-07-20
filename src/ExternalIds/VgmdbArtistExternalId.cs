using MediaBrowser.Controller.Entities.Audio;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;

namespace Jellyfin.Plugin.Vgmdb.ExternalIds
{
	/// <inheritdoc />
	// ReSharper disable once ClassNeverInstantiated.Global
	public class VgmdbArtistExternalId : IExternalId
	{
		public const string ExternalId = "VGMdbArtist";

		public string ProviderName => "VGMdb Artist";

		public string Key => ExternalId;
		
		public ExternalIdMediaType? Type => ExternalIdMediaType.Album;

		public string UrlFormatString => "https://vgmdb.net/artist/{0}";

		public bool Supports(IHasProviderIds item) => item is MusicArtist;
	}
}