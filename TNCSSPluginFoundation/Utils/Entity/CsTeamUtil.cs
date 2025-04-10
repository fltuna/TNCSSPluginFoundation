using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Utils;

namespace TNCSSPluginFoundation.Utils.Entity;

/// <summary>
/// Utility class for manipulate CCSTeam entity
/// </summary>
public static class CsTeamUtil
{
    
    /// <summary>
    /// Set team name to specified string
    /// </summary>
    /// <param name="team">Team ID</param>
    /// <param name="teamName">The name of team</param>
    /// <returns>Returns true if successfully set. Otherwise false</returns>
    public static bool SetTeamName(CsTeam team, string teamName)
    {
        var teamEntity = EntityUtil.GetTeam(team);
        
        if (teamEntity == null)
            return false;
        
        teamEntity.Teamname = teamName;
        Utilities.SetStateChanged(teamEntity, "CTeam", "m_szTeamname");
        return true;
    }


    /// <summary>
    /// Set team score to specified value
    /// </summary>
    /// <param name="team">Team ID</param>
    /// <param name="score">The score of team</param>
    /// <returns>Returns true if successfully set. Otherwise false</returns>
    public static bool SetTeamScore(CsTeam team, int score)
    {
        var teamEntity = EntityUtil.GetTeam(team);
        if (teamEntity == null)
            return false;
        
        teamEntity.Score = score;
        Utilities.SetStateChanged(teamEntity, "CTeam", "m_iScore");
        return true;
    }

    
    public static bool SetTeamLogo(CsTeam team, string logo)
    {
        // TODO(): Investigate logo behaviour and implement
        return true;
    }
}