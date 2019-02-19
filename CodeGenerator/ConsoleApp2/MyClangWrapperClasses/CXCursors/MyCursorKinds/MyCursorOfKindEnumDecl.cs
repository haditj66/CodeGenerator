using System.Collections.Generic;
using ClangSharp;

namespace ConsoleApp2.MyClangWrapperClasses.CXCursors.MyCursorKinds
{
    public class MyCursorOfKindEnumDecl : MyCursorOfKindBase
    {

        protected override CXCursorKind KindIShouldBeOVERRIDE()
        { 
            return CXCursorKind.CXCursor_EnumDecl;
        }

        public List<MyCursorOfKindEnumConstantDecl> GetChildrenOfKind_EnumConstantDecl()
        {
            return GetChildrenCursorsOfKind<MyCursorOfKindEnumConstantDecl>();  
        }
         
    }
}