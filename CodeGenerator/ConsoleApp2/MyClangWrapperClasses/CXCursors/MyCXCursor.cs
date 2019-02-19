using System;
using System.Collections.Generic;
using ClangSharp;
using ConsoleApp2.MyClangWrapperClasses.CXCursors.MyCursorKinds;
using ConsoleApp2.MyLibClangVisitors;
using CPPParser;

namespace ConsoleApp2.MyClangWrapperClasses.CXCursors
{
    public class MyCXCursor
    {
        protected MyCXCursor _parent;
        public MyCXCursor Parent
        {
            get { return _parent;}
            set
            {
                //if a parent is set, add this to the parents child
                value.ChildrenCursors.Add(this);
                _parent = value;
            }
        }
        public List<MyCXCursor> ChildrenCursors { get;  set; }

        public CXCursor CXCursor { get; protected set; }
        public MyCXSourceLocation MyCxSourceLocation { get; private set; } 
        public CXCursorKind Kind { get { return CXCursor.kind; } } 


        protected MyCXCursor()
        {
            
        }

        public MyCXCursor(CXCursor cXCursor)
        {
            ChildrenCursors = new List<MyCXCursor>();
            InitTop(cXCursor);
        }


        protected void InitTop(CXCursor cXCursor)
        { 
            MyCxSourceLocation = new MyCXSourceLocation(cXCursor); 
            //Parent = new MyCXCursor(parent);
            CXCursor = cXCursor;
        }

        public List<TMyCursorOfKindBase> GetChildrenCursorsOfKind<TMyCursorOfKindBase>()
            where TMyCursorOfKindBase : MyCursorOfKindBase, new()
        {
            var visitorKind = new MyVisitorOfKind<TMyCursorOfKindBase>();
            clang.visitChildren(CXCursor, visitorKind.Visit, new CXClientData(IntPtr.Zero));
            visitorKind.SetAllCursorsEndLineLocation(); 
            return visitorKind.MyCursorsOfThatKind;
        }

        public string getCursorSpelling()
        {
            return clang.getCursorSpelling(CXCursor).ToString();
        }

        public string getCursorKindSpelling()
        {
            return clang.getCursorKindSpelling(CXCursor.kind).ToString();
        }
    }
}