using CodeGenerator.IDESettingXMLs.VisualStudioXMLs.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Extensions;
using ExtensionMethods;
using CodeGenerator.FileTemplates;

namespace CodeGenerator.IDESettingXMLs.VisualStudioXMLs
{
    public class MySettingsVS : MySettingsBase
    {

        static Random rng = new Random();
        public MySettingsVS(IDESetting xmlFilterSetting, IDESetting xmlProjectsetting) : base(xmlFilterSetting, xmlProjectsetting)//(Filters.Project xmlFilterClass, CodeGenerator.IDESettingXMLs.VisualStudioXMLs.Project XMLProjectClass) : base(xmlFilterClass, XMLProjectClass)
        {

        }


        public static MySettingsVS CreateMySettingsVS(string pathToProjectSettings)
        {

            IDESettingVSProj settingProj = new IDESettingVSProj(pathToProjectSettings, ".vcxproj", typeof(IDESettingXMLs.VisualStudioXMLs.Project));

            IDESetting settingFilter = new IDESetting(pathToProjectSettings, ".filters", typeof(IDESettingXMLs.VisualStudioXMLs.Filters.Project));

            MySettingsVS vssetting = new MySettingsVS(settingFilter, settingProj);
            vssetting.Initiate();
            return vssetting;
        } 


        protected override void AddSTRINGIncludeAsAdditionalIncludes(string additionalIncludeDir)
        {
            // add a ; if there is no percentage character in the string
            if (!additionalIncludeDir.Contains("%"))
            {
                additionalIncludeDir += ";";
            }

            ItemDefinitionGroup tdgs = ((CodeGenerator.IDESettingXMLs.VisualStudioXMLs.Project)XmlProjectClass).ItemDefinitionGroup.Where((ItemDefinitionGroup tdg) => tdg.ClCompile.AdditionalIncludeDirectories != "").First();

            tdgs.ClCompile.AdditionalIncludeDirectories += additionalIncludeDir;
        }

        protected override bool DoesAdditionalIncludesAlreadyExist(string additionalIncludeDir)
        {
            ItemDefinitionGroup tdgs = ((CodeGenerator.IDESettingXMLs.VisualStudioXMLs.Project)XmlProjectClass).ItemDefinitionGroup.Where((ItemDefinitionGroup tdg) => tdg.ClCompile.AdditionalIncludeDirectories != "").First();
            if (tdgs.ClCompile.AdditionalIncludeDirectories == null)
            {
                return false;
            } 

            List<char> cc = new  List<char>();
            //create regex pattern by putting escapes
            string pat = "";
            foreach (char c in additionalIncludeDir)
            {
                pat += c.ToString();
                if (c.ToString() == "\\")
                {
                    pat += "\\";
                }
            }
            pat += ";";
            string sofar = tdgs.ClCompile.AdditionalIncludeDirectories;   
            if(!Regex.IsMatch(sofar, pat))//(!tdgs.ClCompile.AdditionalIncludeDirectories.ToLiteral().Contains(additionalIncludeDir.ToLiteral())) //(Regex.IsMatch((string.Format(@tdgs.ClCompile.AdditionalIncludeDirectories)), (string.Format(@additionalIncludeDir))))  //!tdgs.ClCompile.AdditionalIncludeDirectories.Contains(additionalIncludeDir))
            {
                return false;
            }
            return true;
        }

        protected override List<string> ConvertAllCurrentAdditionalIncludesAsStrings()
        {
            List<string> additionIncResult = new List<string>();


            ItemDefinitionGroup tdgs = ((CodeGenerator.IDESettingXMLs.VisualStudioXMLs.Project)XmlProjectClass).ItemDefinitionGroup.Where((ItemDefinitionGroup tdg) => tdg.ClCompile.AdditionalIncludeDirectories != "").First();
            if (tdgs.ClCompile.AdditionalIncludeDirectories != null)
            {
                additionIncResult = tdgs.ClCompile.AdditionalIncludeDirectories.Split(';').ToList();
            }
            return additionIncResult;
        }



