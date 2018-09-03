using System.Linq;
using Castle.Core.Internal;
using OoxmlToHtml.Abstracts;

namespace OoxmlToHtml.Analyzers
{
    /*
     * Will copy all the attributes from all children to the parent
     * This will simplify the rendering of paragraphs as divs
     */
    public class AttributeCopierAnalyzer : Analyzer
    {
        public virtual bool ShouldAnalyze(INode node) => true;
        public virtual bool ShouldRemoveChild() => true;
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

            if (node.GetAllAttributes.IsNullOrEmpty())
                return node;
            // we don't want attributes propagating past Runs and paragraphs
            if (!ShouldAnalyze(node)
                || node.Type == KeywordToken.Paragraph
                || node.Type == KeywordToken.Run) return node;
            
            node.Parent?.CopyAttributes(node);
            // only remove empty nodes
            if (ShouldRemoveChild() && node.child == null)
                node.Parent?.RemoveChild(node);

            return node.Parent ?? node;
        }
    }
}