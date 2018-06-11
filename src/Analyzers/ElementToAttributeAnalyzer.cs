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
                Act(child);

                switch (child.Type)
                {
                    case KeywordToken.Color:
                        node.SetAttribute("fontColor", child.GetAttribute("value"));
                        node.RemoveChild(child);
                        break;
                    case KeywordToken.Italic:
                        node.SetAttribute("italic", bool.TrueString);
                        node.RemoveChild(child);
                        break;
                    case KeywordToken.ParagraphStyle:
                        node.SetAttribute("style", child.GetAttribute("value"));
                        node.RemoveChild(child);
                        break;
                    case KeywordToken.Bold:
                        node.SetAttribute("bold", bool.TrueString);
                        node.RemoveChild(child);
                        break;
                }

            }
            return node;
        }
    }
}