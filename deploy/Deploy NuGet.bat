CALL ../../set-nuget-key.bat

del *.nupkg

nuget pack ../Ministry.WebDriver.Extensions/Ministry.WebDriver.Extensions.csproj -Prop Configuration=Release
nuget push *.nupkg

pause