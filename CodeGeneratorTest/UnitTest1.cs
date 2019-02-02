using System;
using System.IO;
using System.Reflection;
using CodeGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeGenerator.IDESettingXMLs;
using CodeGenerator.FileTemplates;
using CodeGenerator.IDESettingXMLs.VisualStudioXMLs;
using CodeGenerator.ProjectBuilders;

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

    }
}
