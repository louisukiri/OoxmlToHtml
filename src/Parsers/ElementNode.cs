using System.Linq;
using OoxmlToHtml.Abstracts;
using OoxmlToHtml.Factories;

namespace OoxmlToHtml.Parsers
{
    public abstract class ElementNode : OoxmlNodeTd, IStatementParser
    {
        protected ElementNode(OoxmlNodeTd parent) : base(parent)
        {
        }
        protected abstract KeywordToken AttributeName { get; }
        protected virtual KeywordToken[] IgnoredTokens => null;
        public virtual INode Parse(Token token)
        {
            var currentNode = NodeFactory.CreateNode(AttributeName);
            NextToken();
            while (CurrentToken.Keyword != KeywordToken.ENDING_ELEMENT
                   && CurrentToken.Keyword != KeywordToken.EOF
                   && CurrentToken.Keyword != KeywordToken.ShortClose)
            {
                IAttributeStatementParser attributeNode = null;
                switch (CurrentToken.Keyword)
                {
                    // parse attributes
                    case KeywordToken.StringLiteral:
                        attributeNode = new UnknownStringLiteralAttribute(this);
                        break;
                    case KeywordToken.Value:
                        attributeNode = new ValueAttribute(this);
                        break;
                    default:
                        NextToken();
                        break;
                }

                if (attributeNode != null)
                {
                    currentNode.SetAttribute(attributeNode.AttributeName,
                        attributeNode.Parse(CurrentToken).GetAttribute("value"));
                }
            }

            if (CurrentToken.Keyword == KeywordToken.ENDING_ELEMENT)
            {
                // we are in the body of the element
                NextToken();
                // if we are not at the end of the file or at the end tag of the of the current tag. go into the while
                // if we are at the end of the current tag, stop collecting children for the current tag
                while (CurrentToken.Keyword != KeywordToken.EOF
                        && (!(CurrentToken.Keyword == KeywordToken.Close
                        && CurrentToken.Text == token.Text))
                       )
                {
                    IStatementParser elementNode = null;
                    switch (CurrentToken.Keyword)
                    {
                        case KeywordToken.StringLiteral:
                            elementNode = new StringLiteralStatementParser(this);
                            break;
                        case KeywordToken.Paragraph:
                            elementNode = new ParagraphStatementParser(this);
                            break;
                        case KeywordToken.PreviousParagraph:
                            elementNode = new PreviousParagraphStatementParser(this);
                            break;
                        case KeywordToken.Color:
                            elementNode = new ColorStatementNode(this);
                            break;
                        case KeywordToken.Text:
                            elementNode = new TextStatementParser(this);
                            break;
                        case KeywordToken.Italic:
                            elementNode = new ItalicStatementParser(this);
                            break;
                        case KeywordToken.ParagraphStyle:
                            elementNode = new ParagraphStyleStatementParser(this);
                            break;
                        case KeywordToken.Unknown:
                            elementNode = new UnknownElementParser(this);
                            break;
                        case KeywordToken.Bold:
                            elementNode = new GenericElementNode(this, KeywordToken.Bold);
                            break;
                        case KeywordToken.Run:
                            elementNode = new GenericElementNode(this, KeywordToken.Run);
                            break;
                        case KeywordToken.Size:
                            elementNode = new GenericElementNode(this, KeywordToken.Size);
                            break;
                    }

                    if (elementNode != null)
                    {
                        currentNode.AddChild(elementNode.Parse(CurrentToken));
                    }

                    // if we are at the closing tag of the current parent (token) skip moving to the next token
                    // This has got to be able to tell a difference between the child end tag and a parent end tag that's exactly
                    //   the same
                    if (CurrentToken.Keyword == KeywordToken.Close
                            && CurrentToken.Text == token.Text
                        // if you comig out of adding a child that has the same type as the parent it was added to,
                        //  you need to advance the NextToken so that it doesn't erroeously "continue" and evaluate the while
                        //  as the end of the parent hereby merging the nodes
                        //  The following checks will check for this
                            && currentNode.Children.Any()
                            && currentNode.Children.Last().Type != currentNode.Type
                        )
                    {
                        continue;
                    }
                    NextToken();
                }
            }
            return currentNode;
        }
    }
}