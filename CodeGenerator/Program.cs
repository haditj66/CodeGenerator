//#define TESTING 

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using System.Xml.Serialization;
using System.IO;
using CodeGenerator.IDESettingXMLs.VisualStudioXMLs;
using CodeGenerator.IDESettingXMLs;
using CodeGenerator.IDESettingXMLs.VisualStudioXMLs.Filters;
using System.Text.RegularExpressions;
using CodeGenerator.ProjectBuilders;
using CodeGenerator.cgenXMLSaves.SaveFiles;
using CodeGenerator.cgenXMLSaves;
using System.Diagnostics;
using CodeGenerator.CMD_Handler;
using ExtensionMethods;
using System.Reflection;
using ClangSharp;
using CodeGenerator.FileTemplates;
using CodeGenerator.FileTemplatesMacros;
using CodeGenerator.GitHandlerForLibraries;
using CodeGenerator.IDESettingXMLs.IAR_XMLs;
using CodeGenerator.ProblemHandler;
using CommandLine.Text;
using ConsoleApp2;
using ConsoleApp2.CPPRefactoring;
using ConsoleApp2.MyClangWrapperClasses;
using ConsoleApp2.MyClangWrapperClasses.CXCursors;
using ConsoleApp2.MyClangWrapperClasses.CXCursors.MyCursorKinds;
using ConsoleApp2.Parsing;
using CPPParser;
using Extensions;
using Project = CodeGenerator.IDESettingXMLs.VisualStudioXMLs.Project;
using CodeGenerator.FileTemplates.GeneralMacoTemplate;

namespace CodeGenerator
{




    public class Program
    {

        #region Options classes  ***************************************************************************
        //*************************************************************************************************** 
        //Verbs help delineate and separate options and values for multiple commands within a single app

        [Verb("projects", HelpText = "Get all CG projects that have been created.")]
        public class ProjectsOptions
        {

        }

        [Verb("macro", HelpText = "create a macro file in this directory out of all .cgenM files")]
        public class MacroOptions
        {

        }


        [Verb("generate", HelpText = "generate the code")]
        public class GenerateOptions
        {

            [Option(HelpText = "only build the configurations for the dependent libraries and not importing all files")]
            public bool config { get; set; }

            [Option(HelpText = "This will use git to go to switch to an appropriate version when fetching other libraries. use this when you have a library that has been changed and you want to use a past version of that library.")]
            public bool ignoreGit { get; set; }

            [Option('i', HelpText = "This will ignore all files that are anywhere in this filter. use this when you have 3rd party files that are shared within ALL libraries.")]
            public string ignoreFilesInFilter { get; set; }

            /*
            [Option('t', Separator = ':')]
            public IEnumerable<string> Types { get; set; }

            [Option('r', Separator = ' ', HelpText = "the file that has the list of all files you want to generate code into")]
            public IEnumerable<string> IncludeFiles { get; set; }

            [Option('d', Separator = ' ', HelpText = "directories that have all files you want to generate code into")]
            public IEnumerable<string> IncludeDirectories { get; set; }

            [Option(Default = false)]
            public bool AIEnabled { get; set; }

            [Value(1, Min = 1, Max = 3)]
            public IEnumerable<string> StringSeq { get; set; }

            [Value(2)]
            public double DoubleValue { get; set; }
            */
        }

        [Verb("degenerate", HelpText = "degenerate the code")]
        public class DegenerateOptions
        {

            [Option('r', Separator = ' ')]
            public IEnumerable<string> IncludeFiles { get; set; }

        }

        [Verb("sync", HelpText = "sync to other project from you main VS project")]
        public class SyncOptions
        {
            //[CommandLine.Value(1), help]
            [Value(0, HelpText = "name of the project you want to sync")]
            public string name { get; set; }
        }


        [Verb("init", HelpText = "create a new CG project")]
        public class InitOptions
        {
            //[CommandLine.Value(1), help]
            [Value(0, HelpText = "name of the project you want to create")]
            public string name { get; set; }
            [Option(HelpText = "if this is enabled, it will initialize git, commiting all, and making the first tag")]
            public bool git { get; set; }
            [Option(HelpText = "setup iar project")]
            public bool iar { get; set; }
        }

        [Verb("config", HelpText = "Configure Code Generator. add and remove targets. configurations are Debug or Release")]
        public class ConfigOptions
        {
            //[CommandLine.Value(1), help]
            [Option(HelpText = "Directory of the Config")]
            public string directoryofconfig { get; set; }

            [Option('r', HelpText = "remove a platform")]
            public string PlatformToRemove { get; set; }

            [Option('a', HelpText = "create a new platform")]
            public string PlatformToAdd { get; set; }

        }


        [Verb("configproj", HelpText = "manage the project's possible configurations. add and remove additional includes and library paths. configurations are Debug or Release")]
        public class ProjConfigOptions
        {
            [Value(0, HelpText = "platform you want to add or remove includes or library directories")]
            public string platform { get; set; }

            [Option(HelpText = "include you want to add")]
            public string addinclude { get; set; }

            [Option(HelpText = "include you want to remove")]
            public string removeinclude { get; set; }

            [Option(HelpText = "library you want to add")]
            public string addlibrary { get; set; }

            [Option(HelpText = "library you want to remove")]
            public string removelibrary { get; set; }

            [Option('r', HelpText = "remove a platform from this project's scope")]
            public string PlatformToRemove { get; set; }

            [Option('a', HelpText = "add a new platform to this project's scope")]
            public string PlatformToAdd { get; set; }

        }
        #endregion


        public static string DIRECTORYOFTHISCG = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string CGSAVEFILESBASEDIRECTORY = "CGensaveFiles";
        public static string CGCONFCOMPILATOINSBASEDIRECTORY = "ConfigCompilations";
        public static string PATHTOCONFIGTEST = @"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\ConfigTest";

#if TESTING
        //public static string envIronDirectory = @"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\Module1AA";//
        //public static string envIronDirectory =   @"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\Module1A";//
        //public static string envIronDirectory = @"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\Module1B";// 
        //public static string envIronDirectory = @"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGeneratorTestModules\Module1AA";
        //public static string envIronDirectory = @"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGeneratorTestModules\Module1A";
        //public static string envIronDirectory = @"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGeneratorTestModules\Module1";
        //public static string envIronDirectory = @"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGeneratorTestModules\Module1B";
        //public static string envIronDirectory = @"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGeneratorTestModules\Module";
        //public static string envIronDirectory = @"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGeneratorTestModules\test";
        //public static string envIronDirectory = @"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\Wavelettransform\WaveletsTrans";
        //public static string envIronDirectory = @"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\microcontroller stuff\My psuedo RNG\RNG psuedo";
        //public static string envIronDirectory = @"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\GA for embedded\GAembeddedcgen\GA embedded";
        //public static string envIronDirectory = @"C:\Users\Hadi\Documents\Visual Studio 2017\Projects\AO Projects\UUartDriver\UUartDriver";
        //public static string envIronDirectory = @"C:\Users\Hadi\Documents\Visual Studio 2017\Projects\AO Projects\UploadDataToPCTDU\UploadDataToPCTDU";
        //public static string envIronDirectory = @" C:\Users\Hadi\Documents\Visual Studio 2017\Projects\AO Projects\WaveletTransformSPB\WaveletTransformSPB";
        //public static string envIronDirectory = @"C:\Users\Hadi\Documents\Visual Studio 2017\Projects\AO Projects\HolterMonitor\HolterMonitor";
        //public static string envIronDirectory = @"C:\Users\Hadi\Documents\Visual Studio 2017\Projects\AO Projects\SPISlaveFSM\SPISlaveFSMcont";
        public static string envIronDirectory = @"C:\Users\Hadi\Documents\Visual Studio 2017\Projects\AO Projects\UVariableSaver\UVariableSaver";


