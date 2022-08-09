using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace API.Starter.Tests.Fixtures;

/// <summary>
/// A fixture for setting the environment variables from LaunchSettings.profiles.Tests during each test and removing
/// them afterwards
/// </summary>
public class LaunchSettingsFixture : IDisposable
{
    private readonly IDictionary<string, string> _environmentVariableByName;

    public LaunchSettingsFixture()
    {
        using var file = File.OpenText(
            Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)!,
                "Properties",
                "launchSettings.json"));
        var reader = new JsonTextReader(file);
        var jObject = JObject.Load(reader);

        _environmentVariableByName = jObject["profiles"]?["Tests"]?["environmentVariables"]
            ?.Children<JProperty>()
            .ToDictionary(p => p.Name, p => p.Value.ToString())!;

        foreach (var variable in _environmentVariableByName)
        {
            Environment.SetEnvironmentVariable(variable.Key, variable.Value);
        }
    }

    public void Dispose()
    {
        foreach (var variable in _environmentVariableByName)
        {
            Environment.SetEnvironmentVariable(variable.Key, null);
        }
    }
}