        protected override void AddMyFilterAsXMLFilter(MyFilter myfilters) // where t : IDESettingXMLs.VisualStudioXMLs.Filters.Filter
        {


            //var CurrentxmlFilters = (((Project)XmlFilterClass).ItemGroup[0].Filter);

            //go through all filters and get their xml filter
            foreach (var filter in myfilters)
            {
                int anythereAlready = (((Filters.Project)XmlFilterClass).ItemGroup[0].Filter).Where((IDESettingXMLs.VisualStudioXMLs.Filters.Filter xmlFilter) => { return xmlFilter.Include == filter.GetFullAddress(); }).Count();
                if (anythereAlready == 0)
                {
                    //not there yet, so create it
                    IDESettingXMLs.VisualStudioXMLs.Filters.Filter xmlFilter = new VisualStudioXMLs.Filters.Filter();
                    xmlFilter.Include = filter.GetFullAddress();
                    xmlFilter.UniqueIdentifier = "{" + (rng.Next(10000000, 99999999)).ToString() + "-" + (rng.Next(1000, 9999)).ToString() + "-" + (rng.Next(1000, 9999)).ToString() + "-" + (rng.Next(1000, 9999)).ToString() + "-" + (rng.Next(10000000, 99999999)).ToString() + (rng.Next(1000, 9999)).ToString() + "}";//93995380-89BD-4b04-88EB-625FBE52EBFB
                    xmlFilter.Extensions = "";
                    (((Filters.Project)XmlFilterClass).ItemGroup[0].Filter).Add(xmlFilter);
                }
            }

        }



        protected override List<IDESettingXMLs.MyFilter> ConvertAllCurrentXMLFiltersAsMyFilters()
        {
            List<IDESettingXMLs.MyFilter> filtersResult = new List<IDESettingXMLs.MyFilter>();
            //create a copy of the 
            foreach (var itemGroup in ((Filters.Project)XmlFilterClass).ItemGroup)
            {
                //iterate through each filter
                foreach (var filter in itemGroup.Filter)
                {

                    //see first if it is a child of another filter
                    if (filter.Include.Contains("\\"))
                    {

                    }
                    else
                    {
                        //else it is top level parent
                        filtersResult.Add(new IDESettingXMLs.MyFilter(filter.Include));
                    }
                }

                //now do this again but for the second and higher level filters
                //iterate through each filter 
                for (int i = 1; i < 20; i++)
                {
                    var childFilters = itemGroup.Filter.Where((filter) => { return (filter.Include.Count((char c) => { return c == '\\'; }) == i); });
                    foreach (var filter in childFilters)
                    {
                        //find the filter that is the parent of that address. truncate the last name
                        string parentAddress = Regex.Replace(filter.Include, @"\\([^\\.]+)$", "");

                        //go through all current filters added and find the parent. 
                        foreach (var filterp in filtersResult)
                        {
                            IDESettingXMLs.MyFilter filtExist = filterp.DoesAddressExist(parentAddress);

                            //check if filterp matches parentAddress
                            if (filtExist != null)
                            {
                                filtExist.AddChildFilter(filter.Include);
                            }
                        }

                    }
                }

            }

            return filtersResult;
        }

        protected override List<MyCLIncludeFile> ConvertAllCurrentCIncludesAsMyClIncludes()
        {
            List<MyCLIncludeFile> MyCLIncludeFileResult = new List<IDESettingXMLs.MyCLIncludeFile>();

            var ItemGroupWithClIncludeFromfilter = ((Filters.Project)XmlFilterClass).ItemGroup
                .Where((Filters.ItemGroup itmG) => { return itmG.ClInclude.Count != 0; })
                .First();

            foreach (var clInclude in ItemGroupWithClIncludeFromfilter.ClInclude)
            {
                //create new Myclinclude
                //the name will be the file at the end of the path minus the extension .h  
                //the filter will be the one that matches the path of the inlcude minus filename
                var filterTheCincBelongsTo = myFilters
                    .Where((MyFilter fil) =>
                    {

                        return fil.GetFilterFromAddress(clInclude.Filter) != null;
                    })
                    .First();
                var MyClInc = new MyCLIncludeFile(filterTheCincBelongsTo, Path.GetFileName(clInclude.Include), Path.GetDirectoryName(clInclude.Include));

                MyCLIncludeFileResult.Add(MyClInc);
            }

            return MyCLIncludeFileResult;
        }

