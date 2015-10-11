using System.Xml.Linq;

namespace ScrapEra.CleanReader
{
    public class AttributeTransformationInput
    {
        public string AttributeValue { get; set; }
        public XElement Element { get; set; }
    }

    public class AttributeTransformationResult
    {
        public string TransformedValue { get; set; }
        public string OriginalValueAttributeName { get; set; }
    }
}