        //static string[] command  = "generate -r fiile.txt oubnfe.tct --aienabled=true".Split(' '); //values should be called LOWER CASED
        //static string[] command  = "degenerate -r fiile.txt oubnfe.tct ".Split(' ');
        //static string[] command  = "init moda".Split(' ');
        //static string[] command  = ("config --directoryofconfig " +  @"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\ConfigTest").Split(' ');
        //static string[] command  = "config".Split(' ');
        //static string[] command  = "config -a stm32f5".Split(' ');
        //static string[] command  = "config -r stm32f4".Split(' ');
        //static string[] command  = "config -r IAR".Split(' ');
        //static string[] command = "configproj".Split(' ');
        //static string[] command = "configproj -a VS".Split(' ');
        //static string[] command = "configproj -r VS".Split(' ');
        //static string[] command = @"configproj VS --addinclude C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator".Split(' ');
        //static string[] command = @"configproj VS --removeinclude C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator".Split(' ');
        //static string[] command = @"configproj VS --addlibrary C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\ConfigTest\ConfigTest.lib".Split(' ');
        //static string[] command = @"configproj VS --removelibrary C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\ConfigTest\ConfigTest.lib".Split(' ');
        //static string[] command = @"configproj ALLPLATFORMS --addinclude C:\Users\Hadi\Downloads\PROClient_64".Split(' ');
        //static string[] command  = "".Split(' '); 
        static string[] command  = "generate -i AE --ignoregit ".Split(' ');
        //static string[] command = "generate".Split(' ');
        //static string[] command = "generate --config".Split(' ');
        //static string[] command = "init".Split(' ');
        //static string[] command = "init Wavelet".Split(' ');
        //static string[] command  = "init Module".Split(' ');
        //static string[] command = "init --git".Split(' ');
        //static string[] command = "init Test".Split(' ');
        //static string[] command  = "init Mod".Split(' ');
        //static string[] command = "init ModAA".Split(' ');
        //static string[] command = "init ModA".Split(' ');
        //static string[] command = "init ModB".Split(' ');
        //static string[] command = "init UUartDriver".Split(' ');
        //static string[] command = "sync iar".Split(' ');
        //static string[] command = "macro".Split(' ');
        //static string[] command = "projects".Split(' ');

#else
        static string[] command;
        public static string envIronDirectory = Environment.CurrentDirectory;
#endif

        public static SaveFilecgenProjectGlobal savefileProjGlobal = new SaveFilecgenProjectGlobal();
        public static SaveFilecgenConfig saveFilecgenConfigGlobal = new SaveFilecgenConfig();
        public static SaveFilecgenProjectLocal SaveFilecgenProjectLocal = new SaveFilecgenProjectLocal();
        //public static ProjectBuilderVS projectBuilderForVs;



        static void Main(string[] args)
        {

            //string e = Environment.CurrentDirectory;
            //Console.WriteLine(e);
            //ProjectVSTest();
            //CreateProjectBuilder(); this should not be done here as a saveddata.xml may not even be created yet


            Action RunParser = () =>
            {
                Parser.Default.ParseArguments<GenerateOptions, DegenerateOptions, InitOptions, SyncOptions, ConfigOptions,ProjectsOptions, MacroOptions, ProjConfigOptions>(command)
.WithParsed<GenerateOptions>(opts => Generate(opts))
.WithParsed<DegenerateOptions>(opts => Degenerate(opts))
.WithParsed<SyncOptions>(opts => Sync(opts))
.WithParsed<InitOptions>(opts => Init(opts))
.WithParsed<ConfigOptions>(opts => Config(opts))
.WithParsed<ProjectsOptions>(opts => Projects(opts))
.WithParsed<MacroOptions>(opts => Macro(opts))
.WithParsed<ProjConfigOptions>(opts => ProjConfig(opts));
                
            };

#if !TESTING
            command = args;

            if (command.Count() != 0)
#else

            if (command != null && command.Count() > 0)
#endif 
            {
                RunParser();
            }
            else
            {
                //just give back any project name in this directory
                //first check if a project directory exist in this directory
                if (IsProjectExistsAtEnvironDirectory())
                {
                    //get the SaveFilecgenProject for this directory
                    SaveFilecgenProjectLocal savefileLocal = new SaveFilecgenProjectLocal(envIronDirectory + "\\" + CGSAVEFILESBASEDIRECTORY);

                    var projHere = savefileProjGlobal.CgenProjects.Projects
                        .First((cgenProjectGlobal cgenproj) => cgenproj.UniqueIdentifier == savefileLocal.CgenProjects.Project.UniqueIdentifier);
                    if (projHere != null)
                    {
                        Console.WriteLine(projHere.NameOfProject + " project exists in this directory");
                    }
                    else
                    {
                        Console.WriteLine(" Problem getting Project here. Check setting files are there. Is this a new Project that needs to be imported?");
                    }
                }
                else
                {
                    //if no project exists than just put back the help menu  
                    command = "help".Split(' ');
                    RunParser();
                }
            }
        }




