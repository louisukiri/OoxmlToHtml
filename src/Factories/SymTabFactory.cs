﻿using System;
using OoxmlToHtml.Abstracts;

namespace OoxmlToHtml.Factories
{
    public class SymTabFactory
    {
        public static ISymTabStack CreateSymTabStack()
        {
            return null;
        }

        public static ISymTab CreateSymTab(int currentNestingLevel)
        {
            return null;
        }

        internal static ISymTabEntry CreateSymTabEntry(string name, SymTab symTab)
        {
            throw new NotImplementedException();
        }
    }
}