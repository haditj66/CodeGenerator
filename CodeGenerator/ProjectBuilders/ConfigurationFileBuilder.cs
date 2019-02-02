using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CodeGenerator.CMD_Handler;
using CodeGenerator.IDESettingXMLs;
using CodeGenerator.ProblemHandler;
using ExtensionMethods;

namespace CodeGenerator.ProjectBuilders
{
    public class ConfigurationFileBuilder
    {
        public MySettingsBase VSsetting { get; }
        public string PathToOutPutCompilation { get; }
        protected string DIRECTORYOFTHISCG { get; }
        public string PATHTOCONFIGTEST { get; }
        public ProblemHandle ProblemHandler { get; private set; }

        public ConfigurationFileBuilder(MySettingsBase settingsOfProjectToBuildConfigfileFore, string PathToOutPutCompilation, string DIRECTORYOFTHISCG, string PATHTOCONFIGTEST,ProblemHandle problemHandler = null)
        {
            if (problemHandler == null)
            {
                ProblemHandler = new ProblemHandle();
            }
            else
            {
                ProblemHandler = problemHandler;
            }
            VSsetting = settingsOfProjectToBuildConfigfileFore;
            VSsetting.Initiate();
            this.PathToOutPutCompilation = PathToOutPutCompilation;
            this.DIRECTORYOFTHISCG = DIRECTORYOFTHISCG;
            this.PATHTOCONFIGTEST = PATHTOCONFIGTEST;
        }


        public void CreateConfigurationToTempFolder()
        {

            //get all .cpp files that are in the top level library and are only in the Config Filter
            var ccompiles = VSsetting.CLCompileFiles.GetCCompilesFromFilter("Config");//projectBuilderForVs.LibTop.GetAllCCompile().GetCCompilesFromFilter("Config");
            CLCommandBuilderForConfigTest cl = new CLCommandBuilderForConfigTest("configGen", PATHTOCONFIGTEST, ccompiles.ToArray());
            foreach (var include in VSsetting.StringIncludes)
            {
                //just add all additional includes that show up in the top level library
                if (include != @"%(AdditionalIncludeDirectories)")
                {
                    //check if it is a relative path
                    if (!include.Contains(@":"))
                    {
                        cl.AdditionalIncludes.Add(Path.Combine(VSsetting.PATHOfProject, include));
                    }
                    else
                    {
                        cl.AdditionalIncludes.Add(include);
                    }

                }
            }
            //add additoinal include for the baseProject as well as the baseproject/Config folder
            cl.AdditionalIncludes.Add(VSsetting.PATHOfProject);
            cl.AdditionalIncludes.Add(Path.Combine(VSsetting.PATHOfProject, "Config"));


            //the output will be the same path as the where the mod_conf.h file is located
            cl.OutputLocation = PathToOutPutCompilation;//Path.Combine(projectBuilderForVs.BaseDirectoryForProject, "Config");

            //Im here. I need to grab the mainCG.cpp, copy it into a temp file locally, change mainCG() { }  to main() { }
            var maincg = ccompiles.Where((MyCLCompileFile ccom) => { return ccom.FullLocationName == "Config\\mainCG.cpp"; }).FirstOrDefault();
            if (maincg == null)
            {
                Console.WriteLine("CGEN ERROR: missing a mainCG.cpp file in your config filter");
            }
            cl.FilesCComp.Remove(maincg);

            string mainCGstr = File.ReadAllText(Path.Combine(VSsetting.PATHOfProject, maincg.FullLocationName));
            mainCGstr = Regex.Replace(mainCGstr, @"int mainCG", @"int main");
            //write it all to a local temp mainCG.cpp
            File.WriteAllText(Path.Combine(DIRECTORYOFTHISCG, "mainCG.cpp"), mainCGstr);
            //add that ccompile instead now
            MyCLCompileFile newMAinCG = new MyCLCompileFile(maincg.FilterIBelongTo, maincg.Name, "");
            cl.FilesCComp.Add(newMAinCG);

            string ss = cl.GetCompileCommand();


            //use CMDHandler to compile and build the config for the top level library
            CMDHandlerVSDev cmdHandler = new CMDHandlerVSDev( DIRECTORYOFTHISCG, DIRECTORYOFTHISCG);
            cmdHandler.SetWorkingDirectory(DIRECTORYOFTHISCG);
            cmdHandler.ExecuteCommand(ss);
            //check if compilation was succesful
            if (cmdHandler.Output.Contains(" : error"))
            {
                ProblemHandler.ThereisAProblem("ERROR: there was a problem with compilation for the Configuration.h file of your library at location \n" + VSsetting.PATHOfProject + "\n make sure your configuration app builds for that library"); 
            }
            else
            {
                //if no problem than run that config.exe, run the 
                cmdHandler.SetWorkingDirectory(Path.Combine(DIRECTORYOFTHISCG, PathToOutPutCompilation));
                cmdHandler.ExecuteCommand("CALL configGen.exe");
                Console.WriteLine("TEMP ConfigurationCG.h File Created");
            }
        }

        public void  WriteTempConfigurationToFinalFile()
        {
            
                //a Configuration.h file was created. grab the contents of that and put it in the proper projectbase/config/Configuration.h file
          string configuration_hStr = File.ReadAllText(Path.Combine(DIRECTORYOFTHISCG, PathToOutPutCompilation, "Configuration.h"));
            string ToFile = Path.Combine(VSsetting.PATHOfProject, "Config", "ConfigurationCG.h");
            File.WriteAllText(ToFile, configuration_hStr);

            Console.WriteLine("ConfigurationCG.h File Created");
        }


    }
}
