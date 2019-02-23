using CodeGenerator.IDESettingXMLs.VisualStudioXMLs.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Extensions;
using ExtensionMethods;
using CodeGenerator.FileTemplates;
using CodeGenerator.ProblemHandler;

namespace CodeGenerator.IDESettingXMLs.VisualStudioXMLs
{
    public class MySettingsVS : MySettingsBase
    {

        static Random rng = new Random();

        public MySettingsVS(IDESetting xmlFilterSetting, IDESetting xmlProjectsetting) :
            base(xmlFilterSetting,
                xmlProjectsetting) //(Filters.Project xmlFilterClass, CodeGenerator.IDESettingXMLs.VisualStudioXMLs.Project XMLProjectClass) : base(xmlFilterClass, XMLProjectClass)
        {
            /*
            IM HERE
            THIS IS NOT WORKING BECAUSE IF I WANT TO REMOVE AN INCLUDE I WILL NEED TO SPOT THE DIFFERENCE
            I NEED TO HAVE IT SO THAT ALL ADDITIONAL INCLUDES, ADDITIONAL LIBRARYDEPENDS AND LIBRARYLIBS ARE SET IN THE CONFIG FILE!!
            THAT IS THE WHOLE POINT OF THE APP ANYWAYS!!! DONT WORRY ABOUT RELEASE AND THINGS LIKe THAT!! THATs NOT PART
            OF THE WORKFLOW!!! IT DOESNT MATTER ABOUT OPTIMIZATIONS! IM ASSUMING YOU WILL ALWAYS BE IN DEBUG OR WILL CHANge CONFIGURATION OF
            VS ON YOUR OWN*/

            //INSTEAD IT WILL BE BETTER TO JUST GET ALL INCLUDES AND LIBRARIES FROM ALL PLATFORMS
            //AND THEN PUT ALL NONDUPLICATES INTO ALL AVAILABLE PLAFORMS

            //skip all this if I am doing the GlobalConfig.h library one.
            if (this.PATHOfProject != Program.PATHTOCONFIGTEST)
            {


                #region getting all allIncludes and allLibraries from all platforms ********************************

                List<ItemDefinitionGroup> itemDefinitionGroups =
                    ((CodeGenerator.IDESettingXMLs.VisualStudioXMLs.Project)XmlProjectClass)
                    .ItemDefinitionGroup; //.Where((ItemDefinitionGroup tdg) => tdg.ClCompile.AdditionalIncludeDirectories != "").ToList();

                //includes
                List<string> addincludesList = itemDefinitionGroups.Where((ItemDefinitionGroup tdg) =>
                    {
                        if (tdg.ClCompile == null)
                        {
                            return false;
                        }
                        return !string.IsNullOrEmpty(tdg.ClCompile.AdditionalIncludeDirectories);
                    }).ToList()
                    .Select(tdg => tdg.ClCompile.AdditionalIncludeDirectories).Distinct().ToList();
                //remove duplicates and flatten to one string 
                string allIncludes = string.Join(";", (string.Join(";",
                    addincludesList.Select((string s) => { return s; }))).Split(';').Distinct()); //{ return s.Split(';'); }));

                //library libs
                List<string> addlibrariesList = itemDefinitionGroups.Where((ItemDefinitionGroup tdg) =>
                {
                    if (tdg.Link == null)
                    {
                        return false;
                    }
                    return !string.IsNullOrEmpty(tdg.Link.AdditionalDependencies);
                }).ToList()

                    .Select(tdg => tdg.Link.AdditionalDependencies).Distinct().ToList();
                //remove duplicates and flatten to one string 
                string allLibraries = string.Join(";", (string.Join(";",
                    addlibrariesList.Select((string s) => { return s; }))).Split(';').Distinct()); //{ return s.Split(';'); }));

                //library dirs
                List<string> addlibrariesDirList = itemDefinitionGroups.Where((ItemDefinitionGroup tdg) =>
                    {
                        if (tdg.Link == null)
                        {
                            return false;
                        }
                        return !string.IsNullOrEmpty(tdg.Link.AdditionalLibraryDirectories);
                    }
                ).ToList()
                    .Select(tdg => tdg.Link.AdditionalLibraryDirectories).Distinct().ToList();
                string allLibrariesdir = string.Join(";", (string.Join(";",
                    addlibrariesDirList.Select((string s) => { return s; }))).Split(';').Distinct());

                #endregion



                #region set all library and includes to ALL configurations  ********************************
                //to make sure all configurations have the SAME additional library includes

                foreach (var tdg in itemDefinitionGroups)
                {
                    if (tdg.Link == null)
                    {
                        tdg.Link = new Link();
                    }
                    if (tdg.ClCompile == null)
                    {
                        tdg.ClCompile = new ClCompile();
                    }

                    tdg.Link.AdditionalDependencies = allLibraries; //string.Join("", allCurrentAdditionalsLibraryDirNoDuplicates);
                    tdg.Link.AdditionalLibraryDirectories = allLibrariesdir;
                    tdg.ClCompile.AdditionalIncludeDirectories = allIncludes;

                }
                #endregion


            }

        }


