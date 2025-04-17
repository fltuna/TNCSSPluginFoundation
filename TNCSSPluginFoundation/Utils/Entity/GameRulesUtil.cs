using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Entities.Constants;

namespace TNCSSPluginFoundation.Utils.Entity;

/// <summary>
/// Utility class for manipulate GameRules entity
/// </summary>
public class GameRulesUtil
{
    /// <summary>
    /// Get current round time
    /// </summary>
    /// <returns>Current round time, if failed to get, then returns -1</returns>
    public static int GetRoundTime()
    {
        var gameRules = EntityUtil.GetGameRules();

        if (gameRules == null)
            return -1;

        return gameRules.RoundTime;
    }

    /// <summary>
    /// Set current round time
    /// </summary>
    /// <param name="newRoundTime">The time to be updated</param>
    /// <returns>Updated time, if update failed it returns -1</returns>
    public static int SetRoundTime(int newRoundTime)
    {
        var gameRules = EntityUtil.GetGameRules();

        if (gameRules == null)
            return -1;

        gameRules.RoundTime = newRoundTime;
        var result = gameRules.RoundTime;
        Utilities.SetStateChanged(EntityUtil.GetGameRulesProxy()!, "CCSGameRulesProxy", "m_pGameRules");
        return result;
    }

    /// <summary>
    /// Check current game state is warmup or not
    /// </summary>
    /// <returns>Returns true if in warmup. Otherwise false</returns>
    public static bool IsWarmup()
    {
        return EntityUtil.GetGameRules()?.WarmupPeriod ?? false;
    }

    /// <summary>
    /// Check current state is freeze period (e.g. freeze time to buy weapons) or not
    /// </summary>
    /// <returns>Returns true if in freeze period. Otherwise false</returns>
    public static bool IsFreezePeriod()
    {
        return EntityUtil.GetGameRules()?.FreezePeriod ?? false;
    }



    /// <summary>
    /// Terminates current round
    /// </summary>
    /// <returns>Returns true if terminate initiation success. Otherwise false</returns>
    public static bool TerminateRound(float delay, RoundEndReason reason)
    {
        var gameRules = EntityUtil.GetGameRules();

        if (gameRules == null)
            return false;
        
        gameRules.TerminateRound(delay, reason);
        return true;
    }
}