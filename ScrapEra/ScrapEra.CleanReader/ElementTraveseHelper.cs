using System;
using System.Xml.Linq;

namespace ScrapEra.CleanReader
{
    internal class ElementTraveseHelper
    {
        private readonly Action<XElement> _elementVisitor;

        public ElementTraveseHelper(Action<XElement> elementVisitor)
        {
            if (elementVisitor == null)
            {
                throw new ArgumentNullException("elementVisitor");
            }

            _elementVisitor = elementVisitor;
        }

        public void Traverse(XElement element)
        {
            _elementVisitor(element);

            var childNode = element.FirstNode;

            while (childNode != null)
            {
                var nextChildNode = childNode.NextNode;

                var node = childNode as XElement;
                if (node != null)
                {
                    Traverse(node);
                }

                childNode = nextChildNode;
            }
        }
    }


    internal class ChildNodesTraverser
    {
        private readonly Action<XNode> _childNodeVisitor;

        public ChildNodesTraverser(Action<XNode> childNodeVisitor)
        {
            if (childNodeVisitor == null)
            {
                throw new ArgumentNullException("childNodeVisitor");
            }
            _childNodeVisitor = childNodeVisitor;
        }

        public void Traverse(XNode node)
        {
            if (!(node is XContainer))
            {
                throw new ApplicationException("node does not contain other nodes");
            }
            var childNode = ((XContainer) node).FirstNode;
            while (childNode != null)
            {
                var nextChildNode = childNode.NextNode;
                _childNodeVisitor(childNode);
                childNode = nextChildNode;
            }
        }
    }
}