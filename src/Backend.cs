using OoxmlToHtml.Abstracts;

namespace OoxmlToHtml
{
    public abstract class Backend
    {
        public abstract void Process(ICode code, ISymTab symTab);
    }
}