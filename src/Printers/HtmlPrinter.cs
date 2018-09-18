using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Core.Internal;
using OoxmlToHtml.Abstracts;
using OoxmlToHtml.Analyzers;

namespace OoxmlToHtml.Printers
{
    enum EndType
    {
        auto,
        Short,
        Long
    }

    enum CodeStatus
    {
        None,
        Start,
        Reading
    }
    public class HtmlPrinter
    {
        private readonly StringBuilder Html = new StringBuilder();
        private bool insideCodeBlock = false;
        private CodeStatus codeStatus = CodeStatus.None;
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
        public void Print(INode node, bool runAnalyzers = true)
        {
            PrintStartingTag(node);
            PrintContent(node);
            if (node.child != null)
            {
                Print(node.child);
            }

            if (node.Next != null)
            {
                Print(node.Next);
            }

            PrintEndingTag(node);
            //if (runAnalyzers) node = RunAnalyzers(node);
            //KeywordToken type = node.Type;
            //switch (type)
            //{
            //    case KeywordToken.Paragraph:
            //        PrintBlock(node);
            //        break;
            //    case KeywordToken.Run:
            //        PrintInlineBlock(node);
            //        break;
            //    case KeywordToken.Body:
            //    case KeywordToken.PreviousParagraph:
            //        PrintNoTag(node);
            //        break;
            //    case KeywordToken.Color:
            //        PrintAttributes(KeywordToken.Color.ToString(), node.GetAttribute("value"));
            //        break;
            //    case KeywordToken.Code:
            //        PrintBlock(node, "Code");
            //        insideCodeBlock = !insideCodeBlock;
            //        break;
            //}
        }

        private void PrintContent(INode node)
        {
            if (!node.HasAttribute("Text")) return;
            Html.Append($"{ node.GetAttribute("Text") } ");
        }
        private void PrintStartingTag(INode node)
        {
            switch (node.Type)
            {
                case KeywordToken.Run:
                case KeywordToken.Paragraph:
                    if (codeStatus == CodeStatus.None)
                        PrintBlockStart(node, node.Type == KeywordToken.Run? "span" : "div");
                    if (node.HasAttribute("style") && node.GetAttribute("style") == "Title")
                    {
                        PrintBlockStart(node, "h1", EndType.auto, true);
                    }
                    break;
                case KeywordToken.Code:
                    if (codeStatus == CodeStatus.None)
                    {
                        codeStatus = CodeStatus.Start;
                        PrintBlockStart(node, "Code", EndType.Short);
                    }
                    break;
            }
        }
        private void PrintEndingTag(INode node)
        {
            switch (node.Type)
            {
                case KeywordToken.Code:
                    if (codeStatus == CodeStatus.Start)
                    {
                        codeStatus = CodeStatus.Reading;
                    }
                    else if (codeStatus == CodeStatus.Reading)
                    {
                        codeStatus = CodeStatus.None;
                        PrintBlockEnd(node, "Code");
                    }
                    break;
                case KeywordToken.Run:
                case KeywordToken.Paragraph:
                    if (node.HasAttribute("style") && node.GetAttribute("style") == "Title")
                    {
                        PrintBlockEnd(node, "h1");
                    }
                    if (codeStatus == CodeStatus.None)
                        PrintBlockEnd(node.Next, node.Type == KeywordToken.Run? "span" : "div");
                    break;
            }
        }

        private void PrintBlockEnd(INode node, string tagName = "div")
        {
            Html.Append($"</{tagName}>");
        }
        private void PrintBlockStart(INode node, string tagName = "div", EndType endType = EndType.auto, bool tagOnly = false)
        {
            Html.Append($"<{tagName}");
            if (!tagOnly)
            {
                PrintStyle(node);
                PrintAttributes(node);
            }
            Html.Append(node.Children.Count == 0 
                        && endType != EndType.Short
                        && !node.HasAttribute("Text")
                ? $" />"
                : ">");
        }