        #region Config command ***************************************************************************
        //***************************************************************************************************  
        static ParserResult<object> Config(ConfigOptions opts)
        {

            //if there is no cgenXMLSaves directory than this must be the first config run. build all the
            //config files needed here.
            if (!Directory.Exists(CGSAVEFILESBASEDIRECTORY))
            {
                Directory.CreateDirectory(CGSAVEFILESBASEDIRECTORY);
            }

            //dealing with option directoryofconfig  ***************************************************************************
            if (!string.IsNullOrEmpty(opts.directoryofconfig))
            {
                saveFilecgenConfigGlobal.CgenConfig.DirectoryOfConfig = opts.directoryofconfig;
                saveFilecgenConfigGlobal.Save();
                Console.WriteLine("directory " + opts.directoryofconfig + " \n has been set as directoryofconfig");
                return null;
            }

            //get all current targets (platforms) that are available to the user to use for projects
            saveFilecgenConfigGlobal.Load();
            string pathToGlobalBuildConfig_h = Path.Combine(saveFilecgenConfigGlobal.CgenConfig.DirectoryOfConfig,
                "GlobalBuildConfig.h");
            CppParser cppParser = new CppParser(pathToGlobalBuildConfig_h);
            var enumsCursors = cppParser.GetAllCursorsOfKind<MyCursorOfKindEnumDecl>();
            //get that platforenum
            MyCursorOfKindEnumDecl platformenum = enumsCursors.Where((MyCursorOfKindEnumDecl myc) => { return myc.getCursorSpelling() == "PlatformEnum"; }).First();
            //get values of enum as string
            List<MyCursorOfKindEnumConstantDecl> platformsAvailableCursor = platformenum.GetChildrenOfKind_EnumConstantDecl();
            List<string> platformsAvailable = platformsAvailableCursor.Select((MyCursorOfKindEnumConstantDecl enumval) => { return enumval.getCursorSpelling(); }).ToList();
            saveFilecgenConfigGlobal.Load();
            saveFilecgenConfigGlobal.CgenConfig.PlatForms.PlatForm.Clear();
            platformsAvailable.ForEach((p) => saveFilecgenConfigGlobal.CgenConfig.PlatForms.PlatForm.Add(p));
            saveFilecgenConfigGlobal.Save();


            #region dealing with option -a  ***************************************************************************
            //***************************************************************************************************
            //get a refactorer and a cgenxmlConfigFile

            CppRefactorer cpprefactorer = new CppRefactorer(pathToGlobalBuildConfig_h);
            saveFilecgenConfigGlobal.Load();
            if (!string.IsNullOrEmpty(opts.PlatformToAdd))
            {
                //make sure that platform does not already exist
                if (!platformsAvailable.Any((string platformAvailable) => { return platformAvailable == opts.PlatformToAdd; }))
                {
                    //insert the new platform into the enum
                    cpprefactorer.AddEnumConstantDecl("GlobalBuildConfig.h", "PlatformEnum", opts.PlatformToAdd);
                    saveFilecgenConfigGlobal.CgenConfig.PlatForms.PlatForm.Add(opts.PlatformToAdd);
                    saveFilecgenConfigGlobal.Save();
                    Console.WriteLine("target " + opts.PlatformToAdd + " has been added");
                    return null;
                }
                else
                {
                    ProblemHandle p = new ProblemHandle();
                    p.ThereisAProblem(opts.PlatformToAdd + " target already exists");
                    return null;
                }

            }
            #endregion


            #region dealing with option -r  ***************************************************************************              
            //***************************************************************************************************
            if (!string.IsNullOrEmpty(opts.PlatformToRemove))
            {
                //make sure that platform DOES already exist
                if (platformsAvailable.Any((string platformAvailable) => { return platformAvailable == opts.PlatformToRemove; }))
                {
                    cpprefactorer.RemoveEnumConstantDecl("GlobalBuildConfig.h", "PlatformEnum", opts.PlatformToRemove);
                    saveFilecgenConfigGlobal.CgenConfig.PlatForms.PlatForm.Remove(saveFilecgenConfigGlobal.CgenConfig.PlatForms.PlatForm.First(plat => plat == opts.PlatformToRemove));
                    saveFilecgenConfigGlobal.Save();
                    Console.WriteLine("target " + opts.PlatformToRemove + " has been Removed");
                    return null;
                }
                else
                {
                    ProblemHandle p = new ProblemHandle();
                    p.ThereisAProblem(opts.PlatformToAdd + " Target does not exist. Check your spelling?");
                    return null;
                }

            }
            #endregion


            Console.WriteLine("Current platforms available are: ");
            foreach (var platformAva in platformsAvailable)
            {
                Console.WriteLine(platformAva);
            }
            Console.WriteLine("\ncurrent configurations available are: \nDEBUG \nRELEASE \n");
            Console.WriteLine("To create another Platform: cgen config -a <PLATFORMNAME>.\nto remove: cgen config -r <PLATFORMNAME>");


            return null;
        }
        #endregion


