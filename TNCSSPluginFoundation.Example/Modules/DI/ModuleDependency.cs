using CounterStrikeSharp.API;
using Microsoft.Extensions.DependencyInjection;
using TNCSSPluginFoundation.Example.Interfaces;
using TNCSSPluginFoundation.Models.Plugin;

namespace TNCSSPluginFoundation.Example.Modules.DI;

public sealed class ModuleDependency(IServiceProvider serviceProvider) : PluginModuleBase(serviceProvider), IModuleDependencyExample
{
    public override string PluginModuleName => "ModuleDependency";
    public override string ModuleChatPrefix => "[ModuleDependency]";


    // You can add self instance to the DI container. (recommend to rely on interfaces not concrete class)
    public override void RegisterServices(IServiceCollection services)
    {
        services.AddSingleton<IModuleDependencyExample>(this);
    }


    public void TestPrintModule()
    {
        Server.PrintToConsole($"[{PluginModuleName}] Hello from {PluginModuleName}!");
        Server.PrintToChatAll($"[{PluginModuleName}] Hello from {PluginModuleName}!");
    }
}