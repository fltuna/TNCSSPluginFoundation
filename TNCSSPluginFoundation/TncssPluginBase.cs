using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Translations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TNCSSPluginFoundation.Configuration;
using TNCSSPluginFoundation.Interfaces;
using TNCSSPluginFoundation.Models.Logger;
using TNCSSPluginFoundation.Models.Plugin;

namespace TNCSSPluginFoundation;

/// <summary>
/// Main class of TNCSSPluginFoundation
/// </summary>
public abstract class TncssPluginBase: BasePlugin, ITncssPluginBase
{
    
    /// <summary>
    /// ConVarConfigurationService for managing ConVar config.
    /// </summary>
    public ConVarConfigurationService ConVarConfigurationService { get; private set; } = null!;
    
    /// <summary>
    /// DebugLogger instance
    /// </summary>
    public IDebugLogger? DebugLogger { get; protected set; }
    
    /// <summary>
    /// Base config directory path, currently do nothing.
    /// </summary>
    public abstract string BaseCfgDirectoryPath { get; }
    
    /// <summary>
    /// ConVar configuration path, this path is used for saving All ConVar config.
    /// Relative path from game/csgo/cfg/
    /// Also if this path defined a specific file, then module config is not generated.
    /// </summary>
    public abstract string ConVarConfigPath { get; }

    private ServiceCollection ServiceCollection { get; } = new();
    
    /// <summary>
    /// DI Container
    /// </summary>
    private ServiceProvider ServiceProvider { get; set; } = null!;
    
    /// <summary>
    /// This prefix used for printing to chat.
    /// </summary>
    protected abstract string PluginPrefix { get; }


    /// <summary>
    /// We can register required services that use entire plugin or modules.
    /// At this time, we can get ConVarConfigurationService and AbstractTncssPluginBase instance from DI container in this method.
    /// </summary>
    protected virtual void RegisterRequiredPluginServices(IServiceCollection collection ,IServiceProvider provider){}
    
    /// <summary>
    /// You can register any services. for instance: external feature obtained from CSSharp's PluginCapability.<br/>
    /// This is a final chance of registering services to DI container.
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="provider"></param>
    protected virtual void LateRegisterPluginServices(IServiceCollection collection, IServiceProvider provider){}

    private void UpdateServices()
    {
        foreach (PluginModuleBase module in _loadedModules)
        {
            module.UpdateServices(ServiceProvider);
        }
    }

    /// <summary>
    /// CounterStrikeSharp's plugin entry point.
    /// </summary>
    /// <param name="hotReload"></param>
    public sealed override void Load(bool hotReload)
    {
        ConVarConfigurationService = new(this);
        // Add self and core service to DI Container
        ServiceCollection.AddSingleton(this);
        ServiceCollection.AddSingleton(ConVarConfigurationService);
        
        // Build first ServiceProvider, because we need a plugin instance to initialize modules
        RebuildServiceProvider();
        
        // Then call register required plugin services
        RegisterRequiredPluginServices(ServiceCollection, ServiceProvider);

        DebugLogger ??= new IgnoredLogger();

        RegisterDebugLogger(DebugLogger);
        
        // And build again
        RebuildServiceProvider();
        
        // Call customizable OnLoad method
        TncssOnPluginLoad(hotReload);
        TncssLateOnPluginLoad(ServiceProvider);
    }

    /// <summary>
    /// This method is can be used to initialize plugin feature.
    /// </summary>
    /// <param name="hotReload">Is hot reload?</param>
    protected virtual void TncssOnPluginLoad(bool hotReload){}

    /// <summary>
    /// You can register plugin capability here.
    /// All modules registered DI dependency is accessible in here.
    /// </summary>
    protected virtual void TncssLateOnPluginLoad(ServiceProvider provider){}

    /// <summary>
    /// CounterStrikeSharp's OnAllPluginsLoaded.
    /// </summary>
    /// <param name="hotReload"></param>
    public sealed override void OnAllPluginsLoaded(bool hotReload)
    {
        LateRegisterPluginServices(ServiceCollection, ServiceProvider);
        RebuildServiceProvider();
        UpdateServices();
        
        TncssAllPluginsLoaded(hotReload);
        CallModulesAllPluginsLoaded();
        ConVarConfigurationService.SaveAllConfigToFile();
        ConVarConfigurationService.ExecuteConfigs();
    }
    

