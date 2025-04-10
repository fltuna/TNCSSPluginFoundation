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
    /// <param name="localizationKey">languageKey</param>
    /// <param name="args">Any params that can call ToString() method</param>
    /// <returns></returns>
    protected string LocalizeString(string localizationKey, params object[] args)
    {
        return Plugin.LocalizeString(localizationKey, args);
    }
}