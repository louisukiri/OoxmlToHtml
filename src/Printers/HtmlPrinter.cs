using System.Collections.Generic;
using System.Linq;
using System.Text;
using OoxmlToHtml.Abstracts;
using OoxmlToHtml.Analyzers;

namespace OoxmlToHtml.Printers
{
    public class HtmlPrinter
    {
        private readonly StringBuilder Html = new StringBuilder();
        public string Print(IRootNode rootNode)
        {
            var root = rootNode.Root;
            return "";
        }

        public string HtmlString => Html.ToString();

        public static INode RunAnalyzers(INode node)
        {
            var htmlAnalyzers = (Analyzer)new ElementToAttributeAnalyzer();
            htmlAnalyzers.Use(new AttributeCopierAnalyzer());

            return htmlAnalyzers.Analyze(node);
        }
        public void Print(INode node)
        {
            node = RunAnalyzers(node);
            switch (node.Type)
            {
                case KeywordToken.Paragraph:
                    PrintBlock(node);
                    break;
                case KeywordToken.PreviousParagraph:
                    PrintNoTag(node);
                    break;
                case KeywordToken.Color:
                    PrintAttributes(KeywordToken.Color.ToString(), node.GetAttribute("value"));
                    break;
            }
        }

        private void PrintBlock(INode node)
        {
            Html.Append("<div ");
            var attributes = node.GetAllAttributes.Keys;
            IEnumerable<string> enumerable = attributes as string[] ?? attributes.ToArray();
            if (enumerable.Any())
            {
                Html.Append("styles=\"");
                foreach (var attr in enumerable)
                {
                    PrintAttributes(attr, node.GetAttribute(attr));
                }
                Html.Append("\"");
            }
            Html.Append(">");

            foreach (var child in node.Children)
            {
                Print(child);
            }

            Html.Append("</div>");
        }

        private void PrintNoTag(INode node)
        {
            foreach (var child in node.Children)
            {
                Print(child);
            }
        }
        private void PrintAttributes(string type, string value)
        {
            switch (type)
            {
                case "fontColor":
                    Html.AppendFormat("color:#{0};", value);
                    break;
                case "italic":
                    Html.AppendFormat("font-style:{0};",
                        value == bool.TrueString
                            ? "italic"
                            : "none");
                    break;
                case "bold":
                    Html.AppendFormat("font-weight:{0};",
                        value == bool.TrueString
                            ? "bold"
                            : "none");
                    break;
            }
        }
    }
}