        #region configproj command***************************************************************************
        //***************************************************************************************************   
        static ParserResult<object> ProjConfig(ProjConfigOptions opts)
        {
            cgenProjectGlobal projGlob = savefileProjGlobal.CgenProjects.Projects.First(p =>
                p.NameOfProject == SaveFilecgenProjectLocal.CgenProjects.Project.NameOfProject);



            #region add a include to a platform --addinclude ***************************************************************************
            //*************************************************************************************************** 
            if ((!string.IsNullOrEmpty(opts.addinclude) && !string.IsNullOrEmpty(opts.platform)))
            {

                if (!isPlatformExistAsACreatedPlatform(opts.platform))
                {
                    ProblemHandle p = new ProblemHandle();
                    p.ThereisAProblem("The platform " + opts.platform + " you are trying to put into scope was not created yet");
                }

                if (!isPlatformInProjectScope(opts.platform, projGlob))
                {
                    ProblemHandle p = new ProblemHandle();
                    p.ThereisAProblem("The platform " + opts.platform + " is a project not in scope for this project \n use option cgen configproj -a <platform> to set project in scope");
                }

                //make sure it is of proper path format. it needs to not have an extension and directory needs to exist 
                if (Path.HasExtension(opts.addinclude))
                {
                    ProblemHandle p = new ProblemHandle();
                    p.ThereisAProblem("The include \n" + opts.addinclude + "\n should be a path to directory, not a file.");
                }
                if (!Directory.Exists(opts.addinclude))
                {
                    ProblemHandle p = new ProblemHandle();
                    p.ThereisAProblem("The include \n" + opts.addinclude + "\n does not exist");
                }

                if (isIncludeExistForPlatform(opts.platform, opts.addinclude))
                {
                    ProblemHandle p = new ProblemHandle();
                    p.ThereisAProblem("The include \n" + opts.addinclude + "\n has already been added");
                }

                //add the include to that platform 
                savefileProjGlobal.CgenProjects.Projects
                    .First(p => p.NameOfProject == SaveFilecgenProjectLocal.CgenProjects.Project.NameOfProject)
                    .PlatFormsInScope.PlatForms.First(pl => pl.PlatFormName == opts.platform)
                    .AdditionalIncludes.AdditionalInclude.Add(opts.addinclude);
                savefileProjGlobal.Save();


                Console.WriteLine(opts.addinclude + "\nhas been added from platform " + opts.platform);
                return null;
            }
            #endregion

            #region remove a include to a platform --removeinclude ***************************************************************************
            //***************************************************************************************************  
            if ((!string.IsNullOrEmpty(opts.removeinclude) && !string.IsNullOrEmpty(opts.platform)))
            {

                if (!isPlatformExistAsACreatedPlatform(opts.platform))
                {
                    ProblemHandle p = new ProblemHandle();
                    p.ThereisAProblem("The platform " + opts.platform + " you are trying to put into scope was not created yet.");
                }

                if (!isPlatformInProjectScope(opts.platform, projGlob))
                {
                    ProblemHandle p = new ProblemHandle();
                    p.ThereisAProblem("The platform " + opts.platform + " is a project not in scope for this project \n use option cgen -a <platform> to set project in scope");
                }

                //make sure it is of proper path format. it needs to not have an extension and directory needs to exist 
                if (Path.HasExtension(opts.removeinclude))
                {
                    ProblemHandle p = new ProblemHandle();
                    p.ThereisAProblem("The include \n" + opts.removeinclude + "\n should be a path to directory, not a file.");
                }
                if (!Directory.Exists(opts.removeinclude))
                {
                    ProblemHandle p = new ProblemHandle();
                    p.ThereisAProblem("The include \n" + opts.removeinclude + "\n does not exist");
                }

                if (!isIncludeExistForPlatform(opts.platform, opts.removeinclude))
                {
                    ProblemHandle p = new ProblemHandle();
                    p.ThereisAProblem("The include \n" + opts.removeinclude + "\n already is not in scope for that platform");
                }


                //remove the include from the platform 
                savefileProjGlobal.CgenProjects.Projects
                    .First(p => p.NameOfProject == SaveFilecgenProjectLocal.CgenProjects.Project.NameOfProject)
                    .PlatFormsInScope.PlatForms.First(pl => pl.PlatFormName == opts.platform)
                    .AdditionalIncludes.AdditionalInclude.Remove(opts.removeinclude);
                savefileProjGlobal.Save();

                Console.WriteLine(opts.removeinclude + "\nhas been removed from platform " + opts.platform);
                return null;
            }


            #endregion

            #region add a library to a platform --addlibrary ***************************************************************************
            //***************************************************************************************************  
            if ((!string.IsNullOrEmpty(opts.addlibrary) && !string.IsNullOrEmpty(opts.platform)))
            {

                if (!isPlatformExistAsACreatedPlatform(opts.platform))
                {
                    ProblemHandle p = new ProblemHandle();
                    p.ThereisAProblem("The platform " + opts.platform + " you are trying to put into scope was not created yet");
                }

                if (!isPlatformInProjectScope(opts.platform, projGlob))
                {
                    ProblemHandle p = new ProblemHandle();
                    p.ThereisAProblem("The platform " + opts.platform + " is a project not in scope for this project \n use option cgen -a <platform> to set project in scope");
                }

                if (!(Path.GetExtension(opts.addlibrary) == ".lib"))
                {
                    ProblemHandle p = new ProblemHandle();
                    p.ThereisAProblem("The library \n" + opts.addlibrary + "\n should point to a file with a .lib extension.");
                }
                if (!File.Exists(opts.addlibrary))
                {
                    ProblemHandle p = new ProblemHandle();
                    p.ThereisAProblem("The library \n" + opts.addlibrary + "\n does not exist");
                }

                if (isLibraryExistForPlatform(opts.platform, opts.addlibrary))
                {
                    ProblemHandle p = new ProblemHandle();
                    p.ThereisAProblem("The library \n" + opts.addlibrary + "\n has already been added");
                }

                //add the include to that platform 
                savefileProjGlobal.CgenProjects.Projects
                    .First(p => p.NameOfProject == SaveFilecgenProjectLocal.CgenProjects.Project.NameOfProject)
                    .PlatFormsInScope.PlatForms.First(pl => pl.PlatFormName == opts.platform)
                    .AdditionalLibraries.AdditionalLibrary.Add(opts.addlibrary);
                savefileProjGlobal.Save();


                Console.WriteLine(opts.addlibrary + "\nhas been added to platform " + opts.platform);
                return null;
            }
            #endregion

            #region remove a library to a platform --removelibrary***************************************************************************
            //***************************************************************************************************  
            if ((!string.IsNullOrEmpty(opts.removelibrary) && !string.IsNullOrEmpty(opts.platform)))
            {

                if (!isPlatformExistAsACreatedPlatform(opts.platform))
                {
                    ProblemHandle p = new ProblemHandle();
                    p.ThereisAProblem("The platform " + opts.platform + " you are trying to put into scope was not created yet");
                }

                if (!isPlatformInProjectScope(opts.platform, projGlob))
                {
                    ProblemHandle p = new ProblemHandle();
                    p.ThereisAProblem("The platform " + opts.platform + " is a project not in scope for this project \n use option cgen -a <platform> to set project in scope");
                }

                if (!(Path.GetExtension(opts.removelibrary) == ".lib"))
                {
                    ProblemHandle p = new ProblemHandle();
                    p.ThereisAProblem("The library \n" + opts.removelibrary + "\n should point to a file with a .lib extension.");
                }
                if (!File.Exists(opts.removelibrary))
                {
                    ProblemHandle p = new ProblemHandle();
                    p.ThereisAProblem("The library \n" + opts.removelibrary + "\n does not exist");
                }

                if (!isLibraryExistForPlatform(opts.platform, opts.removelibrary))
                {
                    ProblemHandle p = new ProblemHandle();
                    p.ThereisAProblem("The library \n" + opts.removelibrary + "\n already does not exist for that platform");
                }

                //add the include to that platform 
                savefileProjGlobal.CgenProjects.Projects
                    .First(p => p.NameOfProject == SaveFilecgenProjectLocal.CgenProjects.Project.NameOfProject)
                    .PlatFormsInScope.PlatForms.First(pl => pl.PlatFormName == opts.platform)
                    .AdditionalLibraries.AdditionalLibrary.Remove(opts.removelibrary);
                savefileProjGlobal.Save();


                Console.WriteLine(opts.removelibrary + "\nhas been removed from platform " + opts.platform);
                return null;
            }
            #endregion

            #region add a platform to the project  -a ***************************************************************************
            //***************************************************************************************************   
            if (!string.IsNullOrEmpty(opts.PlatformToAdd))
            {
                savefileProjGlobal.Load();
                saveFilecgenConfigGlobal.Load();


                if (!isPlatformExistAsACreatedPlatform(opts.PlatformToAdd))
                {
                    ProblemHandle p = new ProblemHandle();
                    p.ThereisAProblem("The platform " + opts.PlatformToAdd + " you are trying to put into scope was not created yet");
                }

                if (isPlatformInProjectScope(opts.PlatformToAdd, projGlob))
                {
                    ProblemHandle p = new ProblemHandle();
                    p.ThereisAProblem("The platform " + opts.PlatformToAdd + " is already a project in scope");
                }

                //add that platform to proj scope then 
                PlatForm newplatformInScope = new PlatForm()
                {
                    AdditionalIncludes = new AdditionalIncludes(),
                    AdditionalLibraries = new AdditionalLibraries(),
                    PlatFormName = opts.PlatformToAdd
                };
                savefileProjGlobal.CgenProjects.Projects.First(p =>
                    p.NameOfProject == SaveFilecgenProjectLocal.CgenProjects.Project.NameOfProject).
                    PlatFormsInScope.PlatForms.Add(newplatformInScope);

                savefileProjGlobal.Save();


                Console.WriteLine(opts.PlatformToAdd + "platform has been added");
                return null;
            }

            #endregion


            #region remove a platform to the project -r ***************************************************************************
            //***************************************************************************************************   
            if (!string.IsNullOrEmpty(opts.PlatformToRemove))
            {
                savefileProjGlobal.Load();
                saveFilecgenConfigGlobal.Load();

                if (!isPlatformExistAsACreatedPlatform(opts.PlatformToRemove))
                {
                    ProblemHandle p = new ProblemHandle();
                    p.ThereisAProblem("The platform " + opts.PlatformToRemove + " you are trying to put into scope");
                }

                if (!isPlatformInProjectScope(opts.PlatformToRemove, projGlob))
                {
                    ProblemHandle p = new ProblemHandle();
                    p.ThereisAProblem("The platform " + opts.PlatformToRemove + " is not in scope already.");
                }
                //you cant remove ALLPLATFORMS
                if (opts.platform == "ALLPLATFORMS")
                {
                    ProblemHandle p = new ProblemHandle();
                    p.ThereisAProblem("The include \n" + opts.removeinclude + "\n already is not in scope for that platform");
                }

                //remove that platform to proj scope then  
                PlatForm platToRemove = savefileProjGlobal.CgenProjects.Projects.First(p =>
                        p.NameOfProject == SaveFilecgenProjectLocal.CgenProjects.Project.NameOfProject).
                    PlatFormsInScope.PlatForms
                    .First(pl => pl.PlatFormName == opts.PlatformToRemove);
                savefileProjGlobal.CgenProjects.Projects.First(p =>
                        p.NameOfProject == SaveFilecgenProjectLocal.CgenProjects.Project.NameOfProject).PlatFormsInScope
                    .PlatForms.Remove(platToRemove);

                savefileProjGlobal.Save();

                Console.WriteLine(opts.PlatformToRemove + "platform has been removed");

                return null;
            }

            #endregion


            #region no options given. just empty command ***************************************************************************
            //*************************************************************************************************** 
            //Over here, start filling this out. I need to load the savefileProjGlobal where the name matches the local.
            //get the project and then the platformsinscope and list them along with the additional includes and libraries
            savefileProjGlobal.Load();
            SaveFilecgenProjectLocal projLocal = new SaveFilecgenProjectLocal();
            Console.WriteLine("Platforms in scope for the project " + SaveFilecgenProjectLocal.CgenProjects.Project.NameOfProject + " are ");
            foreach (var platform in projGlob.PlatFormsInScope.PlatForms)
            {
                Console.WriteLine("\t" + platform.PlatFormName);
                Console.WriteLine("additional includes are: ");
                foreach (var include in platform.AdditionalIncludes.AdditionalInclude)
                {
                    Console.WriteLine("\t\t" + include);
                }
                Console.WriteLine("additional libraries are: ");
                foreach (var library in platform.AdditionalLibraries.AdditionalLibrary)
                {
                    Console.WriteLine("\t\t" + library);
                }

                Console.WriteLine();
            }

            #endregion


            return null;
        }

