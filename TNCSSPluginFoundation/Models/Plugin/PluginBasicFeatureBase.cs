using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Translations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TNCSSPluginFoundation.Interfaces;

namespace TNCSSPluginFoundation.Models.Plugin;

/// <summary>
/// Provides basic plugin feature. you can also make custom module class from this.
/// </summary>
/// <param name="serviceProvider">Microsoft.Extensions.DependencyInjection</param>
public abstract class PluginBasicFeatureBase(IServiceProvider serviceProvider)
{
    
    /// <summary>
    /// Main plugin instance, for registering the commands, listeners, etc...
    /// </summary>
    protected readonly TncssPluginBase Plugin = serviceProvider.GetRequiredService<TncssPluginBase>();
    
    
    /// <summary>
    /// Logger from main plugin instance.
    /// </summary>
    protected readonly ILogger Logger = serviceProvider.GetRequiredService<TncssPluginBase>().Logger;
    
    /// <summary>
    /// Custom debug logger for simple logging. If you didn't make custom implementation of IDebugLogger, it will do nothing.
    /// </summary>
    protected readonly IDebugLogger DebugLogger = serviceProvider.GetRequiredService<IDebugLogger>();
    
    /// <summary>
    /// DI container, you can get any service that you registered in main class from this.
    /// </summary>
    protected IServiceProvider ServiceProvider { get; set; } = serviceProvider;

    /// <summary>
    /// Simple wrapper method for AbstractTncssPluginBase::LocalizeString()
    /// </summary>
    /// <param name="localizationKey">Localization Key</param>
    /// <param name="args">Any args that can be use ToString()</param>
    /// <returns>Translated result</returns>
    protected string LocalizeString(string localizationKey, params object[] args)
    {
        return Plugin.LocalizeString(localizationKey, args);
    }

    /// <summary>
    /// Simple wrapper method for AbstractTncssPluginBase::LocalizeStringForPlayer()
    /// </summary>
    /// <param name="player">Player instance</param>
    /// <param name="localizationKey">Localization Key</param>
    /// <param name="args">Any args that can be use ToString()</param>
    /// <returns>Translated result as player's language</returns>
    protected string LocalizeStringForPlayer(CCSPlayerController player, string localizationKey, params object[] args)
    {
        return Plugin.LocalizeStringForPlayer(player, localizationKey, args);
    }
}