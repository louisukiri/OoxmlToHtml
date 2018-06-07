using System.Linq;
using OoxmlToHtml.Abstracts;

namespace OoxmlToHtml.Analyzers
{
    /*
     * Converts element types to attribute types by specifyingn
     */
    public class ElementToAttributeAnalyzer : Analyzer
    {
        protected override INode Act(INode node)
        {
            foreach (var child in node.Children)
            {
                node = Act(child);

                if (child.Type == KeywordToken.Color)
                if (child.Type == KeywordToken.Color)
                {
                    node.SetAttribute("fontColor", child.GetAttribute("value"));
                    child.RemoveAttribute("value");
                }

            }
            return node;
        }
    }
}