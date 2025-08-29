using CounterStrikeSharp.API;
using TNCSSPluginFoundation.Models.Plugin;

namespace TNCSSPluginFoundation.Example.Modules.TncssCommands;

public class TncssCommandsModule(IServiceProvider serviceProvider) : PluginModuleBase(serviceProvider)
{
    public override string PluginModuleName => "TncssCommands";
    public override string ModuleChatPrefix => "unused";
    protected override bool UseTranslationKeyInModuleChatPrefix => false;

    protected override void OnInitialize()
    {
        RegisterTncssCommand<TestTncssCommandWithValidator>();
    }
}