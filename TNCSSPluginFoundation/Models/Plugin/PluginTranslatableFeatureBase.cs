using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace TNCSSPluginFoundation.Models.Plugin;

/// <summary>
/// Translation Feature Base
/// </summary>
/// <param name="serviceProvider"></param>
public class PluginTranslatableFeatureBase(IServiceProvider serviceProvider) : PluginBasicFeatureBase(serviceProvider)
{
    
    /// <summary>
    /// Helper method for sending localized text to all players.
    /// </summary>
    /// <param name="localizationKey">Language localization key</param>
    /// <param name="args">Any args that can be use ToString()</param>
    protected void PrintLocalizedChatToAll(string localizationKey, params object[] args)
    {
        foreach (CCSPlayerController client in Utilities.GetPlayers())
        {
            if (client.IsBot || client.IsHLTV)
                continue;
            
            client.PrintToChat(GetTextWithPluginPrefix(client, LocalizeString(client, localizationKey, args)));
        }
    }

    /// <summary>
    /// Prints message to server or player's chat
    /// </summary>
    /// <param name="player">Player Instance. if null message will print to server console</param>
    /// <param name="message">Message text</param>
    protected void PrintMessageToServerOrPlayerChat(CCSPlayerController? player, string message)
    {
        if (player == null)
            Server.PrintToConsole(message);
        else
            player.PrintToChat(message);
    }
    
    /// <summary>
    /// Helper method for obtain the localized text.
    /// </summary>
    /// <param name="player">Player instance, If null it will use server language</param>
    /// <param name="localizationKey">Language localization key</param>
    /// <param name="args">Any args that can be use ToString()</param>
    /// <returns></returns>
    protected string LocalizeWithPluginPrefix(CCSPlayerController? player, string localizationKey, params object[] args)
    {
        return GetTextWithPluginPrefix(player, LocalizeString(player, localizationKey, args));
    }

    /// <summary>
    /// Get text with plugin prefix.
    /// </summary>
    /// <param name="player">Player instance, If null it will use server language</param>
    /// <param name="text">original text</param>
    /// <returns>Text combined with original text and prefix, returns translated plugin prefix if Plugin.UseTranslationKeyInPluginPrefix is true</returns>
    protected string GetTextWithPluginPrefix(CCSPlayerController? player, string text)
    {
        if (!Plugin.UseTranslationKeyInPluginPrefix)
            return $"{Plugin.PluginPrefix} {text}";
        
        return $"{LocalizeString(player, Plugin.PluginPrefix)} {text}";
    }
}