﻿using System;

namespace OoxmlToHtml.Flags
{
    [Flags]
    public enum Styles
    {
        None = 0,
        Underline = 1,
        Bold = 2,
        Italic = 4,
        H1 = 8,
        H2 = 16,
        H3 = 32,
        Title = 64
    }
}
