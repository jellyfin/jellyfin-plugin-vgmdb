using MediaBrowser.Controller.Entities.Audio;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;

namespace Jellyfin.Plugin.Vgmdb.Providers
{
	public class VgmdbArtistExternalId : IExternalId
	{
		public static readonly string KEY = "VGMdbArtist";

		public string Name => "VGMdb Artist";

		public string Key => KEY;

		public string UrlFormatString => "https://vgmdb.net/artist/{0}";

		public bool Supports(IHasProviderIds item) => item is MusicArtist;
	}
}