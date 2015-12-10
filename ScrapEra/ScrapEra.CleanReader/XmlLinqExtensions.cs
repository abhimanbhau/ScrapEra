using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ScrapEra.CleanReader
{
    public static class XmlLinqExtensions
    {
        public static XElement GetBody(this XDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }
            var documentRoot = document.Root;
            if (documentRoot == null)
            {
                return null;
            }
            return documentRoot.GetElementsByTagName("body").FirstOrDefault();
        }

        public static string GetTitle(this XDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }
            var documentRoot = document.Root;
            if (documentRoot == null)
            {
                return null;
            }
            var headElement = documentRoot.GetElementsByTagName("head").FirstOrDefault();
            if (headElement == null)
            {
                return "";
            }
            var titleElement = headElement.GetChildrenByTagName("title").FirstOrDefault();
            if (titleElement == null)
            {
                return "";
            }
            return titleElement.Value.Trim();
        }

        public static string GetId(this XElement element)
        {
            return element.GetAttributeValue("id", "");
        }

        public static void SetId(this XElement element, string id)
        {
            element.SetAttributeValue("id", id);
        }

        public static string GetClass(this XElement element)
        {
            return element.GetAttributeValue("class", "");
        }

        public static void SetClass(this XElement element, string @class)
        {
            element.SetAttributeValue("class", @class);
        }

        public static void SetStyle(this XElement element, string style)
        {
            element.SetAttributeValue("style", style);
        }

        public static string GetAttributeValue(this XElement element, string attributeName, string defaultValue)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            if (string.IsNullOrEmpty(attributeName))
            {
                throw new ArgumentNullException("attributeName");
            }
            var attribute = element.Attribute(attributeName);
            return attribute != null
                ? attribute.Value
                : defaultValue;
        }

        public static string GetInnerHtml(this XContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            var resultSb = new StringBuilder();
            foreach (var childNode in container.Nodes())
            {
                resultSb.Append(childNode.ToString(SaveOptions.DisableFormatting));
            }
            return resultSb.ToString();
        }

        public static void SetInnerHtml(this XElement element, string html)
        {
            if (element == null || html == null)
            {
                throw new ArgumentNullException("element" + " | " + "html");
            }
            element.RemoveAll();
            var tmpElement = new SgmlDomFactory().BuildDocument(html);
            if (tmpElement.Root == null)
            {
                return;
            }
            foreach (var node in tmpElement.Root.Nodes())
            {
                element.Add(node);
            }
        }

        public static IEnumerable<XElement> GetElementsByTagName(this XContainer container, string tagName)
        {
            if (container == null || string.IsNullOrEmpty(tagName))
            {
                throw new ArgumentNullException("container" + " | " + "tagName");
            }
            return container.Descendants()
                .Where(e => tagName.Equals(e.Name.LocalName, StringComparison.OrdinalIgnoreCase));
        }

        public static IEnumerable<XElement> GetChildrenByTagName(this XContainer container, string tagName)
        {
            if (container == null || string.IsNullOrEmpty(tagName))
            {
                throw new ArgumentNullException("container" + " | " + "tagName");
            }
            return container.Elements()
                .Where(e => tagName.Equals(e.Name.LocalName, StringComparison.OrdinalIgnoreCase));
        }
    }
}