namespace OoxmlToHtml.Abstracts
{
    public interface IAnalyzer
    {
        IAnalyzer Next { get; set; }
        INode Analyze(INode node);
    }
}
