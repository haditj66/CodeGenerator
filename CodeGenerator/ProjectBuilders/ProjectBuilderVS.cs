using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGenerator.IDESettingXMLs;
using System.IO;
using CodeGenerator.IDESettingXMLs.VisualStudioXMLs;
using ExtensionMethods;
using CodeGenerator.FileTemplates;

namespace CodeGenerator.ProjectBuilders
{
    public class ProjectBuilderVS : ProjectBuilderBase
    {
        public ProjectBuilderVS(IDESetting configSettings) : base(configSettings)
        {

        }


        protected override MySettingsBase GetSettingsOVERRIDE(string pathToProjectSettings)
        {

            return MySettingsVS.CreateMySettingsVS(pathToProjectSettings);
        }

        public override void ImportDependentLibrariesCincAndCcompAndAdditional()
        {

            //5. I need to get all the cIncludes, cClompiles, additionalincludes from the other libraries and add to top level
            // to the top level library. 

            //getting and adding all additional includes
            AllNotTopAndNotGlobal
                .ForEach((Library lib) =>
                {
                    lib.GetAllAdditionalIncludes()
                    .ForEach((string inc) =>
                    {
                        LibTop.AddAdditionalIncludes(inc);
                    });
                });

            //List<myc> cCompToExcludeFromImporting = new List<string>();
            //cCompToExcludeFromImporting.Add();

            //CLCompile
            AllNotTopAndNotGlobal
            .ForEach((Library lib) =>
            {
                lib.GetAllCCompile()
                .ForEach((MyCLCompileFile inc) =>
                {
                    //exclude files that are in the Config filter of that project or the LibraryDependencies. exclude main.cpp and as well
                    if (ConditionForImportingDependencyCComp(inc))
                    {  
                        //change it so that the location of these files will be the same as the filters they are set in
                        inc.LocationOfFile = Path.GetDirectoryName(inc.FullFilterName);

                        LibTop.AddCCompileFile(inc);
                    }

                });

            });

            //CLinclude
            AllNotTopAndNotGlobal
            .ForEach((Library lib) =>
            {
                lib.GetAllCincludes()
                .ForEach((MyCLIncludeFile inc) =>
                {
                    //exclude files that are in the Config filter of that project or the LibraryDependencies
                    if (ConditionForImportingDependencyCInc(inc))
                    {
                        //change it so that the location of these files will be the same as the filters they are set in
                        inc.LocationOfFile = Path.GetDirectoryName(inc.FullFilterName);

                        LibTop.AddCIncludeFile(inc);
                    }
                });
            });

        }


        /*
        public override void RecreateConfigurationFilterFolderIncludes(string NameOfCGenProject, string pathOfConfigTestDir)
        {
            //-------------config filter
            //check if config filter exists
            MyFilter configFilter;
            if (!LibTop.GetAllFitlers().DoesFilterWithNameExist("Config"))
            {
                configFilter = new MyFilter("Config");
                LibTop.AddFilter(configFilter);
            }
            else
            {
                configFilter = LibTop.GetAllFitlers().GetFilterAtAddress("Config");
            }


            //-------------FolderCreation
            //does config folder exist
            string ConfDirPath = Path.Combine(BaseDirectoryForProject, "Config");// Path.Combine(Path.GetDirectoryName(LibTop.config.ConfigFileFullPath), "Config");
            if (!Directory.Exists(ConfDirPath))
            {
                Directory.CreateDirectory(ConfDirPath);
            }

            //-------------mainCG.cpp NameOfCGenProjectConf.h Files  Configuration.h
            //   settings should already check if it exists before adding it
            MyCLCompileFile ccompMainCg = new MyCLCompileFile(configFilter, "mainCG", "Config");
            LibTop.AddCCompileFile(ccompMainCg);
            if (!File.Exists( Path.Combine(ConfDirPath, "mainCG.cpp")))
            {
                FileTemplateMainCG maincgTemplate = new FileTemplateMainCG(ConfDirPath, NameOfCGenProject);
                maincgTemplate.CreateTemplate();
                Console.WriteLine("mainCG.cpp" + "file created");
            } 
            MyCLIncludeFile ccincNameOfCGenProjectConf = new MyCLIncludeFile(configFilter, NameOfCGenProject + "Conf", "Config");
            LibTop.AddCIncludeFile(ccincNameOfCGenProjectConf);
            if (!File.Exists(Path.Combine(ConfDirPath, NameOfCGenProject+ "Conf.h")))
            {
                FileTemplateLibConf maincgTemplate = new FileTemplateLibConf(ConfDirPath, NameOfCGenProject);
                maincgTemplate.CreateTemplate();
                Console.WriteLine(NameOfCGenProject+"Conf.h" + "file created");
            }
            if (!File.Exists(Path.Combine(ConfDirPath, "Configuration.h")))
            {
                File.Create(Path.Combine(ConfDirPath, "Configuration.h"));
                Console.WriteLine("Configuration.h " + "file created");
            }




            //add additionalinclude for configTest
            LibTop.AddAdditionalIncludes(pathOfConfigTestDir);


            //save the settings
            LibTop.GenerateXMLSettings(BaseDirectoryForProject);
             

        }

        */
    }
}
