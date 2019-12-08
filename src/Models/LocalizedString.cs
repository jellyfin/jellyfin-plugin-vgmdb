namespace Jellyfin.Plugin.Vgmdb.Models
{
	public class LocalizedString
	{
		public string en { get; set; }
		public string ja { get; set; }
		public string jaLatn { get; set; }

		public string GetPreferred()
		{
			return jaLatn ?? en ?? ja;
		}
	}
}