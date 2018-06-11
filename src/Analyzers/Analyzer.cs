using OoxmlToHtml.Abstracts;

namespace OoxmlToHtml.Analyzers
{
    public abstract class Analyzer : IAnalyzer
    {
        public IAnalyzer Next { get; private set; }
        protected abstract INode Act(INode node);

        public virtual INode Analyze(INode rootNode)
        {
            var node = Act(rootNode);
            Next?.Analyze(node);

            return node;
        }

        public IAnalyzer Use(IAnalyzer next)
        {
            Next = next;
            return next;
        }
    }
}