using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Translations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TNCSSPluginFoundation.Interfaces;
using TNCSSPluginFoundation.Models.Command;

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
    /// <param name="player">Player instance, If null it will use server language</param>
    /// <param name="localizationKey">Localization Key</param>
    /// <param name="args">Any args that can be use ToString()</param>
    /// <returns>Translated result</returns>
    protected string LocalizeString(CCSPlayerController? player, string localizationKey, params object[] args)
    {
        if (player == null)
            return Plugin.LocalizeString(localizationKey, args);

        return Plugin.LocalizeStringForPlayer(player, localizationKey, args);
    }

    /// <summary>
    /// Register command that inherited TncssAbstractCommandBase
    /// </summary>
    /// <typeparam name="T"></typeparam>
    protected void RegisterTncssCommand<T>() where T : TncssAbstractCommandBase
    {
        var module = (T)Activator.CreateInstance(typeof(T), ServiceProvider)!;
        Plugin.AddTncssCommand(module);
    }
}