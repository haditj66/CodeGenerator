using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CodeGenerator.CMD_Handler;
using CodeGenerator.IDESettingXMLs;
using CodeGenerator.ProblemHandler;
using ExtensionMethods;
using Extensions;

namespace CodeGenerator.ProjectBuilders
{
    public class ConfigurationFileBuilder
    {
        public MyMainSettingsBase VSsetting { get; }
        public string PathToOutPutCompilation { get; }
        protected string DIRECTORYOFTHISCG { get; }
        public string PATHTOCONFIGTEST { get; }
        public bool IsTopLevelConfig { get; }
        public ProblemHandle ProblemHandler { get; private set; }
        private static string ConfigInherittedValues;


        public ConfigurationFileBuilder(MyMainSettingsBase settingsOfProjectToBuildConfigfileFore, string PathToOutPutCompilation, string DIRECTORYOFTHISCG, string PATHTOCONFIGTEST, ProblemHandle problemHandler = null, bool isTopLevelConfig = false)
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
            IsTopLevelConfig = isTopLevelConfig;
        }


        public void CreateConfigurationToTempFolder()
        {

            //get all .cpp files that are in the top level library and are only in the Config Filter
            var ccompiles = VSsetting.CLCompileFiles.GetCCompilesFromLocationOfFile("Config");//projectBuilderForVs.LibTop.GetAllCCompile().GetCCompilesFromFilter("Config");
            CLCommandBuilderForConfigTest cl = new CLCommandBuilderForConfigTest("configGen", PATHTOCONFIGTEST, ccompiles.ToArray());
            foreach (var include in VSsetting.StringIncludes)
            {
                //just add all additional includes that show up in the top level library
                if (include != @"%(AdditionalIncludeDirectories)" && !include.Contains("LibraryDependencies"))
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
                ProblemHandler.ThereisAProblem("CGEN ERROR: missing a mainCG.cpp file in your config filter");
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
            CMDHandlerVSDev cmdHandler = new CMDHandlerVSDev(DIRECTORYOFTHISCG, DIRECTORYOFTHISCG);
            cmdHandler.SetWorkingDirectory(DIRECTORYOFTHISCG);
            cmdHandler.ExecuteCommandWithProblemCheck(ss, ProblemHandler, "building the ConfigurationCG.h for library at path \n" + VSsetting.PATHOfProject + "\n encountered a compilation error. go to that commit to see what is wrong. \n");
            //cmdHandler.ExecuteCommandWithProblemCheck(ss, ProblemHandler, "ERROR: there was a problem with compilation for the Configuration.h file of your library at location \n" + VSsetting.PATHOfProject + "\n make sure your configuration app builds for that library");

            //if no problem than run that config.exe, run the 
            cmdHandler.SetWorkingDirectory(Path.Combine(DIRECTORYOFTHISCG, PathToOutPutCompilation));
            cmdHandler.ExecuteCommandWithProblemCheck("CALL configGen.exe", ProblemHandler, "building the ConfigurationCG.h for library at path \n" + VSsetting.PATHOfProject + "\n encountered a runtime error. go to that commit to see what is wrong. \n");

            //check to see if there were any problems with generating the configurationfrom the conig.exe.
            Match m = Regex.Match(cmdHandler.Output, @"PROBLEM::(.*)");
            if (m.Success)
            {
                ProblemHandler.ThereisAProblem("building the ConfigurationCG.h for library at path \n" + VSsetting.PATHOfProject + "\n encountered a problem \n" + m.Groups[1].Value);
            }

            if (IsTopLevelConfig)
            {
                ConfigInherittedValues = cmdHandler.Output;
            }


            Console.WriteLine("TEMP ConfigurationCG.h File Created");
        }

        public void WriteTempConfigurationToFinalFile(string ToNewFile = "")
        {

            //a Configuration.h file was created. grab the contents of that and put it in the proper projectbase/config/Configuration.h file
            string configuration_hStr = File.ReadAllText(Path.Combine(DIRECTORYOFTHISCG, PathToOutPutCompilation, "Configuration.h"));


            ToNewFile = string.IsNullOrEmpty(ToNewFile)
              ? Path.Combine(VSsetting.PATHOfProject, "Config", "ConfigurationCG.h")
              : ToNewFile;
            File.WriteAllText(ToNewFile, configuration_hStr);

            Console.WriteLine("ConfigurationCG.h File Created");
        }

        public void InheritFromTopConfig(Config ConfigOfLibraryOfTheOneInheriting)
        {

            if (!IsTopLevelConfig && !string.IsNullOrEmpty(ConfigInherittedValues))
            {
                string configuration_hStr = File.ReadAllText(Path.Combine(DIRECTORYOFTHISCG, PathToOutPutCompilation, "Configuration.h"));
                List<string> configuration_hStrNew = configuration_hStr.Split('\n').ToList();//new string[configuration_hStr.Split('\n').Length];

                //go through the ConfigInherittedValues line by line and change the value for the define
                //that it matches in the configuration_hStr
                foreach (var line in ConfigInherittedValues.Split('\n'))
                {
                    //go through each line in configuration_hStr and look for a #define line
                    int index = 0;
                    foreach (var lineconfStr in configuration_hStr.Split('\n'))
                    {
                        //first make sure that that line is a define
                        Match m = Regex.Match(lineconfStr, @"#define(\b.+?\b)\s+(.+)$", RegexOptions.Multiline);
                        if (m.Success)
                        {
                            //check this define has form of the classname*conftypeprefix*definename 
                            string strPattern = !string.IsNullOrEmpty(ConfigOfLibraryOfTheOneInheriting.ConfTypePrefix)
                                ? @"#define " + ConfigOfLibraryOfTheOneInheriting.ClassName + @"\*" + ConfigOfLibraryOfTheOneInheriting.ConfTypePrefix + @"\*" + m.Groups[1].Value.Trim() + @"\s+(.*)"
                                : @"#define " + ConfigOfLibraryOfTheOneInheriting.ClassName + @"\*" + m.Groups[1].Value.Trim() + @"\s+(.*)";
                            Match m2 = Regex.Match(line, strPattern);//@"#define modaaConf1\*MODE0\*BUFFERSIZE\s*(.*)");
                            if (m2.Success)
                            {
                                //there is a match that this define needs to be overriden. so change the value
                                string newLine = lineconfStr.Substring(0, m.Groups[2].Index);//, lineconfStr.Length-1);
                                newLine += m2.Groups[1];
                                configuration_hStrNew[index] = newLine;

                                //I need to check for any nonstatic defines also and append them to the end
                                var nonstaticDefine = ConfigOfLibraryOfTheOneInheriting.Defines.Define.FirstOrDefault(define =>
                                {
                                    return (define.DefineName == m.Groups[1].Value.Trim())
                                           && (define.IsStatic == "false")
                                           && (ConfigOfLibraryOfTheOneInheriting.MyInstanceNum != "0");
                                });
                                if (nonstaticDefine != null)
                                {
                                    //append all define instances to the end
                                    for (int i = 1; i < Convert.ToInt32(ConfigOfLibraryOfTheOneInheriting.MyInstanceNum) + 1; i++)
                                    {
                                        string defineStr = @"#define " + nonstaticDefine.DefineName + "_" + i.ToString() + " " + nonstaticDefine.Value;
                                        configuration_hStrNew.Add(defineStr);
                                    }
                                }
                            }
                        }

                        index++;
                    }
                }


                File.WriteAllText(Path.Combine(DIRECTORYOFTHISCG, PathToOutPutCompilation, "Configuration.h"), string.Join("\n", configuration_hStrNew));

            }

        }
    }
}