        public static MySettingsVS CreateMySettingsVS(string pathToProjectSettings)
        {

            IDESettingVSProj settingProj = new IDESettingVSProj(pathToProjectSettings, ".vcxproj", typeof(IDESettingXMLs.VisualStudioXMLs.Project));

            IDESetting settingFilter = new IDESetting(pathToProjectSettings, ".filters", typeof(IDESettingXMLs.VisualStudioXMLs.Filters.Project));

            MySettingsVS vssetting = new MySettingsVS(settingFilter, settingProj);
            vssetting.Initiate();
            return vssetting;
        }


        #region  additionalinclude stuff ********************************************************************

        protected override void AddSTRINGIncludeAsAdditionalIncludes(string additionalIncludeDir)
        {
            // add a ; if there is no percentage character in the string
            if (!additionalIncludeDir.Contains("%"))
            {
                additionalIncludeDir += ";";
            }


            //I NEED TO ADD THE INCLUDE TO ALL CONFIGURATIONS AS THE WHOLE POINT OF THIS APP IS THAT CONFIGURATIONS ARE HANDLED THROUGH THE
            // CONFIGURATION FILE GENERATED!

            List<ItemDefinitionGroup> itemDefinitionGroups = ((CodeGenerator.IDESettingXMLs.VisualStudioXMLs.Project)XmlProjectClass).ItemDefinitionGroup;//.Where((ItemDefinitionGroup tdg) => tdg.ClCompile.AdditionalIncludeDirectories != "").ToList();

            foreach (var idg in itemDefinitionGroups)
            {
                idg.ClCompile.AdditionalIncludeDirectories = additionalIncludeDir + idg.ClCompile.AdditionalIncludeDirectories;
            }

            //tdgs.ClCompile.AdditionalIncludeDirectories = additionalIncludeDir + tdgs.ClCompile.AdditionalIncludeDirectories;
        }

        protected override bool DoesAdditionalIncludesAlreadyExist(string additionalIncludeDir)
        {
            ItemDefinitionGroup tdgs = ((CodeGenerator.IDESettingXMLs.VisualStudioXMLs.Project)XmlProjectClass).ItemDefinitionGroup.Where((ItemDefinitionGroup tdg) => tdg.ClCompile.AdditionalIncludeDirectories != "").FirstOrDefault();
            if (tdgs == null)
            {
                return false;
            }
            /*
            List<char> cc = new List<char>();
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
            pat += ";";*/

            List<string> sofar = tdgs.ClCompile.AdditionalIncludeDirectories.Split(';').ToList();
            return sofar.Any(s => s == additionalIncludeDir);

            /*if (!Regex.IsMatch(sofar, additionalIncludeDir))//(!tdgs.ClCompile.AdditionalIncludeDirectories.ToLiteral().Contains(additionalIncludeDir.ToLiteral())) //(Regex.IsMatch((string.Format(@tdgs.ClCompile.AdditionalIncludeDirectories)), (string.Format(@additionalIncludeDir))))  //!tdgs.ClCompile.AdditionalIncludeDirectories.Contains(additionalIncludeDir))
            {
                return false;
            }*/
            return true;
        }



        protected override List<string> ConvertAllCurrentAdditionalIncludesAsStrings()
        {
            //List<string> additionIncResult = new List<string>();



            List<ItemDefinitionGroup> tdgs = ((CodeGenerator.IDESettingXMLs.VisualStudioXMLs.Project)XmlProjectClass).ItemDefinitionGroup.ToList();//.Where((ItemDefinitionGroup tdg) => tdg.ClCompile.AdditionalIncludeDirectories != "").ToList();

            var allCurrentAdditionals = tdgs.Select((ItemDefinitionGroup idg) =>
            {
                return idg.ClCompile.AdditionalIncludeDirectories;
            }).ToList();
            allCurrentAdditionals = allCurrentAdditionals[0].Split(';').ToList();//string.Join(";", 
            allCurrentAdditionals.RemoveAll(s => string.IsNullOrEmpty(s));

            /*
            //set all includes to ALL configurations to make sure all configurations have the SAME additional includes
            foreach (var tdg in tdgs)
            {
                tdg.ClCompile.AdditionalIncludeDirectories = string.Join("", allCurrentAdditionalsNoDuplicates);
            }
             
            //now get them as strings
            if (tdgs[0].ClCompile.AdditionalIncludeDirectories != null)
            {
                additionIncResult = tdgs[0].ClCompile.AdditionalIncludeDirectories.Split(';').ToList();
            }
            */
            return allCurrentAdditionals;
        }


 
        #endregion



