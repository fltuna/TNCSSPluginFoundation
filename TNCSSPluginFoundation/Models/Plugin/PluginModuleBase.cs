using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Cvars;
using Microsoft.Extensions.DependencyInjection;
using TNCSSPluginFoundation.Configuration;
using TNCSSPluginFoundation.Interfaces;

namespace TNCSSPluginFoundation.Models.Plugin;

/// <summary>
/// This is a base class for all plugin modules. you can make custom module class from this.
/// </summary>
/// <param name="serviceProvider">Microsoft.Extensions.DependencyInjection</param>
public abstract class PluginModuleBase(IServiceProvider serviceProvider) : PluginBasicFeatureBase(serviceProvider), IPluginModule
{
    /// <summary>
    /// This string is used to saving config and internal management.<br/>
    /// When we specify HelloModule as name, then ConVar config name become HelloModule.cfg
    /// </summary>
    public abstract string PluginModuleName { get; }
    
    /// <summary>
    /// This string is used as a prefix for printing to the in-game chat.
    /// </summary>
    public abstract string ModuleChatPrefix { get; } 

    /// <summary>
    /// ConVarConfigurationService
    /// </summary>
    private ConVarConfigurationService ConVarConfigurationService => Plugin.ConVarConfigurationService;
    
    
    

    // public FakeConVar<float> ConVarVariableName = new(
    //     "convar_name",
    //     "Description",
    //     DefaultValue,
    //     ConVarFlags.FCVAR_NONE,
    //     new RangeValidator<float>(0.0F, float.MaxValue));
    //
    // You can register ConVar in Module like above.
    //
    // If you want to add this ConVar to ConVar config file automatic generation
    // You need to call TrackConVar() method in OnInitialize()
    //
    //
    
    
    
    /// <summary>
    /// This method will call while registering module, and module registration is called in plugins Load method.
    /// Also, we can get ConVarConfigurationService and AbstractTunaPluginBase from DI container from this method.
    /// </summary>
    /// <param name="services">ServiceCollection</param>
    public virtual void RegisterServices(IServiceCollection services) {}

    /// <summary>
    /// This method will call while BasePlugin's OnAllPluginsLoaded.
    /// This serviceProvider should contain latest and all module dependency.
    /// </summary>
    /// <param name="services">Latest DI container</param>
    public virtual void UpdateServices(IServiceProvider services)
    {
        ServiceProvider = services;
    }

    /// <summary>
    /// Module initialization method (Internal)
    /// </summary>
    public void Initialize()
    {
        OnInitialize();
    }

    /// <summary>
    /// This method will call while registering module, and module registration is called from plugin's Load method.
    /// Also, this time you can call TrackConVar() method to add specific ConVar to ConVar config file automatic generation.
    /// </summary>
    protected virtual void OnInitialize()
    {
        // For instance
        // TrackConVar(ConVarVariableName);
    }


    /// <summary>
    /// Called when all plugins loaded (Internal)
    /// </summary>
    public void AllPluginsLoaded()
    {
        OnAllPluginsLoaded();
    }
    
    /// <summary>
    /// Called when all plugins loaded, so you can late initialize your module.<br/>
    /// For instance, obtaining the plugin capability, or registered self services.
    /// </summary>
    protected virtual void OnAllPluginsLoaded()
    {
        
    }

    /// <summary>
    /// Called when unloading module (Internal)
    /// </summary>
    public void UnloadModule()
    {
        OnUnloadModule();
        ConVarConfigurationService.UntrackModule(PluginModuleName);
    }

    
    /// <summary>
    /// Called when unloading module
    /// </summary>
    protected virtual void OnUnloadModule(){}
    
    
    /// <summary>
    /// Helper method for sending localized text to all players.
    /// </summary>
    /// <param name="localizationKey">Language localization key</param>
    /// <param name="args">Any args that can be use ToString()</param>
    protected void PrintLocalizedChatToAll(string localizationKey, params object[] args)
    {
        Server.PrintToChatAll(LocalizeWithPluginPrefix(localizationKey, args));
    }

    /// <summary>
    /// Helper method for obtain the localized text.
    /// </summary>
    /// <param name="localizationKey">Language localization key</param>
    /// <param name="args">Any args that can be use ToString()</param>
    /// <returns></returns>
    protected string LocalizeWithPluginPrefix(string localizationKey, params object[] args)
    {
        return Plugin.LocalizeStringWithPluginPrefix(localizationKey, args);
    }

    /// <summary>
    /// Helper method for obtain the localized text.
    /// </summary>
    /// <param name="localizationKey">Language localization key</param>
    /// <param name="args">Any args that can be use ToString()</param>
    /// <returns></returns>
    protected string LocalizeWithModulePrefix(string localizationKey, params object[] args)
    {
        return $"{ModuleChatPrefix} {LocalizeString(localizationKey, args)}";
    }


    /// <summary>
    /// Add ConVar to tracking list. if you want to generate config automatically, then call this method with ConVar that you wanted to track.
    /// </summary>
    /// <param name="conVar">Any FakeConVar</param>
    /// <typeparam name="T">FakeConVarType</typeparam>
    protected void TrackConVar<T>(FakeConVar<T> conVar) where T : IComparable<T>
    {
        ConVarConfigurationService.TrackConVar(PluginModuleName ,conVar);
    }


    /// <summary>
    /// Removes all module ConVar from tracking list.
    /// </summary>
    protected void UnTrackConVar()
    {
        ConVarConfigurationService.UntrackModule(PluginModuleName);
    }
}