using System.Linq;
using OoxmlToHtml.Abstracts;

namespace OoxmlToHtml.Analyzers
{
    public class SiblingsAnalyzer : Analyzer
    {
        protected override INode Act(INode node)
        {
            //var children = node.Children.ToArray();
            
            //foreach (var child in children)
            //{
            //    Act(child);
            //    var previousSibling = child.Previous;
            //    if (previousSibling == null || previousSibling.Type != child.Type) continue;
            //    var childAttributes = child.GetAllAttributes;
            //    if (childAttributes.Keys.Any(attribute => !previousSibling.SetAttribute(attribute, childAttributes[attribute],
            //        AttributeMergeStrategy.Merge)))
            //    {
            //        return node;
            //    }
            //    previousSibling.CopyChildren(child);
            //    child.Parent.RemoveChild(child);
            //}
            return node;
        }
    }
}