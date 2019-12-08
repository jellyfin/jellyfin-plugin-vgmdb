using MediaBrowser.Controller.Entities.Audio;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;

namespace Jellyfin.Plugin.Vgmdb.ExternalIds
{
	/// <inheritdoc />
	// ReSharper disable once ClassNeverInstantiated.Global
	public class VgmdbArtistExternalId : IExternalId
	{
		public const string ExternalId = "VGMdbArtist";

		public string Name => "VGMdb Artist";

		public string Key => ExternalId;

		public string UrlFormatString => "https://vgmdb.net/artist/{0}";

		public bool Supports(IHasProviderIds item) => item is MusicArtist;
	}
}