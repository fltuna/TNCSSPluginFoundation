using CounterStrikeSharp.API;

namespace TNCSSPluginFoundation.Utils.Other;

/// <summary>
/// 
/// </summary>
public static class MapUtil
{
    private static HashSet<string> _officialMaps = new()
    {
        "de_dust2",
    };
    
    /// <summary>
    /// Returns current map workshop ID
    /// </summary>
    /// <returns>Returns workshop id if server in workshop map, Otherwise returns -1</returns>
    public static long GetCurrentMapWorkshopId()
    {
        if (_officialMaps.Contains(Server.MapName))
            return -1;

        // If failed to obtain workshop id or not valid, then return -1
        if (!long.TryParse(ForceFullUpdate.GetWorkshopId(), out long result))
            return -1;
        
        return result;
    }

    /// <summary>
    /// Tries to reload the map.
    /// But this method is not ensure to reloading the workshop map.
    /// </summary>
    public static void ReloadMap()
    {
        string mapName = Server.MapName;

        if (_officialMaps.Contains(mapName))
        {
            Server.ExecuteCommand($"changelevel {mapName}");
            return;
        }

        Server.PrintToConsole(GetCurrentMapWorkshopId().ToString());
        bool executed =  ChangeToWorkshopMap(GetCurrentMapWorkshopId());
        Server.PrintToConsole($"{executed}");
    }

    /// <summary>
    /// Change map to official or workshop map.
    /// We will try to change map to official maps, but if failed it will try to change to workshop map as fallback.
    /// This method is not ensure to changing to workshop map.
    /// </summary>
    /// <param name="map">Map name or workshop ID</param>
    /// <returns>Returns true if map change command executed, Otherwise false</returns>
    public static bool ChangeMap(string map)
    {
        // If map name is official map, then change to official map
        if (_officialMaps.Contains(map))
        {
            Server.ExecuteCommand($"changelevel {map}");
            return true;
        }


        bool executed = ChangeToWorkshopMap(map);

        if (executed)
            return true;
            
        // If map name is not ID, then return false.
        if (!long.TryParse(map, out long result))
            return false;
        
        
        executed = ChangeToWorkshopMap(result);
        
        return executed;
    }

    /// <summary>
    /// Change map to workshop map using workshop ID.
    /// This method is not ensure to changing to workshop map.
    /// To change to map, you should specify the CORRECT workshop ID.
    /// </summary>
    /// <param name="workshopId">Workshop ID of map</param>
    /// <returns>Returns true if map change command executed, Otherwise false</returns>
    public static bool ChangeToWorkshopMap(long workshopId)
    {
        if (workshopId < 0)
            return false;
        
        Server.ExecuteCommand($"host_workshop_map {workshopId}");
        return true;
    }

    
    /// <summary>
    /// Change map to workshop map using workshop map name.
    /// This method is not ensure to changing to workshop map.
    /// To change to map, you should specify the CORRECT workshop map name.
    /// Also, workshop map name is may conflict with other workshop map, so We recommend to use workshopID version instead.
    /// </summary>
    /// <param name="mapName">The workshop map name</param>
    /// <returns>Returns true if map change command executed, Otherwise, like specify the official map's name(e.g. de_dust2, de_mirage...) will false</returns>
    public static bool ChangeToWorkshopMap(string mapName)
    {
        if (_officialMaps.Contains(mapName))
            return false;
        
        Server.ExecuteCommand($"ds_workshop_changelevel {mapName}");
        return true;
    }
}