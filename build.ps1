$baseDir = Resolve-Path .\
$buildDir = "$baseDir\build"
$sourceDir = "$baseDir\src"
$projectName = "Buckler.NET"
$unitTestPath = "$sourceDir\$projectName.Tests"
$projectPath = "$sourceDir\$projectName"
$dotnetCliVerbosity = "m"
$buildConfiguration

function Init {
    Write-Output "Creating build directory"
    Remove-Item $buildDir -Recurse -Force -ErrorAction Ignore
    New-Item -Path $buildDir -ItemType Directory > $null 

    Write-Output "Clean & restore solution"
    & dotnet clean "$sourceDir\$projectName.sln" -nologo -v $dotnetCliVerbosity
    & dotnet restore "$sourceDir\$projectName.sln" -nologo --interactive -v $dotnetCliVerbosity
}

function Compile {
    & dotnet build "$sourceDir\$projectName.sln" -nologo --no-restore -v $dotnetCliVerbosity --no-incremental -c $buildConfiguration
}

function UnitTests {
    & dotnet test "$unitTestPath\$projectName.Tests.csproj" -nologo -v $dotnetCliVerbosity --no-build --no-restore -c $buildConfiguration
}

function CopyOutput {
    $outputPath = "$projectPath\bin\$buildConfiguration\net8.0"
    Copy-Item "$outputPath\*" -Destination "$buildDir" -Recurse
}

function PrivateBuild {
    $buildConfiguration = "Debug"
    Init
    Compile
    UnitTests
    CopyOutput
}