        #region additionallibrarystuff  ************************************************************
        protected override List<string> ConvertAllCurrentAdditionalLibrariesAsStrings()
        {
            //((CodeGenerator.IDESettingXMLs.VisualStudioXMLs.Project)XmlProjectClass).ItemDefinitionGroup[0].Link.
            //List<ItemDefinitionGroup> itemDefinitionGroups = ((CodeGenerator.IDESettingXMLs.VisualStudioXMLs.Project)XmlProjectClass).ItemDefinitionGroup;//.Where((ItemDefinitionGroup tdg) => tdg.ClCompile.AdditionalIncludeDirectories != "").ToList();

            List<ItemDefinitionGroup> tdgs = ((CodeGenerator.IDESettingXMLs.VisualStudioXMLs.Project)XmlProjectClass).ItemDefinitionGroup.ToList();//.Where((ItemDefinitionGroup tdg) => tdg.ClCompile.AdditionalIncludeDirectories != "").ToList();

            List<string> allCurrentlibs = tdgs.Select((ItemDefinitionGroup idg) =>
            {
                return idg.Link.AdditionalDependencies;
            }).ToList();
            var allCurrentlibsstr = allCurrentlibs[0].Split(';').ToList();//string.Join(";",
            allCurrentlibsstr.RemoveAll(s => string.IsNullOrEmpty(s));
            allCurrentlibsstr.RemoveAll(s => s == "%(AdditionalDependencies)");


            List<string> allCurrentlibsDirs = tdgs.Select((ItemDefinitionGroup idg) =>
            {
                return idg.Link.AdditionalLibraryDirectories;
            }).ToList();
            var allCurrentlibsDirsstr = allCurrentlibsDirs[0].Split(';').ToList();//string.Join(";",
            allCurrentlibsDirsstr.RemoveAll(s => string.IsNullOrEmpty(s));
            allCurrentlibsDirsstr.RemoveAll(s => s == "%(AdditionalDependencies)");

            //this is a little more tricky as I do not know which libs belong to which directory(multiple libs could belong to multiipl directories)
            //i need to go thrpough each directory and search
            List<string> additionalLibDirs = new List<string>();
            foreach (var allCurrentlibstr in allCurrentlibsstr)
            {
                foreach (var allCurrentlibsDir in allCurrentlibsDirsstr)
                {
                    if (File.Exists(Path.Combine(allCurrentlibsDir, allCurrentlibstr)))
                    {
                        additionalLibDirs.Add(Path.Combine(allCurrentlibsDir, allCurrentlibstr));
                        break;
                    }
                }
            }

            if (additionalLibDirs.Count != allCurrentlibsstr.Count)
            {
                StackTrace st = new StackTrace(new StackFrame(true));
                var sf = st.GetFrame(0);
                throw new Exception("one of the libs was not found in " + sf.GetFileName() + " at line " + sf.GetFileLineNumber());
            }

            return additionalLibDirs;
        }


        protected override void AddSTRINGLibraryAsAdditionalLibraries(string additionalLibraryDir)
        {
            //get libs and dirs seperately
            string lib = Path.GetFileName(additionalLibraryDir);
            string libdir = Path.GetDirectoryName(additionalLibraryDir);

            if (!lib.Contains("%"))
            {
                lib += ";";
            }
            if (!libdir.Contains("%"))
            {
                libdir += ";";
            }


            //I NEED TO ADD THE libraries TO ALL CONFIGURATIONS  
            List<ItemDefinitionGroup> itemDefinitionGroups = ((CodeGenerator.IDESettingXMLs.VisualStudioXMLs.Project)XmlProjectClass).ItemDefinitionGroup;//.Where((ItemDefinitionGroup tdg) => tdg.ClCompile.AdditionalIncludeDirectories != "").ToList();

            foreach (var idg in itemDefinitionGroups)
            {
                idg.Link.AdditionalDependencies = lib + idg.Link.AdditionalDependencies;
                idg.Link.AdditionalLibraryDirectories = libdir + idg.Link.AdditionalLibraryDirectories;
            }

        }

