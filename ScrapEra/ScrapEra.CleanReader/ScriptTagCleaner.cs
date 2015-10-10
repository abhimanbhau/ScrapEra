using System;

namespace ScrapEra.CleanReader
{
    public static class ScriptTagCleaner
    {
        public static string RemoveScriptTags(string htmlContent)
        {
            if (htmlContent == null)
            {
                throw new ArgumentNullException("htmlContent");
            }
            if (htmlContent.Length == 0)
            {
                return string.Empty;
            }
            var indexOfScriptTagStart = htmlContent.IndexOf(Constants.ScriptTag, StringComparison.OrdinalIgnoreCase);
            if (indexOfScriptTagStart == -1)
            {
                return htmlContent;
            }
            var indexOfScriptTagEnd = htmlContent.IndexOf(Constants.ScriptEndTag, indexOfScriptTagStart,
                StringComparison.OrdinalIgnoreCase);
            if (indexOfScriptTagEnd == -1)
            {
                return htmlContent.Substring(0, indexOfScriptTagStart);
            }
            var strippedHtmlContent =
                htmlContent.Substring(0, indexOfScriptTagStart) +
                htmlContent.Substring(indexOfScriptTagEnd + Constants.ScriptEndTagLength);
            return RemoveScriptTags(strippedHtmlContent);
        }
    }
}