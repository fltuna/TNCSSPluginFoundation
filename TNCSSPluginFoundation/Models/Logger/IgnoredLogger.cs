namespace TNCSSPluginFoundation.Models.Logger;

/// <summary>
/// This is a IgnoredLogger <br/>
/// If there is no custom IDebugLogger implementation, then this class is used, and does nothing.
/// </summary>
public sealed class IgnoredLogger: AbstractDebugLoggerBase
{
    
    /// <summary>
    /// See <see cref="TNCSSPluginFoundation.Models.Logger.AbstractDebugLoggerBase.DebugLogLevel"/>
    /// </summary>
    public override int DebugLogLevel => 0;
    
    /// <summary>
    /// See <see cref="TNCSSPluginFoundation.Models.Logger.AbstractDebugLoggerBase.PrintToAdminClientsConsole"/>
    /// </summary>
    public override bool PrintToAdminClientsConsole => false;
    
    
    /// <summary>
    /// See <see cref="TNCSSPluginFoundation.Models.Logger.AbstractDebugLoggerBase.RequiredFlagForPrintToConsole"/>
    /// </summary>
    public override string RequiredFlagForPrintToConsole => "";
    
    
    /// <summary>
    /// See <see cref="TNCSSPluginFoundation.Models.Logger.AbstractDebugLoggerBase.LogPrefix"/>
    /// </summary>
    public override string LogPrefix => "";
}