using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using TNCSSPluginFoundation.Models.Command;
using TNCSSPluginFoundation.Models.Command.Validators;

namespace TNCSSPluginFoundation.Example.Modules.TncssCommands.CustomValidator;

public class AuthorizedSteamIdValidator(IServiceProvider provider): ICommandValidator
{
    public static List<ulong> AuthorizedIds { get; } = new();

    public string ValidatorName => "TncssExampleAuthorizedSteamIdValidator";

    public TncssCommandValidationResult Validate(CCSPlayerController? player, CommandInfo commandInfo)
    {
        if (player == null)
            return TncssCommandValidationResult.Success;

        if (AuthorizedIds.Contains(player.SteamID))
            return TncssCommandValidationResult.Success;
        
        return TncssCommandValidationResult.FailedIgnoreDefault;
    }
}