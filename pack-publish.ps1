dotnet clean Lib.All.sln
dotnet build Lib.All.sln -c Release
dotnet pack Lib.All.sln --include-symbols --include-source -p:SymbolPackageFormat=snupkg -o Release -c Release
nuget push Release\*.nupkg -ApiKey icmrCcSPKTLdPMdMAePGD5Ybn3Ec -Source https://baget.ludd.info/v3/index.json
Remove-Item Release\*
Remove-Item Release\