using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace TNCSSPluginFoundation.Utils.Entity;

/// <summary>
/// Utility class for manipulate game entity
/// </summary>
public static class EntityUtil
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns>Returns CCSGameRules instance if found. Otherwise null</returns>
    public static CCSGameRules? GetGameRules()
    {
        return Utilities.FindAllEntitiesByDesignerName<CCSGameRulesProxy>("cs_gamerules").First().GameRules;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Returns CCSGameRulesProxy</returns>
    public static CCSGameRulesProxy GetGameRulesProxy()
    {
        return Utilities.FindAllEntitiesByDesignerName<CCSGameRulesProxy>("cs_gamerules").First();
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

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Returns a CCSTeam instances.</returns>
    public static IEnumerable<CCSTeam?> GetTeams()
    {
        return Utilities.FindAllEntitiesByDesignerName<CCSTeam>("cs_team_manager");
    }
}