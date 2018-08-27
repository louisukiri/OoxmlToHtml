using System.Linq;
using OoxmlToHtml.Abstracts;

namespace OoxmlToHtml.Analyzers
{
    public class ChildrenAnalyzer : Analyzer
    {
        protected override INode Act(INode node)
        {
            if (node.child != null)
            {
                Act(node.child);
            }

            if (node.Next != null)
            {
                Act(node.Next);
            }

            if (node.Parent == null || node.Parent.Type != node.Type) return node;
            var attributes = node.GetAllAttributes;
            var attributesAreMergeable = attributes.Keys.All(attribute => node.Parent.CanSetAttribute(attribute,
                attributes[attribute],
                AttributeMergeStrategy.Merge));
            if (!attributesAreMergeable) return node;

            node.Parent.CopyAttributes(node);
            node.Parent.RemoveChild(node);
            node.Parent.AddChild(node.child);
            return node.Parent;
        }
    }
}