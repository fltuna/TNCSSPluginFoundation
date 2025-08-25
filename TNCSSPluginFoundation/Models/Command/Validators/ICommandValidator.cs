using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;

namespace TNCSSPluginFoundation.Models.Command.Validators;

/// <summary>
/// Base interface for command validators
/// </summary>
public interface ICommandValidator
{
    /// <summary>
    /// Name of this validator for identification purposes
    /// </summary>
    string ValidatorName { get; }

    /// <summary>
    /// Validates player command input
    /// </summary>
    /// <param name="player">CCSPlayerController</param>
    /// <param name="commandInfo">CommandInfo</param>
    /// <returns>TncssCommandValidationResult</returns>
    TncssCommandValidationResult Validate(CCSPlayerController? player, CommandInfo commandInfo);
}