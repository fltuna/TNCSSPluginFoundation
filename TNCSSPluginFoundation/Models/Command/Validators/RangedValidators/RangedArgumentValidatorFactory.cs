using System.Numerics;

namespace TNCSSPluginFoundation.Models.Command.Validators.RangedValidators;

/// <summary>
/// Factory methods for creating RangedArgumentValidator instances
/// </summary>
public static class RangedArgumentValidator
{
    private const int DefaultArgumentIndex = 2;
    
    /// <summary>
    /// Creates an integer range validator
    /// </summary>
    /// <param name="min">Minimum value</param>
    /// <param name="max">Maximum value</param>
    /// <param name="argumentIndex">Argument index (1-based)</param>
    /// <param name="dontNotifyWhenFailed">Whether to suppress failure notifications</param>
    /// <returns>RangedArgumentValidator for int</returns>
    public static RangedArgumentValidator<int> CreateInt(int min, int max, int argumentIndex = DefaultArgumentIndex, bool dontNotifyWhenFailed = false)
        => new(min, max, argumentIndex, dontNotifyWhenFailed);

    /// <summary>
    /// Creates a float range validator
    /// </summary>
    /// <param name="min">Minimum value</param>
    /// <param name="max">Maximum value</param>
    /// <param name="argumentIndex">Argument index (1-based)</param>
    /// <param name="dontNotifyWhenFailed">Whether to suppress failure notifications</param>
    /// <returns>RangedArgumentValidator for float</returns>
    public static RangedArgumentValidator<float> CreateFloat(float min, float max, int argumentIndex = DefaultArgumentIndex, bool dontNotifyWhenFailed = false)
        => new(min, max, argumentIndex, dontNotifyWhenFailed);

    /// <summary>
    /// Creates a double range validator
    /// </summary>
    /// <param name="min">Minimum value</param>
    /// <param name="max">Maximum value</param>
    /// <param name="argumentIndex">Argument index (1-based)</param>
    /// <param name="dontNotifyWhenFailed">Whether to suppress failure notifications</param>
    /// <returns>RangedArgumentValidator for double</returns>
    public static RangedArgumentValidator<double> CreateDouble(double min, double max, int argumentIndex = DefaultArgumentIndex, bool dontNotifyWhenFailed = false)
        => new(min, max, argumentIndex, dontNotifyWhenFailed);

    /// <summary>
    /// Creates a long range validator
    /// </summary>
    /// <param name="min">Minimum value</param>
    /// <param name="max">Maximum value</param>
    /// <param name="argumentIndex">Argument index (1-based)</param>
    /// <param name="dontNotifyWhenFailed">Whether to suppress failure notifications</param>
    /// <returns>RangedArgumentValidator for long</returns>
    public static RangedArgumentValidator<long> CreateLong(long min, long max, int argumentIndex = DefaultArgumentIndex, bool dontNotifyWhenFailed = false)
        => new(min, max, argumentIndex, dontNotifyWhenFailed);

    /// <summary>
    /// Creates a decimal range validator
    /// </summary>
    /// <param name="min">Minimum value</param>
    /// <param name="max">Maximum value</param>
    /// <param name="argumentIndex">Argument index (1-based)</param>
    /// <param name="dontNotifyWhenFailed">Whether to suppress failure notifications</param>
    /// <returns>RangedArgumentValidator for decimal</returns>
    public static RangedArgumentValidator<decimal> CreateDecimal(decimal min, decimal max, int argumentIndex = DefaultArgumentIndex, bool dontNotifyWhenFailed = false)
        => new(min, max, argumentIndex, dontNotifyWhenFailed);

    /// <summary>
    /// Creates a generic range validator
    /// </summary>
    /// <typeparam name="T">Numeric type</typeparam>
    /// <param name="min">Minimum value</param>
    /// <param name="max">Maximum value</param>
    /// <param name="argumentIndex">Argument index (1-based)</param>
    /// <param name="dontNotifyWhenFailed">Whether to suppress failure notifications</param>
    /// <returns>RangedArgumentValidator for type T</returns>
    public static RangedArgumentValidator<T> Create<T>(T min, T max, int argumentIndex = DefaultArgumentIndex, bool dontNotifyWhenFailed = false)
        where T : struct, INumber<T>, IComparable<T>
        => new(min, max, argumentIndex, dontNotifyWhenFailed);

    /// <summary>
    /// Creates a range validator from string parameters
    /// </summary>
    /// <param name="typeName">Type name (int, float, double, long, decimal)</param>
    /// <param name="min">Minimum value as string</param>
    /// <param name="max">Maximum value as string</param>
    /// <param name="argumentIndex">Argument index (1-based)</param>
    /// <param name="dontNotifyWhenFailed">Whether to suppress failure notifications</param>
    /// <returns>IRangedArgumentValidator or null if invalid parameters</returns>
    public static IRangedArgumentValidator? Create(string typeName, string min, string max, int argumentIndex = DefaultArgumentIndex, bool dontNotifyWhenFailed = false)
    {
        return typeName.ToLower() switch
        {
            "int" or "int32" => int.TryParse(min, out var intMin) && int.TryParse(max, out var intMax)
                ? CreateInt(intMin, intMax, argumentIndex, dontNotifyWhenFailed) : null,
            
            "float" or "single" => float.TryParse(min, out var floatMin) && float.TryParse(max, out var floatMax)
                ? CreateFloat(floatMin, floatMax, argumentIndex, dontNotifyWhenFailed) : null,
            
            "double" => double.TryParse(min, out var doubleMin) && double.TryParse(max, out var doubleMax)
                ? CreateDouble(doubleMin, doubleMax, argumentIndex, dontNotifyWhenFailed) : null,
            
            "long" or "int64" => long.TryParse(min, out var longMin) && long.TryParse(max, out var longMax)
                ? CreateLong(longMin, longMax, argumentIndex, dontNotifyWhenFailed) : null,
            
            "decimal" => decimal.TryParse(min, out var decimalMin) && decimal.TryParse(max, out var decimalMax)
                ? CreateDecimal(decimalMin, decimalMax, argumentIndex, dontNotifyWhenFailed) : null,
            
            _ => null
        };
    }
}