dotnet publish
Copy-Item ".\bin\Debug\net5.0\Jellyfin.Plugin.Vgmdb.dll" -Destination "C:\ProgramData\Jellyfin\Server\plugins"
Copy-Item ".\bin\Debug\net5.0\Jellyfin.Plugin.Vgmdb.pdb" -Destination "C:\ProgramData\Jellyfin\Server\plugins"
