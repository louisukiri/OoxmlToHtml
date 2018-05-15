using System;
using System.Collections.Generic;
using System.Text;

namespace OoxmlToHtml.Flags
{
    [Flags]
    public enum Styles
    {
        None = 0,
        Underline = 1,
        Bold = 2,
        Italic = 4
    }
}
