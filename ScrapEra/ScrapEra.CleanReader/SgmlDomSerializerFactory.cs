using System;
using System.Linq;
using System.Xml.Linq;

namespace ScrapEra.CleanReader
{
    public class SgmlDomSerializerFactory
    {
        public string SerializeDocument(XDocument document, DomSerializationParams domSerializationParams)
        {
            var documentRoot = document.Root;
            if (documentRoot == null)
            {
                throw new ArgumentException("The document must have a root.");
            }
            if (!"html".Equals(documentRoot.Name.LocalName, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("The document's root must be an html element.");
            }
            var headElement = documentRoot.GetChildrenByTagName("head").FirstOrDefault();
            if (headElement == null)
            {
                headElement = new XElement("head");
                documentRoot.AddFirst(headElement);
            }
            var result =
                document.ToString(domSerializationParams.PrettyPrint ? SaveOptions.None : SaveOptions.DisableFormatting);
            result =
                Constants.HtmlDocumentHeader + result;
            return result;
        }

        public string SerializeDocument(XDocument document)
        {
            return SerializeDocument(document, DomSerializationParams.CreateDefault());
        }
    }
}