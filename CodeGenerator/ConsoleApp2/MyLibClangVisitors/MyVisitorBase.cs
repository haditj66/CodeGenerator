using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClangSharp;
using ClangSharpPInvokeGenerator;
using ConsoleApp2.MyClangWrapperClasses.CXCursors;

namespace ConsoleApp2.MyLibClangVisitors
{
    public abstract class MyVisitorBase : ICXCursorVisitor
    {
        public MyCXCursor ParentCursor { get; set; }
        public int EndOfFileLine { get; private set; }

        public List<MyCXCursor> AllCursorsSoFar;


        public MyVisitorBase()
        {
            AllCursorsSoFar = new List<MyCXCursor>();
        }


        public CXChildVisitResult Visit(CXCursor cursor, CXCursor parent, IntPtr data)
        {
            if (clang.Location_isFromMainFile(clang.getCursorLocation(cursor)) == 0)
            {
                return CXChildVisitResult.CXChildVisit_Continue;
            }


            MyCXCursor myCxCursor;
            //if AllCursorsSoFar is zero, i need to add translational unit cursor
            if (AllCursorsSoFar.Count == 0)
            {
                myCxCursor = new MyCXCursor(parent);
                AllCursorsSoFar.Add(myCxCursor);
                var content = File.ReadAllLines(myCxCursor.MyCxSourceLocation.MyCxFilefileStart.FileFullName);//(clang.getCursorSpelling(parent).ToString());
                EndOfFileLine = content.Length;
                VisitOVERRIDE(ref myCxCursor);
            } 
                 
            //I need to keep track of all cursors so far 
            myCxCursor = new MyCXCursor(cursor);
            AllCursorsSoFar.Add(myCxCursor);

            //get the mycursor parent, it should have already been added
            MyCXCursor myCursorParent = AllCursorsSoFar
                .Where((MyCXCursor mycxc) => { return clang.equalCursors(mycxc.CXCursor, parent) == 1; }).FirstOrDefault();
            //set parent
            myCxCursor.Parent = myCursorParent; 

            VisitOVERRIDE(ref myCxCursor);


            return CXChildVisitResult.CXChildVisit_Recurse;
        }


        public void SetAllCursorsEndLineLocation()
        {
            /*
            int index = 0;
            foreach (var CursorSoFar in AllCursorsSoFar)
            {
                //if this is the first one, then then it must be the translation unit. lineend will just be endoffile
                if (index == 0)
                {
                    CursorSoFar.MyCxSourceLocationBegin.lineEnd = (uint)EndOfFileLine + 1;
                }

                //if this is the last one, lineend should be endoffile
                else if (AllCursorsSoFar.Count == index)
                {
                    //if it is the last one then endoffile+1 is the last
                    CursorSoFar.MyCxSourceLocationBegin.lineEnd = (uint)EndOfFileLine + 1;
                }

                //get that cursor's "brother" by going through a cursor's children.
                //grab the last child and see where that child's index is inAllCursorsSoFar.  
                else
                {
                    MyCXCursor veryLastChild;
                    if (CursorSoFar.ChildrenCursors.Count != 0)
                    { 
                            //i need to get the last child of the last child
                            veryLastChild = CursorSoFar.ChildrenCursors.LastOrDefault();
                        while (veryLastChild.ChildrenCursors.Count != 0)
                        {
                            veryLastChild = veryLastChild.ChildrenCursors.LastOrDefault();
                        }
                    }
                    else
                    {
                        //if no children then very last child will just be itself
                        veryLastChild = CursorSoFar;
                    }

                    var lastChildIndex = AllCursorsSoFar
                        .FindIndex((MyCXCursor cx) =>
                        {
                            return clang.equalCursors(cx.CXCursor, veryLastChild.CXCursor) == 1;
                        }); //last child


                    //get source locations
                    //clang.getCursorLocation()
                     
                    var cxloc1 = clang.getCursorLocation(AllCursorsSoFar[lastChildIndex].CXCursor);
                    var cxloc2 = clang.getCursorLocation(AllCursorsSoFar[lastChildIndex+1].CXCursor);
                    CXSourceRange cxrange = clang.getCursorExtent(AllCursorsSoFar[lastChildIndex].CXCursor);

                    //CXSourceRange cxrange = clang.getRange(cxloc1, cxloc2);
                    var beginSource = clang.getRangeStart(cxrange);
                    var endSource = clang.getRangeEnd(cxrange);
                    uint l;
                    uint c;
                    uint of;
                    CXFile file;
                    clang.getExpansionLocation(beginSource, out file, out l, out c, out of);
                    uint l2;
                    uint c2;
                    uint of2;
                    CXFile file2;
                    clang.getExpansionLocation(endSource, out file2, out l2, out c2, out of2);
                    //clang.getRangeEnd(cxrange.)


                    // the next AllCursorsSoFar will be the brother. the brother's start line will be the cursor's end line
                    //if it is last index, than it must be for endoffile. just remember that if the source file is changed, I will always need to reload to get the new endline values
                    if (lastChildIndex+1 == AllCursorsSoFar.Count)
                    {
                        CursorSoFar.MyCxSourceLocationBegin.lineEnd = (uint)EndOfFileLine;
                    }
                    else
                    {
                        CursorSoFar.MyCxSourceLocationBegin.lineEnd =
                            AllCursorsSoFar[lastChildIndex + 1].MyCxSourceLocationBegin.lineStart;
                    } 

                } 

                index++;
            }

            SetAllCursorsEndLineLocationOVERRIDE();
            */
        }

        protected virtual void SetAllCursorsEndLineLocationOVERRIDE() { }
        protected abstract void VisitOVERRIDE(ref MyCXCursor mycursor);
        
    }
}
