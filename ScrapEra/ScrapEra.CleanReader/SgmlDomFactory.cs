using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Sgml;

namespace ScrapEra.CleanReader
{
    public class SgmlDomFactory
    {
        public XDocument BuildDocument(string htmlContent)
        {
            if (htmlContent == null)
            {
                throw new ArgumentNullException("htmlContent");
            }
            if (htmlContent.Trim().Length == 0)
            {
                return new XDocument();
            }
            const string htmlEnd = "</html";
            var indexOfHtmlEnd = htmlContent.LastIndexOf(htmlEnd, StringComparison.Ordinal);
            if (indexOfHtmlEnd != -1)
            {
                var indexOfHtmlEndBracket = htmlContent.IndexOf('>', indexOfHtmlEnd);
                if (indexOfHtmlEndBracket != -1)
                {
                    htmlContent = htmlContent.Substring(0, indexOfHtmlEndBracket + 1);
                }
            }
            XDocument document;
            try
            {
                document = LoadDocument(htmlContent);
            }
            catch (InvalidOperationException exc)
            {
                if (!exc.Message.Contains("EndOfFile"))
                {
                    throw;
                }
                htmlContent = ScriptTagCleaner.RemoveScriptTags(htmlContent);
                document = LoadDocument(htmlContent);
            }
            return document;
        }

        private static XDocument LoadDocument(string htmlContent)
        {
            using (var sgmlReader = new SgmlReader())
            {
                sgmlReader.CaseFolding = CaseFolding.ToLower;
                sgmlReader.DocType = "HTML";
                sgmlReader.WhitespaceHandling = WhitespaceHandling.None;
                using (var sr = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(htmlContent))))
                {
                    sgmlReader.InputStream = sr;
                    var document = XDocument.Load(sgmlReader);
                    return document;
                }
            }
        }
    }
}