        #endregion



        #region Sync command ***************************************************************************
        //***************************************************************************************************  
        static ParserResult<object> Sync(SyncOptions opts)
        {

            SynchronizeProjectBase synchronizeProject = null;

            if (opts.name == "iar")
            {
                synchronizeProject = SynchronizeProjectIAR.CreateSynchronizeProjectIAR(Path.Combine(envIronDirectory,
                    SynchronizeProjectIAR.FirstIARDir, SynchronizeProjectIAR.SecondIARDir));
                synchronizeProject.Initiate();
            }

            if (synchronizeProject != null)
            {
                synchronizeProject = SynchronizeProjectIAR.CreateSynchronizeProjectIAR(Path.Combine(envIronDirectory, SynchronizeProjectIAR.FirstIARDir, SynchronizeProjectIAR.SecondIARDir));
                synchronizeProject.Initiate();
                MySettingsVS settings = MySettingsVS.CreateMySettingsVS(envIronDirectory,true);
                /*
                synchronizeProjectiar.AddAdditionalInclude("bla\\bla2");
                MyFilter blaFilter = new MyFilter("bla");
                synchronizeProjectiar.AddFilter(blaFilter);
                MyCLCompileFile ccom = new MyCLCompileFile(blaFilter, "someLibrary.cpp", "Config");
                synchronizeProjectiar.AddCLCompileFile(ccom);
                */

                //first sync all filters
                foreach (var settingsMyFilter in settings.myFilters)
                {
                    synchronizeProject.AddFilter(settingsMyFilter); 
                }

                //sync all files exxcept mainCG.cpp
                foreach (var ccomps in settings.CLCompileFiles)
                {
                    if (ccomps.Name != "mainCG.cpp")
                    {
                        synchronizeProject.AddCLCompileFile(ccomps);
                    } 
                }
                foreach (var cincl in settings.CLIncludeFiles)
                {
                    synchronizeProject.AddCLIncludeFile(cincl);
                }

                //sync all additional includes
                foreach (var additinclude in settings.StringIncludes)
                {
                    synchronizeProject.AddAdditionalInclude(additinclude);
                }

                synchronizeProject.Save(Path.Combine(envIronDirectory, SynchronizeProjectIAR.FirstIARDir, SynchronizeProjectIAR.SecondIARDir));

                Console.WriteLine("project IAR was synced up with VS");
            }
            else
            {
                ProblemHandle p = new ProblemHandle();
                p.ThereisAProblem("No projects of that name availabe. available projects to sync are: \niar");

            }
            return null;
        }
        #endregion


