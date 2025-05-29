using System.Runtime.CompilerServices;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
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
    /// <param name="client">Target CCSPlayerController instance. This parameter shouldn't be null</param>
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
    /// <param name="client">Target CCSPlayerController instance. This parameter shouldn't be null</param>
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
    /// <param name="client">Target CCSPlayerController</param>
    /// <returns></returns>
    public static string GetPlayerName(CCSPlayerController? client)
    {
        if (client == null)
            return ServerConsoleName;
        
        return client.PlayerName;
    }

    /// <summary>
    /// Set player's name
    /// </summary>
    /// <param name="client">Target CCSPlayerController</param>
    /// <param name="playerName">Name of player</param>
    public static void SetPlayerName(CCSPlayerController client, string playerName)
    {
        client.PlayerName = playerName;
        Utilities.SetStateChanged(client, "CBasePlayerController", "m_iszPlayerName");

        var fakeEvent = new EventNextlevelChanged(false);
        fakeEvent.FireEvent(false);
    }

    
    /// <summary>
    /// Set player's clan tag
    /// </summary>
    /// <param name="client">Target CCSPlayerController</param>
    /// <param name="playerClanTag">Tag name string</param>
    public static void SetPlayerClanTag(CCSPlayerController client, string playerClanTag)
    {
        client.Clan = playerClanTag;
        Utilities.SetStateChanged(client, "CCSPlayerController", "m_szClan");

        var fakeEvent = new EventNextlevelChanged(false);
        fakeEvent.FireEvent(false);
    }

    /// <summary>
    /// Set player's team
    /// </summary>
    /// <param name="client">Target CCSPlayerController</param>
    /// <param name="playerTeam"></param>
    public static void SetPlayerTeam(CCSPlayerController client, CsTeam playerTeam)
    {
        if (client.TeamNum == (byte)playerTeam)
            return;
        
        Server.NextFrame(() =>
        {
            client.ChangeTeam(playerTeam);
        });
    }

    /// <summary>
    /// Set player's health to specified value
    /// </summary>
    /// <param name="client">Target CCSPlayerController instance. This parameter shouldn't be null</param>
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
    /// <param name="client">Target CCSPlayerController instance. This parameter shouldn't be null</param>
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
    /// Set player's armor to specified value
    /// </summary>
    /// <param name="client">Target CCSPlayerController instance. This parameter shouldn't be null</param>
    /// <param name="amount">Value of kevlar armor</param>
    /// <param name="hasHelmet">Boolean to specify player should have a helmet</param>
    /// <param name="hasHeavyArmor">Boolean to specify player should have a heavy armor</param>
    /// <returns>Return true if successfully to set. Otherwise false</returns>
    public static bool SetPlayerArmor(CCSPlayerController client, int amount, bool hasHelmet = false, bool hasHeavyArmor = false)
    {
        if (client.PlayerPawn.Value == null)
            return false;
        
        client.PlayerPawn.Value.ArmorValue = amount;
        Utilities.SetStateChanged(client.PlayerPawn.Value!, "CCSPlayerPawn", "m_ArmorValue");
        
        
        if (!hasHelmet && !hasHeavyArmor)
            return true;
        
        if (client.PlayerPawn.Value.ItemServices == null)
            return false;
        
        
        _ = new CCSPlayer_ItemServices(client.PlayerPawn.Value.ItemServices.Handle)
        {
            HasHelmet = hasHelmet,
            HasHeavyArmor = hasHeavyArmor
        };
        Utilities.SetStateChanged(client.PlayerPawn.Value, "CBasePlayerPawn", "m_pItemServices");
        return true;
    }

    /// <summary>
    /// Set player's money to specified value
    /// </summary>
    /// <param name="client">Target CCSPlayerController instance. This parameter shouldn't be null</param>
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
    /// <param name="client">Target CCSPlayerController instance. This parameter shouldn't be null</param>
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


    /// <summary>
    /// Get alive players in the server
    /// </summary>
    /// <returns>Returns alive players list, if team is out of range then empty list.</returns>
    public static List<CCSPlayerController> GetAlivePlayers(CsTeam team = CsTeam.None)
    {
        if (team is CsTeam.None or CsTeam.Spectator)
            return [];

        return Utilities.GetPlayers().Where(p => IsPlayerAlive(p) && p.Connected == PlayerConnectedState.PlayerConnected).ToList();
    }

    /// <summary>
    /// Get dead players in the server
    /// </summary>
    /// <returns>Returns alive players list, if team is out of range then empty list.</returns>
    public static List<CCSPlayerController> GetDeadPlayers(CsTeam team = CsTeam.None)
    {
        if (team is CsTeam.None or CsTeam.Spectator)
            return [];

        return Utilities.GetPlayers().Where(p => !IsPlayerAlive(p)).ToList();
    }

    /// <summary>
    /// Get players by team
    /// </summary>
    /// <param name="team">Team ID</param>
    /// <returns>Returns players list of specified team.</returns>
    public static List<CCSPlayerController> GetPlayersByTeam(CsTeam team)
    {
        return Utilities.GetPlayers().Where(p => p.Team == team).ToList();
    }

    /// <summary>
    /// Get players by who have specific weapon
    /// </summary>
    /// <param name="item">Weapon</param>
    /// <returns>Returns players list who have specified item</returns>
    public static List<CCSPlayerController> GetPlayersBySpecificWeapon(CsItem item)
    {
        List<CCSPlayerController> result = [];
        
        foreach (CCSPlayerController player in Utilities.GetPlayers())
        {
            if (player.PlayerPawn.Value == null)
                continue;
            
            if (player.PlayerPawn.Value.WeaponServices == null)
                continue;
            
            var weaponServices = new CCSPlayer_WeaponServices(player.PlayerPawn.Value.WeaponServices.Handle);
            
            foreach (CHandle<CBasePlayerWeapon> weapon in weaponServices.MyWeapons)
            {
                var weaponItem = weapon.Get();
                if (weaponItem == null)
                {
                    continue;
                }
            
            
                var weaponData = weaponItem.VData;
                if (weaponData == null)
                {
                    continue;
                }
        
                if(weaponItem.DesignerName != EnumUtils.GetEnumMemberAttributeValue(item))
                    continue;
                
                result.Add(player);
            }
        }

        return result;
    }

    /// <summary>
    /// Show progress bar hud to player <br/>
    /// You should call <see cref="RemoveProgressBarHud"/> to remove progress bar hud, Otherwise hud will remain on player's screen.
    /// </summary>
    /// <param name="client">Target CCSPlayerController instance. This parameter shouldn't be null</param>
    /// <param name="durationSeconds">Countdown count specify with seconds</param>
    /// <param name="action">At this time, we can only use k_CSPlayerBlockingUseAction_None, otherwise hud will not appear to client</param>
    public static void ShowProgressBarHud(CCSPlayerController client, int durationSeconds, CSPlayerBlockingUseAction_t action = CSPlayerBlockingUseAction_t.k_CSPlayerBlockingUseAction_None)
    {
        if (client.PlayerPawn.Value == null)
            return;
        
        var pawn = client.PlayerPawn.Value;

        float currentTime = Server.CurrentTime;

        pawn.SimulationTime = currentTime + durationSeconds;
        pawn.ProgressBarDuration = durationSeconds;
        pawn.ProgressBarStartTime = currentTime;
        pawn.BlockingUseActionInProgress = action;
        
        Utilities.SetStateChanged(pawn, "CBaseEntity", "m_flSimulationTime");
        Utilities.SetStateChanged(pawn, "CCSPlayerPawnBase", "m_iProgressBarDuration");
        Utilities.SetStateChanged(pawn, "CCSPlayerPawnBase", "m_flProgressBarStartTime");
        Utilities.SetStateChanged(pawn, "CCSPlayerPawn", "m_iBlockingUseActionInProgress");
    }

    /// <summary>
    /// Remove progress bar hud from player. if player is not showing any progress bar hud, this method will do nothing.
    /// </summary>
    /// <param name="client">Target CCSPlayerController instance. This parameter shouldn't be null</param>
    public static void RemoveProgressBarHud(CCSPlayerController client)
    {
        if (client.PlayerPawn.Value == null)
            return;
        
        var pawn = client.PlayerPawn.Value;
    
        pawn.ProgressBarDuration = 0;
        pawn.ProgressBarStartTime = 0.0f;
        
        Utilities.SetStateChanged(pawn, "CCSPlayerPawnBase", "m_iProgressBarDuration");
        Utilities.SetStateChanged(pawn, "CCSPlayerPawnBase", "m_flProgressBarStartTime");
    }
}