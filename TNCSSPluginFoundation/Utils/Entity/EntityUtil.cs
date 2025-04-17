using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace TNCSSPluginFoundation.Utils.Entity;

/// <summary>
/// Utility class for manipulate game entity
/// </summary>
public static class EntityUtil
{
    
    
    private static CCSGameRulesProxy? _rulesProxy;
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns>Returns CCSGameRules instance if found. Otherwise null</returns>
    public static CCSGameRules? GetGameRules()
    {
        return GetGameRulesProxy()?.GameRules;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Returns CCSGameRulesProxy</returns>
    public static CCSGameRulesProxy? GetGameRulesProxy()
    {
        if (_rulesProxy == null || !_rulesProxy.IsValid)
        {
            _rulesProxy = Utilities.FindAllEntitiesByDesignerName<CCSGameRulesProxy>("cs_gamerules").FirstOrDefault();
        }
        
        return _rulesProxy;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Returns a CCSTeam instances.</returns>
    public static IEnumerable<CCSTeam?> GetTeams()
    {
        return Utilities.FindAllEntitiesByDesignerName<CCSTeam>("cs_team_manager");
    }

    /// <summary>
    /// Return a specified CsTeam's CCSTeam instance.
    /// </summary>
    /// <param name="csTeam">Team to want to obtain</param>
    /// <returns>Returns CCSTeam instance if found. Otherwise null</returns>
    public static CCSTeam? GetTeam(CsTeam csTeam)
    {
        var teams = GetTeams();

        foreach (CCSTeam? team in teams)
        {
            if(team?.TeamNum == (byte)csTeam)
                return team;
        }
        
        return null;
    }
}