        public override void RecreateConfigurationFilterFolderIncludes(string NameOfCGenProject)
        {
            
            MyFilter configFilter;
            if (!myFilters.DoesFilterWithNameExist("Config"))
            {
                configFilter = new MyFilter("Config"); 
                AddFilter(configFilter);
            }
            else
            {
                configFilter = myFilters.GetFilterAtAddress("Config");
            }
            

            //-------------FolderCreation
            //does config folder exist
            string ConfDirPath = Path.Combine(Program.envIronDirectory, "Config");// Path.Combine(Path.GetDirectoryName(LibTop.config.ConfigFileFullPath), "Config");
            if (!Directory.Exists(ConfDirPath))
            {
                Directory.CreateDirectory(ConfDirPath);
            }

            //-------------mainCG.cpp NameOfCGenProjectConf.h Files  Configuration.h
            //   settings should already check if it exists before adding it
            MyCLCompileFile ccompMainCg = new MyCLCompileFile(configFilter, "mainCG", "Config");
            AddCLCompileFile(ccompMainCg); 
            if (!File.Exists(Path.Combine(ConfDirPath, "mainCG.cpp")))
            {
                FileTemplateMainCG maincgTemplate = new FileTemplateMainCG(ConfDirPath, NameOfCGenProject);
                maincgTemplate.CreateTemplate();
                Console.WriteLine("mainCG.cpp" + "file created");
            }
            MyCLIncludeFile ccincNameOfCGenProjectConf = new MyCLIncludeFile(configFilter, NameOfCGenProject + "Conf", "Config");
            AddCLIncludeFile(ccincNameOfCGenProjectConf);
            if (!File.Exists(Path.Combine(ConfDirPath, NameOfCGenProject + "Conf.h")))
            {
                FileTemplateLibConf maincgTemplate = new FileTemplateLibConf(ConfDirPath, NameOfCGenProject);
                maincgTemplate.CreateTemplate();
                Console.WriteLine(NameOfCGenProject + "Conf.h" + "file created");
            }
            MyCLIncludeFile ccincConfiguration = new MyCLIncludeFile(configFilter, "Configuration.h", "Config");
            AddCLIncludeFile(ccincConfiguration);
            if (!File.Exists(Path.Combine(ConfDirPath, "Configuration.h")))
            {
                File.Create(Path.Combine(ConfDirPath, "Configuration.h"));
                Console.WriteLine("Configuration.h " + "file created");
            }


            //add additionalinclude for configTest
            AddAdditionalInclude(Program.PATHTOCONFIGTEST);
            

            //save the settings 
            GenerateXMLSettings(Program.envIronDirectory);


        }

        protected override void AddMyClIncludesAsCIncludes(MyCLIncludeFile CLIncludeFile)
        {
            //CCinclude affects TWO xml classes   
            //XmlFIlters Itemgroup CLCompile
            //xmlProject Itemgroup CLCompile
            //make sure both are created and added  
            var ItemGroupWithCincludesPROJ = ((Project)XmlProjectClass).ItemGroup.Where((ItemGroup itmG) => { return itmG.ClInclude.Count > 0; }).First();
            var ItemGroupWithCCompilesFILT = ((Filters.Project)XmlFilterClass).ItemGroup.Where((Filters.ItemGroup itmG) => { return itmG.ClInclude.Count > 0; }).First();

            // create it 
            ClInclude theClIncToCreate = new ClInclude();
            Filters.ClInclude theClIncToCreateFromFilter = new Filters.ClInclude();

            theClIncToCreate.Include = CLIncludeFile.FullLocationName;
            theClIncToCreateFromFilter.Filter = CLIncludeFile.FilterIBelongTo.GetFullAddress();
            theClIncToCreateFromFilter.Include = CLIncludeFile.FullLocationName; //this should be the physical location of the file.

            ItemGroupWithCincludesPROJ.ClInclude.Add(theClIncToCreate);
            ItemGroupWithCCompilesFILT.ClInclude.Add(theClIncToCreateFromFilter);

        }


        protected override bool DoesClIncludeExist(MyCLIncludeFile CLIncludeFile)
        {
            var ItemGroupWithCincludesPROJ = ((Project)XmlProjectClass).ItemGroup.Where((ItemGroup itmG) => { return itmG.ClInclude.Count > 0; }).First();

            foreach (var CLInclude in ItemGroupWithCincludesPROJ.ClInclude)
            {

                if (CLIncludeFile.FullFilterName == CLInclude.Include)
                {
                    return true;
                }
            }

            return false;
        }