        #region Init command ***************************************************************************
        //***************************************************************************************************  
        static ParserResult<object> Init(InitOptions opts)
        {
            Console.WriteLine(DIRECTORYOFTHISCG);

            #region --iar -----------------------------------------------------

            if (opts.iar)
            {
                try
                {
                    //create directory to synced projects like IAR.
                    string IARDirPath = Path.Combine(Program.envIronDirectory, "IAR");
                    if (!Directory.Exists(IARDirPath))
                    {
                        Directory.CreateDirectory(IARDirPath);
                        //also put in an empty IAR project
                        //Directory.CreateDirectory(Path.Combine(IARDirPath,"EWARM"));
                        string templateIARDir = Path.Combine(Program.DIRECTORYOFTHISCG, "IARDefaultProj");
                        Extensions.restOfExtensions.CopyAllContentsInDirectory(templateIARDir, Path.Combine(IARDirPath));
                        Console.WriteLine("template project for IAR was created");

                        //change project name 
                        string pathToFile = Path.Combine(IARDirPath, "EWARM");
                        string[] files = Directory.GetFiles(pathToFile);
                        string fileEWW = files.First(f => Path.GetExtension(f) == ".eww");
                        File.Move(Path.Combine(pathToFile, Path.GetFileName(fileEWW)), Path.Combine(pathToFile, Path.GetFileName(opts.name + ".eww")));
                    }
                }
                catch (Exception e)
                {

                    Console.WriteLine("there was a problem importing the IAR project. Check to make sure all was imported ok by building that project");
                }

                return null;
            }
            #endregion


            #region --git -----------------------------------------------

            if (opts.git)
            {
                if (IsProjectExistsAtEnvironDirectory())
                {
                    //check if a git exists, if not, create one and put a gitignore in. 
                    CMDHandler cmd = new CMDHandler(envIronDirectory);
                    GitHandlerForLibrary gitHandler = new GitHandlerForLibrary(cmd);
                    //check that git exists
                    if (!gitHandler.IsPathHaveGit(envIronDirectory))
                    {
                        gitHandler.InitGitHere(envIronDirectory);
                        if (!File.Exists(Path.Combine(envIronDirectory, ".gitignore")))
                        {
                            File.Copy(".gitignore", Path.Combine(envIronDirectory, ".gitignore"));
                        }

                        if (!File.Exists(Path.Combine(envIronDirectory, ".gitattributes")))
                        {
                            File.Copy(".gitattributes", Path.Combine(envIronDirectory, ".gitattributes"));
                        }

                        gitHandler.CommitAll(envIronDirectory,true);
                        if (gitHandler.Cmd.Output.Contains("error:") || gitHandler.Cmd.Error.Contains("error:"))
                        {
                            ProblemHandle p = new ProblemHandle();
                            p.ThereisAProblem(
                                "there was a problem  doing the first commit all. you should do this manually");
                        }

                        gitHandler.AddVersionTag(envIronDirectory, 0, 0, 1);
                        Console.WriteLine("git initialized here");
                    }
                    else
                    {
                        Console.WriteLine("there is already a git repo here. nothing was done.");
                    }

                }
                else
                {
                    Console.WriteLine("A project does not exist here yet.  to create one use \ncgen init <NAMEOFPROJECT>   ");
                }

                return null;
            }
            #endregion

            //get the project builder for Visual Studio, as VS is right now the only supporting starting project build
            //IDESetting settingConfig = new IDESetting(@"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\ConfigTest", ".xml", typeof(Root));
            //ProjectBuilderBase projectBuilderForVs = new ProjectBuilderVS(settingConfig);

            //if that name already exists
            if (savefileProjGlobal.CgenProjects.Projects.Any(p => p.NameOfProject == opts.name))
            {
                ProblemHandle p = new ProblemHandle();
                p.ThereisAProblem("The name " + opts.name + " already exists");
            }

            //check that there isnt already a project here.  
            if (IsProjectExistsAtEnvironDirectory())
            {
                //get project settings for this.
                MySettingsVS settings = MySettingsVS.CreateMySettingsVS(envIronDirectory);
                settings.RecreateConfigurationFilterFolderIncludes(GetSaveFilecgenProjectAtEnvironDirectory().CgenProjects.Project.NameOfProject);
                //projectBuilderForVs.RecreateConfigurationFilterFolderIncludes(GetSaveFilecgenProjectAtEnvironDirectory().CgenProjects.Projects.First().NameOfProject, saveFilecgenConfigLocal.CgenConfig.DirectoryOfConfig);
                Console.WriteLine(GetSaveFilecgenProjectAtEnvironDirectory().CgenProjects.Project.NameOfProject + " already exists as a project here");
                return null;
            }
            else if (string.IsNullOrEmpty(opts.name))
            {
                Console.WriteLine("A project does not exist here yet.  to create one use \ncgen init <NAMEOFPROJECT>   ");
            }
            else
            {
                //get settings for poject you are wanting to init
                MySettingsVS settings = MySettingsVS.CreateMySettingsVS(envIronDirectory);



                if (Regex.IsMatch(opts.name, @"\d"))
                {
                    Console.WriteLine("you cant have a number in the name of the project");
                    return null;
                }

                ProblemHandle problemHandle = new ProblemHandle();

                //then I need to create a new project as none exist and user wants to create one 
                try
                {


                    Directory.CreateDirectory(envIronDirectory + "\\" + CGSAVEFILESBASEDIRECTORY);
                    //CREATE ALL THE FILES THAT ARE NEEDED FOR PROJECT INITIATION!

                    cgenProjectLocal projToAdd = new cgenProjectLocal();
                    projToAdd.NameOfProject = opts.name;
                    projToAdd.PathOfProject = envIronDirectory;// + "\\" + CGSAVEFILESBASEDIRECTORY;
                    projToAdd.UniqueIdentifier = cgenXMLMemeberCreationHelper.UniqueIdentifierCreator();
                    //also add the project to the GlobalProjSettings  
                    savefileProjGlobal.AddNewProject((cgenProjectGlobal)projToAdd);
                    SaveFilecgenProjectLocal.CgenProjects.Project = projToAdd;
                    SaveFilecgenProjectLocal.Save();
                    savefileProjGlobal.Save();

                    RemoveProjectCleanup removeProjectCleanup = new RemoveProjectCleanup(envIronDirectory + "\\" + CGSAVEFILESBASEDIRECTORY, savefileProjGlobal, projToAdd);
                    var projectCleanup = (ICleanUp)removeProjectCleanup;
                    problemHandle.AddCleanUp(ref projectCleanup);
                    settings.ProblemHandle = problemHandle;

                    #region add platform in scope "ALLPLATFORMS", configtest.lib, and additional include Program.PATHTOCONFIGTEST

                    //over here. tests that this works by init mod again 
                    savefileProjGlobal.Load();
                    SaveFilecgenProjectLocal.Load();

                    PlatForm newplatformInScope = new PlatForm()
                    {
                        AdditionalIncludes = new AdditionalIncludes(),
                        AdditionalLibraries = new AdditionalLibraries(),
                        PlatFormName = "ALLPLATFORMS"
                    };
                    savefileProjGlobal.AddNewPlatformInScope(opts.name, newplatformInScope);
                    savefileProjGlobal.AddNewInclude(opts.name, "ALLPLATFORMS", PATHTOCONFIGTEST);
                    savefileProjGlobal.AddNewLibrary(opts.name, "ALLPLATFORMS", Path.Combine(PATHTOCONFIGTEST, "ConfigTest.lib"));


                    settings.AddAdditionalInclude(PATHTOCONFIGTEST);
                    settings.AddAdditionalLibrary(Path.Combine(PATHTOCONFIGTEST, "ConfigTest.lib"));
                    settings.Save(envIronDirectory);

                    #endregion

                    //save created files
                    SaveFilecgenProjectLocal.Save();
                    savefileProjGlobal.Save();
                    settings.RecreateConfigurationFilterFolderIncludes(opts.name);
                    //projectBuilderForVs.RecreateConfigurationFilterFolderIncludes(opts.name, saveFilecgenConfigLocal.CgenConfig.DirectoryOfConfig);

                    Console.WriteLine("Project successfully created");



                    //5. update CGKeywords.h and alllibraryincludes.h here 
                    UpdateCCGKeywordsIncludes();

                }
                catch (Exception e)
                {
                    
                    //todo. I should clean up and use a problem handle to save the settings
                    //todo file in a simple string and replace the .vcxproj and .filters and delete the locat CGenSaveFiles directory
                    problemHandle.ThereisAProblem("Something went wrong initializing. error : \n" + e.Message);
                    Console.WriteLine(e);
                }
                // so we have a name and no project exists here. create one 


                try
                { 
                    //create directory to synced projects like IAR.
                    string IARDirPath = Path.Combine(Program.envIronDirectory, "IAR");
                    if (!Directory.Exists(IARDirPath))
                    {
                        Directory.CreateDirectory(IARDirPath);
                        //also put in an empty IAR project
                        //Directory.CreateDirectory(Path.Combine(IARDirPath,"EWARM"));
                        string templateIARDir = Path.Combine(Program.DIRECTORYOFTHISCG, "IARDefaultProj");
                        Extensions.restOfExtensions.CopyAllContentsInDirectory(templateIARDir, Path.Combine(IARDirPath));
                        Console.WriteLine("template project for IAR was created");

                        //change project name 
                        string pathToFile = Path.Combine(IARDirPath, "EWARM");
                        string[] files = Directory.GetFiles(pathToFile);
                        string fileEWW = files.First(f => Path.GetExtension(f) == ".eww");
                        File.Move(Path.Combine(pathToFile, Path.GetFileName(fileEWW)), Path.Combine(pathToFile, Path.GetFileName(opts.name + ".eww")));
                    }
                }
                catch (Exception e )
                {

                    Console.WriteLine("there was a problem importing the IAR project. Check to make sure all was imported ok by building that project");
                }


            }


            return null;
        }
        #endregion


