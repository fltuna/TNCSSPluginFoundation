using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Commands.Targeting;
using Microsoft.Extensions.DependencyInjection;
using TNCSSPluginFoundation.Models.Command;
using TNCSSPluginFoundation.Models.Command.Validators;

namespace TNCSSPluginFoundation.Example.Modules.ExtendedTargetingTest;

/// <summary>
/// Sample command demonstrating ExtendedTargeting usage with custom parameter
/// 
/// Usage: tncss_extended_targeting_test [target]
/// Examples: 
/// - tncss_extended_targeting_test @all (targets all players)
/// - tncss_extended_targeting_test @ct (targets all Counter-Terrorist players)
/// - tncss_extended_targeting_test @hpge=100 (targets players with health >= 100 using custom parameter)
/// - tncss_extended_targeting_test @hpge=50 (targets players with health >= 50 using custom parameter)
/// - tncss_extended_targeting_test player_name (targets specific player)
/// </summary>
public class TestTncssCommandWithExtendedTargeting(ServiceProvider provider) : TncssAbstractCommandBase(provider)
{
    public override string CommandName => "tncss_extended_targeting_test";
    public override string CommandDescription => "Test command for ExtendedTargeting with custom @hpge parameter";

    protected override ICommandValidator GetValidator() => new CompositeValidator()
        .Add(new ArgumentCountValidator(1, true))
        .Add(new PermissionValidator("css/root", true))
        .Add(new ExtendedTargetValidator(1, true));

    protected override ValidationFailureResult OnValidationFailed(ValidationFailureContext context)
    {
        switch (context.Validator)
        {
            case ArgumentCountValidator:
                context.CommandInfo.ReplyToCommand("[ExtendedTargeting] Usage: tncss_extended_targeting_test <target>");
                context.CommandInfo.ReplyToCommand("[ExtendedTargeting] Examples: @all, @ct, @t, @hpge=100, player_name");
                return ValidationFailureResult.SilentAbort();

            case PermissionValidator:
                context.CommandInfo.ReplyToCommand("[ExtendedTargeting] You don't have permission to use this command!");
                return ValidationFailureResult.SilentAbort();

            case ExtendedTargetValidator extendedValidator:
                var targetStr = extendedValidator.GetLastTargetString();
                context.CommandInfo.ReplyToCommand($"[DEBUG] ExtendedTarget validation failed for: '{targetStr}'");
                context.CommandInfo.ReplyToCommand($"[DEBUG] Argument index: {extendedValidator.GetArgumentIndex()}");
                context.CommandInfo.ReplyToCommand("[ExtendedTargeting] No players found matching the target!");
                return ValidationFailureResult.SilentAbort();

            default:
                return ValidationFailureResult.UseDefaultFallback();
        }
    }

    protected override void ExecuteCommand(CCSPlayerController? player, CommandInfo commandInfo, ValidatedArguments? validatedArguments)
    {
        var foundTargets = validatedArguments!.GetArgument<TargetResult>(1);
        
        var targetString = commandInfo.GetArg(1);

        // This should not be happened
        if (foundTargets == null || !foundTargets.Any())
        {
            commandInfo.ReplyToCommand($"[ExtendedTargeting] No targets found!");
            return;
        }

        commandInfo.ReplyToCommand($"[ExtendedTargeting] Target: '{targetString}' - Found {foundTargets.Count()} player(s):");

        foreach (var target in foundTargets)
        {
            if (target.PlayerPawn.Value != null)
            {
                var health = target.PlayerPawn.Value.Health;
                var team = target.Team == CounterStrikeSharp.API.Modules.Utils.CsTeam.CounterTerrorist ? "CT" : 
                          target.Team == CounterStrikeSharp.API.Modules.Utils.CsTeam.Terrorist ? "T" : "SPEC";
                
                commandInfo.ReplyToCommand($"  - {target.PlayerName} (HP: {health}, Team: {team})");
            }
            else
            {
                commandInfo.ReplyToCommand($"  - {target.PlayerName} (No pawn data)");
            }
        }

        if (targetString.StartsWith("@hpge="))
        {
            var healthParam = targetString.Substring(6);
            commandInfo.ReplyToCommand($"[ExtendedTargeting] Custom parameter used: Health >= {healthParam}");
            commandInfo.ReplyToCommand($"[ExtendedTargeting] This demonstrates the @hpge parameter registered in ExtendedTargetingModule");
        }

        commandInfo.ReplyToCommand($"[ExtendedTargeting] Command executed by: {player?.PlayerName ?? "Console"}");
        commandInfo.ReplyToCommand("[ExtendedTargeting] In a real scenario, you could apply effects to all found targets here!");
    }
}