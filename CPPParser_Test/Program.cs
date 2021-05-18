using ClangSharp;
using ConsoleApp2.MyClangWrapperClasses;
using MyLibClangVisitors.ConsoleApp2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPPParser_Test
{
    class Program
    {
        static void Main(string[] args)
        {

            var file = new FileInfo(Path.Combine(@"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\CodeGenerator\ConsoleApp2\bin\Debug", "test2" + ".h"));
            MyCXTranslationUnit transUnit = new MyCXTranslationUnit(file, new[] { "-x", "c++" });

            var tt = new MyAllVisitor();
            //var tt = new EnumVisitor(Console.Out);   
            clang.visitChildren(transUnit.myCursor.CXCursor, tt.Visit, new CXClientData(IntPtr.Zero));
        }
    }
}