        #region Projects command ***************************************************************************
        static ParserResult<object> Projects(ProjectsOptions opts)
        { 
            foreach (var proj in savefileProjGlobal.CgenProjects.Projects)
            {
                Console.Write("Project: ");
                Console.WriteLine(proj.NameOfProject);
                Console.WriteLine(proj.PathOfProject + "\n");
            } 

            return null;
        }
        #endregion


        #region Macro command ***************************************************************************
        static ParserResult<object> Macro(MacroOptions opts)
        {

            //get all macro files in the environment directory
            List<string> cgenMFiles = Directory.GetFiles(envIronDirectory).Where((file) =>
            { 
                return Path.GetExtension(file).Equals(".cgenM");
            }).ToList();

            if (cgenMFiles.Count == 0)
            {
                Console.WriteLine("no files with extension .cgenM was found at this directory.");
            }
            else
            {

                //go through each .cgenM file and create it
                foreach (var cgenMFilePath in cgenMFiles)
                {
                    string s = Path.GetDirectoryName(cgenMFilePath);
                    GeneralMacro generalMacro = new GeneralMacro(s, Path.GetFileName(cgenMFilePath));
                    generalMacro.CreateTemplate();

                    Console.WriteLine(Path.GetFileName(cgenMFilePath)+" macro was created.");
                }  

            }
             
            return null;
        }
        #endregion


        #region Generate command ***************************************************************************
        //***************************************************************************************************  

        //private static bool UsingGit = false;

        static ParserResult<object> Generate(GenerateOptions opts)
        {

            if (opts.ignoreFilesInFilter != null)
            {
                if (!opts.ignoreFilesInFilter.IsAnEmptyLine())
                {
                    Library.IgnoreFilesFromFilter = opts.ignoreFilesInFilter;
                }
            }

            #region --config 
            if (opts.config)
            {
                //first make sure that a project exists here
                if (IsProjectExistsAtEnvironDirectory())
                {

                    //get the project settings for the project configs I want to generate. for VS for NOW!
                    MySettingsVS VSsetting = MySettingsVS.CreateMySettingsVS(envIronDirectory);

                    //1. I need to create the configuration file for the top level library just so that I can have all
                    // the libraries configs information that it depends on. in the saveddata.xml file.
                    ConfigurationFileBuilder configFileBuilder = new ConfigurationFileBuilder(VSsetting, CGCONFCOMPILATOINSBASEDIRECTORY, DIRECTORYOFTHISCG, PATHTOCONFIGTEST, null, true);
                    configFileBuilder.CreateConfigurationToTempFolder();

                    //create project builder. and check if all libraries support the platform Im on
                    ProjectBuilderVS projectBuilderForVs = CreateProjectBuilderVS();
                    string libraryNameNotSupportingPlat = projectBuilderForVs.GetLibraryThatDoesNOTSupportPlatform(savefileProjGlobal);
                    if (libraryNameNotSupportingPlat != null)
                    {
                        ProblemHandle p = new ProblemHandle();
                        p.ThereisAProblem(libraryNameNotSupportingPlat + " does not support the platform you are building for. \n use cgen projconfig -a <nameofScope> \n to add platform to that project scope.");
                    }
                    configFileBuilder.WriteTempConfigurationToFinalFile();


                    //2.  add filters and folders directories from that toplevel project directory as:
                    //LibraryDependencies
                    //  prefix(of libraries top level uses)
                    //      confTypePrefix(for libraries that are same but different template type.)    
                    //projectBuilderForVs.RecreateLibraryDependenciesFoldersFilters();

                    //3. I need to go through each library, git checkout their correct major. (master should be the branch with tag name of major)
                    //first grab all lowest level libraries that have no dependencies
                    projectBuilderForVs.ImportConfigFiles(!opts.ignoreGit);



                    //4. For the settings(.vcxproj .filters) of the top level I need to get all the cIncludes,
                    //cClompiles, additionalincludes additional libraries from the other libraries and add to top level
                    // to the top level library. 
                    //projectBuilderForVs.ImportDependentSettingsLibrariesCincAndCcompAndAdditional(savefileProjGlobal);


                    //5. Finally recreate the xml settings files. 
                    //projectBuilderForVs.LibTop.GenerateXMLSettings(projectBuilderForVs.BaseDirectoryForProject);



                }
                else
                {
                    //project does not exist
                    Console.WriteLine("A project does not exist here yet.  to create one use \ncgen init <NAMEOFPROJECT>   ");
                }

                return null;
            }
            #endregion


            //first make sure that a project exists here
            if (IsProjectExistsAtEnvironDirectory())
            {

                //get the project settings for the project configs I want to generate. for VS for NOW!
                MySettingsVS VSsetting = MySettingsVS.CreateMySettingsVS(envIronDirectory);

                //1. I need to create the configuration file for the top level library just so that I can have all
                // the libraries configs information that it depends on. in the saveddata.xml file.
                ConfigurationFileBuilder configFileBuilder = new ConfigurationFileBuilder(VSsetting, CGCONFCOMPILATOINSBASEDIRECTORY, DIRECTORYOFTHISCG, PATHTOCONFIGTEST, null, true);
                configFileBuilder.CreateConfigurationToTempFolder();

                //create project builder. and check if all libraries support the platform Im on
                ProjectBuilderVS projectBuilderForVs = CreateProjectBuilderVS();
                string libraryNameNotSupportingPlat = projectBuilderForVs.GetLibraryThatDoesNOTSupportPlatform(savefileProjGlobal);
                if (libraryNameNotSupportingPlat != null)
                {
                    ProblemHandle p = new ProblemHandle();
                    p.ThereisAProblem("\n you are building for a platform "+ projectBuilderForVs.GetPlatFormThisisSetupFor()+ " as stated in your mainCG.cpp file, but "+ libraryNameNotSupportingPlat + " does not support the platform you are building for.  \n use cgen configproj -a <nameofScope> \n to add platform to that project scope.");
                }
                configFileBuilder.WriteTempConfigurationToFinalFile();


                //2.  add filters and folders directories from that toplevel project directory as:
                //LibraryDependencies
                //  prefix(of libraries top level uses)
                //      confTypePrefix(for libraries that are same but different template type.)    
                projectBuilderForVs.RecreateLibraryDependenciesFoldersFilters();

                //3. I need to go through each library, git checkout their correct major. (master should be the branch with tag name of major)
                //first grab all lowest level libraries that have no dependencies
                projectBuilderForVs.ImportDependentLibrariesFiles(!opts.ignoreGit);



                //4. For the settings(.vcxproj .filters) of the top level I need to get all the cIncludes,
                //cClompiles, additionalincludes additional libraries from the other libraries and add to top level
                // to the top level library. 
                projectBuilderForVs.ImportDependentSettingsLibrariesCincAndCcompAndAdditional(savefileProjGlobal);


                //5. Finally recreate the xml settings files. 
                projectBuilderForVs.LibTop.GenerateXMLSettings(projectBuilderForVs.BaseDirectoryForProject);



            }
            else
            {
                //project does not exist
                Console.WriteLine("A project does not exist here yet.  to create one use \ncgen init <NAMEOFPROJECT>   ");
            }


            return null;
        }

