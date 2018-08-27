using System.Linq;
using OoxmlToHtml.Abstracts;

namespace OoxmlToHtml.Analyzers
{
    public class SiblingsAnalyzer : Analyzer
    {
        protected override INode Act(INode node)
        {
            //var previousChild = _children.LastOrDefault();
            //var childAttr = child.GetAllAttributes;
            //if (child.Type == KeywordToken.Run
            //    && previousChild != null 
            //    && previousChild.Type == child.Type
            //    && childAttr.Keys.All(attribute =>
            //        previousChild.CanSetAttribute(attribute, childAttr[attribute], AttributeMergeStrategy.Merge)))
            //{
            //    foreach (var childAttrKey in childAttr.Keys)
            //    {
            //        previousChild.SetAttribute(childAttrKey, childAttr[childAttrKey], AttributeMergeStrategy.Merge);
            //    }
            //    previousChild.CopyChildren(child);
            //    return previousChild;
            //}
            if (node.child != null)
            {
                Act(node.child);
            }

            if (node.Next != null)
            {
                Act(node.Next);
            }

            if (node.Previous == null) return node;
            var attributes = node.GetAllAttributes;
            if (node.Type == KeywordToken.Run
                && node.Previous.Type == node.Type
                && node.GetAllAttributes.Keys.All(attribute => 
                    node.Previous.CanSetAttribute(attribute, attributes[attribute], AttributeMergeStrategy.Merge))
                )
            {
                node.Previous.AddChild(node.child);
                node.Parent.RemoveChild(node);
            }
                        
            return node.Previous;
        }
    }
}