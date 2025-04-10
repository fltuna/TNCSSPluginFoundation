namespace TNCSSPluginFoundation.Interfaces;

/// <summary>
/// Interface for logging debug information.
/// </summary>
public interface IDebugLogger
{
    /// <summary>
    /// Print an Information log
    /// </summary>
    /// <param name="information">The message</param>
    public void LogInformation(string information);
    
    /// <summary>
    /// Print a Warning log
    /// </summary>
    /// <param name="information">The message</param>
    public void LogWarning(string information);
    
    /// <summary>
    /// Print an Error log
    /// </summary>
    /// <param name="information">The message</param>
    public void LogError(string information);
    
    /// <summary>
    /// Print a Debug log
    /// </summary>
    /// <param name="information">The message</param>
    public void LogDebug(string information);
    
    /// <summary>
    /// Print a Trace log
    /// </summary>
    /// <param name="information">The message</param>
    public void LogTrace(string information);
}