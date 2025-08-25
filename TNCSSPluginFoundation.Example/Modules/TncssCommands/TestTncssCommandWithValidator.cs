using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using Microsoft.Extensions.DependencyInjection;
using TNCSSPluginFoundation.Example.Modules.TncssCommands.CustomValidator;
using TNCSSPluginFoundation.Models.Command;
using TNCSSPluginFoundation.Models.Command.Validators;
using TNCSSPluginFoundation.Models.Command.Validators.RangedValidators;

namespace TNCSSPluginFoundation.Example.Modules.TncssCommands;

public class TestTncssCommandWithValidator(ServiceProvider provider) : TncssAbstractCommandBase(provider)
{
    public override string CommandName => "tncss_command_with_validator";
    public override string CommandDescription => "The test command of tncss command";

    protected override ICommandValidator GetValidator() => new CompositeValidator()
        .Add(new PermissionValidator("css/root", true))
        .Add(new RangedArgumentValidator<int>(0, 10, 2, true))
        .Add(new AuthorizedSteamIdValidator(ServiceProvider));

    protected override ValidationFailureResult OnValidationFailed(ValidationFailureContext context)
    {
        // DO something
        // e.g. send error message to player.
        
        
        return ValidationFailureResult.SilentAbort();
    }

    protected override void ExecuteCommand(CCSPlayerController? player, CommandInfo commandInfo)
    {
        // DO Something
        // e.g. execute any command effect
    }
}