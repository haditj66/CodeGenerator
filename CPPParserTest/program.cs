 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ClangSharp;
using ClangSharpPInvokeGenerator;
using ConsoleApp2.MyClangWrapperClasses;
using MyLibClangVisitors.ConsoleApp2;

namespace CPPParserTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //CXIndex ind = clang.createIndex(0, 0);
            /* 


            var file = new FileInfo(Path.Combine(@"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\CodeGenerator\ConsoleApp2\bin\Debug", "test2" + ".h"));
            //File.WriteAllText(file.FullName, "int main() { return 0; }");

            CXUnsavedFile[] unsavedFile = new CXUnsavedFile[0];
            string[] arr = { "-x", "c++" };

            var translationUnit = clang.parseTranslationUnit(index, file.FullName, arr, 2, new CXUnsavedFile[0], 0, 0);
             
            CXCursor cursor = clang.getTranslationUnitCursor(translationUnit);
            */
            var file = new FileInfo(Path.Combine(@"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\CodeGenerator\ConsoleApp2\bin\Debug", "test2" + ".h"));
            MyCXTranslationUnit transUnit = new MyCXTranslationUnit(file, new[] { "-x", "c++" });

            var tt = new MyAllVisitor();
            //var tt = new EnumVisitor(Console.Out);   
            clang.visitChildren(transUnit.myCursor.CXCursor, tt.Visit, new CXClientData(IntPtr.Zero));

            /*
            string output = @"C:\Users\Hadi\source\repos\ConsoleApp2\ConsoleApp2\bin\Debug\output\out.txt";
            using (var sw = new StreamWriter(output))
            {
                //var tt = new EnumVisitor(sw);
                

            }*/

        }
    }
}
