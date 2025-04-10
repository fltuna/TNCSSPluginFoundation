namespace TNCSSPluginFoundation.Utils.UI.CenterHud;

/// <summary>
/// Simple extension class for CenterHtmlSize
/// </summary>
public static class CenterHtmlSizeExtensions
{
    /// <summary>
    /// Lowers CenterHtmlSize enum text.
    /// </summary>
    /// <param name="size">CenterHtmlSize</param>
    /// <returns>Lowered size text</returns>
    public static string ToLowerString(this CenterHtmlSize size)
    {
        return size.ToString().ToLower();
    }
}


/// <summary>
///
/// Size data of CenterHTMLHud text<br/>
/// These text can be used as:
/// 
/// <![CDATA[
/// <font class='fontSize-{CenterHtmlSize.ToLowerString()}'></font>
/// ]]>
/// </summary>
public enum CenterHtmlSize
{
    /// <summary>
    /// .fontSize-xxxl
    /// {
    /// font-size: 64px;
    /// }
    /// </summary>
    XXXL,
    
    /// <summary>
    /// .fontSize-xxl
    /// {
    /// font-size: 40px;
    /// }
    /// </summary>
    XXL,
    
    /// <summary>
    /// .fontSize-xl
    /// {
    /// font-size: 32px;
    /// }
    /// </summary>
    XL,
    
    /// <summary>
    /// .fontSize-l
    /// {
    /// font-size: 24px;
    /// }
    /// </summary>
    L,
    
    /// <summary>
    /// .fontSize-ml
    /// {
    /// font-size: 20px;
    /// }
    /// </summary>
    ML,
    
    /// <summary>
    /// .fontSize-m
    /// {
    /// font-size: 18px;
    /// }
    /// </summary>
    M,
    
    /// <summary>
    /// .fontSize-sm
    /// {
    /// font-size: 16px;
    /// }
    /// </summary>
    SM,
    
    /// <summary>
    /// .fontSize-s
    /// {
    /// font-size: 12px;
    /// }
    /// </summary>
    S,
    
    /// <summary>
    /// .fontSize-xs
    /// {
    /// font-size: 8px;
    /// }
    /// </summary>
    XS,
}