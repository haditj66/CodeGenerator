using CgenCompileLibrary;
using CompileKeywords;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace CgenCompileTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void postCompileFile_test()
        {
            PostCompileFile postCompileFile = new PostCompileFile(new FileInfo("../AE_Init.ii"));

            Assert.IsTrue(postCompileFile.SourceFile.FullName == "C:\\Users\\Hadi\\OneDrive\\Documents\\VisualStudioprojects\\Projects\\cSharp\\CodeGenerator\\CodeGenerator\\CgenCompileTests\\bin\\Debug\\source\\AE_Init.h");

            Assert.IsTrue(postCompileFile.IntermediaryFileContents != null);
        }


        [TestMethod]
        public void GetWeakKeywordUnits_test()
        {
            PostCompileFile postCompileFile = new PostCompileFile(new FileInfo("../AE_Init.ii"));

            WeakKeywordsContainer weakKeywordsContainer = WeakKeywordsContainer.FactoryCompileKeyword<WeakKeywordsContainer>();// weakKeywordsContainer = new WeakKeywordsContainer();
            Assert.IsTrue(weakKeywordsContainer.Keyword == "cgenweak");

            bool anyFound = weakKeywordsContainer.FindInstanceOfUnit(postCompileFile);
            
            Assert.IsTrue(weakKeywordsContainer.AllCompileKeywordUnits.Count == 2);

        }
    }
}
