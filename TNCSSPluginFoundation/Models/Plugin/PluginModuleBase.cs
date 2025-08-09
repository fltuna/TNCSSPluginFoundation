using System.Globalization;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Translations;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API.Modules.Entities;
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
    /// Helper method for sending localized text to all players.
    /// </summary>
    /// <param name="localizationKey">Language localization key</param>
    /// <param name="args">Any args that can be use ToString()</param>
    protected void PrintLocalizedChatToAll(string localizationKey, params object[] args)
    {
        foreach (CCSPlayerController client in Utilities.GetPlayers())
        {
            if (client.IsBot || client.IsHLTV)
                continue;
            
            client.PrintToChat(GetTextWithPluginPrefix(client, LocalizeString(client, localizationKey, args)));
        }
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
    /// Prints message to server or player's chat
    /// </summary>
    /// <param name="player">Player Instance. if null message will print to server console</param>
    /// <param name="message">Message text</param>
    protected void PrintMessageToServerOrPlayerChat(CCSPlayerController? player, string message)
    {
        if (player == null)
            Server.PrintToConsole(message);
        else
            player.PrintToChat(message);
    }
    

    /// <summary>
    /// Helper method for obtain the localized text.
    /// </summary>
    /// <param name="player">Player instance, If null it will use server language</param>
    /// <param name="localizationKey">Language localization key</param>
    /// <param name="args">Any args that can be use ToString()</param>
    /// <returns></returns>
    protected string LocalizeWithPluginPrefix(CCSPlayerController? player, string localizationKey, params object[] args)
    {
        return GetTextWithPluginPrefix(player, LocalizeString(player, localizationKey, args));
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
    /// Get text with plugin prefix.
    /// </summary>
    /// <param name="player">Player instance, If null it will use server language</param>
    /// <param name="text">original text</param>
    /// <returns>Text combined with original text and prefix, returns translated plugin prefix if Plugin.UseTranslationKeyInPluginPrefix is true</returns>
    protected string GetTextWithPluginPrefix(CCSPlayerController? player, string text)
    {
        if (!Plugin.UseTranslationKeyInPluginPrefix)
            return $"{Plugin.PluginPrefix} {text}";
        
        return $"{LocalizeString(player, Plugin.PluginPrefix)} {text}";
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