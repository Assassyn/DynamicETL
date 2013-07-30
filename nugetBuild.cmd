nuget pack Sources\AraneaIT.Migration.Engine\AraneaIT.Migration.Engine.csproj -Prop Configuration=Release
nuget pack Sources\AraneaIT.Migration.Providers.FirebirdDatabase\AraneaIT.Migration.Providers.FirebirdDatabase.csproj -Prop Configuration=Release
nuget pack Sources\AraneaIt.Migration.Providers.SqlServerDatabase\AraneaIT.Migration.Providers.SqlServerDatabase.csproj -Prop Configuration=Release
nuget pack Sources\AraneaIT.Migration.Providers.WebAPI\AraneaIT.Migration.Providers.WebAPI.csproj -Prop Configuration=Release

nuget push AraneaIT.Migration.Engine.1.0.0.0.nupkg -s http://nuget-ait.azurewebsites.net/ 14122011
nuget push AraneaIT.Migration.Providers.FirebirdDatabase.1.0.0.0.nupkg -s http://nuget-ait.azurewebsites.net/ 14122011
nuget push AraneaIT.Migration.Providers.SqlServerDatabase.1.0.0.0.nupkg -s http://nuget-ait.azurewebsites.net/ 14122011
nuget push AraneaIT.Migration.Providers.WebAPI.1.0.3.0.nupkg -s http://nuget-ait.azurewebsites.net/ 14122011
