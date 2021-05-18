using System;
using System.IO;
using ConsoleApp2.CPPRefactoring;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CPPParserTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ChangeFileName()
        {  
            CppRefactorer refact = new CppRefactorer(new DirectoryInfo(@"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\CodeGeneratorTest\bin\Debug\TestCppRefactor\FileNameChange"));

            refact.ChangeNameOfFile(@"rg.h", "pre_rg.h");
        }
    }
}
