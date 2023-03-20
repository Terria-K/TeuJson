#tool nuget:?package=NuGet.CommandLine&version=5.9.1

const string version = "2.0.0";
var target = Argument("target", "Build");
var configuration = Argument("configuration", "Release");
var outputFolder = "./artifacts";
var mainKey = EnvironmentVariable<string>("MAIN_KEY", "Empty Key");
var generatorKey = EnvironmentVariable<string>("GENERATOR_KEY", "Empty Key");

void BuildProject(string project) 
{
    DotNetBuild(project, new DotNetBuildSettings 
    {
        NoRestore = true,
        Configuration = configuration
    });
}

void RestoreProject(string project) 
{
    DotNetRestore(project);
}

Task("Clean")
    .Does(() => 
    {
        CleanDirectory(outputFolder);
    });

Task("RestoreGenerator")
    .Does(() => 
    {
        RestoreProject("TeuJson.Generator");
    });

Task("Restore")
    .Does(() => 
    {
        RestoreProject("TeuJson");
    });

Task("BuildGenerator")
    .IsDependentOn("RestoreGenerator")
    .Does(() => 
    {
        BuildProject("TeuJson.Generator");
    });

Task("Build")
    .IsDependentOn("Restore")
    .Does(() => 
    {
        BuildProject("TeuJson");
    });

Task("RestoreTest")
    .Does(() => 
    {
        RestoreProject("TeuJson.Tests");
    });

Task("BuildTest")
    .IsDependentOn("RestoreTest")
    .Does(() => 
    {
        BuildProject("TeuJson.Tests");
    });

Task("Test")
    .IsDependentOn("BuildTest")
    .Does(() => 
    {
        DotNetTest("TeuJson.Tests/TeuJson.Tests.csproj", new DotNetTestSettings 
        {
            NoRestore = true,
            Configuration = configuration,
            NoBuild = true
        });
    });

Task("Package")
    .IsDependentOn("Test")
    .Does(() => 
    {
        DotNetPack("TeuJson/TeuJson.csproj", new DotNetPackSettings 
        {
            NoRestore = true,
            Configuration = configuration,
            NoBuild = true
        });

        DotNetPack("TeuJson.Generator/TeuJson.Generator.csproj", new DotNetPackSettings 
        {
            NoRestore = true,
            Configuration = configuration,
            NoBuild = true
        });
    });

Task("Push")
    .IsDependentOn("Clean")
    .IsDependentOn("Package")
    .Does(() => 
    {
        var mainPackage = $"TeuJson.{version}.nupkg";
        NuGetPush(mainPackage, new NuGetPushSettings {
            Source = "https://api.nuget.org/v3/index.json",
            ApiKey = mainKey
        });
        var generatorPackage = $"TeuJson.Generator.{version}.nupkg";
        NuGetPush(generatorPackage, new NuGetPushSettings {
            Source = "https://api.nuget.org/v3/index.json",
            ApiKey = generatorKey
        });
    });

RunTarget(target);