using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using Microsoft.Extensions.DependencyInjection;
using TNCSSPluginFoundation.Models.Command.Validators;
using TNCSSPluginFoundation.Models.Command.Validators.RangedValidators;

namespace TNCSSPluginFoundation.Models.Command;

/// <summary>
/// Abstracted Command Base
/// </summary>
public abstract class TncssAbstractCommandBase(IServiceProvider provider)
{
    /// <summary>
    /// Name of command (e.g. css_tset)
    /// </summary>
    public abstract string CommandName { get; }
    
    /// <summary>
    /// Description of command
    /// </summary>
    public abstract string CommandDescription { get; }
    
    /// <summary>
    /// Default validation failure message 
    /// </summary>
    protected virtual string CommonValidationFailureMessage => "Common.Validation.Failure";
    
    /// <summary>
    /// ServiceProvider of TncssPluginFoundation DI container
    /// </summary>
    protected IServiceProvider ServiceProvider => provider;

    /// <summary>
    /// Internal function handled by TncssPluginFoundation
    /// </summary>
    /// <param name="player">CCSPlayerController</param>
    /// <param name="commandInfo">CommandInfo</param>
    public void Execute(CCSPlayerController? player, CommandInfo commandInfo)
    {
        var validator = GetValidator();
        if (validator != null)
        {
            var result = validator.Validate(player, commandInfo);
            
            if (result != TncssCommandValidationResult.Success)
            {
                // Handle validation failure
                var context = new ValidationFailureContext(validator, player, commandInfo, result);
                var failureResult = OnValidationFailed(context);

                switch (failureResult.Action)
                {
                    case ValidationFailureAction.UseDefaultFallback:
                        // Use validator's default behavior
                        if (result == TncssCommandValidationResult.Failed)
                        {
                            var message = GetDefaultValidationMessage(context);
                            commandInfo.ReplyToCommand(message);
                        }
                        return;

                    case ValidationFailureAction.SilentAbort:
                        return;

                    default:
                        // Unknown action, fall back to silent abort
                        return;
                }
            }
        }

        ExecuteCommand(player, commandInfo);
    }

    /// <summary>
    /// Gets the validator for this command
    /// </summary>
    /// <returns>ICommandValidator or null</returns>
    protected virtual ICommandValidator? GetValidator() => null;

    /// <summary>
    /// Called when validation fails - override to customize failure handling
    /// </summary>
    /// <param name="context">Validation failure context</param>
    /// <returns>ValidationFailureResult specifying how to handle the failure</returns>
    protected virtual ValidationFailureResult OnValidationFailed(ValidationFailureContext context)
    {
        return ValidationFailureResult.UseDefaultFallback();
    }

    /// <summary>
    /// Gets the default validation message
    /// </summary>
    /// <param name="context">Validation failure context</param>
    /// <returns>Default message</returns>
    protected virtual string GetDefaultValidationMessage(ValidationFailureContext context)
    {
        // Check for ranged validation specific messages
        if (context.RangedValidator != null && context.RangedResult.HasValue)
            return GetRangedValidationMessage(context.RangedValidator, context.RangedResult.Value);
        
        if (!string.IsNullOrEmpty(context.Validator.ValidationFailureMessage))
            return context.Validator.ValidationFailureMessage;
        
        return CommonValidationFailureMessage;
    }

    /// <summary>
    /// Gets validation message for ranged validators
    /// </summary>
    /// <param name="rangedValidator">The ranged validator</param>
    /// <param name="result">The ranged validation result</param>
    /// <returns>Validation message</returns>
    protected virtual string GetRangedValidationMessage(IRangedArgumentValidator rangedValidator, TncssRangedCommandValidationResult result)
    {
        var range = rangedValidator.GetRangeDescription();
        
        return result switch
        {
            TncssRangedCommandValidationResult.FailedLowerThanExpected => $"Value is too low. Valid range: {range}",
            TncssRangedCommandValidationResult.FailedHigherThanExpected => $"Value is too high. Valid range: {range}",
            TncssRangedCommandValidationResult.FailedOutOfRange => $"Invalid value or out of range. Valid range: {range}",
            _ => "Range validation failed."
        };
    }

    /// <summary>
    /// Command body - implement in derived classes
    /// </summary>
    protected abstract void ExecuteCommand(CCSPlayerController? player, CommandInfo commandInfo);
}