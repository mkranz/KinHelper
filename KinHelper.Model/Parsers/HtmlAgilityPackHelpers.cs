using HtmlAgilityPack;

namespace KinHelper.Model.Parsers
{
    public static class HtmlAgilityPackHelpers
    {
         public static string GetFirstClass(this HtmlNode node)
         {
             var clazz = node.GetAttributeValue("class", null);
             if (clazz == null)
                 return null;
             return clazz.Split(' ')[0];
         }
    }
}