        protected override bool DoesAdditionalLibraryAlreadyExist(string additionalLibraryDir)
        {
            ItemDefinitionGroup tdgs = ((CodeGenerator.IDESettingXMLs.VisualStudioXMLs.Project)XmlProjectClass).ItemDefinitionGroup[0];//.Where((ItemDefinitionGroup tdg) => tdg.ClCompile.AdditionalIncludeDirectories != "").FirstOrDefault();
            if (tdgs == null)
            {
                return false;
            }

            //grab all allCurrentlibsdirlist 
            List<string> listAdditionalLibsFulDir = ConvertAllCurrentAdditionalLibrariesAsStrings();

            bool existinSetting = listAdditionalLibsFulDir.Any(alib => alib == additionalLibraryDir);
            //bool existInString = this.StringLibraries.Any(alib => alib == additionalLibraryDir);

            return existinSetting;
        }

        #endregion



        #region Filter stuff ********************************************************************


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
            MyCLIncludeFile ccincConfiguration = new MyCLIncludeFile(configFilter, "ConfigurationCG.h", "Config");
            AddCLIncludeFile(ccincConfiguration);
            if (!File.Exists(Path.Combine(ConfDirPath, "ConfigurationCG.h")))
            {
                File.Create(Path.Combine(ConfDirPath, "ConfigurationCG.h"));
                Console.WriteLine("ConfigurationCG.h " + "file created");
            }


            //add additionalinclude for configTest
            AddAdditionalInclude(Program.PATHTOCONFIGTEST);


            //save the settings 
            Save(Program.envIronDirectory);


        }

        protected override void RemoveAllMentionsOfLibraryDependencyFilters()
        {
            //filters only show up in the filter settings
            var filtersOfLibDep = ((Filters.Project)XmlSettings[0].RootOfSetting)
                .ItemGroup
                .Where((Filters.ItemGroup itg) => { return itg.Filter.Count != 0; })
                .ToList()
                .FirstOrDefault();
            if (filtersOfLibDep != null)
            {
                filtersOfLibDep.Filter
                    .RemoveAll((Filters.Filter filt) => { return filt.Include.Contains("LibraryDependencies"); });
            }


        }

        #endregion



        #region CLInclude stuff **********************************************************************

        protected override List<MyCLIncludeFile> ConvertAllCurrentCIncludesAsMyClIncludes()
        {
            List<MyCLCompileFile> MyCLCompileFileResult = new List<IDESettingXMLs.MyCLCompileFile>();

            var itgs = ((Filters.Project)XmlFilterClass).ItemGroup
                .Where((Filters.ItemGroup itmG) => { return itmG.ClInclude.Count != 0; });
            //.Select(idgs => idgs.ClCompile.).ToList();
            List<Filters.ClInclude> CCincs = itgs.SelectMany(i => i.ClInclude).ToList();
             
            List<MyCLIncludeFile> MyCLIncludeFileResult = new List<IDESettingXMLs.MyCLIncludeFile>();
              
            if (CCincs != null)
            {

                foreach (var clInclude in CCincs)
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
            }

            return MyCLIncludeFileResult;
        }

        protected override void AddMyClIncludesAsCIncludes(MyCLIncludeFile CLIncludeFile)
        {
            //CCinclude affects TWO xml classes   
            //XmlFIlters Itemgroup CLCompile
            //xmlProject Itemgroup CLCompile
            //make sure both are created and added  
            var ItemGroupWithCincludesPROJ = ((Project)XmlProjectClass).ItemGroup.Where((ItemGroup itmG) => { return itmG.ClInclude.Count > 0; }).FirstOrDefault();
            var ItemGroupWithCCompilesFILT = ((Filters.Project)XmlFilterClass).ItemGroup.Where((Filters.ItemGroup itmG) => { return itmG.ClInclude.Count > 0; }).FirstOrDefault();

            if (ItemGroupWithCincludesPROJ == null || ItemGroupWithCCompilesFILT == null)
            {
                ProblemHandle.ThereisAProblem("you need to have at least one .h file in your project to initialize. \n If you do and you still get this message, try reloading the project.");
            }


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
            var ItemGroupWithCincludesPROJ = ((Project)XmlProjectClass).ItemGroup
                .Where((ItemGroup itmG) => { return itmG.ClInclude.Count > 0; }).FirstOrDefault();

            if (ItemGroupWithCincludesPROJ == null)
            {
                return false;
            }

            foreach (var CLInclude in ItemGroupWithCincludesPROJ.ClInclude)
            {

                if (CLIncludeFile.FullFilterName == CLInclude.Include)
                {
                    return true;
                }
            }

            return false;
        }

