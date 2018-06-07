using OoxmlToHtml.Abstracts;

namespace OoxmlToHtml.Analyzers
{
    public class ParagraphNodeAnalyzer : AttributeCopierAnalyzer
    {
        public override bool ShouldAnalyze(INode node)
        {
            return node.Type == KeywordToken.PreviousParagraph;
        }
    }
}