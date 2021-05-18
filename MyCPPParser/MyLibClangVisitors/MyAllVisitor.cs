using System;
using System.Collections.Generic;
using ClangSharp;
using ClangSharpPInvokeGenerator;
using MyCPPParser.MyClangWrapperClasses;
using MyCPPParser.MyClangWrapperClasses.CXCursors;
using CPPParser;
using ClangSharp.Interop;

namespace MyLibClangVisitors.MyCPPParser
{
    public class MyAllVisitor : ICXCursorVisitor
    {
        
        public List<MyCXCursor> AllCursorsVisited { get; private set; }

        public MyAllVisitor()
        {
            AllCursorsVisited = new List<MyCXCursor>();
        }

        public CXChildVisitResult Visit(CXCursor cursor, CXCursor parent, IntPtr data )
        {
            

            //this is so that I can skip include files.
            if (clang.Location_isFromMainFile(clang.getCursorLocation(cursor)) == 0)
            {
                return CXChildVisitResult.CXChildVisit_Continue;
            }
            
            MyCXCursor mycursor = new MyCXCursor(cursor);
            AllCursorsVisited.Add(mycursor); 
             
            //Console.WriteLine(mycursor.getCursorSpelling() + " of kind " +   mycursor.getCursorKindSpelling());


            //--------------------get source location
            MyCXSourceLocation sloc = new MyCXSourceLocation(cursor);  
            //Console.WriteLine("at file: " + sloc.MyCxFilefileStart.FileName);
           // Console.WriteLine("at line: " + sloc.lineStart);
           // Console.WriteLine("at column: " + sloc.columnStart);
 
             return CXChildVisitResult.CXChildVisit_Recurse;
            //https://clang.llvm.org/doxygen/group__CINDEX__CURSOR__TRAVERSAL.html
            /*CXChildVisit_Break  Terminates the cursor traversal.
            CXChildVisit_Continue 	 Continues the cursor traversal with the next sibling of the cursor just visited, without visiting its children.
            CXChildVisit_Recurse  Recursively traverse the children of this cursor, using the same visitor and client data. */
        }
    }
}