using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using TNCSSPluginFoundation.Models.Command.Validators.RangedValidators;

namespace TNCSSPluginFoundation.Models.Command.Validators;

/// <summary>
/// Context information for validation failures
/// </summary>
public class ValidationFailureContext
{
    /// <summary>
    /// The validator that failed
    /// </summary>
    public ICommandValidator Validator { get; }
    
    /// <summary>
    /// The player who executed the command
    /// </summary>
    public CCSPlayerController? Player { get; }
    
    /// <summary>
    /// Command information
    /// </summary>
    public CommandInfo CommandInfo { get; }
    
    /// <summary>
    /// Basic validation result
    /// </summary>
    public TncssCommandValidationResult ValidationResult { get; }
    
    /// <summary>
    /// Ranged validation result (if applicable)
    /// </summary>
    public TncssRangedCommandValidationResult? RangedResult { get; }
    
    /// <summary>
    /// The ranged validator instance (if applicable)
    /// </summary>
    public IRangedArgumentValidator? RangedValidator { get; }

    /// <summary>
    /// Initializes a new ValidationFailureContext
    /// </summary>
    /// <param name="validator">The validator that failed</param>
    /// <param name="player">The player who executed the command</param>
    /// <param name="commandInfo">Command information</param>
    /// <param name="validationResult">Basic validation result</param>
    public ValidationFailureContext(ICommandValidator validator, CCSPlayerController? player, CommandInfo commandInfo, TncssCommandValidationResult validationResult)
    {
        Validator = validator;
        Player = player;
        CommandInfo = commandInfo;
        ValidationResult = validationResult;
        
        // Extract ranged validator information if available
        if (validator is CompositeValidator composite)
        {
            RangedValidator = composite.GetRangedValidator();
            RangedResult = RangedValidator?.GetLastRangedResult();
        }
        else if (validator is IRangedArgumentValidator rangedValidator)
        {
            RangedValidator = rangedValidator;
            RangedResult = rangedValidator.GetLastRangedResult();
        }
    }
}