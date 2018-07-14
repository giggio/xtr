Push-Location $PSScriptRoot\..\xtr
dotnet pack -c Release -o $PSScriptRoot
Pop-Location
