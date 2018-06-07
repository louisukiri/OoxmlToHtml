using System.Linq;
using OoxmlToHtml.Abstracts;

namespace OoxmlToHtml.Analyzers
{
    /*
     * Will copy all the attributes from all children to the parent
     * This will simplify the rendering of paragraphs as divs
     */
    public class AttributeCopierAnalyzer : IAnalyzer
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