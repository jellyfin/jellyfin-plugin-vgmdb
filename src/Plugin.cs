using System;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Serialization;

namespace Jellyfin.Plugin.Vgmdb
{
	/// <inheritdoc />
	// ReSharper disable once UnusedMember.Global
	public class Plugin : BasePlugin<PluginConfiguration>
	{
		public Plugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer) : base(applicationPaths, xmlSerializer)
		{
		}

		public override string Name => "VGMdb";
		public override Guid Id => Guid.Parse("44616595-5798-47ad-8658-3c09f3030505");
	}
}