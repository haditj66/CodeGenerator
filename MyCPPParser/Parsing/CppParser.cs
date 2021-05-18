using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClangSharp;
using MyCPPParser.MyClangWrapperClasses;
using MyCPPParser.MyClangWrapperClasses.CXCursors;
using MyCPPParser.MyClangWrapperClasses.CXCursors.MyCursorKinds;
using MyCPPParser.MyLibClangVisitors;

namespace MyCPPParser.Parsing
{
    public class CppParser
    {
        private readonly string _fileToParse;
        private readonly string[] _argumentsForTheParse;

        public MyCXTranslationUnit MyCxTranslationUnit { get; set; }

        public CppParser(string fileToParse, string[] argumentsForTheParse = null)
        {
            _fileToParse = fileToParse;
            _argumentsForTheParse = argumentsForTheParse;
            ReloadParser();
        }

        public void ReloadParser()
        {
            MyCxTranslationUnit = new MyCXTranslationUnit(new FileInfo(_fileToParse), _argumentsForTheParse);

        }


        public List<TCursorOfKind> GetAllCursorsOfKind<TCursorOfKind>()
        where TCursorOfKind : MyCursorOfKindBase, new()
        {
            return  ((MyCxTranslationUnit.myCursor)).GetChildrenCursorsOfKind<TCursorOfKind>(); 
            //return visitorKind.MyCursorsOfThatKind;

        }


    }
}
