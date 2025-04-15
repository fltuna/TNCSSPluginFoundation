using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API.Modules.Cvars.Validators;
using Microsoft.Extensions.DependencyInjection;
using TNCSSPluginFoundation.Models.Plugin;

namespace TNCSSPluginFoundation.Example;

public sealed class ModuleClassTemplate(IServiceProvider serviceProvider) : PluginModuleBase(serviceProvider)
{
    // This name used to tracking ConVar and should be unique.  Do not duplicate with any other module names in this plugin modules. 
    public override string PluginModuleName => "ClassTemplate";

    // This is a chat prefix. when you use PluginModuleBase::LocalizeWithModulePrefix() and PluginModuleBase::PrintLocalizedChatToAllWithModulePrefix(), it will return translated string with this prefix.
    // For instance: `[TNCSSExample] This is a translated message!`
    // But you can still send translated message with plugin prefix using PluginModuleBase::PrintLocalizedChatToAll() or PluginModuleBase::LocalizeWithPluginPrefix()
    public override string ModuleChatPrefix => "[ClassTemplate]";

    public FakeConVar<float> ConVarVariableName = new(
        "convar_name",
        "Description",
        0.0F,
        ConVarFlags.FCVAR_NONE,
        new RangeValidator<float>(0.0F, float.MaxValue));
    //
    // You can register ConVar in Module.
    //
    // If you want to add this ConVar to ConVar config file automatic generation,
    // You need to call TrackConVar() method in OnInitialize()
    //
    //
    
    
    
    /// <summary>
    /// This method will call while registering module, and module registration is called in plugins Load method.
    /// Also, we can get ConVarConfigurationService and AbstractTunaPluginBase from DI container from this method.
    /// </summary>
    /// <param name="services">ServiceCollection</param>
    public override void RegisterServices(IServiceCollection services)
    {
    }

    
    /// <summary>
    /// This method will call while registering module, and module registration is called from plugin's Load method.
    /// Also, this time you can call TrackConVar() method to add specific ConVar to ConVar config file automatic generation.
    /// </summary>
    protected override void OnInitialize()
    {
        // For instance
        // TrackConVar(ConVarVariableName);
    }

    /// <summary>
    /// This method will call while BasePlugin's OnAllPluginsLoaded.
    /// This serviceProvider should contain latest and all module dependency.
    /// </summary>
    /// <param name="services">Latest DI container</param>
    public override void UpdateServices(IServiceProvider services)
    {
    }
    
    /// <summary>
    /// This method will call in end of PluginModuleBase::AllPluginsLoaded()
    /// Also, All dependencies should be available in this time.
    /// </summary>
    protected override void OnAllPluginsLoaded()
    {
    }

    /// <summary>
    /// This method will call in end of PluginModuleBase::UnloadModule()
    /// You don't have to manually untrack the ConVar.
    /// Just de-register things that you have registered in OnInitialize()
    /// </summary>
    protected override void OnUnloadModule()
    {
    }
}