using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API.Modules.Cvars.Validators;
using Microsoft.Extensions.DependencyInjection;
using TNCSSPluginFoundation.Configuration;
using TNCSSPluginFoundation.Models.Logger;

namespace TNCSSPluginFoundation.Example;

/// <summary>
/// This framework provides SimpleDebugLogger base named AbstractDebugLoggerBase
/// But you can implement your own debug logger implment the TNCSSPluginFoundation.Models.Logger.IDebugLogger interface.
/// </summary>
public sealed class SimpleDebugLogger : AbstractDebugLoggerBase
{
    // You can use any ConVar name.
    public readonly FakeConVar<int> DebugLogLevelConVar = new("tncss_debug_level",
        "0: Nothing, 1: Print info, warn, error message, 2: Print previous one and debug message, 3: Print previous one and trace message", 0, ConVarFlags.FCVAR_NONE,
        new RangeValidator<int>(0, 3));
    
    // You can use any ConVar name.
    public readonly FakeConVar<bool> PrintToAdminClientsConsoleConVar = new("tncss_debug_show_console", "Debug message shown in client console?", false);
    
    // You can use any ConVar name.
    public readonly FakeConVar<string> RequiredFlagForPrintToConsoleConVar = new ("tncss_debug_console_print_required_flag", "Required flag for print to client console", "css/generic");

    
    public override int DebugLogLevel => DebugLogLevelConVar.Value;
    public override bool PrintToAdminClientsConsole => PrintToAdminClientsConsoleConVar.Value;
    public override string RequiredFlagForPrintToConsole => RequiredFlagForPrintToConsoleConVar.Value;

    // This is a log prefix.
    // Will print like this: `[TNCSSExample] [DEBUG] XXXXXXXX`
    public override string LogPrefix => "[TNCSSExample]";

    
    // Ssed for tracking ConVar.
    // Also, this ModuleName should be unique. Do not duplicate with any other module names in this plugin modules.
    private const string ModuleName = "DebugLogger";
    
    public SimpleDebugLogger(IServiceProvider serviceProvider)
    {
        // If you want to save these ConVar that defined in this class, then call ConVarConfigurationService::TrackConVar();
        var conVarService = serviceProvider.GetRequiredService<ConVarConfigurationService>();
        conVarService.TrackConVar(ModuleName, DebugLogLevelConVar);
        conVarService.TrackConVar(ModuleName, PrintToAdminClientsConsoleConVar);
        conVarService.TrackConVar(ModuleName, RequiredFlagForPrintToConsoleConVar);
    }
}