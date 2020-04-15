dotnet pack Lib.All.sln -p:IncludeSymbols=true -p:SymbolPackageFormat=snupkg -o Release
nuget push Release\*.nupkg -ApiKey R9385gmqDta6SkACSMnqSijjk3EK -Source https://baget.ludd.info/v3/index.json
Remove-Item Release\*
Remove-Item Release\