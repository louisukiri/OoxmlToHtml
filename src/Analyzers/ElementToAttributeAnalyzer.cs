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
            if (node.Child != null)
            {
                Act(node.Child);
            }

            if (node.Next != null)
            {
                Act(node.Next);
            }

            if (node.Parent == null) return node;

            switch (node.Type)
            {
                case KeywordToken.Color:
                    node.Parent.SetAttribute("fontColor", node.GetAttribute("value"));
                    node.Parent.RemoveChild(node);
                    break;
                case KeywordToken.Italic:
                    node.Parent.SetAttribute("italic", bool.TrueString);
                    node.Parent.RemoveChild(node);
                    break;
                case KeywordToken.ParagraphStyle:
                    node.Parent.SetAttribute("style", node.GetAttribute("value"));
                    node.Parent.RemoveChild(node);
                    break;
                case KeywordToken.Bold:
                    node.Parent.SetAttribute("bold", bool.TrueString);
                    node.Parent.RemoveChild(node);
                    break;
                case KeywordToken.Size:
                    node.Parent.SetAttribute("size", node.GetAttribute("value"));
                    break;
            }

            return node;
        }
    }
}