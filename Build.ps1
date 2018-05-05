Push-Location $PSScriptRoot

# prerequisites for build tasks and testing libraries
NuGet restore packages.config -PackagesDirectory .\packages

MSBuild Build.proj

Pop-Location