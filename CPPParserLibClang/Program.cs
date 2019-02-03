using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using ClangSharp;
namespace CPPParserLibClang
{
    class Program
    {
        static void Main(string[] args)
        {
            CXUnsavedFile cxf = new CXUnsavedFile();
            CXIndex index =  clang.createIndex(0, 0);
            clang.parseTranslationUnit(
                index, 
                @"test.hpp", null,0,
                out cxf, 0, 
                (int)CXTranslationUnit_Flags.CXTranslationUnit_None);

        }
    }
}
