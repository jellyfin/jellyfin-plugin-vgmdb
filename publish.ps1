dotnet publish
Copy-Item ".\bin\Debug\netstandard2.1\Jellyfin.Plugin.Vgmdb.dll" -Destination "C:\ProgramData\Jellyfin\Server\plugins"
Copy-Item ".\bin\Debug\netstandard2.1\Jellyfin.Plugin.Vgmdb.pdb" -Destination "C:\ProgramData\Jellyfin\Server\plugins"