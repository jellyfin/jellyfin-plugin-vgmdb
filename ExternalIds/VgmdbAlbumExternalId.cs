using MediaBrowser.Controller.Entities.Audio;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;

namespace Jellyfin.Plugin.Vgmdb.Providers
{
	public class VgmdbAlbumExternalId : IExternalId
	{
		public static readonly string KEY = "VGMdbAlbum";

		public string Name => "VGMdb Album";

		public string Key => KEY;

		public string UrlFormatString => "https://vgmdb.net/album/{0}";

		public bool Supports(IHasProviderIds item) => item is MusicAlbum;
	}
}