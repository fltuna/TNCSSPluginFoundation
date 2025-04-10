using TNCSSPluginFoundation.Configuration;
using TNCSSPluginFoundation.Models.Logger;

namespace TNCSSPluginFoundation.Interfaces;

/// <summary>
/// Required basic feature of TNCSSPlugin base.
/// </summary>
internal interface ITncssPluginBase
{
    /// <summary>
    /// ConVarConfigurationService for managing ConVar config.
    /// </summary>
    public ConVarConfigurationService ConVarConfigurationService { get; }
    
    /// <summary>
    /// DebugLogger instance
    /// </summary>
    public IDebugLogger? DebugLogger { get; }
    
    /// <summary>
    /// Base config directory path
    /// </summary>
    public string BaseCfgDirectoryPath { get; }
    
    /// <summary>
    /// ConVar configuration path.
    /// </summary>
    public string ConVarConfigPath { get; }
    
    /// <summary>
    /// Localize language key and format args with plugin prefix
    /// </summary>
    /// <param name="languageKey">string key</param>
    /// <param name="args">Any args that supports ToString()</param>
    /// <returns>Formatted message</returns>
    public string LocalizeStringWithPluginPrefix(string languageKey, params object[] args);
    
    /// <summary>
    /// Simply localize string, same as AnyPlugin.Localizer[languageKey, args]
    /// </summary>
    /// <param name="languageKey">string key</param>
    /// <param name="args">Any args that supports ToString()</param>
    /// <returns>Translated message</returns>
    public string LocalizeString(string languageKey, params object[] args);
}