using OoxmlToHtml.Abstracts;

namespace OoxmlToHtml.Analyzers
{
    public class ParagraphNodeAnalyzer : StyleAnalyzer
    {
        public override bool ShouldAnalyze(INode node)
        {
            return node.Type == KeywordToken.PreviousParagraph;
        }
    }
}