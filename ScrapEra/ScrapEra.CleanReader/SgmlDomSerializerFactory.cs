using System;
using System.Linq;
using System.Xml.Linq;
using ScrapEra.Utils;

namespace ScrapEra.CleanReader
{
    public class SgmlDomSerializerFactory
    {
        public string SerializeDocument(XDocument document)
        {
            var documentRoot = document.Root;
            if (documentRoot == null)
            {
                throw new Exception("The document must have a root.");
            }
            if (!"html".Equals(documentRoot.Name.LocalName, StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("html node not found");
            }
            var headElement = documentRoot.GetChildrenByTagName("head").FirstOrDefault();
            if (headElement == null)
            {
                headElement = new XElement("head");
                documentRoot.AddFirst(headElement);
            }
            var result =
                document.ToString(SaveOptions.None);
            result =
                Constants.HtmlDocumentHeader + result;
            return result;
        }
    }
}