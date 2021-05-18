using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClangSharp;
using MyCPPParser.MyClangWrapperClasses.CXCursors;
 
using ClangSharpPInvokeGenerator;
using MyCPPParser.MyClangWrapperClasses;
using MyLibClangVisitors.MyCPPParser;
using ClangSharp.Interop;

namespace MyCPPParser.MyClangWrapperClasses
{
    public class MyCXTranslationUnit
    {
        public MyCXCursor myCursor { get; private set; }

        public CXTranslationUnit CxTranslationUnit { get; }
        private CXCursor _CXCursor;



        public MyCXTranslationUnit( FileInfo CppFileLocationForParsing, string[] argumentsForTheParse = null, CXUnsavedFile[] cXUnsavedFile = null)
        {
            if (argumentsForTheParse == null)  {  argumentsForTheParse =  new []{ "-x", "c++" };   }
            if (cXUnsavedFile == null) { cXUnsavedFile = new CXUnsavedFile[0];}  

            CXIndex index = clang.createIndex(0, 0);
            this.CxTranslationUnit = clang.parseTranslationUnit(index, CppFileLocationForParsing.FullName, argumentsForTheParse, argumentsForTheParse.Length, cXUnsavedFile, 0, 0);

            Init(this.CxTranslationUnit);
        }

        public MyCXTranslationUnit(CXTranslationUnit CxTranslationUnit)
        {
            this.CxTranslationUnit = CxTranslationUnit; 
            Init(this.CxTranslationUnit);
        }

        private void Init(CXTranslationUnit cxTranslationUnit)
        {
            //get cursor for translation unit
            _CXCursor = clang.getTranslationUnitCursor(cxTranslationUnit);
            myCursor = new MyCXCursor(_CXCursor);
        }

    }
}
