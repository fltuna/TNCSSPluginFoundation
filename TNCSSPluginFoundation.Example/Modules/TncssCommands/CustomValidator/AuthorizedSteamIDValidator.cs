using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using TNCSSPluginFoundation.Models.Command;
using TNCSSPluginFoundation.Models.Command.Validators;

namespace TNCSSPluginFoundation.Example.Modules.TncssCommands.CustomValidator;

public class AuthorizedSteamIdValidator: CommandValidatorBase
{
    public static List<ulong> AuthorizedIds { get; } = new();

    public override string ValidatorName => "TncssExampleAuthorizedSteamIdValidator";
    public override string ValidationFailureMessage => "Common.Validation.Failure.AuthorizedSteamId";

    public override TncssCommandValidationResult Validate(CCSPlayerController? player, CommandInfo commandInfo)
    {
        if (player == null)
            return TncssCommandValidationResult.Success;

        if (AuthorizedIds.Contains(player.SteamID))
            return TncssCommandValidationResult.Success;
        
        return TncssCommandValidationResult.FailedIgnoreDefault;
    }
}