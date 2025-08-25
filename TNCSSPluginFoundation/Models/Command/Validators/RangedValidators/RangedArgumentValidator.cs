using System.Numerics;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;

namespace TNCSSPluginFoundation.Models.Command.Validators.RangedValidators;

/// <summary>
/// Validates command arguments within a specified numeric range
/// </summary>
/// <typeparam name="T">Numeric type to validate</typeparam>
public sealed class RangedArgumentValidator<T> : ICommandValidator, IRangedArgumentValidator 
    where T : struct, INumber<T>, IComparable<T>
{
    private readonly T _min;
    private readonly T _max;
    private readonly int _argumentIndex;
    private readonly bool _dontNotifyWhenFailed;
    private T? _lastParsedValue;
    private TncssRangedCommandValidationResult _lastRangedResult;

    /// <summary>
    /// Whether to notify when validation fails
    /// </summary>
    public bool ShouldNotifyOnFailure => !_dontNotifyWhenFailed;

    /// <summary>
    /// Initializes a new instance of RangedArgumentValidator
    /// </summary>
    /// <param name="min">Minimum allowed value</param>
    /// <param name="max">Maximum allowed value</param>
    /// <param name="argumentIndex">Index of the argument to validate (1-based)</param>
    /// <param name="dontNotifyWhenFailed">Whether to suppress failure notifications</param>
    public RangedArgumentValidator(T min, T max, int argumentIndex = 2, bool dontNotifyWhenFailed = false)
    {
        _min = min;
        _max = max;
        _argumentIndex = argumentIndex;
        _dontNotifyWhenFailed = dontNotifyWhenFailed;
    }

    /// <summary>
    /// Name of this validator for identification purposes
    /// </summary>
    public string ValidatorName => "TncssBuiltinRangedArgumentValidator";

    /// <summary>
    /// Validates command input for ICommandValidator interface
    /// </summary>
    /// <param name="player">CCSPlayerController</param>
    /// <param name="commandInfo">CommandInfo</param>
    /// <returns>TncssCommandValidationResult</returns>
    public TncssCommandValidationResult Validate(CCSPlayerController? player, CommandInfo commandInfo)
    {
        var rangedResult = ValidateRange(player, commandInfo);
        
        return rangedResult == TncssRangedCommandValidationResult.Success 
            ? TncssCommandValidationResult.Success
            : (_dontNotifyWhenFailed ? TncssCommandValidationResult.FailedIgnoreDefault : TncssCommandValidationResult.Failed);
    }

    /// <summary>
    /// Validates range-specific command input
    /// </summary>
    /// <param name="player">CCSPlayerController</param>
    /// <param name="commandInfo">CommandInfo</param>
    /// <returns>TncssRangedCommandValidationResult</returns>
    public TncssRangedCommandValidationResult ValidateRange(CCSPlayerController? player, CommandInfo commandInfo)
    {
        _lastParsedValue = null;

        if (commandInfo.ArgCount <= _argumentIndex)
        {
            _lastRangedResult = TncssRangedCommandValidationResult.FailedOutOfRange;
            return _lastRangedResult;
        }

        var argString = commandInfo.GetArg(_argumentIndex);
        
        if (!T.TryParse(argString, null, out var value))
        {
            _lastRangedResult = TncssRangedCommandValidationResult.FailedOutOfRange;
            return _lastRangedResult;
        }

        _lastParsedValue = value;

        if (value.CompareTo(_min) < 0)
        {
            _lastRangedResult = TncssRangedCommandValidationResult.FailedLowerThanExpected;
            return _lastRangedResult;
        }
        
        if (value.CompareTo(_max) > 0)
        {
            _lastRangedResult = TncssRangedCommandValidationResult.FailedHigherThanExpected;
            return _lastRangedResult;
        }

        _lastRangedResult = TncssRangedCommandValidationResult.Success;
        return _lastRangedResult;
    }

    /// <summary>
    /// Gets the last range validation result
    /// </summary>
    /// <returns>TncssRangedCommandValidationResult</returns>
    public TncssRangedCommandValidationResult GetLastRangedResult() => _lastRangedResult;
    
    /// <summary>
    /// Gets description of the valid range
    /// </summary>
    /// <returns>String representation of the range</returns>
    public string GetRangeDescription() => $"[{_min} - {_max}]";

    /// <summary>
    /// Gets the parsed value as an object
    /// </summary>
    /// <returns>Parsed value as object or null</returns>
    public object? GetParsedValueAsObject() => _lastParsedValue;

    /// <summary>
    /// Gets the parsed value converted to the specified type
    /// </summary>
    /// <typeparam name="TResult">Target type</typeparam>
    /// <returns>Converted value or null</returns>
    public TResult? GetParsedValueAs<TResult>() where TResult : struct
    {
        if (!_lastParsedValue.HasValue) return null;

        try
        {
            var value = _lastParsedValue.Value;
            
            // Same type case
            if (typeof(T) == typeof(TResult))
                return (TResult)(object)value;

            // Type conversion
            return (TResult)Convert.ChangeType(value, typeof(TResult));
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Gets the parsed value in its original type
    /// </summary>
    /// <returns>Parsed value or null</returns>
    public T? GetParsedValue() => _lastParsedValue;
    
    /// <summary>
    /// Gets the parsed value or a default value if null
    /// </summary>
    /// <param name="defaultValue">Default value to return if parsed value is null</param>
    /// <returns>Parsed value or default value</returns>
    public T GetParsedValueOrDefault(T defaultValue = default) => _lastParsedValue ?? defaultValue;
}