dotnet publish
New-Item -ItemType Directory -Force -Path "C:\ProgramData\Jellyfin\Server\plugins\VGMdb_local\"
Copy-Item ".\bin\Debug\net5.0\Jellyfin.Plugin.Vgmdb.dll" -Destination "C:\ProgramData\Jellyfin\Server\plugins\VGMdb_local\"
Copy-Item ".\bin\Debug\net5.0\Jellyfin.Plugin.Vgmdb.pdb" -Destination "C:\ProgramData\Jellyfin\Server\plugins\VGMdb_local\"