        #endregion



        #region Degenerate command ***************************************************************************
        //***************************************************************************************************  
        static ParserResult<object> Degenerate(DegenerateOptions opts)
        {



            Console.WriteLine(string.Join(" ", command));

            foreach (var item in opts.IncludeFiles)
            {
                Console.WriteLine(item);
            }


            return null;
        }

        #endregion



        #region helper static functions  ***************************************************************************
        //***************************************************************************************************  



        public static void UpdateCCGKeywordsIncludes()
        {
            //update CGKeywords.h and alllibraryincludes.h here
            FileTemplateAllLibraryInlcudes faAllLibraryInlcudes = new FileTemplateAllLibraryInlcudes(PATHTOCONFIGTEST, savefileProjGlobal);
            FileTemplateCGKeywordDefine fileTemplateCgKeyword = new FileTemplateCGKeywordDefine(PATHTOCONFIGTEST, savefileProjGlobal);
            faAllLibraryInlcudes.CreateTemplate();
            fileTemplateCgKeyword.CreateTemplate();
            Console.WriteLine("LibraryIncludes updated");
        }


        static bool isIncludeExistForPlatform(string forPlatform, string forInclude)
        {
            var additionals = savefileProjGlobal.CgenProjects.Projects
                .First(p => p.NameOfProject == SaveFilecgenProjectLocal.CgenProjects.Project.NameOfProject)
                .PlatFormsInScope.PlatForms.First(pl => pl.PlatFormName == forPlatform)
                .AdditionalIncludes.AdditionalInclude;
            if (additionals.Count == 0)
            {
                return false;
            }
            return additionals.Any(ad => ad == forInclude);
        }

        static bool isLibraryExistForPlatform(string forPlatform, string forLibrary)
        {
            var libraries = savefileProjGlobal.CgenProjects.Projects
                .First(p => p.NameOfProject == SaveFilecgenProjectLocal.CgenProjects.Project.NameOfProject)
                .PlatFormsInScope.PlatForms.First(pl => pl.PlatFormName == forPlatform)
                .AdditionalLibraries.AdditionalLibrary;
            if (libraries.Count == 0)
            {
                return false;
            }
            return libraries.Any(ad => ad == forLibrary);
        }

        /// <summary>
        /// check that the platform exists as a created platform
        /// </summary>
        /// <param name="PlatformName"></param>
        /// <returns></returns>
        static bool isPlatformExistAsACreatedPlatform(string PlatformName)
        {
            //check that the platform exists as a created platform
            if (saveFilecgenConfigGlobal.CgenConfig.PlatForms.PlatForm.Any(pl => pl == PlatformName))
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// is the local project has platform in scope
        /// </summary>
        /// <param name="PlatformName"></param>
        /// <returns></returns>
        static bool isPlatformInProjectScope(string PlatformName, cgenProjectGlobal projGlob)
        {
            //is the local project has platform in scope
            if (projGlob.PlatFormsInScope.PlatForms.Any(pl => pl.PlatFormName == PlatformName))
            {
                return true;
            }
            return false;
        }


        static bool IsProjectExistsAtEnvironDirectory()
        {
            string fullpath = Path.Combine(envIronDirectory, CGSAVEFILESBASEDIRECTORY, "cgenProjs.cgx");
            if (Directory.Exists(Path.GetDirectoryName(fullpath)))
            {
                if (File.Exists(fullpath))
                {
                    return true;
                }
            }
            return false;
        }

        static SaveFilecgenProjectLocal GetSaveFilecgenProjectAtEnvironDirectory()
        {
            if (!IsProjectExistsAtEnvironDirectory())
            {
                throw new Exception("there is no such project here yet");
            }

            return (new SaveFilecgenProjectLocal(envIronDirectory + "\\" + CGSAVEFILESBASEDIRECTORY));

        }


        public static ProjectBuilderVS CreateProjectBuilderVS()
        {

            //DONT WORRY ABOUT LIBRARIES THAT DEPEND ON LIBRARIES FOR NOW JUST GET THIS MUCH TO WORK.
            // steps to import a new library would be
            //1. get the libraries config xmlclasses created. (located in configTest/savedData.xml)
            IDESetting settingConfig = new IDESetting(PATHTOCONFIGTEST, ".xml", typeof(Root));


            //2: get the libraries settings xml file (they will be in the same directory as the config 
            //file path )converted to the settingxml   

            return new ProjectBuilderVS(settingConfig);

        }


        #endregion



    }


}
