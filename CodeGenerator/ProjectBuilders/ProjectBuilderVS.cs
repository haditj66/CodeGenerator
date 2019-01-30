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


        protected override MySettingsBase GetSettingsOVERRIDE(Config configClass, string pathToProjectSettings)
        {
            


        IDESettingVSProj settingProj = new IDESettingVSProj(pathToProjectSettings, ".vcxproj", typeof(IDESettingXMLs.VisualStudioXMLs.Project));

            IDESetting settingFilter = new IDESetting(pathToProjectSettings, ".filters", typeof(IDESettingXMLs.VisualStudioXMLs.Filters.Project));

            //create the settings
            return new MySettingsVS(settingFilter, settingProj);
        }

         

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
            string ConfDirPath = Path.Combine(Path.GetDirectoryName(LibTop.config.ConfigFileFullPath), "Config");
            if (!Directory.Exists(ConfDirPath))
            {
                Directory.CreateDirectory(ConfDirPath);
            }

            //-------------mainCG.cpp NameOfCGenProjectConf.h Files
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


            //add additionalinclude for configTest
            LibTop.AddAdditionalIncludes(pathOfConfigTestDir);


            //save the settings
            LibTop.GenerateXMLSettings();


            /*
            bool ShouldRecreatemainCG = false;
            if (LibTop.GetAllCCompile().GetCCompileWithName("mainCG") != null)
            {
                // since one exists make sure it is in the right filter
                ShouldRecreatemainCG = LibTop.GetAllCCompile().GetCCompileWithName("mainCG").FilterIBelongTo.GetFullAddress() == "Config";
            }
            else
            {
                //it doesnt exist so make one
                ShouldRecreatemainCG = true;
            } 
            if (ShouldRecreatemainCG)
            {
                MyCLCompileFile ccompMainCg = new MyCLCompileFile(configFilter,"mainCG", ConfDirPath);
                LibTop.AddCCompileFile(ccompMainCg);
            }
            */
            //NameOfCGenProjectConf.h 




            //.GetAllCCompile().First().FilterIBelongTo.Name


        }
    }
}
