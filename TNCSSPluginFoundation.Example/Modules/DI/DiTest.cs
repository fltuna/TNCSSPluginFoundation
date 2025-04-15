using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using Microsoft.Extensions.DependencyInjection;
using TNCSSPluginFoundation.Example.Interfaces;
using TNCSSPluginFoundation.Models.Plugin;

namespace TNCSSPluginFoundation.Example.Modules.DI;

public class DiTest(IServiceProvider serviceProvider) : PluginModuleBase(serviceProvider)
{
    public override string PluginModuleName => "DI Test";
    public override string ModuleChatPrefix => "[DI Test]";
    
    
    private IPluginDependencyExample _dependencyExample = null!;
    private IModuleDependencyExample _moduleDependencyExample = null!;
    

    protected override void OnInitialize()
    {
        Plugin.AddCommand("tncss_di", "DI container test command", CommandPrintDependency);
    }

    // You can only get dependency from this method.
    // Otherwise, dependency is not registered.
    protected override void OnAllPluginsLoaded()
    {
        _dependencyExample = ServiceProvider.GetRequiredService<IPluginDependencyExample>();
        _moduleDependencyExample = ServiceProvider.GetRequiredService<IModuleDependencyExample>();
    }

    protected override void OnUnloadModule()
    {
        Plugin.RemoveCommand("tncss_di", CommandPrintDependency);
    }


    private void CommandPrintDependency(CCSPlayerController? player, CommandInfo info)
    {
        Server.PrintToChatAll(_dependencyExample.GetText());
        Server.PrintToConsole(_dependencyExample.GetText());
        
        _moduleDependencyExample.TestPrintModule();
    }
}