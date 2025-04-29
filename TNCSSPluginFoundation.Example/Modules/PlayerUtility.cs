using System.Runtime.Serialization;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Utils;
using TNCSSPluginFoundation.Models.Plugin;
using TNCSSPluginFoundation.Utils;
using TNCSSPluginFoundation.Utils.Entity;

namespace TNCSSPluginFoundation.Example.Modules;

public sealed class PlayerUtility(IServiceProvider serviceProvider) : PluginModuleBase(serviceProvider)
{
    public override string PluginModuleName => "PlayerUtility";

    public override string ModuleChatPrefix => "[PlayerUtility]";
    protected override bool UseTranslationKeyInModuleChatPrefix => false;


    protected override void OnInitialize()
    {
        Plugin.AddCommand("tncss_getaliveplayer", "Print the alive players", CommandGetAlivePlayer);
        Plugin.AddCommand("tncss_getdeadplayer", "Print the dead players", CommandGetDeadPlayer);
        Plugin.AddCommand("tncss_getplayersbyteam", "Print the players by team", CommandGetPlayersByTeam);
        Plugin.AddCommand("tncss_getplayersbyweapon", "Print the players by who have the specified weapon", CommandGetPlayersByWeapon);
    }

    protected override void OnUnloadModule()
    {
        Plugin.RemoveCommand("tncss_getaliveplayer", CommandGetAlivePlayer);
        Plugin.RemoveCommand("tncss_getdeadplayer", CommandGetDeadPlayer);
        Plugin.RemoveCommand("tncss_getplayersbyteam", CommandGetPlayersByTeam);
        Plugin.RemoveCommand("tncss_getplayersbyweapon", CommandGetPlayersByWeapon);
    }


    private void CommandGetAlivePlayer(CCSPlayerController? player, CommandInfo info)
    {
        if (info.ArgCount > 1)
        {
            if (!byte.TryParse(info.ArgByIndex(1), out byte team))
            {
                info.ReplyToCommand("Valid team range is 2-3");
                return;
            }

            if (team <= (byte)CsTeam.Spectator || team > (byte)CsTeam.CounterTerrorist)
            {
                info.ReplyToCommand("Valid team range is 2-3");
                return;
            }
            
            info.ReplyToCommand($"Alive players count: {PlayerUtil.GetAlivePlayers((CsTeam)team).Count}");
        }
        else
        {
            info.ReplyToCommand($"Alive players count: {PlayerUtil.GetAlivePlayers().Count}");
        }
    }


    private void CommandGetDeadPlayer(CCSPlayerController? player, CommandInfo info)
    {
        if (info.ArgCount > 1)
        {
            if (!byte.TryParse(info.ArgByIndex(1), out byte team))
            {
                info.ReplyToCommand("Valid team range is 2-3");
                return;
            }

            if (team <= (byte)CsTeam.Spectator || team > (byte)CsTeam.CounterTerrorist)
            {
                info.ReplyToCommand("Valid team range is 2-3");
                return;
            }
            
            info.ReplyToCommand($"Dead players count: {PlayerUtil.GetDeadPlayers((CsTeam)team).Count}");
        }
        else
        {
            info.ReplyToCommand($"Dead players count: {PlayerUtil.GetDeadPlayers().Count}");
        }
    }
    

    private void CommandGetPlayersByTeam(CCSPlayerController? player, CommandInfo info)
    {
        if (info.ArgCount < 2)
        {
            info.ReplyToCommand("Usage: tncss_getplayerbyteam <teamID>");
            return;
        }

        if (!byte.TryParse(info.ArgByIndex(1), out byte team))
        {
            info.ReplyToCommand("Valid team range is 0-3");
            return;
        }

        if (team > (byte)CsTeam.CounterTerrorist)
        {
            info.ReplyToCommand("Valid team range is 0-3");
            return;
        }
        
        info.ReplyToCommand($"Players count: {PlayerUtil.GetPlayersByTeam((CsTeam)team).Count}");
    }

    private void CommandGetPlayersByWeapon(CCSPlayerController? player, CommandInfo info)
    {
        if (info.ArgCount < 2)
        {
            info.ReplyToCommand("Usage: tncss_getplayerbyteam <weapon>");
            return;
        }
        
        string weaponName = info.ArgByIndex(1);
        info.ReplyToCommand($"Searching for weapon: '{weaponName}'");
        
        
        if (!EnumUtility.TryGetEnumByEnumMemberValue(weaponName, out CsItem item))
        {
            info.ReplyToCommand("Weapon not found");
            return;
        }
        
        info.ReplyToCommand($"Player counts who have {weaponName}: {PlayerUtil.GetPlayersBySpecificWeapon(item).Count}");
    }
}