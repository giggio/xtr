. $PSScriptRoot\pack.ps1
if ($LASTEXITCODE -ne 0) { exit }
if ((dotnet tool list -g | Where-Object { $_.Contains('xtr') })) {
    dotnet tool uninstall -g xtr
}
dotnet tool install -g xtr
