using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Cvars;
using Microsoft.Extensions.DependencyInjection;
using TNCSSPluginFoundation.Configuration;
using TNCSSPluginFoundation.Interfaces;
using TNCSSPluginFoundation.Models.Command;

namespace TNCSSPluginFoundation.Models.Plugin;

/// <summary>
/// This is a base class for all plugin modules. you can make custom module class from this.
/// </summary>
/// <param name="serviceProvider">Microsoft.Extensions.DependencyInjection</param>
public abstract class PluginModuleBase(IServiceProvider serviceProvider) : PluginTranslatableFeatureBase(serviceProvider), IPluginModule
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
    /// Is ModuleChatPrefix is translation key?
    /// </summary>
    protected abstract bool UseTranslationKeyInModuleChatPrefix { get; }


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
    /// Register command that inherited TncssAbstractCommandBase
    /// </summary>
    /// <typeparam name="T"></typeparam>
    protected void RegisterTncssCommand<T>() where T : TncssAbstractCommandBase
    {
        var module = (T)Activator.CreateInstance(typeof(T), ServiceProvider)!;
        Plugin.AddTncssCommand(module);
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


    /// <summary>
    /// Helper method for sending localized text with module prefix to all players.
    /// </summary>
    /// <param name="localizationKey">Language localization key</param>
    /// <param name="args">Any args that can be use ToString()</param>
    protected void PrintLocalizedChatToAllWithModulePrefix(string localizationKey, params object[] args)
    {
        foreach (CCSPlayerController client in Utilities.GetPlayers())
        {
            if (client.IsBot || client.IsHLTV)
                continue;

            client.PrintToChat(GetTextWithModulePrefix(client, LocalizeString(client, localizationKey, args)));
        }
    }
    

    /// <summary>
    /// Helper method for obtain the localized text.
    /// </summary>
    /// <param name="player">Player instance, If null it will use server language</param>
    /// <param name="localizationKey">Language localization key</param>
    /// <param name="args">Any args that can be use ToString()</param>
    /// <returns></returns>
    protected string LocalizeWithModulePrefix(CCSPlayerController? player, string localizationKey, params object[] args)
    {
        return GetTextWithModulePrefix(player, LocalizeString(player, localizationKey, args));
    }
    
    /// <summary>
    /// Get text with module prefix.
    /// </summary>
    /// <param name="player">Player Instance</param>
    /// <param name="text">original text</param>
    /// <returns>Text combined with original text and prefix, returns translated module prefix if UseTranslationKeyInModuleChatPrefix is true</returns>
    protected string GetTextWithModulePrefix(CCSPlayerController? player, string text)
    {
        if (!UseTranslationKeyInModuleChatPrefix)
            return $"{ModuleChatPrefix} {text}";

        return $"{LocalizeString(player, ModuleChatPrefix)} {text}";
    }
}