        protected override void RemoveAllMentionsOfLibraryDependencyCLIncludes()
        {
            //clincludes shows up in filter settings and project settings
            var inclibdep1 = ((Filters.Project)XmlSettings[0].RootOfSetting).ItemGroup
                .Where((Filters.ItemGroup itg) => { return itg.ClInclude.Count != 0; })
                .ToList().FirstOrDefault();
            if (inclibdep1 != null)
            {
                inclibdep1.ClInclude
                    .RemoveAll((Filters.ClInclude cl) => { return cl.Filter.Contains("LibraryDependencies"); });
            }

            //for proj settings
            var inclibdep2 = ((Project)XmlSettings[1].RootOfSetting).ItemGroup
                .Where((ItemGroup itg) => { return itg.ClInclude.Count != 0; })
                .ToList().FirstOrDefault();
            if (inclibdep2 != null)
            {
                inclibdep2.ClInclude
                    .RemoveAll((ClInclude cl) => { return cl.Include.Contains("LibraryDependencies"); });
            }

        }

        #endregion


        #region CLCompile stuff **********************************************************************

        protected override List<MyCLCompileFile> ConvertAllCurrentCCompilessAsMyClCompiles()
        {


            List<MyCLCompileFile> MyCLCompileFileResult = new List<IDESettingXMLs.MyCLCompileFile>();

            var itgs = ((Filters.Project) XmlFilterClass).ItemGroup
                .Where((Filters.ItemGroup itmG) => { return itmG.ClCompile.Count != 0; });
            //.Select(idgs => idgs.ClCompile.).ToList();
            List<Filters.ClCompile> CComps = itgs.SelectMany(i => i.ClCompile).ToList(); 
             
            if (CComps != null)
            {

                foreach (var cCompile in CComps)
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

            }

            return MyCLCompileFileResult;
        }

        protected override void AddMyCLCompileFileAsCLCompileFile(MyCLCompileFile CLCompileFile)
        {
            //CComile affects TWO xml classes   
            //XmlFIlters Itemgroup CLCompile
            //xmlProject Itemgroup CLCompile
            //make sure both are created and added 

            var ItemGroupWithCCompilesPROJ = ((Project)XmlProjectClass).ItemGroup.Where((ItemGroup itmG) => { return itmG.ClCompile.Count > 0; }).FirstOrDefault();
            var ItemGroupWithCCompilesFILT = ((Filters.Project)XmlFilterClass).ItemGroup.Where((Filters.ItemGroup itmG) => { return itmG.ClCompile.Count > 0; }).FirstOrDefault();

            if (ItemGroupWithCCompilesPROJ == null || ItemGroupWithCCompilesFILT == null)
            { 
                ProblemHandle.ThereisAProblem("you need to have at least one .cpp file in your project to initialize. \n If you do and you still get this message, try reloading the project.");
            }


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
            var ItemGroupWithCincludesPROJ = ((Project)XmlProjectClass).ItemGroup
                .Where((ItemGroup itmG) => { return itmG.ClCompile.Count > 0; }).FirstOrDefault();


            if (ItemGroupWithCincludesPROJ == null)
            {
                return false;
            }

            foreach (var CLCompil in ItemGroupWithCincludesPROJ.ClCompile)
            {

                if (clCompileToCheck.FullFilterName == CLCompil.Include)
                {
                    return true;
                }
            }

            return false;
        }


        protected override void RemoveAllMentionsOfLibraryDependencyCLCompiles()
        {
            //clincludes shows up in filter settings and project settings
            var complibdep1 = ((Filters.Project)XmlSettings[0].RootOfSetting).ItemGroup
                .Where((Filters.ItemGroup itg) => { return itg.ClCompile.Count != 0; })
                .ToList().FirstOrDefault();
            if (complibdep1 != null)
            {
                complibdep1.ClCompile
                    .RemoveAll((Filters.ClCompile cl) => { return cl.Filter.Contains("LibraryDependencies"); });
            }

            //for proj settings
            var complibdep2 = ((Project)XmlSettings[1].RootOfSetting).ItemGroup
                .Where((ItemGroup itg) => { return itg.ClCompile.Count != 0; })
                .ToList().FirstOrDefault();
            if (complibdep2 != null)
            {
                complibdep2.ClCompile
                    .RemoveAll((ClCompile cl) => { return cl.Include.Contains("LibraryDependencies"); });
            }

        }
        #endregion






    }
}
