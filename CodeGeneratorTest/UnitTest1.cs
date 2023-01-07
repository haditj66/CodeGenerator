using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
//using ClangSharp;
using CodeGenerator;
using CodeGenerator.cgenXMLSaves.SaveFiles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeGenerator.IDESettingXMLs;
using CodeGenerator.FileTemplates;
using CodeGenerator.FileTemplates.Files;
using CodeGenerator.FileTemplatesMacros;
using CodeGenerator.IDESettingXMLs.VisualStudioXMLs;
using CodeGenerator.ProjectBuilders;
using CodeGenerator.ProjectBuilders.FileDependentImporters;
//using ConsoleApp2.CPPRefactoring;
//using ConsoleApp2.MyClangWrapperClasses.CXCursors.MyCursorKinds;
//using ConsoleApp2.Parsing;
//using CPPParser;
using System.Text.RegularExpressions;
using CodeGenerator.FileTemplates.GeneralMacoTemplate;
//using ConsoleApp2.MyClangWrapperClasses;
//using MyLibClangVisitors.ConsoleApp2;

namespace CodeGeneratorTest
{
    [TestClass]
    public class UnitTest1
    {
        MyFilter filter1 = new MyFilter("name1");
        MyFilter filter2 = new MyFilter("name2");
        MyFilter filter3 = new MyFilter("name3");
        MyFilter fitler11;
        MyFilter fitler113;
        public UnitTest1()
        {
            fitler11 = filter1.AddChildFilter("name11");
            filter1.AddChildFilter("name12");
            filter1.AddChildFilter("name13");

            fitler11.AddChildFilter("name111");
            fitler11.AddChildFilter("name112");
            fitler113 = fitler11.AddChildFilter("name113");

            filter2.AddChildFilter("name21");
            filter2.AddChildFilter("name22");
        }

        //testing filter -------------------------------------------------------

        [TestMethod]
        public void GetFilterFromAddress()
        {


            //get last child filter
            MyFilter child = filter2.GetFilterFromAddress("name1\\name3\\name4");
            Assert.IsTrue(child == null);
            child = filter2.GetFilterFromAddress("name1\\name11\\name113");
            Assert.IsTrue(child == null);
            child = filter1.GetFilterFromAddress("name1\\name11\\name113");
            Assert.IsTrue(child.Name == "name113");
        }

        [TestMethod]
        public void FilterGetAddress()
        {
            string address = fitler113.GetFullAddress();
            Assert.IsTrue(address == "name1\\name11\\name113");

        }


        [TestMethod]
        public void FilterDoesAddressExist()
        {
            MyFilter filterAtAddress = filter1.DoesAddressExist("name1\\name11\\name112");
            Assert.IsTrue(filterAtAddress.GetFullAddress() == "name1\\name11\\name112");

        }

        [TestMethod]
        public void MacroFileTest()
        {
            FileTemplateMainCG maincgTemplate = new FileTemplateMainCG("", "moda1");
            maincgTemplate.CreateTemplate();
        }


        [TestMethod]
        public void GeneralTemplateUserCodesTest()
        {
            string pathtoTemplateFileAndOutputFiles = @"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\CodeGeneratorTest\bin\Debug";
            string nameOfcGenMacroFile = "testForUserCodes.cgenM";

            GeneralMacro generalMacro = new GeneralMacro(pathtoTemplateFileAndOutputFiles, nameOfcGenMacroFile);
            generalMacro.CreateTemplate();

        }


        [TestMethod]
        public void GeneralTemplateTest()
        {
            string pathtoTemplateFileAndOutputFiles = @"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\CodeGeneratorTest\bin\Debug";
            string nameOfcGenMacroFile = "testForGeneral.cgenM";

            GeneralMacro generalMacro = new GeneralMacro(pathtoTemplateFileAndOutputFiles, nameOfcGenMacroFile);
            generalMacro.CreateTemplate();
             
        }

        [TestMethod]
        public void GeneralTemplateTest2()
        {
            string pathtoTemplateFileAndOutputFiles = @"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\CodeGeneratorTest\bin\Debug";
            string nameOfcGenMacroFile = "AEObjectTest.cgenM";

            GeneralMacro generalMacro = new GeneralMacro(pathtoTemplateFileAndOutputFiles, nameOfcGenMacroFile);
            generalMacro.CreateTemplate();

        }


        [TestMethod]
        public void MacroLoopSectionTest()
        {
            SaveFilecgenProjectGlobal saveFilecgenProjectGlobal = new SaveFilecgenProjectGlobal(@"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\CodeGenerator\bin\Debug\CGensaveFiles");
            FileTemplateAllLibraryInlcudes faAllLibraryInlcudes = new FileTemplateAllLibraryInlcudes("", saveFilecgenProjectGlobal);
            FileTemplateCGKeywordDefine fileTemplateCgKeyword = new FileTemplateCGKeywordDefine("", saveFilecgenProjectGlobal);
            faAllLibraryInlcudes.CreateTemplate();
            fileTemplateCgKeyword.CreateTemplate();
        }


