using OoxmlToHtml.Abstracts;
using OoxmlToHtml.Factories;

namespace OoxmlToHtml.Parsers
{
    public abstract class ElementNode : OoxmlNodeTd
    {
        public ElementNode(OoxmlNodeTd parent) : base(parent)
        {
        }
        protected abstract KeywordToken AttributeName { get; }
        public virtual INode Parse(Token token)
        {
            var m = NodeFactory.CreateNode(AttributeName);
            NextToken();
            while (CurrentToken.Keyword != KeywordToken.ENDING_ELEMENT
                   && CurrentToken.Keyword != KeywordToken.EOF
                   && CurrentToken.Keyword != KeywordToken.ShortClose)
            {
                switch (CurrentToken.Keyword)
                {
                    case KeywordToken.Value:
                        var a = new ValueStatementNode(this);
                        var b = a.Parse(CurrentToken);
                        m.SetAttribute("value", b.GetAttribute("value"));
                        b.SetParent(m);
                        break;
                }
                NextToken();
            }

            if (CurrentToken.Keyword == KeywordToken.ENDING_ELEMENT)
            {
                NextToken();
                while (CurrentToken.Keyword != KeywordToken.EOF
                       && CurrentToken.Keyword != KeywordToken.Close)
                {
                    switch (CurrentToken.Keyword)
                    {
                        case KeywordToken.StringLiteral:

                            break;
                        case KeywordToken.Paragraph:

                            break;
                        case KeywordToken.ParagraphStyle:

                            break;
                    }
                }
            }
            return m;
        }
    }
}