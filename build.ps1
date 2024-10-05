$baseDir = Resolve-Path .\
$buildDir = "$baseDir\build"
$sourceDir = "$baseDir\src"
$projectName = "Buckler.NET"
$unitTestPath = "$sourceDir\Buckler.NET.Tests"
$projectPath = "$sourceDir\Buckler.NET"
$dotnetCliVerbosity = "m"

function Init {
    Remove-Item $buildDir -Recurse -Force -ErrorAction Ignore
    New-Item -Path $buildDir -ItemType Directory > $null 

    & dotnet clean "$sourceDir\$projectName.sln" -nologo -v $dotnetCliVerbosity
    & dotnet restore "$sourceDir\$projectName.sln" -nologo --interactive -v $dotnetCliVerbosity
}

function Compile {
    & dotnet build "$sourceDir\$projectName.sln" -nologo --no-restore -v $dotnetCliVerbosity --no-incremental
}

function PrivateBuild {
    Init
    Compile
}