        [TestMethod]
        public void TestConfigurationBuilder()
        {
            string DIRECTORYOFTHISCG = @"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\CodeGenerator\bin\Debug";
            string CGSAVEFILESBASEDIRECTORY = "CGensaveFiles";
            string CGCONFCOMPILATOINSBASEDIRECTORY = "ConfigCompilations";
            string PATHTOCONFIGTEST = @"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\ConfigTest";


            //get the project settings for the project configs I want to generate. for VS for NOW!
            MySettingsVS VSsetting = MySettingsVS.CreateMySettingsVS(@"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\Module1AA");

            //create the configuration file configurationCG.h 
            ConfigurationFileBuilder configFileBuilder = new ConfigurationFileBuilder(VSsetting,
                @"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\CodeGeneratorTest\bin\Debug\TestConfigBuilder", DIRECTORYOFTHISCG, PATHTOCONFIGTEST);
            configFileBuilder.CreateConfigurationToTempFolder();
            configFileBuilder.WriteTempConfigurationToFinalFile();
        }


        [TestMethod]
        public void TestFileDependentImporter()
        { 
            
            MySettingsVS vsSettingmodAA = MySettingsVS.CreateMySettingsVS(@"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\Module1AA");
            vsSettingmodAA.Initiate(); 
             

            FileDepedentsImporter imprter = new FileDepedentsImporter("pref1o2",vsSettingmodAA.CLCompileFiles,vsSettingmodAA.CLIncludeFiles, vsSettingmodAA.PATHOfProject);
            imprter.ImportFilesToPath(@"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\CodeGeneratorTest\bin\Debug\TestImporter");
        }

        [TestMethod]
        public void TestisStartsWithHashtagInclude()
        {
            // string line1 = @"#include blabla";
            // string line2 = @"// somethign sdds #include blabla";
            // string line3 = @"  #include blabla";
            // string line4 = @"int e = 4 #include blabla";

            //Assert.IsTrue(line1.isStartsWithHashtagInclude());
            //Assert.IsTrue(!line2.isStartsWithHashtagInclude());
            //Assert.IsTrue(line3.isStartsWithHashtagInclude());
            //Assert.IsTrue(!line4.isStartsWithHashtagInclude());

        }

        [TestMethod]
        public void ChangeFileName()
        {


            //CppRefactorer refact = new CppRefactorer(new DirectoryInfo(@"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\CodeGeneratorTest\bin\Debug\TestCppRefactor\FileNameChange"));

            //refact.ChangeNameOfFile(@"rg.h","pre_rg.h"); 
        }

        [TestMethod]
        public void InsertNAmespaceIntoFile()
        {
            //set the test files-------------------------------------
            //if (Directory.Exists(@"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\CodeGeneratorTest\bin\Debug\TestCppRefactor\FileNameChangeTemp"))
            //{
            //    Directory.Delete(@"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\CodeGeneratorTest\bin\Debug\TestCppRefactor\FileNameChangeTemp",true);
            //} 
            //Directory.CreateDirectory(   @"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\CodeGeneratorTest\bin\Debug\TestCppRefactor\FileNameChangeTemp");
            //string[] files = Directory.GetFiles(
            //    @"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\CodeGeneratorTest\bin\Debug\TestCppRefactor\DontChangeAnythingHere");
            //foreach (var file in files)
            //{
            //    File.Copy(file, Path.Combine(@"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\CodeGeneratorTest\bin\Debug\TestCppRefactor\FileNameChangeTemp", Path.GetFileName(file)));
            //}
            ////-------------------------------------------------------


            //CppRefactorer refact = new CppRefactorer(new DirectoryInfo(@"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\CodeGeneratorTest\bin\Debug\TestCppRefactor\FileNameChangeTemp"));

            //refact.InsertNamespaceIntoAllFiles("bla"); 
        }


        [TestMethod]
        public void VisitorOfKind()
        {
            //thesting for enumDeclaration

            //CppParser parser = new CppParser(@"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\ConfigTest\GlobalBuildConfig.h");
            //List<MyCursorOfKindEnumDecl> enumcursors =  parser.GetAllCursorsOfKind<MyCursorOfKindEnumDecl>();


            //Assert.IsTrue(enumcursors.Count == 2);

            //MyCursorOfKindEnumDecl platformenum = enumcursors.Where((MyCursorOfKindEnumDecl myc) =>
            //{
            //    return myc.getCursorSpelling() == "PlatformEnum";
            //}).First();
            //var children = platformenum.GetChildrenOfKind_EnumConstantDecl();
            
            ////var z = enumcursors[0].getEnumConstantDeclValue();
        }


    }
}
