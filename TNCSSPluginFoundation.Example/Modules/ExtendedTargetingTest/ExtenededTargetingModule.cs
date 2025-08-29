using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using TNCSSPluginFoundation.Extensions.Targeting;
using TNCSSPluginFoundation.Models.Plugin;

namespace TNCSSPluginFoundation.Example.Modules.ExtendedTargetingTest;

public class ExtenededTargetingModule(IServiceProvider serviceProvider) : PluginModuleBase(serviceProvider)
{
    public override string PluginModuleName => "Extended Targeting";
    public override string ModuleChatPrefix => "unused";
    protected override bool UseTranslationKeyInModuleChatPrefix => false;

    protected override void OnInitialize()
    {
        ExtendedTargeting.RegisterCustomParameterizedTarget("@hpge", IsPlayerGreaterThanHealth);
        RegisterTncssCommand<TestTncssCommandWithExtendedTargeting>();
    }

    protected override void OnUnloadModule()
    {
        ExtendedTargeting.UnregisterCustomParameterizedTarget("@hpge");
    }

    private bool IsPlayerGreaterThanHealth(string param, CCSPlayerController player, CCSPlayerController? caller)
    {
        if (!int.TryParse(param, out int health))
            return false;

        return player.PlayerPawn.Value?.Health >= health;
    }
}