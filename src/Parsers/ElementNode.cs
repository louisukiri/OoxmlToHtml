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
        public virtual INode Parse(Token token, int level = 0)
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
                    parser.NextToken();
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
                        currentNode.AddChild(elementNode.Parse(parser.CurrentToken,
                            parser.CurrentToken.Keyword == currentNode.Type
                                ? level + 1
                                : level));
                    }
                    // if we are at the closing tag of the current parent (token) skip moving to the next token
                    // This has got to be able to tell a difference between the child end tag and a parent end tag that's exactly
                    //   the same
                    if (parser.CurrentToken.Keyword == KeywordToken.Close
                            && level > 0
                        // if you comig out of adding a child that has the same type as the parent it was added to,
                        //  you need to advance the NextToken so that it doesn't erroeously "continue" and evaluate the while
                        //  as the end of the parent hereby merging the nodes
                        //  The following checks will check for this
                            && currentNode.Children.Any()
                            && currentNode.Children.Last().Type != currentNode.Type
                        )
                    {
                        level--;
                        continue;
                    }
                    //parser.NextToken();
                }

                parser.NextToken();
            }
            return currentNode;
        }
    }
}