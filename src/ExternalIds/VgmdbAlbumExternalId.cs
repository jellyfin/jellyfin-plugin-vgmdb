using MediaBrowser.Controller.Entities.Audio;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;

namespace Jellyfin.Plugin.Vgmdb.ExternalIds
{
	/// <inheritdoc />
	// ReSharper disable once ClassNeverInstantiated.Global
	public class VgmdbAlbumExternalId : IExternalId
	{
		public const string ExternalId = "VGMdbAlbum";

		public string Name => "VGMdb Album";

		public string Key => ExternalId;

		public string UrlFormatString => "https://vgmdb.net/album/{0}";

		public bool Supports(IHasProviderIds item) => item is MusicAlbum;
	}
}