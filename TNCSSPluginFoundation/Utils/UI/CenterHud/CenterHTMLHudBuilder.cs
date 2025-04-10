using System.Text;

namespace TNCSSPluginFoundation.Utils.UI.CenterHud;

/// <summary>
/// Builder for CenterHtmlHud html element.
/// </summary>
public class CenterHtmlHudBuilder
{
    private readonly StringBuilder _builder = new StringBuilder();


    private const CenterHtmlSize DefaultSize = CenterHtmlSize.M;
    private const string DefaultColorWhite = "#FFFFFF";
    private const string LineSeparatorText = "━━━━━━━━━━━━━━━━━━━━━━━━";
    
    
    private CenterHtmlHudBuilder() {}
    
    /// <summary>
    /// Create a new CenterHtmlHudBuilder
    /// </summary>
    /// <returns>new instance of CenterHtmlHudBuilder</returns>
    public static CenterHtmlHudBuilder Create()
    {
        return new CenterHtmlHudBuilder();
    }

    
    // ================== NORMAL TEXT ==================

    /// <summary>
    /// Append text to current builder.
    /// </summary>
    /// <param name="text">Text to append</param>
    /// <returns>Current builder instance</returns>
    public CenterHtmlHudBuilder Text(string text)
    {
        Text(text, DefaultSize, DefaultColorWhite);
        return this;
    }
    
    /// <summary>
    /// Append text to current builder.
    /// </summary>
    /// <param name="text">Text to append</param>
    /// <param name="size">Text size see <see cref="TNCSSPluginFoundation.Utils.UI.CenterHud.CenterHtmlSize"/></param>
    /// <returns>Current builder instance</returns>
    public CenterHtmlHudBuilder Text(string text, CenterHtmlSize size)
    {
        Text(text, size, DefaultColorWhite);
        return this;
    }
    
    /// <summary>
    /// Append text to current builder.
    /// </summary>
    /// <param name="text">Text to append</param>
    /// <param name="textColor">Hex color code. e.g. #FFFFFF</param>
    /// <returns>Current builder instance</returns>
    public CenterHtmlHudBuilder Text(string text, string textColor)
    {
        Text(text, DefaultSize, textColor);
        return this;
    }
    
    /// <summary>
    /// Append text to current builder.
    /// </summary>
    /// <param name="text">Text to append</param>
    /// <param name="size">Text size see <see cref="TNCSSPluginFoundation.Utils.UI.CenterHud.CenterHtmlSize"/></param>
    /// <param name="textColor">Hex color code. e.g. #FFFFFF</param>
    /// <returns>Current builder instance</returns>
    public CenterHtmlHudBuilder Text(string text, CenterHtmlSize size, string textColor)
    {
        _builder.Append($"<font color='{textColor}' class='fontSize-{size.ToLowerString()}'>{text}</font>");
        return this;
    }
    
    
    // ================== OTHER THINGS ==================

    
    /// <summary>
    /// Appends a new line.
    /// </summary>
    /// <returns>Current builder instance</returns>
    public CenterHtmlHudBuilder NewLine()
    {
        _builder.Append("<br>");
        return this;
    }


    /// <summary>
    /// Append a line separator text: "━━━━━━━━━━━━━━━━━━━━━━━━"
    /// </summary>
    /// <param name="size">Text size see <see cref="TNCSSPluginFoundation.Utils.UI.CenterHud.CenterHtmlSize"/></param>
    /// <returns>Current builder instance</returns>
    public CenterHtmlHudBuilder LineSeperator(CenterHtmlSize size)
    {
        LineSeparator(size, DefaultColorWhite);
        return this;
    }

    /// <summary>
    /// Append a line separator text: "━━━━━━━━━━━━━━━━━━━━━━━━"
    /// </summary>
    /// <param name="textColor">Hex color code. e.g. #FFFFFF</param>
    /// <returns>Current builder instance</returns>
    public CenterHtmlHudBuilder LineSeparator(string textColor)
    {
        LineSeparator(CenterHtmlSize.M, textColor);
        return this;
    }
    
    /// <summary>
    /// Append a line separator text: "━━━━━━━━━━━━━━━━━━━━━━━━"
    /// </summary>
    /// <param name="size">Text size see <see cref="TNCSSPluginFoundation.Utils.UI.CenterHud.CenterHtmlSize"/></param>
    /// <param name="textColor">Hex color code. e.g. #FFFFFF</param>
    /// <returns>Current builder instance</returns>
    public CenterHtmlHudBuilder LineSeparator(CenterHtmlSize size = DefaultSize, string textColor = DefaultColorWhite)
    {
        _builder.Append("<br>");
        Text(LineSeparatorText, size, textColor);
        _builder.Append("<br>");
        return this;
    }

    /// <summary>
    /// <br/>
    /// CAUTION: We cannot specify the picture size, so we need to resize the picture before showing in game.
    /// </summary>
    /// <param name="pictureUrl"></param>
    /// <returns>Current builder instance</returns>
    public CenterHtmlHudBuilder AddPicture(string pictureUrl)
    {
        _builder.Append($"<img src='{pictureUrl}'></img>");
        return this;
    }


    /// <summary>
    /// Build to string
    /// </summary>
    /// <returns>Result of built string</returns>
    public string Build()
    {
        return _builder.ToString();
    }
}