using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Admin;
using TNCSSPluginFoundation.Interfaces;

namespace TNCSSPluginFoundation.Models.Logger;

/// <summary>
/// Convenient class for basic DebugLogging. <br/>
/// You can still make custom logger implementing the IDebugLogger.
/// </summary>
public abstract class AbstractDebugLoggerBase: IDebugLogger
{
    
    /// <summary>
    /// Level of debugging <br/>
    /// 0 Do nothing. <br/>
    /// Greater than 1: Info, warn and error logs <br/>
    /// Greater than 2: Debug logs <br/>
    /// Greater than 3: Trace logs <br/>
    /// </summary>
    public abstract int DebugLogLevel { get; }
    
    /// <summary>
    /// Should print the debug log to admin client console?
    /// </summary>
    public abstract bool PrintToAdminClientsConsole { get; }

    /// <summary>
    /// Required flag for recognize as an admin user. (e.g. "css/generic")
    /// </summary>
    public abstract string RequiredFlagForPrintToConsole { get; }
    
    /// <summary>
    /// Logging prefix<br/>
    /// When we set to "[DebugLogger]" then print result become like this -> "[DebugLogger] [INFO] xxxxxxxxxxx" <br/>
    /// So I suggest to name prefix not identical to another plugin that uses this foundation.
    /// </summary>
    public abstract string LogPrefix { get; }
    
    /// <summary>
    /// Simply print an Information log
    /// </summary>
    /// <param name="information">The message</param>
    public void LogInformation(string information)
    {
        if (DebugLogLevel < 1)
            return;
        
        PrintInformation("[INFO] ", information);
    }

    
    /// <summary>
    /// Simply print a Warning log
    /// </summary>
    /// <param name="information">The message</param>
    public void LogWarning(string information)
    {
        if (DebugLogLevel < 1)
            return;
        
        PrintInformation("[WARN] ", information);
    }

    
    /// <summary>
    /// Simply print an Error log
    /// </summary>
    /// <param name="information">The message</param>
    public void LogError(string information)
    {
        if (DebugLogLevel < 1)
            return;
        
        PrintInformation("[ERROR] ", information);
    }

    
    /// <summary>
    /// Simply print a Debug log
    /// </summary>
    /// <param name="information">The message</param>
    public void LogDebug(string information)
    {
        if (DebugLogLevel < 2)
            return;
        
        PrintInformation("[DEBUG] ", information);
    }

    
    /// <summary>
    /// Simply print a Trace log
    /// </summary>
    /// <param name="information">The message</param>
    public void LogTrace(string information)
    {
        if (DebugLogLevel < 3)
            return;
        
        PrintInformation("[TRACE] ", information);
    }


    private void PrintInformation(string debugLevelPrefix ,string information)
    {
        Server.NextFrame(() =>
        {
            string msg = $"{LogPrefix} {debugLevelPrefix} {information}";
        
            Server.PrintToConsole(msg);
        
            if (!PrintToAdminClientsConsole)
                return;
        
            foreach (var client in Utilities.GetPlayers())
            {
                if (client.IsBot || client.IsHLTV)
                    continue;
            
                if (!AdminManager.PlayerHasPermissions(client, RequiredFlagForPrintToConsole))
                    continue;

                client.PrintToConsole(msg);
            }
        });
    }
}