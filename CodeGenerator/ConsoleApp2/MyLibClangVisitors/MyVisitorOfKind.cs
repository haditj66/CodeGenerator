using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClangSharp;
using ClangSharpPInvokeGenerator;
using ConsoleApp2.MyClangWrapperClasses;
using ConsoleApp2.MyClangWrapperClasses.CXCursors;
using ConsoleApp2.MyClangWrapperClasses.CXCursors.MyCursorKinds;

namespace ConsoleApp2.MyLibClangVisitors
{
    public class MyVisitorOfKind<TMyCursorOfKindBase> : MyVisitorBase
        where TMyCursorOfKindBase : MyCursorOfKindBase, new()
    {
        public List<TMyCursorOfKindBase> MyCursorsOfThatKind { get; set; }


        public MyVisitorOfKind( )
        {

            MyCursorsOfThatKind = new List<TMyCursorOfKindBase>();
        }


        protected override void VisitOVERRIDE(ref MyCXCursor mycursor)
        {
            //this is not working. I need to make it point to it but I cant cast as the endline does not change when it i sset

            TMyCursorOfKindBase mycursorbase = new TMyCursorOfKindBase(); 
            //mycursorbase = (TMyCursorOfKindBase)mycursor;
            CXCursorKind curKind = clang.getCursorKind(mycursor.CXCursor);
            if (curKind == mycursorbase.KindIShouldBe)
            {
                mycursorbase.Init(ref mycursor);
                mycursor = mycursorbase;
                MyCursorsOfThatKind.Add((TMyCursorOfKindBase)mycursor);
                //MyCursorsOfThatKind.Add(mycursorbase);

            }
        }

        protected override void SetAllCursorsEndLineLocationOVERRIDE()
        {
            /*
            //I need to set endline location for MyCursorsOfThatKind.
            //I need to go through each one of MyCursorsOfThatKind and find the CursorsSoFar that matches, then set the endline from that one.
            foreach (var  CursorOfKind  in MyCursorsOfThatKind)
            { 
                var cursorSoFar = AllCursorsSoFar.Where((MyCXCursor cx) =>   { return clang.equalCursors(cx.CXCursor, CursorOfKind.CXCursor) == 1;  }).First();
                CursorOfKind.MyCxSourceLocationBegin.lineEnd = cursorSoFar.MyCxSourceLocationBegin.lineEnd;
            }

            */
        }


    }



}
