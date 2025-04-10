using CounterStrikeSharp.API.Modules.Cvars;
using Microsoft.Extensions.Logging;

namespace TNCSSPluginFoundation.Configuration;

/// <summary>
/// Manages plugin ConVar configuration file and execution.
/// </summary>
/// <param name="plugin">Instance of <see cref="TncssPluginBase"/></param>
public class ConVarConfigurationService(TncssPluginBase plugin)
{
    private readonly Dictionary<string, List<object>> _moduleConVars = new();
    
    /// <summary>
    /// Add ConVar to the list of ConVars to be saved in the config file.
    /// </summary>
    /// <param name="moduleName">ModuleName should be unique, otherwise overriden to new one</param>
    /// <param name="conVar">FakeConVar</param>
    /// <typeparam name="T">Any type that FakeConVar supports</typeparam>
    public void TrackConVar<T>(string moduleName, FakeConVar<T> conVar) where T : IComparable<T>
    {
        if (!_moduleConVars.TryGetValue(moduleName, out var list))
        {
            list = new List<object>();
            _moduleConVars[moduleName] = list;
        }
        
        list.Add(conVar);
    }
    
    /// <summary>
    /// Save module config to file.
    /// </summary>
    /// <param name="moduleName">If there is no registered ConVar with module, it will be ignored</param>
    public void SaveModuleConfigToFile(string moduleName)
    {
        string moduleConfigPath = Path.Combine(plugin.ConVarConfigPath, moduleName + ".cfg");
        
        if(IsFileExists(moduleConfigPath))
            return;
        
        
        if (!_moduleConVars.TryGetValue(moduleName, out var list))
            return;

        using (StreamWriter writer = new StreamWriter(moduleConfigPath))
        {
            foreach (var conVarObj in list)
            {
                dynamic conVar = conVarObj;
                writer.WriteLine($"// {conVar.Description}");
                
                // If value is boolean, then convert it to 0|1
                if (conVarObj.GetType().GenericTypeArguments[0] == typeof(bool))
                {
                    bool value = conVar.Value;
                    writer.WriteLine($"{conVar.Name} {Convert.ToInt32(value)}");
                }
                else
                {
                    writer.WriteLine($"{conVar.Name} {conVar.Value}");
                }
                
                writer.WriteLine();
            }
        }
    }
    
    /// <summary>
    /// Save all module ConVar config to one file.
    /// </summary>
    public void SaveAllConfigToFile()
    {
        if(IsFileExists(plugin.ConVarConfigPath))
            return;

        using (StreamWriter writer = new StreamWriter(plugin.ConVarConfigPath))
        {
            foreach (var moduleName in _moduleConVars.Keys)
            {
                writer.WriteLine($"// ===== {moduleName} =====");
                writer.WriteLine();
                
                foreach (var conVarObj in _moduleConVars[moduleName])
                {
                    dynamic conVar = conVarObj;
                    writer.WriteLine($"// {conVar.Description}");
                    
                    if (conVarObj.GetType().GenericTypeArguments[0] == typeof(bool))
                    {
                        bool value = conVar.Value;
                        writer.WriteLine($"{conVar.Name} {Convert.ToInt32(value)}");
                    }
                    else
                    {
                        writer.WriteLine($"{conVar.Name} {conVar.Value}");
                    }
                    
                    writer.WriteLine();
                }
                
                writer.WriteLine();
            }
        }
    }

    private bool IsFileExists(string path)
    {
        string directory = Path.GetDirectoryName(path)!;
        if (!Directory.Exists(directory))
        {
            plugin.Logger.LogInformation($"Failed to find the config folder. Trying to generate...");
                
            Directory.CreateDirectory(directory);

            if (!Directory.Exists(directory))
            {
                plugin.Logger.LogError($"Failed to generate the Config folder! cancelling the config generation!");
                return false;
            }
        }

        return File.Exists(path);
    }
    
    /// <summary>
    /// Remove ConVar from the list of ConVars to be saved in the config file.
    /// </summary>
    /// <param name="moduleName"></param>
    public void UntrackModule(string moduleName)
    {
        _moduleConVars.Remove(moduleName);
    }
}