using System.Linq;
using OoxmlToHtml.Abstracts;

namespace OoxmlToHtml.Analyzers
{
    /*
     * Will copy all the attributes from all children to the parent
     * This will simplify the rendering of paragraphs as divs
     */
    public class AttributeCopierAnalyzer : Analyzer
    {
        public virtual bool ShouldAnalyze(INode node) => true;
        public virtual bool ShouldRemoveChild() => true;
        private int _level = 1;
        protected override INode Act(INode node)
        {
            _level++;
            var children = node.Children.ToArray();
            foreach (var child in children)
            {
                if (!ShouldAnalyze(child))
                {
                    continue;
                }
                if (child.Type == KeywordToken.Paragraph
                    || child.Type == KeywordToken.Run)
                {
                    _level = 1;
                }
                Act(child);
                if (_level <= 1)
                {
                    continue; ;
                }
                node.CopyAttributes(child);
                if (ShouldRemoveChild())
                {
                    node.RemoveChild(child);
                }
            }

            _level--;
            return node;
        }
    }
}