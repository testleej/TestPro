dotnet --info
dotnet restore
dotnet test src/DotnetSpider.Core.Test/DotnetSpider.Core.Test.csproj -f netcoreapp2.0 -c Release
dotnet test src/DotnetSpider.Extension.Test/DotnetSpider.Extension.Test.csproj -f netcoreapp2.0 -c Release