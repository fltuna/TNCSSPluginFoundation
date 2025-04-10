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
        if (team != CsTeam.Terrorist && team != CsTeam.CounterTerrorist)
            return false;

        string cmd;
        if (team == CsTeam.CounterTerrorist)
        {
            cmd = $"mp_teamname_1 {teamName}";
        }
        else
        {
            cmd = $"mp_teamname_2 {teamName}";
        }
        
        Server.ExecuteCommand(cmd);
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
        if (team != CsTeam.Terrorist && team != CsTeam.CounterTerrorist)
            return false;
        
        var teamEntity = EntityUtil.GetTeam(team);
        if (teamEntity == null)
            return false;
        
        teamEntity.Score = score;
        Utilities.SetStateChanged(teamEntity, "CTeam", "m_iScore");
        return true;
    }

    
    /// <summary>
    /// Set team logo to specified string
    /// </summary>
    /// <param name="team">Team ID</param>
    /// <param name="logo">The name of logo</param>
    /// <returns>Returns true if successfully set. Otherwise false</returns>
    public static bool SetTeamLogo(CsTeam team, string logo)
    {
        if (team != CsTeam.Terrorist && team != CsTeam.CounterTerrorist)
            return false;

        string cmd;
        if (team == CsTeam.CounterTerrorist)
        {
            cmd = $"mp_teamlogo_1 {logo}";
        }
        else
        {
            cmd = $"mp_teamlogo_2 {logo}";
        }
        
        Server.ExecuteCommand(cmd);
        
        return true;
    }
}