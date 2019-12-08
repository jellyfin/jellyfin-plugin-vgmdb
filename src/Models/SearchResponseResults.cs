using System.Collections.Generic;

namespace Jellyfin.Plugin.Vgmdb.Models
{
	public class SearchResponseResults
	{
		public List<SearchResponseResultsAlbum> albums { get; set; }
		public List<SearchResponseResultsArtist> artists { get; set; }
	}
}