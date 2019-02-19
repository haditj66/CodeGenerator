using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
 using Extensions;
using CommandLine; 
using CodeGenerator.IDESettingXMLs.VisualStudioXMLs;
using CodeGenerator.IDESettingXMLs;

namespace CodeGenerator.IDESettingXMLs.VisualStudioXMLs
{
    public class IDESettingVSProj : IDESetting
    {
        public IDESettingVSProj(string PathWithoutFileNameOfXmlSetting, string projectExtension, Type typeOfRootSetting) : base(PathWithoutFileNameOfXmlSetting, projectExtension, typeOfRootSetting)
        {
        }

        public override void GenerateXMLSetting(string FullPathToPutGeneratedXMLFileWITHOUTFILENAME)
        {

            //NOTE VS project SETTING IS STUBBORN AND WILL OWNLY WORK WITH EXACT ORDERING OF THE XML ELEMENTS!!!!
            //EXPLAINED HERE
            //https://docs.microsoft.com/en-us/cpp/ide/vcxproj-file-structure?view=vs-2017

            //here is the order
            /*<Project DefaultTargets="Build" ToolsVersion="4.0" xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
             <ItemGroup Label="ProjectConfigurations" />
             <PropertyGroup Label="Globals" />
             <Import Project="$(VCTargetsPath)\Microsoft.Cpp.default.props" />
             <PropertyGroup Label="Configuration" />
             <Import Project="$(VCTargetsPath)\Microsoft.Cpp.props" />
             <ImportGroup Label="ExtensionSettings" />
             <ImportGroup Label="PropertySheets" />
             <PropertyGroup Label="UserMacros" />
             <PropertyGroup />
             <ItemDefinitionGroup />
             <ItemGroup />
             <Import Project="$(VCTargetsPath)\Microsoft.Cpp.targets" />
             <ImportGroup Label="ExtensionTargets" />
            </Project>
            */


            //creat a deep copy of root setting using a memory stream 
            IDESettingXMLs.VisualStudioXMLs.Project RootOfSetting2 = restOfExtensions.DeepCopy<IDESettingXMLs.VisualStudioXMLs.Project>(RootOfSetting);
  


            string FULLFILEPATH = GetFullFilePathFromPathWithoutFile(FullPathToPutGeneratedXMLFileWITHOUTFILENAME);
            List<IDESettingXMLs.VisualStudioXMLs.Project> projsJustForSer = new List<IDESettingXMLs.VisualStudioXMLs.Project>();
             
            //serialize one by one.
            //first  serialize the importgroups with  label = ProjectConfigurations 
            XmlSerializer xmlser = new XmlSerializer(typeof(IDESettingXMLs.VisualStudioXMLs.Project));

            Func<IDESettingXMLs.VisualStudioXMLs.ItemGroup, bool> pred = (IDESettingXMLs.VisualStudioXMLs.ItemGroup ig) => { return ig.Label == "ProjectConfigurations"; };
            var ToSerialize = ((IDESettingXMLs.VisualStudioXMLs.Project)RootOfSetting).ItemGroup
                .Where(pred).ToList();
            RootOfSetting.ItemGroup = ((IDESettingXMLs.VisualStudioXMLs.Project)RootOfSetting).ItemGroup.Except(ToSerialize).ToList();
            projsJustForSer.Add(new IDESettingXMLs.VisualStudioXMLs.Project());
            projsJustForSer[0].ItemGroup = ToSerialize;



            //get propertygroup next 
            Func<IDESettingXMLs.VisualStudioXMLs.PropertyGroup, bool> pred2 = (IDESettingXMLs.VisualStudioXMLs.PropertyGroup ig) => { return ig.Label == "Globals"; };
            var ToSerialize2 = ((IDESettingXMLs.VisualStudioXMLs.Project)RootOfSetting).PropertyGroup
               .Where(pred2).ToList();
            RootOfSetting.PropertyGroup = ((IDESettingXMLs.VisualStudioXMLs.Project)RootOfSetting).PropertyGroup.Except(ToSerialize2).ToList();

            projsJustForSer.Add(new IDESettingXMLs.VisualStudioXMLs.Project());
            projsJustForSer[1].PropertyGroup = ToSerialize2;

            //next is the  <Import Project="$(VCTargetsPath)\Microsoft.Cpp.default.props" />
            Func<IDESettingXMLs.VisualStudioXMLs.Import, bool> pred3 = (IDESettingXMLs.VisualStudioXMLs.Import ig) => { return ig.Project.ToLower() == @"$(VCTargetsPath)\Microsoft.Cpp.default.props".ToLower(); };
            var ToSerialize3 = ((IDESettingXMLs.VisualStudioXMLs.Project)RootOfSetting).Import
            .Where(pred3).ToList();
            RootOfSetting.Import = ((IDESettingXMLs.VisualStudioXMLs.Project)RootOfSetting).Import.Except(ToSerialize3).ToList();

            projsJustForSer.Add(new IDESettingXMLs.VisualStudioXMLs.Project());
            projsJustForSer[2].Import = ToSerialize3;

            //propertygoup with label = configuration is next
            Func<IDESettingXMLs.VisualStudioXMLs.PropertyGroup, bool> pred4 = (IDESettingXMLs.VisualStudioXMLs.PropertyGroup ig) => { return ig.Label == "Configuration"; };
            var ToSerialize4 = ((IDESettingXMLs.VisualStudioXMLs.Project)RootOfSetting).PropertyGroup
            .Where(pred4).ToList();
            RootOfSetting.PropertyGroup = ((IDESettingXMLs.VisualStudioXMLs.Project)RootOfSetting).PropertyGroup.Except(ToSerialize4).ToList();

            projsJustForSer.Add(new IDESettingXMLs.VisualStudioXMLs.Project());
            projsJustForSer[3].PropertyGroup = ToSerialize4;

            //<Import Project="$(VCTargetsPath)\Microsoft.Cpp.props" /> is next
            Func<IDESettingXMLs.VisualStudioXMLs.Import, bool> pred5 = (IDESettingXMLs.VisualStudioXMLs.Import ig) => { return ig.Project.ToLower() == @"$(VCTargetsPath)\Microsoft.Cpp.props".ToLower(); };
            var ToSerialize5 = ((IDESettingXMLs.VisualStudioXMLs.Project)RootOfSetting).Import
            .Where(pred5).ToList();
            RootOfSetting.Import = ((IDESettingXMLs.VisualStudioXMLs.Project)RootOfSetting).Import.Except(ToSerialize5).ToList();

            projsJustForSer.Add(new IDESettingXMLs.VisualStudioXMLs.Project());
            projsJustForSer[4].Import = ToSerialize5;

            //importgroups with  label = ExtensionSettings is next 
            Func<IDESettingXMLs.VisualStudioXMLs.ImportGroup, bool> pred6 = (IDESettingXMLs.VisualStudioXMLs.ImportGroup ig) => { return ig.Label == "ExtensionSettings"; };
            var ToSerialize6 = ((IDESettingXMLs.VisualStudioXMLs.Project)RootOfSetting).ImportGroup
                .Where(pred6).ToList();
            RootOfSetting.ImportGroup = ((IDESettingXMLs.VisualStudioXMLs.Project)RootOfSetting).ImportGroup.Except(ToSerialize6).ToList();

            projsJustForSer.Add(new IDESettingXMLs.VisualStudioXMLs.Project());
            projsJustForSer[5].ImportGroup = ToSerialize6;

            //importgroups with  label = Shared is next 
            Func<IDESettingXMLs.VisualStudioXMLs.ImportGroup, bool> pred7 = (IDESettingXMLs.VisualStudioXMLs.ImportGroup ig) => { return ig.Label == "Shared"; };
            var ToSerialize7 = ((IDESettingXMLs.VisualStudioXMLs.Project)RootOfSetting).ImportGroup
                .Where(pred7).ToList();
            RootOfSetting.ImportGroup = ((IDESettingXMLs.VisualStudioXMLs.Project)RootOfSetting).ImportGroup.Except(ToSerialize7).ToList();

            projsJustForSer.Add(new IDESettingXMLs.VisualStudioXMLs.Project());
            projsJustForSer[6].ImportGroup = ToSerialize7;

            //importgroups with  label = LocalAppDataPlatform is next
            Func<IDESettingXMLs.VisualStudioXMLs.ImportGroup, bool> pred8 = (IDESettingXMLs.VisualStudioXMLs.ImportGroup ig) => { return ig.Label == "PropertySheets"; };
            var ToSerialize8 = ((IDESettingXMLs.VisualStudioXMLs.Project)RootOfSetting).ImportGroup
                .Where(pred8).ToList();
            RootOfSetting.ImportGroup = ((IDESettingXMLs.VisualStudioXMLs.Project)RootOfSetting).ImportGroup.Except(ToSerialize8).ToList();

            projsJustForSer.Add(new IDESettingXMLs.VisualStudioXMLs.Project());
            projsJustForSer[7].ImportGroup = ToSerialize8;


            //all import groups that DONT have the label = ExtensionTargets is next
            Func<IDESettingXMLs.VisualStudioXMLs.ImportGroup, bool> pred9 = (IDESettingXMLs.VisualStudioXMLs.ImportGroup ig) => { return ig.Label != "ExtensionTargets"; };
            var ToSerialize9 = ((IDESettingXMLs.VisualStudioXMLs.Project)RootOfSetting).ImportGroup
                .Where(pred9).ToList();
            RootOfSetting.ImportGroup = ((IDESettingXMLs.VisualStudioXMLs.Project)RootOfSetting).ImportGroup.Except(ToSerialize9).ToList();

            projsJustForSer.Add(new IDESettingXMLs.VisualStudioXMLs.Project());
            projsJustForSer[8].ImportGroup = ToSerialize9;

            //<PropertyGroup Label="UserMacros" /> is next
            Func<IDESettingXMLs.VisualStudioXMLs.PropertyGroup, bool> pred10 = (IDESettingXMLs.VisualStudioXMLs.PropertyGroup ig) => { return ig.Label == "UserMacros"; };
            var ToSerialize10 = ((IDESettingXMLs.VisualStudioXMLs.Project)RootOfSetting).PropertyGroup
                .Where(pred10).ToList();
            RootOfSetting.PropertyGroup = ((IDESettingXMLs.VisualStudioXMLs.Project)RootOfSetting).PropertyGroup.Except(ToSerialize10).ToList();

            projsJustForSer.Add(new IDESettingXMLs.VisualStudioXMLs.Project());
            projsJustForSer[9].PropertyGroup = ToSerialize10;

            //put the rest of the property groups here
            Func<IDESettingXMLs.VisualStudioXMLs.PropertyGroup, bool> pred11 = (IDESettingXMLs.VisualStudioXMLs.PropertyGroup ig) => { return true; };
            var ToSerialize11 = ((IDESettingXMLs.VisualStudioXMLs.Project)RootOfSetting).PropertyGroup
                .Where(pred11).ToList();
            RootOfSetting.PropertyGroup = ((IDESettingXMLs.VisualStudioXMLs.Project)RootOfSetting).PropertyGroup.Except(ToSerialize11).ToList();

            projsJustForSer.Add(new IDESettingXMLs.VisualStudioXMLs.Project());
            projsJustForSer[10].PropertyGroup = ToSerialize11;

            //put the rest of the ItemDefinitionGroup here
            Func<IDESettingXMLs.VisualStudioXMLs.ItemDefinitionGroup, bool> pred12 = (IDESettingXMLs.VisualStudioXMLs.ItemDefinitionGroup ig) => { return true; };
            var ToSerialize12 = ((IDESettingXMLs.VisualStudioXMLs.Project)RootOfSetting).ItemDefinitionGroup
                .Where(pred12).ToList();
            RootOfSetting.ItemDefinitionGroup = ((IDESettingXMLs.VisualStudioXMLs.Project)RootOfSetting).ItemDefinitionGroup.Except(ToSerialize12).ToList();

            projsJustForSer.Add(new IDESettingXMLs.VisualStudioXMLs.Project());
            projsJustForSer[11].ItemDefinitionGroup = ToSerialize12;

            //put itemgroup for compile here
            Func<IDESettingXMLs.VisualStudioXMLs.ItemGroup, bool> pred13 = (IDESettingXMLs.VisualStudioXMLs.ItemGroup ig) => { return ig.ClCompile.Count > 0; };
            var ToSerialize13 = ((IDESettingXMLs.VisualStudioXMLs.Project)RootOfSetting).ItemGroup
                .Where(pred13).ToList();
            RootOfSetting.ItemGroup = ((IDESettingXMLs.VisualStudioXMLs.Project)RootOfSetting).ItemGroup.Except(ToSerialize13).ToList();

            projsJustForSer.Add(new IDESettingXMLs.VisualStudioXMLs.Project());
            projsJustForSer[12].ItemGroup = ToSerialize13;

            //put itemgroup for include here
            Func<IDESettingXMLs.VisualStudioXMLs.ItemGroup, bool> pred14 = (IDESettingXMLs.VisualStudioXMLs.ItemGroup ig) => { return ig.ClInclude.Count > 0; };
            var ToSerialize14 = ((IDESettingXMLs.VisualStudioXMLs.Project)RootOfSetting).ItemGroup
                .Where(pred14).ToList();
            RootOfSetting.ItemGroup = ((IDESettingXMLs.VisualStudioXMLs.Project)RootOfSetting).ItemGroup.Except(ToSerialize14).ToList();

            projsJustForSer.Add(new IDESettingXMLs.VisualStudioXMLs.Project());
            projsJustForSer[13].ItemGroup = ToSerialize14;

            //put the rest of itemgroups here
            Func<IDESettingXMLs.VisualStudioXMLs.ItemGroup, bool> pred15 = (IDESettingXMLs.VisualStudioXMLs.ItemGroup ig) => { return ig.ClInclude.Count > 0; };
            var ToSerialize15 = ((IDESettingXMLs.VisualStudioXMLs.Project)RootOfSetting).ItemGroup
                .Where(pred15).ToList();
            RootOfSetting.ItemGroup = ((IDESettingXMLs.VisualStudioXMLs.Project)RootOfSetting).ItemGroup.Except(ToSerialize15).ToList();

            projsJustForSer.Add(new IDESettingXMLs.VisualStudioXMLs.Project());
            projsJustForSer[14].ItemGroup = ToSerialize15;

            //<Import Project="$(VCTargetsPath)\Microsoft.Cpp.targets" /> is next
            Func<IDESettingXMLs.VisualStudioXMLs.Import, bool> pred16 = (IDESettingXMLs.VisualStudioXMLs.Import ig) => { return ig.Project.ToLower() == @"$(VCTargetsPath)\Microsoft.Cpp.targets".ToLower(); };
            var ToSerialize16 = ((IDESettingXMLs.VisualStudioXMLs.Project)RootOfSetting).Import
            .Where(pred16).ToList();
            RootOfSetting.Import = ((IDESettingXMLs.VisualStudioXMLs.Project)RootOfSetting).Import.Except(ToSerialize16).ToList();

            projsJustForSer.Add(new IDESettingXMLs.VisualStudioXMLs.Project());
            projsJustForSer[15].Import = ToSerialize16;

            //the rest of import groups go here
            Func<IDESettingXMLs.VisualStudioXMLs.ImportGroup, bool> pred17 = (IDESettingXMLs.VisualStudioXMLs.ImportGroup ig) => { return true; };
            var ToSerialize17 = ((IDESettingXMLs.VisualStudioXMLs.Project)RootOfSetting).ImportGroup
                .Where(pred17).ToList();
            RootOfSetting.ImportGroup = ((IDESettingXMLs.VisualStudioXMLs.Project)RootOfSetting).ImportGroup.Except(ToSerialize17).ToList();

            projsJustForSer.Add(new IDESettingXMLs.VisualStudioXMLs.Project());
            projsJustForSer[16].ImportGroup = ToSerialize17;


            using (StreamWriter sw = new StreamWriter(FULLFILEPATH))
            {
                foreach (var projForSer in projsJustForSer)
                {
                    xmlser.Serialize(sw, projForSer);
                }
            }


            //now that it has been serialized,  need to go back and take out the project header tags
            string FileWithCorrectHeaders;
            using (StreamReader sr = new StreamReader(FULLFILEPATH))
            {
                string filestr = sr.ReadToEnd();
                //replace all mentions of
                //<?xml version="1.0" encoding="utf-8"?>
                //<Project xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
                //</Project>  
                string newstr = Regex.Replace(filestr, @"<\?xml version=""1.0"" encoding=""utf-8""\?>", "");
                string newstr3 = Regex.Replace(newstr, @"<Project \w*:?\w{3}="".*"" \w*:?\w{3}="".*"" \w*:?\w{3}="".*""\s*/?>", "");
                //string newstr3 = Regex.Replace(newstr2, @"<Project xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"" />", "");

                FileWithCorrectHeaders = Regex.Replace(newstr3, @"</Project>", "");
                FileWithCorrectHeaders = @"<?xml version=""1.0"" encoding=""utf-8""?>" + "\n" + @"<Project DefaultTargets=""Build"" ToolsVersion=""15.0"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">" + FileWithCorrectHeaders;

                FileWithCorrectHeaders += @"</Project>";


            }

            //finally write the new file with appropriate headers
            File.WriteAllText(FULLFILEPATH, FileWithCorrectHeaders);


            //set root setting back to what it was before all this
              RootOfSetting = restOfExtensions.DeepCopy<IDESettingXMLs.VisualStudioXMLs.Project>(RootOfSetting2);


        }
    }
}
