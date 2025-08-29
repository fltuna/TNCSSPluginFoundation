using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands.Targeting;

namespace TNCSSPluginFoundation.Extensions.Targeting;

/// <summary>
/// Extended customizable targeting system for TNCSSPlugins
/// </summary>
public static class ExtendedTargeting
{
    /// <summary>
    /// Target predication delegate
    /// </summary>
    /// <param name="player">The subject player for testing.</param>
    /// <param name="caller">The command executor. player or console. (this is useful for traceing player's ray when adding like @aim targeting)</param>
    public delegate bool TargetPredicateDelegate(CCSPlayerController player, CCSPlayerController? caller);
    
    
    /// <summary>
    /// Parameterized Target predication delegate
    /// </summary>
    /// <param name="param">Any parameters for inputted to this targeting. (e.g. if @test:100 then input is "100")</param>
    /// <param name="player">The subject player for testing.</param>
    /// <param name="caller">The command executor. player or console. (this is useful for traceing player's ray when adding like @aim targeting)</param>
    public delegate bool ParameterizedTargetPredicateDelegate(string param, CCSPlayerController player, CCSPlayerController? caller);

    
    // Custom target
    private static readonly Dictionary<string, TargetPredicateDelegate> CustomTargets = new(StringComparer.OrdinalIgnoreCase);
        
    // Parameterized target
    private static readonly Dictionary<string, ParameterizedTargetPredicateDelegate> ParamTargets = new(StringComparer.OrdinalIgnoreCase);

    // Register custom target
    /// <summary>
    /// Register custom targeting
    /// </summary>
    /// <param name="prefix">targeting prefix (e.g. @vip, @friends)</param>
    /// <param name="predicate"></param>
    public static void RegisterCustomTarget(string prefix, TargetPredicateDelegate predicate)
    {
        if (!prefix.StartsWith('@'))
        {
            prefix = '@' + prefix;
        }
        CustomTargets[prefix] = predicate;
    }

    /// <summary>
    /// Unregister custom target from extended targeting
    /// </summary>
    /// <param name="prefix">prefix of targeting</param>
    /// <returns>true if deleted successfully, otherwise false</returns>
    public static bool UnregisterCustomTarget(string prefix)
    {
        return CustomTargets.Remove(prefix);
    }

    // Register parameterized target
    /// <summary>
    /// Registers a custom parameterized targeting
    /// </summary>
    /// <param name="prefix">targeting prefix (e.g. @vip, @friends)</param>
    /// <param name="predicate"></param>
    public static void RegisterCustomParameterizedTarget(string prefix, ParameterizedTargetPredicateDelegate predicate)
    {
        if (!prefix.StartsWith('@'))
        {
            prefix = '@' + prefix;
        }
        ParamTargets[prefix] = predicate;
    }

    /// <summary>
    /// Unregister custom parameterized target from extended targeting
    /// </summary>
    /// <param name="prefix">prefix of targeting</param>
    /// <returns>true if deleted successfully, otherwise false</returns>
    public static bool UnregisterCustomParameterizedTarget(string prefix)
    {
        return ParamTargets.Remove(prefix);
    }

    // Extended target resolve
    /// <summary>
    /// Resolve extended targeting. If no custom targeting matched, then it will fallback to CS#'s default targeting system.
    /// </summary>
    /// <param name="targetString">Target string for finding targets</param>
    /// <param name="caller"></param>
    /// <param name="foundTargets">Returns filled TargetResult if found, otherwise empty TargetReuslt</param>
    /// <returns>true if at least 1 player found. otherwise false</returns>
    public static bool ResolveExtendedTarget(string targetString, CCSPlayerController? caller, out TargetResult? foundTargets)
    {
        if (CustomTargets.TryGetValue(targetString, out var predicate))
        {
            // foundTargets = new TargetResult() 
            // {
            //     Players = Utilities.GetPlayers().Where(p => predicate(p, caller)).ToList() 
            // };

            // To avoid N+1 problem for using CS#'s Utilities.GetPlayers()
            List<CCSPlayerController> GetFilteredPlayers()
            {
                List<CCSPlayerController> players = new();
                for (int i = 0; i < Server.MaxPlayers; ++i)
                {
                    var player = Utilities.GetPlayerFromSlot(i);
                    if (player != null && predicate(player, caller))
                    {
                        players.Add(player);
                    }
                }
                return players;
            }

            foundTargets = new TargetResult()
            {
                Players = GetFilteredPlayers()
            };
            
            return foundTargets.Any();
        }

        if (targetString.StartsWith('@') && targetString.Contains('='))
        {
            var parts = targetString.Split('=', 2);
            var prefix = parts[0];
            var param = parts[1];

            if (ParamTargets.TryGetValue(prefix, out var paramPredicate))
            {
                // foundTargets = new TargetResult() 
                // { 
                //     Players = Utilities.GetPlayers().Where(p => paramPredicate(param, p, caller)).ToList() 
                // };
                

                // To avoid N+1 problem for using CS#'s Utilities.GetPlayers()
                List<CCSPlayerController> GetParamFilteredPlayers()
                {
                    List<CCSPlayerController> players = new();
                    for (int i = 0; i < Server.MaxPlayers; ++i)
                    {
                        var player = Utilities.GetPlayerFromSlot(i);
                        if (player != null && paramPredicate(param, player, caller))
                        {
                            players.Add(player);
                        }
                    }
                    return players;
                }

                foundTargets = new TargetResult()
                {
                    Players = GetParamFilteredPlayers()
                };
                
                return foundTargets.Any();
            }
        }

        // If doesn't match with custom targeting, then fallback to CS#'s target system.
        foundTargets = new Target(targetString).GetTarget(caller);
        return foundTargets.Any();
    }
}