        private void PrintStyle(INode node)
        {
            var attributes = node.GetAllAttributes.Keys;
            IEnumerable<string> enumerable = attributes as string[] ?? attributes.ToArray();
            if (enumerable.IsNullOrEmpty()) return;

            bool? styleInitialized = false;
            foreach (var attr in enumerable)
            {
                var value = node.GetAttribute(attr);
                switch (attr)
                {
                    case "fontColor":
                        StyleInitialized(ref styleInitialized);
                        Html.AppendFormat("color:#{0};", value);
                        break;
                    case "italic":
                        StyleInitialized(ref styleInitialized);
                        Html.AppendFormat("font-style:{0};",
                            value == bool.TrueString
                                ? "italic"
                                : "none");
                        break;
                    case "bold":
                        StyleInitialized(ref styleInitialized);
                        Html.AppendFormat("font-weight:{0};",
                            value == bool.TrueString
                                ? "bold"
                                : "none");
                        break;
                    case "size":
                        StyleInitialized(ref styleInitialized);
                        Html.AppendFormat("font-size:{0}px;", value);
                        break;
                }
            }
            if (styleInitialized.GetValueOrDefault())
                Html.Append("\"");
        }

        private void StyleInitialized(ref bool? styleInitialized)
        {
            if (!styleInitialized.HasValue) return;
            var value = styleInitialized.Value;
            if (value) return;
            Html.Append(" style=\"");
            styleInitialized = true;
        }

        private void PrintAttributes(INode node)
        {
            var attributes = node.GetAllAttributes.Keys;
            IEnumerable<string> enumerable = attributes as string[] ?? attributes.ToArray();
            if (enumerable.Any(z => z != "Text" && z != "style"))
            {
                Html.Append(" style=\"");
                foreach (var attr in enumerable)
                {
                    PrintAttributes(attr, node.GetAttribute(attr));
                }
                Html.Append("\"");
            }
        }
        private void PrintInlineBlock(INode node)
        {
            if (!insideCodeBlock)
            {
                Html.Append("<span");
               var attributes = node.GetAllAttributes.Keys;
                IEnumerable<string> enumerable = attributes as string[] ?? attributes.ToArray();
                if (enumerable.Any(z => z != "Text" && z != "style"))
                {
                    Html.Append(" style=\"");
                    foreach (var attr in enumerable)
                    {
                        PrintAttributes(attr, node.GetAttribute(attr));
                    }
                    Html.Append("\"");
                }
                Html.Append(">");
            }
            if (node.HasAttribute("Text"))
            {
                Html.Append(node.GetAttribute("Text"));
            }
            foreach (var child in node.Children)
            {
                Print(child);
            }
            if (!insideCodeBlock)
                Html.Append("</span>");
        }
        private void PrintBlock(INode node, string tagName = "div")
        {
            if (!insideCodeBlock)
            {
                Html.Append($"<{tagName}");
                var attributes = node.GetAllAttributes.Keys;
                IEnumerable<string> enumerable = attributes as string[] ?? attributes.ToArray();
                if (enumerable.Any(z => z != "Text" && z != "style"))
                {
                    Html.Append(" style=\"");
                    foreach (var attr in enumerable)
                    {
                        PrintAttributes(attr, node.GetAttribute(attr));
                    }
                    Html.Append("\"");
                }
                Html.Append(">");
                if (node.HasAttribute("style"))
                {
                    switch (node.GetAttribute("style"))
                    {
                        case "Title":
                            Html.Append("<h1>");
                            break;
                    }
                }
            }
            if (node.HasAttribute("Text"))
            {
                Html.Append(node.GetAttribute("Text"));
            }
            foreach (var child in node.Children)
            {
                Print(child);
            }

            if (!insideCodeBlock)
            {
                if (node.HasAttribute("style"))
                {
                    switch (node.GetAttribute("style"))
                    {
                        case "Title":
                            Html.Append("</h1>");
                            break;
                    }
                }

                Html.Append($"</{tagName}>");

            }
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
                case "size":
                    Html.AppendFormat("font-size:{0}px;", value);
                    break;
            }
        }
    }
}