using CounterStrikeSharp.API;
using TNCSSPluginFoundation.Example.Interfaces;

namespace TNCSSPluginFoundation.Example.Dependency;

public class PluginDependencyExample: IPluginDependencyExample
{
    public string GetText()
    {
        return "Hello from PluginRequiredDependencyExample!";
    }
}