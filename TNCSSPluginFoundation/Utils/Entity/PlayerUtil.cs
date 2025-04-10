using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;

namespace TNCSSPluginFoundation.Utils.Entity;

/// <summary>
/// Utility class for manipulate player entity
/// </summary>
public static class PlayerUtil
{
    /// <summary>
    /// Check player is alive
    /// </summary>
    /// <param name="client">Client instance</param>
    /// <returns>Returns true if player alive. Otherwise false</returns>
    public static bool IsPlayerAlive(CCSPlayerController? client)
    {
        if (client == null)
            return false;
        
        var playerPawn = client.PlayerPawn.Value;
        
        if (playerPawn == null)
            return false;
        
        return playerPawn.LifeState == (byte)LifeState_t.LIFE_ALIVE;
    }
    
    /// <summary>
    /// Get player model path
    /// </summary>
    /// <param name="client">CCSPlayerController instance. This parameter shouldn't be null</param>
    /// <returns>Returns model name if found. Otherwise, empty string</returns>
    public static string GetPlayerModel(CCSPlayerController client)
    {
        if (client.PlayerPawn.Value == null)
            return string.Empty;

        CCSPlayerPawn playerPawn = client.PlayerPawn.Value;

        if (playerPawn.CBodyComponent?.SceneNode == null)
            return string.Empty;

        return playerPawn.CBodyComponent.SceneNode.GetSkeletonInstance().ModelState.ModelName;
    }

    /// <summary>
    /// Set player model
    /// </summary>
    /// <param name="client">CCSPlayerController instance. This parameter shouldn't be null</param>
    /// <param name="modelPath">Path to model end with .vmdl</param>
    /// <returns>Return true if successfully to set. Otherwise false</returns>
    public static bool SetPlayerModel(CCSPlayerController client, string modelPath)
    {
        if (client.PlayerPawn.Value == null)
            return false;
        
        client.PlayerPawn.Value.SetModel(modelPath);
        return true;
    }
    
    private static readonly string ServerConsoleName = $" {ChatColors.DarkRed}CONSOLE{ChatColors.Default}";
    
    /// <summary>
    /// Returns a name of player, If param is null, then returns CONSOLE
    /// This method is useful for replying command or broadcasting executor name.
    /// </summary>
    /// <param name="client">CCSPlayerController</param>
    /// <returns></returns>
    public static string GetPlayerName(CCSPlayerController? client)
    {
        if (client == null)
            return ServerConsoleName;
        
        return client.PlayerName;
    }


    /// <summary>
    /// Set player's health to specified value
    /// </summary>
    /// <param name="client">CCSPlayerController instance. This parameter shouldn't be null</param>
    /// <param name="health">The value of health</param>
    /// <returns>Return true if successfully to set. Otherwise false</returns>
    public static bool SetPlayerHealth(CCSPlayerController client, int health)
    {
        if (client.PlayerPawn.Value == null)
            return false;
        
        client.PlayerPawn.Value.Health = health;
        Utilities.SetStateChanged(client.PlayerPawn.Value!, "CBaseEntity", "m_iHealth");
        return true;
    }

    /// <summary>
    /// Set player's max health to specified value
    /// </summary>
    /// <param name="client">CCSPlayerController instance. This parameter shouldn't be null</param>
    /// <param name="maxHealth">Value of max health</param>
    /// <returns>Return true if successfully to set. Otherwise false</returns>
    public static bool SetPlayerMaxHealth(CCSPlayerController client, int maxHealth)
    {
        if (client.PlayerPawn.Value == null)
            return false;
        
        client.PlayerPawn.Value.MaxHealth = maxHealth;
        Utilities.SetStateChanged(client.PlayerPawn.Value!, "CBaseEntity", "m_iMaxHealth");
        return true;
    }

    /// <summary>
    /// Set player's kevlar to specified value
    /// </summary>
    /// <param name="client">CCSPlayerController instance. This parameter shouldn't be null</param>
    /// <param name="amount">Value of kevlar armor</param>
    /// <returns>Return true if successfully to set. Otherwise false</returns>
    public static bool SetPlayerKevlar(CCSPlayerController client, int amount)
    {
        if (client.PlayerPawn.Value == null)
            return false;
        
        client.PlayerPawn.Value!.ArmorValue = amount;
        Utilities.SetStateChanged(client.PlayerPawn.Value!, "CCSPlayerPawn", "m_ArmorValue");
        return true;
    }

    /// <summary>
    /// Set player's money to specified value
    /// </summary>
    /// <param name="client">CCSPlayerController instance. This parameter shouldn't be null</param>
    /// <param name="money">Value of player money</param>
    /// <returns>Return true if successfully to set. Otherwise false</returns>
    public static bool SetPlayerMoney(CCSPlayerController client, int money)
    {
        if (client.PlayerPawn.Value == null)
            return false;
        
        client.InGameMoneyServices!.Account = money;
        Utilities.SetStateChanged(client, "CCSPlayerController", "m_pInGameMoneyServices");
        return true;
    }

    /// <summary>
    /// Set player's BuyZone status to specified value
    /// </summary>
    /// <param name="client">CCSPlayerController instance. This parameter shouldn't be null</param>
    /// <param name="inBuyZone">Value of player BuyZone status</param>
    /// <returns>Return true if successfully to set. Otherwise false</returns>
    public static bool SetPlayerBuyZoneStatus(CCSPlayerController client, bool inBuyZone)
    {
        if (client.PlayerPawn.Value == null)
            return false;
        
        client.PlayerPawn.Value!.InBuyZone = inBuyZone;
        Utilities.SetStateChanged(client.PlayerPawn.Value!, "CCSPlayerPawn", "m_bInBuyZone");
        return true;
    }
}