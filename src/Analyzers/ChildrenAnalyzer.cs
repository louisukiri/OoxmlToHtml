using System.Linq;
using OoxmlToHtml.Abstracts;

namespace OoxmlToHtml.Analyzers
{
    public class ChildrenAnalyzer : Analyzer
    {
        protected override INode Act(INode node)
        {
            var children = node.Children.ToArray();
            foreach (var child in children)
            {
                Act(child);
                var parentNode = child.Parent;
                if (parentNode == null || parentNode.Type != child.Type) continue;
                var childAttributes = child.GetAllAttributes;
                if (childAttributes.Keys.Any(attribute => !parentNode.SetAttribute(attribute, childAttributes[attribute],
                    AttributeMergeStrategy.Merge)))
                {
                    return node;
                }
                parentNode.CopyChildren(child);
                parentNode.RemoveChild(child);
            }
            return node;
        }
    }
}