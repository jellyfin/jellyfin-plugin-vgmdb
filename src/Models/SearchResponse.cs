using System;
using System.Collections.Generic;

namespace Jellyfin.Plugin.Vgmdb.Models
{
	public class SearchResponse
	{
		public SearchResponseResults results { get; set; }
	}

	public class SearchResponseResults
	{
		public List<SearchResponseResultsAlbum> albums { get; set; }
		public List<SearchResponseResultsArtist> artists { get; set; }
	}

	public class SearchResponseResultsAlbum
	{
		public String catalog { get; set; }
		public String link { get; set; }
		public String release_date { get; set; }
		public LocalizedString titles { get; set; }

		public int Id { get => int.Parse(link.Replace("album/", "")); }
	}

	public class SearchResponseResultsArtist
	{
		public List<String> aliases { get; set; }
		public String link { get; set; }
		public LocalizedString names { get; set; }

		public int Id { get => int.Parse(link.Replace("artist/", "")); }
	}

	public class LocalizedString
	{
		public String en { get; set; }
		public String ja { get; set; }
		public String jaLatn { get; set; }

		public String GetPreferred()
		{
			if (this.jaLatn != null) return this.jaLatn;
			else if (this.en != null) return this.en;
			else if (this.ja != null) return this.ja;
			else return null;
		}
	}

	public class ArtistResponse
	{
		public String name { get; set; }
		public String picture_full { get; set; }
		public String picture_small { get; set; }
		public String link { get; set; }
		public String notes { get; set; }

		public int Id { get => int.Parse(link.Replace("artist/", "")); }
	}

	public class AlbumResponse
	{
		public LocalizedString names { get; set; }
		public String picture_full { get; set; }
		public String picture_small { get; set; }
		public String picture_thumb { get; set; }
		public String link { get; set; }
		public String notes { get; set; }

		public List<String> categories { get; set; }
		public List<Organisation> organizations { get; set; }

		public int Id { get => int.Parse(link.Replace("album/", "")); }
	}

	public class Organisation
	{
		public String link { get; set; }
		public LocalizedString names { get; set; }
		public String role { get; set; }

		public int Id { get => int.Parse(link.Replace("org/", "")); }
	}
}