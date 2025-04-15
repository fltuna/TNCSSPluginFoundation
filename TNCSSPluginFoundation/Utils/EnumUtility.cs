using System.Runtime.Serialization;
using CounterStrikeSharp.API.Modules.Entities.Constants;

namespace TNCSSPluginFoundation.Utils;

/// <summary>
/// Provides simple but useful enum extensions
/// </summary>
public static class EnumUtility
{
    
    /// <summary>
    /// Get enum by enum member value. For instance:<br/>
    /// [EnumMember(Value = "item_kevlar")] <br/>
    /// Kevlar = 000,<br/>
    /// Enum of CsItem has a EnumMember, and you can see the EnumMember attribute.<br/>
    /// If we want to get an Enum value from EnumMember attribute, then yes, this method will help you.
    /// </summary>
    /// <param name="value">EnumMember attribute value</param>
    /// <param name="result">Result of Enum</param>
    /// <typeparam name="T">Any type of enum</typeparam>
    /// <returns>True if found, Otherwise false</returns>
    public static bool TryGetEnumByEnumMemberValue<T>(string value, out T result) where T : struct, Enum
    {
        result = default;
    
        // I'm not sure why weapon_hegrenade can't be found during iteration.
        // But since we can't find it normally, we'll handle it specifically here.
        if (typeof(T) == typeof(CsItem) && string.Equals(value, "weapon_hegrenade", StringComparison.OrdinalIgnoreCase))
        {
            result = (T)(object)CsItem.HighExplosive;
            return true;
        }
        
        var found = Enum.GetValues(typeof(T))
            .Cast<T>()
            .Where(e => 
            {
                var memberInfo = typeof(T).GetMember(e.ToString()).FirstOrDefault();
                if (memberInfo == null) return false;
            
                var attribute = memberInfo.GetCustomAttributes(typeof(EnumMemberAttribute), false)
                    .FirstOrDefault() as EnumMemberAttribute;
                return attribute?.Value == value;
            })
            .ToList();

        if (found.Count <= 0)
            return false;
        
        result = found.First();
        return true;
    }
    
    
    /// <summary>
    /// Get all enum members. <br/>
    /// This is a very edge case, but sometimes wants to print an all enum member values like a weapon_xxxx name in CsItem, then yes, this method will help you.
    /// </summary>
    /// <param name="values">List of enum</param>
    /// <typeparam name="T">Any type of enum</typeparam>
    /// <returns>True if found, Otherwise false</returns>
    public static bool TryGetAllEnumMemberValues<T>(out List<string> values) where T : struct, Enum
    {
        try
        {
            values = new List<string>();
        
            var standardValues = Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(e => 
                {
                    var memberInfo = typeof(T).GetMember(e.ToString()!).FirstOrDefault();
                    if (memberInfo == null) return null;
            
                    var attribute = memberInfo.GetCustomAttributes(typeof(EnumMemberAttribute), false)
                        .FirstOrDefault() as EnumMemberAttribute;
                    return attribute?.Value;
                })
                .Where(value => !string.IsNullOrEmpty(value))
                .ToList();
        
            values.AddRange(standardValues!);
        
            // Specific case
            if (typeof(T) == typeof(CsItem))
            {
                // check weapon_hegrenade is already contained in list. (99% not)
                bool hasHeGrenade = values.Any(v => string.Equals(v, "weapon_hegrenade", StringComparison.OrdinalIgnoreCase));
            
                if (!hasHeGrenade)
                {
                    values.Add("weapon_hegrenade");
                }
            }

            values = values.Distinct().ToList();
        
            return values.Count > 0;
        }
        catch
        {
            values = [];
            return false;
        }
    }
}