namespace OoxmlToHtml.Abstracts
{
    public interface ISymTabStack
    {
        int CurrentNestingLevel { get; }
        ISymTab LocalSymTab { get; }
        ISymTabEntry EnterLocal(string name);
        ISymTabEntry LookupLocal(string name);
        ISymTabEntry Lookup(string name);
    }
}