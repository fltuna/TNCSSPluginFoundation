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
        .Add(new ArgumentCountValidator(1, true))
        .Add(new PermissionValidator("css/root", true))
        .Add(new RangedArgumentValidator<int>(0, 10, 1, true))
        .Add(new AuthorizedSteamIdValidator());

    protected override ValidationFailureResult OnValidationFailed(ValidationFailureContext context)
    {
        switch (context.Validator)
        {
            case ArgumentCountValidator argumentCountValidator:
                context.CommandInfo.ReplyToCommand($"[Sample] Requires more argument");
                return ValidationFailureResult.SilentAbort();
            
            case PermissionValidator permissionValidator:
                context.CommandInfo.ReplyToCommand($"[Sample] You don't have required permission: {permissionValidator.ValidationFailureMessage}");
                return ValidationFailureResult.SilentAbort();
                
            case RangedArgumentValidator<int> rangedValidator:
                var range = rangedValidator.GetRangeDescription();
                context.CommandInfo.ReplyToCommand($"[Sample] Invalid number! Valid range: {range}");
                return ValidationFailureResult.SilentAbort();
                
            case AuthorizedSteamIdValidator:
                context.CommandInfo.ReplyToCommand("[Sample] You are not authorized to use this command!");
                return ValidationFailureResult.SilentAbort();
                
            default:
                return ValidationFailureResult.UseDefaultFallback();
        }
    }

    protected override void ExecuteCommand(CCSPlayerController? player, CommandInfo commandInfo, ValidatedArguments? validatedArguments)
    {
        var commandName = validatedArguments!.GetArgument<string>(0);
        
        var rangeValue = validatedArguments.GetArgument<int>(1);
        
        commandInfo.ReplyToCommand($"[Sample] Command '{commandName}' executed successfully!");
        commandInfo.ReplyToCommand($"[Sample] Validated number: {rangeValue}");
        commandInfo.ReplyToCommand($"[Sample] Player: {player?.PlayerName ?? "Console"}");
        
        var result = rangeValue * 10;
        commandInfo.ReplyToCommand($"[Sample] Calculation result: {rangeValue} * 10 = {result}");
        
        for (int i = 0; i < validatedArguments.ArgumentCount; i++)
        {
            var arg = validatedArguments.GetArgument<string>(i);
            Console.WriteLine($"[Sample] Arg[{i}]: {arg}");
        }
    }
}