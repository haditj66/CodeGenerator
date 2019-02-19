using ClangSharp;

namespace ConsoleApp2.MyClangWrapperClasses.CXCursors.MyCursorKinds
{
    public class MyCursorOfKindEnumConstantDecl : MyCursorOfKindBase
    {
        protected override CXCursorKind KindIShouldBeOVERRIDE()
        {
            return CXCursorKind.CXCursor_EnumConstantDecl;
        }

        /// <summary>
        /// Retrieve the integer value of an enum constant declaration as a signed long long
        /// </summary>
        /// <returns></returns>
        public long getEnumConstantDeclValue()
        {

            return clang.getEnumConstantDeclValue(this.CXCursor);
        }

    }
}