    /// <summary>
    /// This method is can be used to late initialize plugin feature.
    /// </summary>
    /// <param name="hotReload">Is hot reload?</param>
    protected virtual void TncssAllPluginsLoaded(bool hotReload){}

    

    /// <summary>
    /// CounterStrikeSharp's UnLoad.
    /// </summary>
    /// <param name="hotReload"></param>
    public sealed override void Unload(bool hotReload)
    {
        TncssOnPluginUnload(hotReload);
        UnloadAllModules();
    }
    

    /// <summary>
    /// This method is can be used to unload plugin feature.
    /// </summary>
    /// <param name="hotReload">Is hot reload?</param>
    protected virtual void TncssOnPluginUnload(bool hotReload){}


    private void RebuildServiceProvider()
    {
        ServiceProvider = ServiceCollection.BuildServiceProvider();
    }
    
    /// <summary>
    /// Localize string with plugin prefix.
    /// </summary>
    /// <param name="languageKey">Language Key</param>
    /// <param name="args">Any args that can be use ToString()</param>
    /// <returns>"{PluginPrefix} {LocalizedString}"</returns>
    public string LocalizeStringWithPluginPrefix(string languageKey, params object[] args)
    {
        return $"{PluginPrefix} {LocalizeString(languageKey, args)}";
    }

    /// <summary>
    /// Same as Plugin.Localizer[langaugeKey, args]
    /// </summary>
    /// <param name="localizationKey">Localization Key</param>
    /// <param name="args">Any args that can be use ToString()</param>
    /// <returns>Translated result</returns>
    public string LocalizeString(string localizationKey, params object[] args)
    {
        return Localizer[localizationKey, args];
    }
    
    /// <summary>
    /// Same as Plugin.Localizer.ForPlayer(player, localizationKey, args)
    /// </summary>
    /// <param name="player">Player instance</param>
    /// <param name="localizationKey">Localization Key</param>
    /// <param name="args">Any args that can be use ToString()</param>
    /// <returns>Translated result as player's language</returns>
    public string LocalizeStringForPlayer(CCSPlayerController player, string localizationKey, params object[] args)
    {
        return Localizer.ForPlayer(player, localizationKey, args);
    }
    

    private readonly HashSet<PluginModuleBase> _loadedModules = [];

    /// <summary>
    /// Register module without hot reload state.
    /// </summary>
    /// <typeparam name="T">Any classes that inherited a PluginModuleBase</typeparam>
    protected void RegisterModule<T>() where T : PluginModuleBase
    {
        var module = (T)Activator.CreateInstance(typeof(T), ServiceProvider)!;
        _loadedModules.Add(module);
        module.Initialize();
        module.RegisterServices(ServiceCollection);
        RegisterFakeConVars(module.GetType(), module);
        // Rebuild, because some modules are depend on other modules.
        // And if the module is API or something, required before call OnAllPluginsLoaded.
        RebuildServiceProvider();
        Logger.LogInformation($"{module.PluginModuleName} has been initialized");
    }

    /// <summary>
    /// Register module with hot reload state.
    /// </summary>
    /// <typeparam name="T">Any classes that inherited a PluginModuleBase</typeparam>
    protected void RegisterModule<T>(bool hotReload) where T : PluginModuleBase
    {
        var module = (T)Activator.CreateInstance(typeof(T), ServiceProvider, hotReload)!;
        _loadedModules.Add(module);
        module.Initialize();
        module.RegisterServices(ServiceCollection);
        RegisterFakeConVars(module.GetType(), module);
        // Rebuild, because some modules are depend on other modules.
        // And if the module is API or something, required before call OnAllPluginsLoaded.
        RebuildServiceProvider();
        Logger.LogInformation($"{module.PluginModuleName} has been initialized");
    }

    private void CallModulesAllPluginsLoaded()
    {
        foreach (IPluginModule loadedModule in _loadedModules)
        {
            loadedModule.AllPluginsLoaded();
        }
    }


    private void UnloadAllModules()
    {
        foreach (PluginModuleBase loadedModule in _loadedModules)
        {
            loadedModule.UnloadModule();
            Logger.LogInformation($"{loadedModule.PluginModuleName} has been unloaded.");
        }
        _loadedModules.Clear();
    }

    private void RegisterDebugLogger(IDebugLogger logger)
    {
        RegisterFakeConVars(logger.GetType(), logger);
        ServiceCollection.AddSingleton(logger);
    }
}