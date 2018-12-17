using System.Linq;
using OoxmlToHtml.Abstracts;
using OoxmlToHtml.Factories;

namespace OoxmlToHtml.Parsers
{
    public abstract class ElementNode : IStatementParser
    {
        protected OoxmlNodeTd parser;
        protected ElementNode(OoxmlNodeTd parser)
        {
            this.parser = parser.parser;
        }
        protected abstract KeywordToken AttributeName { get; }
        protected virtual KeywordToken[] IgnoredTokens => null;
        public virtual INode Parse(Token token)
        {
            var currentNode = NodeFactory.CreateNode(AttributeName);
            parser.NextToken();
            while (parser.CurrentToken.Keyword != KeywordToken.ENDING_ELEMENT
                   && parser.CurrentToken.Keyword != KeywordToken.EOF
                   && parser.CurrentToken.Keyword != KeywordToken.ShortClose)
            {
                IAttributeStatementParser attributeNode = null;
                switch (parser.CurrentToken.Keyword)
                {
                    // parse attributes
                    case KeywordToken.StringLiteral:
                        attributeNode = new UnknownStringLiteralAttribute(parser);
                        break;
                    case KeywordToken.Value:
                        attributeNode = new ValueAttribute(parser);
                        break;
                    default:
                        parser.NextToken();
                        break;
                }

                if (attributeNode != null)
                {
                    currentNode.SetAttribute(attributeNode.AttributeName,
                        attributeNode.Parse(parser.CurrentToken).GetAttribute("value"));
                }
            }

            if (parser.CurrentToken.Keyword == KeywordToken.ENDING_ELEMENT)
            {
                // if we are not at the end of the file or at the end tag of the of the current tag. go into the while
                // if we are at the end of the current tag, stop collecting children for the current tag
                while (parser.CurrentToken.Keyword != KeywordToken.EOF
                       && parser.CurrentToken.Keyword != KeywordToken.Close
                 )
                {
                    // we are in the body of the element
                    //parser.NextToken();
                    IStatementParser elementNode = null;
                    switch (parser.CurrentToken.Keyword)
                    {
                        case KeywordToken.StringLiteral:
                            elementNode = new StringLiteralStatementParser(parser);
                            break;
                        case KeywordToken.Paragraph:
                            elementNode = new ParagraphStatementParser(parser);
                            break;
                        case KeywordToken.PreviousParagraph:
                            elementNode = new PreviousParagraphStatementParser(parser);
                            break;
                        case KeywordToken.Color:
                            elementNode = new ColorStatementNode(parser);
                            break;
                        case KeywordToken.Text:
                            elementNode = new TextStatementParser(parser);
                            break;
                        case KeywordToken.Italic:
                            elementNode = new ItalicStatementParser(parser);
                            break;
                        case KeywordToken.ParagraphStyle:
                            elementNode = new ParagraphStyleStatementParser(parser);
                            break;
                        case KeywordToken.Unknown:
                            elementNode = new UnknownElementParser(parser);
                            break;
                        case KeywordToken.Bold:
                            elementNode = new GenericElementNode(parser, KeywordToken.Bold);
                            break;
                        case KeywordToken.Run:
                            elementNode = new GenericElementNode(parser, KeywordToken.Run);
                            break;
                        case KeywordToken.Size:
                            elementNode = new GenericElementNode(parser, KeywordToken.Size);
                            break;
                    }

                    if (elementNode != null)
                    {
                        currentNode.AddChild(elementNode.Parse(parser.CurrentToken));
                    }
                    else if (parser.CurrentToken.Keyword != KeywordToken.EOF
                        && parser.CurrentToken.Keyword != KeywordToken.Close)
                    {
                        parser.NextToken();
                    }
                }

                parser.NextToken();
            }
            return currentNode;
        }
    }
}