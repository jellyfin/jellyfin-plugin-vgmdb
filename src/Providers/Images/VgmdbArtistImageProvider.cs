using System.Collections.Generic;
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
		private readonly IHttpClient _httpClient;
		private readonly VgmdbApi _api;

		public VgmdbArtistImageProvider(IHttpClient httpClient, IJsonSerializer json)
		{
			_httpClient = httpClient;
			_api = new VgmdbApi(httpClient, json);
		}

		public string Name => "VGMdb";

		public Task<HttpResponseInfo> GetImageResponse(string url, CancellationToken cancellationToken)
		{
			return _httpClient.GetResponse(new HttpRequestOptions
			{
				Url = url,
				CancellationToken = cancellationToken
			});
		}

		public async Task<IEnumerable<RemoteImageInfo>> GetImages(BaseItem item, CancellationToken cancellationToken)
		{
			var images = new List<RemoteImageInfo>();

			var id = item.GetProviderId(VgmdbArtistExternalId.ExternalId);
			
			if (id != null) //todo use a search to find id
			{
				var artist = await _api.GetArtistById(int.Parse(id), cancellationToken);

				images.Add(new RemoteImageInfo
				{
					Url = artist.picture_full,
					ThumbnailUrl = artist.picture_small
				});
			}

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