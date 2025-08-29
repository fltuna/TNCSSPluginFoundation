using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;

namespace TNCSSPluginFoundation.Models.Command.Validators;

/// <summary>
/// Base class for command validators that provides default implementation for backward compatibility
/// </summary>
public abstract class CommandValidatorBase : ICommandValidator
{
    /// <summary>
    /// Name of this validator for identification purposes
    /// </summary>
    public abstract string ValidatorName { get; }

    /// <summary>
    /// Message of validation failure
    /// </summary>
    public abstract string ValidationFailureMessage { get; }

    /// <summary>
    /// Validates player command input
    /// </summary>
    /// <param name="player">CCSPlayerController</param>
    /// <param name="commandInfo">CommandInfo</param>
    /// <returns>TncssCommandValidationResult</returns>
    public abstract TncssCommandValidationResult Validate(CCSPlayerController? player, CommandInfo commandInfo);

    /// <summary>
    /// Validates player command input and returns validated arguments
    /// Default implementation calls Validate and returns empty ValidatedArguments on success
    /// Override this method to populate validated arguments
    /// </summary>
    /// <param name="player">CCSPlayerController</param>
    /// <param name="commandInfo">CommandInfo</param>
    /// <returns>TncssCommandValidationContext</returns>
    public virtual TncssCommandValidationContext ValidateWithArguments(CCSPlayerController? player, CommandInfo commandInfo)
    {
        var result = Validate(player, commandInfo);
        
        if (result == TncssCommandValidationResult.Success)
        {
            var validatedArguments = ExtractArguments(player, commandInfo);
            return TncssCommandValidationContext.Success(validatedArguments);
        }
        
        return result switch
        {
            TncssCommandValidationResult.Failed => TncssCommandValidationContext.Failed(),
            TncssCommandValidationResult.FailedIgnoreDefault => TncssCommandValidationContext.FailedIgnoreDefault(),
            _ => TncssCommandValidationContext.Failed()
        };
    }

    /// <summary>
    /// Extracts and validates arguments from command info
    /// Override this method to populate ValidatedArguments with parsed/converted arguments
    /// </summary>
    /// <param name="player">CCSPlayerController</param>
    /// <param name="commandInfo">CommandInfo</param>
    /// <returns>ValidatedArguments containing parsed arguments</returns>
    protected virtual ValidatedArguments ExtractArguments(CCSPlayerController? player, CommandInfo commandInfo)
    {
        var validatedArguments = new ValidatedArguments();
        
        // Default implementation: add raw arguments by index (matching CommandInfo indexing)
        // Index 0 = command name, Index 1+ = arguments
        for (int i = 0; i < commandInfo.ArgCount; i++)
        {
            validatedArguments.SetArgument(i, commandInfo.GetArg(i));
        }
        
        return validatedArguments;
    }
}