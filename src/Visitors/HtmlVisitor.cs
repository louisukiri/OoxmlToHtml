using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OoxmlToHtml.AST.abstracts;
using OoxmlToHtml.Flags;
using OoxmlToHtml.Statements;

namespace OoxmlToHtml.Visitors
{
    public class HtmlVisitor : IVisitor
    {
        private readonly StringBuilder _value;
        public string Value => _value.ToString();

        public void Visit(ColorStatement statement)
        {
            _value.AppendFormat("color:#{0}; ", statement.Value);
        }

        public void Visit(AttributeStatement statement)
        {
            throw new NotImplementedException();
        }

        public void Visit(ParagraphPropertyStatement statement)
        {
        }

        public void Visit(RunStatement statement)
        {
            throw new NotImplementedException();
        }

        public void Visit(StringStatement statement)
        {
            throw new NotImplementedException();
        }

        public void Visit(IdentifierExpression expression)
        {
            throw new NotImplementedException();
        }

        public void Visit(Program program)
        {
            foreach(var s in program.Statements)
            {
                s.Accept(this);
            }
        }

        public void Visit(ParagraphStatement statement)
        {
            _value.Append("<p ");
            if (statement.styles != Styles.None)
            {
                _value.Append("style=\"");
                if (statement.styles.HasFlag(Styles.Bold))
                {
                    _value.Append("font-weight: bold; ");
                }

                if (statement.styles.HasFlag(Styles.Italic))
                {
                    _value.Append("font-style: italic; ");
                }

                foreach (var props in statement.Properties)
                {
                    props.Accept(this);
                }
                _value.Append("\"");
            }
            _value.Append("></p>");
        }

        public void Visit(SizeStatement statement)
        {
            _value.AppendFormat("font-size: {0}px; ", statement.TokenLiteral());
        }

        public HtmlVisitor()
        {
            _value = new StringBuilder(string.Empty);
        }
    }
}
