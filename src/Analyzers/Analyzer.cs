using OoxmlToHtml.Abstracts;

namespace OoxmlToHtml.Analyzers
{
    public abstract class Analyzer : IAnalyzer
    {
        public IAnalyzer Next { get; set; }
        protected abstract INode Act(INode node);

        public virtual INode Analyze(INode rootNode)
        {
            var node = Act(rootNode);
            Next?.Analyze(node);

            return node;
        }
    }
}