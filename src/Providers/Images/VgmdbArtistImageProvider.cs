using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Jellyfin.Plugin.Vgmdb.ExternalIds;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.Audio;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
using MediaBrowser.Model.Serialization;

namespace Jellyfin.Plugin.Vgmdb.Providers.Images
{
	public class VgmdbArtistImageProvider : IRemoteImageProvider
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly VgmdbApi _api;

		public VgmdbArtistImageProvider(IHttpClientFactory httpClientFactory, IJsonSerializer json)
		{
			_httpClientFactory = httpClientFactory;
			_api = new VgmdbApi(httpClientFactory, json);
		}

		public string Name => "VGMdb";

		public Task<HttpResponseMessage> GetImageResponse(string url, CancellationToken cancellationToken)
		{
			return _httpClientFactory.CreateClient().GetAsync(url);
		}

		public async Task<IEnumerable<RemoteImageInfo>> GetImages(BaseItem item, CancellationToken cancellationToken)
		{
			var images = new List<RemoteImageInfo>();

			var id = item.GetProviderId(VgmdbArtistExternalId.ExternalId);

			//todo use a search to find id
			if (id == null) return images;

			var artist = await _api.GetArtistById(int.Parse(id), cancellationToken);

			images.Add(new RemoteImageInfo
			{
				Url = artist.picture_full,
				ThumbnailUrl = artist.picture_small
			});

			return images;
		}

		public IEnumerable<ImageType> GetSupportedImages(BaseItem item)
		{
			return new List<ImageType>
			{
				ImageType.Primary
			};
		}

		public bool Supports(BaseItem item) => item is MusicArtist;
	}
}
