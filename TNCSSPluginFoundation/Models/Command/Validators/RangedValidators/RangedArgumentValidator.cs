using System.Numerics;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;

namespace TNCSSPluginFoundation.Models.Command.Validators.RangedValidators;

/// <summary>
/// Validates command arguments within a specified numeric range
/// </summary>
/// <typeparam name="T">Numeric type to validate</typeparam>
public sealed class RangedArgumentValidator<T> : CommandValidatorBase, IRangedArgumentValidator 
    where T : struct, INumber<T>, IComparable<T>
{
    private readonly T _min;
    private readonly T _max;
    private readonly int _argumentIndex;
    private readonly bool _dontNotifyWhenFailed;
    private readonly bool _isOptional;
    private readonly T? _defaultValue;
    private T? _lastParsedValue;
    private TncssRangedCommandValidationResult _lastRangedResult;
    private bool _isUsingDefaultValue;

    /// <summary>
    /// Whether to notify when validation fails
    /// </summary>
    public bool ShouldNotifyOnFailure => !_dontNotifyWhenFailed;

    /// <summary>
    /// Initializes a new instance of RangedArgumentValidator for required arguments
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
        _isOptional = false;
        _defaultValue = null;
    }

    /// <summary>
    /// Initializes a new instance of RangedArgumentValidator for optional arguments
    /// </summary>
    /// <param name="min">Minimum allowed value</param>
    /// <param name="max">Maximum allowed value</param>
    /// <param name="argumentIndex">Index of the argument to validate (1-based)</param>
    /// <param name="defaultValue">Default value to use if argument is not provided</param>
    /// <param name="dontNotifyWhenFailed">Whether to suppress failure notifications</param>
    public RangedArgumentValidator(T min, T max, int argumentIndex, T defaultValue, bool dontNotifyWhenFailed = false)
    {
        _min = min;
        _max = max;
        _argumentIndex = argumentIndex;
        _dontNotifyWhenFailed = dontNotifyWhenFailed;
        _isOptional = true;
        _defaultValue = defaultValue;
    }

    /// <summary>
    /// Name of this validator for identification purposes
    /// </summary>
    public override string ValidatorName => "TncssBuiltinRangedArgumentValidator";
    
    /// <summary>
    /// Message of validation failure
    /// </summary>
    public override string ValidationFailureMessage => "Common.Validation.Failure.Ranged";

    /// <summary>
    /// Validates command input for ICommandValidator interface
    /// </summary>
    /// <param name="player">CCSPlayerController</param>
    /// <param name="commandInfo">CommandInfo</param>
    /// <returns>TncssCommandValidationResult</returns>
    public override TncssCommandValidationResult Validate(CCSPlayerController? player, CommandInfo commandInfo)
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
        _isUsingDefaultValue = false;

        if (commandInfo.ArgCount <= _argumentIndex)
        {
            // If optional and argument not provided, use default value
            if (_isOptional && _defaultValue.HasValue)
            {
                _lastParsedValue = _defaultValue.Value;
                _isUsingDefaultValue = true;
                _lastRangedResult = TncssRangedCommandValidationResult.Success;
                return _lastRangedResult;
            }
            
            // Required argument not provided
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
        _isUsingDefaultValue = false; // Argument was provided

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

    /// <summary>
    /// Checks if this validator is using the default value because the argument was not provided
    /// </summary>
    /// <returns>True if using default value, false if argument was provided</returns>
    public bool IsUsingDefaultValue() => _isUsingDefaultValue;

    /// <summary>
    /// Checks if this validator accepts optional arguments
    /// </summary>
    /// <returns>True if validator is configured for optional arguments</returns>
    public bool IsOptional() => _isOptional;

    /// <summary>
    /// Gets the default value configured for this validator (if optional)
    /// </summary>
    /// <returns>Default value or null if not optional</returns>
    public T? GetDefaultValue() => _defaultValue;

    /// <summary>
    /// Extracts validated arguments after successful validation
    /// </summary>
    /// <param name="player">CCSPlayerController</param>
    /// <param name="commandInfo">CommandInfo</param>
    /// <returns>ValidatedArguments with parsed range value</returns>
    protected override ValidatedArguments ExtractArguments(CCSPlayerController? player, CommandInfo commandInfo)
    {
        var validatedArguments = base.ExtractArguments(player, commandInfo);
        
        if (_lastParsedValue.HasValue)
        {
            validatedArguments.SetArgument(_argumentIndex, _lastParsedValue.Value);
        }
        
        return validatedArguments;
    }
}