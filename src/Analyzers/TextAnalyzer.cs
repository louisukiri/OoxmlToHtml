using OoxmlToHtml.Abstracts;

namespace OoxmlToHtml.Analyzers
{
    public class TextAnalyzer : Analyzer
    {
        protected override INode Act(INode node)
        {
            return node;
        }
    }
}