using System;
using System.Collections.Generic;
using ClangSharp;
using ConsoleApp2.MyLibClangVisitors;
using ConsoleApp2.Parsing;

namespace ConsoleApp2.MyClangWrapperClasses.CXCursors.MyCursorKinds
{
    public abstract class MyCursorOfKindBase : MyCXCursor
    {
        public CXCursorKind KindIShouldBe { get; private set; }
        protected abstract CXCursorKind KindIShouldBeOVERRIDE(); 

        //private List<MyCursorOfKindBase> PossibleChildrenKinds { get; set; } 
        //protected abstract List<MyCursorOfKindBase> PossibleChildrenKindsOVERRIDE();
          
        

        public MyCursorOfKindBase() : base()
        {
            KindIShouldBe = KindIShouldBeOVERRIDE();
            //PossibleChildrenKinds = PossibleChildrenKindsOVERRIDE();
        }

        public void Init(ref MyCXCursor mycXCursor)
        {
            ChildrenCursors = mycXCursor.ChildrenCursors;
            _parent = mycXCursor.Parent;
            InitTop(mycXCursor.CXCursor);
            //MyParentCursor = parentMyCursorKind;
              
        }

        /*public static implicit operator MyCXCursor(MyCursorOfKindBase mycursorToCast)
        {

        }*/




        //public abstract MyCursorOfKindBase FactoryMethodOVERRIDE(CXCursor cXCursor, MyCursorOfKindBase parentMyCursorKind, CXCursorKind kindOfCursorIAm);


        /*
        protected List<MyCursorOfKindBase> GetChildMyCursorsOfKind(CXCursor cXCursor)
        {

        }*/

    }
}
