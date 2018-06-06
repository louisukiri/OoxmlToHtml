using System.Linq;
using OoxmlToHtml.Abstracts;

namespace OoxmlToHtml.Analyzers
{
    public class StyleAnalyzer : IAnalyzer
    {
        public IAnalyzer Next { get; set; }
        public virtual bool ShouldAnalyze(INode node) => true;
        public INode Analyze(INode node)
        {
            var children = node.Children.ToArray();
            foreach (var child in children)
            {
                if (!ShouldAnalyze(child))
                {
                    continue;
                }
                Analyze(child);
                node.CopyAttributes(child);
                node.RemoveChild(child);
            }
            return node;
        }
    }
}