using System.Linq;
using OoxmlToHtml.Abstracts;

namespace OoxmlToHtml.Analyzers
{
    public class SiblingsAnalyzer : Analyzer
    {
        protected override INode Act(INode node)
        {
            if (node.Child != null)
            {
                Act(node.Child);
            }

            if (node.Next != null)
            {
                Act(node.Next);
            }

            if (node.Previous == null) return node;
            var attributes = node.GetAllAttributes;
            if (node.Type != KeywordToken.Paragraph
                && node.Previous.Type == node.Type
                && node.GetAllAttributes.Keys.All(attribute => 
                    node.Previous.CanSetAttribute(attribute, attributes[attribute], AttributeMergeStrategy.Merge))
                && node.Child != null
                )
            {
                    node.Previous.AddChild(node.Child);
                    node.Parent.RemoveChild(node);
                    Act(node.Previous.Child);
            }
                        
            return node.Previous;
        }
    }
}