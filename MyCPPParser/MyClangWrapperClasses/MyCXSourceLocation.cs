using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClangSharp;
using ClangSharp.Interop;

namespace MyCPPParser.MyClangWrapperClasses
{
    public class MyCXSourceLocation
    {
        public CXCursor CxCursor { get; }
        public CXSourceLocation CxSourceLocation { get; }

        public uint lineStart => _line; 
        public uint columnStart => _column;
        public uint offsetStart => _offset;
        public MyCXFile MyCxFilefileStart { get; private set; }

        public uint lineEnd => _lineEnd;
        public uint columnEnd => _columnEnd;
        public uint offsetEnd => _offsetEnd;
        public MyCXFile MyCxFilefileEnd { get; private set; }


        private static CXFile CXFile;
        private readonly uint _line;
        private readonly uint _column;
        private readonly uint _offset;

        private static CXFile CXFileEnd;
        private readonly uint _lineEnd;
        private readonly uint _columnEnd;
        private readonly uint _offsetEnd;

        public MyCXSourceLocation(CXCursor cXCursor)
        {
            CxCursor = cXCursor;

            //get the cxrange
            CXSourceRange cxrange = clang.getCursorExtent(cXCursor);  
            var beginSource = clang.getRangeStart(cxrange);//begin source
            var endSource = clang.getRangeEnd(cxrange); //end source

            //get file line and column start
            this.CxSourceLocation = clang.getCursorLocation(cXCursor);
            clang.getExpansionLocation(beginSource, out CXFile, out _line, out _column, out _offset);

            //get file line and column ends
            clang.getExpansionLocation(endSource, out CXFileEnd, out _lineEnd, out _columnEnd, out _offsetEnd);

            //if it is the translation unit, I need to set the fullpath manually
            if (CxCursor.kind == CXCursorKind.CXCursor_TranslationUnit)
            {
                MyCxFilefileStart = new MyCXFile(CXFile, clang.getCursorSpelling(CxCursor).ToString());
                MyCxFilefileEnd = new MyCXFile(CXFileEnd, clang.getCursorSpelling(CxCursor).ToString());
            }
            else
            {
                MyCxFilefileStart = new MyCXFile(CXFile);
                MyCxFilefileEnd = new MyCXFile(CXFileEnd);
            }

            

        }

    }
}