        protected override List<MyCLCompileFile> ConvertAllCurrentCCompilessAsMyClCompiles()
        {


            List<MyCLCompileFile> MyCLCompileFileResult = new List<IDESettingXMLs.MyCLCompileFile>();

            var ItemGroupWithClCompFromFilter = ((Filters.Project)XmlFilterClass).ItemGroup
                .Where((Filters.ItemGroup itmG) => { return itmG.ClCompile.Count != 0; })
                .First();

            foreach (var cCompile in ItemGroupWithClCompFromFilter.ClCompile)
            {
                //create new Myclinclude
                //the name will be the file at the end of the path minus the extension .h  
                //the filter will be the one that matches the path of the inlcude minus filename
                var filterTheCcompBelongsTo = myFilters
                    .Where((MyFilter fil) => { return fil.GetFilterFromAddress(cCompile.Filter) != null; })
                    .First();

                var MyCComp = new MyCLCompileFile(filterTheCcompBelongsTo, Path.GetFileName(cCompile.Include), Path.GetDirectoryName(cCompile.Include));

                MyCLCompileFileResult.Add(MyCComp);
            }

            return MyCLCompileFileResult;
        }

        protected override void AddMyCLCompileFileAsCLCompileFile(MyCLCompileFile CLCompileFile)
        {
            //CComile affects TWO xml classes   
            //XmlFIlters Itemgroup CLCompile
            //xmlProject Itemgroup CLCompile
            //make sure both are created and added 

            var ItemGroupWithCCompilesPROJ = ((Project)XmlProjectClass).ItemGroup.Where((ItemGroup itmG) => { return itmG.ClCompile.Count > 0; }).First();
            var ItemGroupWithCCompilesFILT = ((Filters.Project)XmlFilterClass).ItemGroup.Where((Filters.ItemGroup itmG) => { return itmG.ClCompile.Count > 0; }).First();

            // create it  
            ClCompile theClCompToCreate = new ClCompile();
            Filters.ClCompile theClCompToCreateFromFilter = new Filters.ClCompile();

            theClCompToCreateFromFilter.Filter = CLCompileFile.FilterIBelongTo.GetFullAddress();
            theClCompToCreateFromFilter.Include = CLCompileFile.FullLocationName;
            theClCompToCreate.Include = CLCompileFile.FullLocationName;

            ItemGroupWithCCompilesPROJ.ClCompile.Add(theClCompToCreate);
            ItemGroupWithCCompilesFILT.ClCompile.Add(theClCompToCreateFromFilter);
        }

        protected override bool DoesCCompileExist(MyCLCompileFile clCompileToCheck)
        {
            var ItemGroupWithCincludesPROJ = ((Project)XmlProjectClass).ItemGroup.Where((ItemGroup itmG) => { return itmG.ClCompile.Count > 0; }).First();

            foreach (var CLCompil in ItemGroupWithCincludesPROJ.ClCompile)
            {

                if (clCompileToCheck.FullFilterName == CLCompil.Include)
                {
                    return true;
                }
            }

            return false;
        }


        protected override void RemoveAllMentionsOfLibraryDependencyFilters()
        {
            //filters only show up in the filter settings
            ((Filters.Project)XmlSettings[0].RootOfSetting).ItemGroup
                .Where((Filters.ItemGroup itg) => { return itg.Filter.Count != 0; })
                .ToList().First().Filter
                .RemoveAll((Filters.Filter filt) => { return filt.Include.Contains("LibraryDependencies"); });

        }

        protected override void RemoveAllMentionsOfLibraryDependencyCLIncludes()
        {
            //clincludes shows up in filter settings and project settings
            ((Filters.Project)XmlSettings[0].RootOfSetting).ItemGroup
                .Where((Filters.ItemGroup itg) => { return itg.ClInclude.Count != 0; })
                .ToList().First().ClInclude
                .RemoveAll((Filters.ClInclude cl) => { return cl.Filter.Contains("LibraryDependencies"); });


            //for proj settings
            ((Project)XmlSettings[1].RootOfSetting).ItemGroup
                .Where((ItemGroup itg) => { return itg.ClInclude.Count != 0; })
                .ToList().First().ClInclude
                .RemoveAll((ClInclude cl) => { return cl.Include.Contains("LibraryDependencies"); });
        }

        protected override void RemoveAllMentionsOfLibraryDependencyCLCompiles()
        {
            //clincludes shows up in filter settings and project settings
            ((Filters.Project)XmlSettings[0].RootOfSetting).ItemGroup
                .Where((Filters.ItemGroup itg) => { return itg.ClCompile.Count != 0; })
                .ToList().First().ClCompile
                .RemoveAll((Filters.ClCompile cl) => { return cl.Filter.Contains("LibraryDependencies"); });


            //for proj settings
            ((Project)XmlSettings[1].RootOfSetting).ItemGroup
                .Where((ItemGroup itg) => { return itg.ClCompile.Count != 0; })
                .ToList().First().ClCompile
                .RemoveAll((ClCompile cl) => { return cl.Include.Contains("LibraryDependencies"); });
        }
         
    }
}
