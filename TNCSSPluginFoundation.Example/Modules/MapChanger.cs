using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using Microsoft.Extensions.DependencyInjection;
using TNCSSPluginFoundation.Models.Plugin;
using TNCSSPluginFoundation.Utils.Other;

namespace TNCSSPluginFoundation.Example.Modules;

public class MapChanger(IServiceProvider serviceProvider) : PluginModuleBase(serviceProvider)
{
    public override string PluginModuleName => "MapChanger";
        
    public override string ModuleChatPrefix => "[MapChanger]";




    protected override void OnInitialize()
    {
        Plugin.AddCommand("tncss_currentmap", "prints a current map", CommandCurrentMap);
        Plugin.AddCommand("tncss_changemap", "change map", CommandChangeMap);
        Plugin.AddCommand("tncss_reloadmap", "reload map",CommandReloadMap);
    }

    protected override void OnUnloadModule()
    {
        Plugin.RemoveCommand("tncss_currentmap", CommandCurrentMap);
        Plugin.RemoveCommand("tncss_changemap", CommandChangeMap);
        Plugin.RemoveCommand("tncss_reloadmap", CommandReloadMap);
    }


    private void CommandCurrentMap(CCSPlayerController? player, CommandInfo info)
    {
        info.ReplyToCommand(ForceFullUpdate.GetWorkshopId());
    }
    
    private void CommandChangeMap(CCSPlayerController? player, CommandInfo info)
    {
        if (info.ArgCount < 2)
        {
            info.ReplyToCommand("Usage: tncss_changemap <mapName|workshopID>");
            return;
        }

        string mapIdentifier = info.ArgByIndex(1);
        
        Server.PrintToChatAll($"map identifier: {mapIdentifier}");

        if (MapUtil.ChangeMap(mapIdentifier))
        {
            info.ReplyToCommand("Map changed");
            return;
        }
        
        info.ReplyToCommand("Failed to change map");
    }

    private void CommandReloadMap(CCSPlayerController? player, CommandInfo info)
    {
        MapUtil.ReloadMap();
    }
}