namespace OoxmlToHtml.Abstracts
{
    public interface IAnalyzer
    {
        IAnalyzer Next { get; }
        INode Analyze(INode node);
        IAnalyzer Use(IAnalyzer next);
    }
}
