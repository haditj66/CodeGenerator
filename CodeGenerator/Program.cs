#define TESTING

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
//using ClangSharp;
using CodeGenerator.FileTemplates;
using CodeGenerator.FileTemplatesMacros;
using CodeGenerator.GitHandlerForLibraries;
using CodeGenerator.IDESettingXMLs.IAR_XMLs;
using CodeGenerator.ProblemHandler;
using CommandLine.Text;
//using ConsoleApp2;
//using ConsoleApp2.CPPRefactoring;
//using ConsoleApp2.MyClangWrapperClasses;
//using ConsoleApp2.MyClangWrapperClasses.CXCursors;
//using ConsoleApp2.MyClangWrapperClasses.CXCursors.MyCursorKinds;
//using ConsoleApp2.Parsing;
//using CPPParser;
using Extensions;
using Project = CodeGenerator.IDESettingXMLs.VisualStudioXMLs.Project;
using CodeGenerator.FileTemplates.GeneralMacoTemplate;
//using MyLibClangVisitors.ConsoleApp2;
using System.Threading;
using CgenMin.MacroProcesses;
using CodeGenerator.MacroProcesses.AESetups;

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

        [Verb("macro2", HelpText = "create a macro using the new version of macro .cgenMM files")]
        public class Macro2Options
        {
            [Value(0, HelpText = "This is the macro process you want to use to generate all the code. Macro Processes")]
            public string macroProcess { get; set; }
        }

        [Verb("aeinit", HelpText = "aertos utility to intialize aertos projects.")]
        public class aeinitOptions
        {
            [Value(0, HelpText = "name of the aertos project")]
            public string nameOfTheProject { get; set; }
        }

        [Verb("aeselect", HelpText = "aertos utility to selecting integration tests, and code generating AOs")]
        public class aeselectOptions
        {
            [Value(0, HelpText = "name of the aertos project to select")]
            public string projectNameSelection { get; set; }
            [Value(1, HelpText = "name of the aertos exe to select")]
            public string projectEXETestSelection { get; set; }
        }

        [Verb("aegenerate", HelpText = "aertos utility to generate current selected aertos project")]
        public class aegenerateOptions
        {
        }

        [Verb("aeserial", HelpText = "aertos utility to launch the serial utility. This needs to be runnning to read uploaded data from a running AE device.")]
        public class aeserialOptions
        {
        }

        [Verb("aebuild", HelpText = "aertos utility to build projects. This will build dependencies in the proper order.")]
        public class aebuildOptions
        {
        }


        [Verb("cmakegui", HelpText = "create a macro file in this directory out of all .cgenM files")]
        public class cmakeguiOptions
        {
        }

        [Verb("post_compile", HelpText = "compile custom cgen keywords in c++")]
        public class post_compileOptions
        {
        }

        //[Verb("generate", HelpText = "generate the code")]
        //public class GenerateOptions
        //{

        //    [Option(HelpText = "only build the configurations for the dependent libraries and not importing all files")]
        //    public bool config { get; set; }

        //    [Option(HelpText = "This will use git to go to switch to an appropriate version when fetching other libraries. use this when you have a library that has been changed and you want to use a past version of that library.")]
        //    public bool git { get; set; }

        //    [Option('i', HelpText = "This will ignore all files that are anywhere in this filter. use this when you have 3rd party files that are shared within ALL libraries.")]
        //    public string ignoreFilesInFilter { get; set; }


        //}

        //[Verb("degenerate", HelpText = "degenerate the code")]
        //public class DegenerateOptions
        //{

        //    [Option('r', Separator = ' ')]
        //    public IEnumerable<string> IncludeFiles { get; set; }

        //}

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


        [Verb("QRinit", HelpText = "create a new QR project")]
        public class QRInitOptions
        {
            //[CommandLine.Value(1), help]
            [Value(0, HelpText = "name of the module you want to create")]
            public string name { get; set; }

            [Option('r', HelpText = "rename an existing project. (dunno if this works)")]
            public bool isToRename { get; set; }

        }
        #endregion

        // C:\\Users\\Hadi\\OneDrive\\Documents\\VisualStudioprojects\\Projects\\cSharp\\CodeGenerator\\CodeGenerator\\CodeGenerator\\bin\\Debug"
        public static string DIRECTORYOFTHISCG = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string CGSAVEFILESBASEDIRECTORY = "CGensaveFiles";
        public static string CGCONFCOMPILATOINSBASEDIRECTORY = "ConfigCompilations";
        public static string PATHTOCONFIGTEST = DIRECTORYOFTHISCG + "..\\..\\..\\..\\ConfigTest"; //@"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\ConfigTest";
        public static string PATHTOCMAKEGUI = DIRECTORYOFTHISCG + "..\\..\\..\\..\\CgenCmakeGui";

        public static string PATHTO_SERIAL_UTILITY = DIRECTORYOFTHISCG + "\\..\\..\\..\\CodeGenerator\\SerialUtility\\SerialWinform\\HolterMonitorGui";

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
        //public static string envIronDirectory = @"C:\Users\Hadi\Documents\Visual Studio 2017\Projects\AO Projects\UVariableSaver\UVariableSaver";
        //public static string envIronDirectory = @"C:\Users\Hadi\Documents\Visual Studio 2017\Projects\AO Projects\UMasterToArduino\UMasterToArduino";
        //public static string envIronDirectory = @"C:\Users\Hadi\Documents\Visual Studio 2017\Projects\AO Projects\blinds\WaveletTransformSPB";
        //public static string envIronDirectory = @"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\CgenCmakeGui\TestFiles";
        //public static string envIronDirectory = @"C:\visualgdb_projects\AERTOSCopy\build\VSGDBCmakeNinja_armnoneabiid\Debug";
        //public static string envIronDirectory = @"C:\QR_Core\build\win";
        //public static string envIronDirectory = @"C:\visualgdb_projects\AERTOSCopy";
        //public static string envIronDirectory = @"C:\visualgdb_projects\AERTOSCopy\build\VSGDBCmakeNinja_armnoneabiid\Debug";
        //public static string envIronDirectory = @"C:\CodeGenerator\CodeGenerator\macro2Test\CGENTest";
        //public static string envIronDirectory = @"C:/AERTOS/AERTOS/src/AE/AESamples";
        //public static string envIronDirectory = @"C:/Users/hadi/Documents/AERTOSProjects/test";
        public static string envIronDirectory = @"C:/Users/hadi/Documents/AERTOSProjects/commonHalAOs";
        //public static string envIronDirectory = @"C:\QR_sync";


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
        //static string[] command  = "generate -i AE ".Split(' ');
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
        //static string[] command = "cmakegui".Split(' ');
        //static string[] command = "macro2 AEServiceMacro".Split(' ');
        //static string[] command = "macro2 AEInitializing".Split(' ');
        //static string[] command = "aeinit test".Split(' ');
        //static string[] command = "aeinit commonHalAOs".Split(' ');
        //static string[] command = "aeselect CGENTest ".Split(' ');
        static string[] command = "aegenerate".Split(' ');
        //static string[] command = "aeserial".Split(' ');
        //static string[] command = "aebuild".Split(' ');
        //static string[] command = "aeinit AESamples".Split(' ');
        //static string[] command = "post_compile".Split(' ');
        //static string[] command = "QRinit tutthree".Split(' ');

#else
        static string[] command;
        public static string envIronDirectory = Environment.CurrentDirectory;
#endif

        public static SaveFilecgenProjectGlobal savefileProjGlobal = new SaveFilecgenProjectGlobal();
        public static SaveFilecgenConfig saveFilecgenConfigGlobal = new SaveFilecgenConfig();
        public static SaveFilecgenProjectLocal SaveFilecgenProjectLocal = new SaveFilecgenProjectLocal();
        //public static ProjectBuilderVS projectBuilderForVs;



        public class test
        {
            int[] arr;
            public test()
            {
                arr = new int[10];
                arr[0] = 4;
            }
            public void grow()
            {
                int[] arr2 = new int[10 * 2];
                Array.Copy(arr, arr2, 10);
                arr = arr2;
            }
        }


        static void Main(string[] args)
        {

            test aaa = new test();
            aaa.grow();

            //string e = Environment.CurrentDirectory;
            //Console.WriteLine(e);
            //ProjectVSTest();
            //CreateProjectBuilder(); this should not be done here as a saveddata.xml may not even be created yet


            Action RunParser = () =>
            {
                Parser.Default.ParseArguments<Macro2Options, aeinitOptions, aeselectOptions, aegenerateOptions, aeserialOptions, aebuildOptions, InitOptions, QRInitOptions, SyncOptions, ConfigOptions, ProjectsOptions, MacroOptions, ProjConfigOptions, post_compileOptions, cmakeguiOptions>(command)
//.WithParsed<GenerateOptions>(opts => Generate(opts))
//.WithParsed<DegenerateOptions>(opts => Degenerate(opts))
.WithParsed<SyncOptions>(opts => Sync(opts))
.WithParsed<InitOptions>(opts => Init(opts))
.WithParsed<QRInitOptions>(opts => QRInit(opts))
.WithParsed<Macro2Options>(opts => Macro2(opts))
.WithParsed<aeinitOptions>(opts => aeinit(opts))
.WithParsed<aeselectOptions>(opts => aeselect(opts))
.WithParsed<aegenerateOptions>(opts => aegenerate(opts))
.WithParsed<aeserialOptions>(opts => aeserial(opts))
.WithParsed<aebuildOptions>(opts => aebuild(opts))
//.WithParsed<ConfigOptions>(opts => Config(opts))
.WithParsed<ProjectsOptions>(opts => Projects(opts))
.WithParsed<MacroOptions>(opts => Macro(opts))
.WithParsed<cmakeguiOptions>(opts => cmakegui(opts))
.WithParsed<post_compileOptions>(opts => post_compile(opts))
//.WithParsed<ProjConfigOptions>(opts => ProjConfig(opts))
;

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


#if NOTDEPRECATED_GENERATE_AND_DEGENERATE

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
#endif


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
                MySettingsVS settings = MySettingsVS.CreateMySettingsVS(envIronDirectory, true);
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

                        gitHandler.CommitAll(envIronDirectory, true);
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
                catch (Exception e)
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

                    Console.WriteLine(Path.GetFileName(cgenMFilePath) + " macro was created.");
                }

            }

            return null;
        }
        #endregion






        #region Macro2 command ***************************************************************************



        static ParserResult<object> Macro2(Macro2Options opts)
        {
            ProblemHandle prob = new ProblemHandle();

            if (opts.macroProcess == null || opts.macroProcess == "")
            {
                prob.ThereisAProblem("You didnt provide a macroProcess name. do that with \"macro <macroProcess>\"");
                return null;
            }

            //use reflection to get all classes that inherit from the abstract MacroProcess class
            var type = typeof(MacroProcess);
            var typeProcessToRun = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p))
                .Where(p => p.Name == opts.macroProcess)
                .FirstOrDefault();

            if (typeProcessToRun == null)
            {
                prob.ThereisAProblem($"the process {opts.macroProcess} you gave does not have an implementation here. create a class that derives from MacroProcess and give it the name of the process you provided");
            }

            MacroProcess myProc = (MacroProcess)Activator.CreateInstance(typeProcessToRun);
            myProc.Init(envIronDirectory);
            myProc.RunProcess();






            return null;
        }

        #endregion




        #region aeinit command ***************************************************************************

        static ParserResult<object> _aeinitProjectFileStructure(aeinitOptions opts, AEInitializing aEInitializing, AEProject projAlreadyExists, string pathToProject)
        {

            // write to all initialization files within the project. DONT OVERWRITE THESE FILES IF THEY ALREADY EXIST!

            string PathToThis = pathToProject;
            //root======================================================
            //AEConfigProject.cmake 
            //AEConfigProjectUser.cmake     (blank) 
            //IntegTestPipeline.h           (blank)
            //main.cpp
            string AEConfigProject = Path.Combine(PathToThis, "AEConfigProject.cmake");
            string AEConfigProjectUser = Path.Combine(PathToThis, "AEConfigProjectUser.cmake");
            string IntegTestPipeline = Path.Combine(PathToThis, "IntegTestPipeline.h");
            string main = Path.Combine(PathToThis, "main.cpp");

            aEInitializing.WriteFileContents_FromCGENMMFile_ToFullPath(
                "AERTOS\\AEConfigProject",
                AEConfigProject,
                false, false,
                 new MacroVar() { MacroName = "TestNamesList", VariableValue = string.Join(" ", "") },
                 new MacroVar() { MacroName = "LibrariesIDependOn", VariableValue = string.Join(" ", "") },
                 new MacroVar() { MacroName = "ProjectName", VariableValue = string.Join(" ", "") },
                 new MacroVar() { MacroName = "ProjectDir", VariableValue = string.Join(" ", "") },
                 new MacroVar() { MacroName = "AnyAdditionalIncludeDirs", VariableValue = string.Join(" ", "") },
                 new MacroVar() { MacroName = "AnyAdditionalSRCDirs", VariableValue = string.Join(" ", "") },
                 new MacroVar() { MacroName = "DependsInit", VariableValue = string.Join(" ", "") }
                );

            if (File.Exists(AEConfigProjectUser) == false)
            { File.Create(AEConfigProjectUser); }

            if (File.Exists(IntegTestPipeline) == false)
            { File.Create(IntegTestPipeline); }

            aEInitializing.WriteFileContents_FromCGENMMFile_ToFullPath(
                "AERTOS\\main",
                main,
                false, false
                );


            PathToThis = Path.Combine(pathToProject, "include");
            //root/include======================================================
            if (Directory.Exists(PathToThis) == false)
            { Directory.CreateDirectory(PathToThis); }

            PathToThis = Path.Combine(pathToProject, "src");
            //root/src======================================================
            if (Directory.Exists(PathToThis) == false)
            { Directory.CreateDirectory(PathToThis); }


            PathToThis = Path.Combine(pathToProject, "conf");
            //root/conf======================================================
            //AEConfig.h            (blank)
            //EventsForProject.h    (blank)
            //EventsForProject.cpp  (blank)
            //UserBSPConfig.cpp   
            string AEConfig = Path.Combine(PathToThis, "AEConfig.h");
            string EventsForProjecth = Path.Combine(PathToThis, "EventsForProject.h");
            string EventsForProjectcpp = Path.Combine(PathToThis, "EventsForProject.cpp");
            string UserBSPConfig = Path.Combine(PathToThis, "UserBSPConfig.cpp");

            if (Directory.Exists(PathToThis) == false)
            { Directory.CreateDirectory(PathToThis); }

            if (File.Exists(AEConfig) == false)
            { File.Create(AEConfig); }

            if (File.Exists(EventsForProjecth) == false)
            { File.Create(EventsForProjecth); }

            if (File.Exists(EventsForProjectcpp) == false)
            { File.Create(EventsForProjectcpp); }

            aEInitializing.WriteFileContents_FromCGENMMFile_ToFullPath(
                "AERTOS\\UserBSPConfig",
                UserBSPConfig,
                false, false
                );

            PathToThis = Path.Combine(pathToProject, "CGensaveFiles");
            //root/CGensaveFiles======================================================
            //SavedOptions.txt (blank)
            if (Directory.Exists(PathToThis) == false)
            { Directory.CreateDirectory(PathToThis); }
            string SavedOptions = Path.Combine(PathToThis, "SavedOptions.txt");

            if (File.Exists(SavedOptions) == false)
            { File.Create(SavedOptions); }


            PathToThis = Path.Combine(pathToProject, "CGensaveFiles", "cmakeGui");
            //root/CGensaveFiles/cmakeGui------------------------
            if (Directory.Exists(PathToThis) == false)
            { Directory.CreateDirectory(PathToThis); }


            //root/CGensaveFiles/cmakeGui/{ProjectName}_{boardTarget}/Debug======================================================
            //cgenCmakeCache.cmake      (blank)
            //IntegrationTestMacros.h   
            List<string> tdepends = new List<string>();
            if (projAlreadyExists != null)
            {
                tdepends = projAlreadyExists.GetAllLibrariesIDependOnFlattenedSTR();
            }
            IntegrationMacroFileHandler integMacroFile = new IntegrationMacroFileHandler(opts.nameOfTheProject, tdepends, pathToProject, aEInitializing);
            integMacroFile.CreateAllIntegrationFilesTheFiles();

            foreach (var boardTarget in AEProject.ListOfBoardTargets)
            {
                PathToThis = Path.Combine(pathToProject, "CGensaveFiles", "cmakeGui", $"{opts.nameOfTheProject}_{boardTarget}", "DEBUG");

                string cgenCmakeCache = Path.Combine(PathToThis, "cgenCmakeCache.cmake");
                string IntegrationTestMacros = Path.Combine(PathToThis, "IntegrationTestMacros.h");
                if (Directory.Exists(PathToThis) == false)
                { Directory.CreateDirectory(PathToThis); }

                if (File.Exists(cgenCmakeCache) == false)
                { File.Create(cgenCmakeCache); }

            }

            return null;
        }



        static ParserResult<object> aeinit(aeinitOptions opts)
        {
            ProblemHandle prob = new ProblemHandle();

            //check if a project already exists here.
            AEProject projAlreadyExists = AEInitializing.GetProjectIfDirExists(envIronDirectory);
            if (opts.nameOfTheProject == null)
            {

                if (projAlreadyExists == null)
                {
                    prob.ThereisAProblem("You didnt provide a AEProject name and no project exists here yet. do that with \"aeinit <projectName>\"");
                }

                opts.nameOfTheProject = projAlreadyExists.Name;


            }
            else if (opts.nameOfTheProject != null)
            {
                if (projAlreadyExists != null)
                {
                    if (projAlreadyExists.Name != opts.nameOfTheProject)
                    {
                        prob.ThereisAProblem($"There already exists a project here of different name {projAlreadyExists.Name}");
                    }

                }

                //check that the current name is not already in use by another project at a different directory
                if (AEInitializing.GetProjectIfDirExists(opts.nameOfTheProject) != null)
                {
                    prob.ThereisAProblem($"There already exists a project of name {projAlreadyExists.Name}, at directory, {projAlreadyExists.DirectoryOfLibrary}. choose another name.");
                }
            }

            AEInitializing aEInitializing = new AEInitializing();


            if (projAlreadyExists == null)
            {

                //just use the relative directory if in base directory
                string basdir_ = AEProject.BaseAEDir.Replace("\\", "/");
                string envIronDirectory_ = envIronDirectory.Replace("\\", "/");
                //Console.WriteLine($"basdir_: {basdir_}");
                //Console.WriteLine($"envIronDirectory_: {envIronDirectory_}");
                //Console.WriteLine($"isSubDirOfPath(basdir_, envIronDirectory_): {isSubDirOfPath(basdir_, envIronDirectory_)}");
                string DirOfProject = isSubDirOfPath(basdir_, envIronDirectory_) ?
                    envIronDirectory_.Replace(basdir_ + "/", "") :
                    envIronDirectory_;



                string PathToconfcs = Path.Combine(envIronDirectory, "conf");

                Console.WriteLine($"creating { opts.nameOfTheProject}.cs at directory  {DirOfProject}");
                //create a .cs class file that will start the project type 
                aEInitializing.WriteFileContents_FromCGENMMFile_ToFullPath(
                    "AERTOS\\AEProjectCS",
                    Path.Combine(PathToconfcs, $"{opts.nameOfTheProject}.cs"),
                    false, false,
                     new MacroVar() { MacroName = "NameOfProject", VariableValue = opts.nameOfTheProject },
                     new MacroVar() { MacroName = "DirOfProject", VariableValue = DirOfProject }
                     );


            }

            Console.WriteLine($"initing { opts.nameOfTheProject}");
            _aeinitProjectFileStructure(opts, aEInitializing, projAlreadyExists, envIronDirectory);
            Console.WriteLine($"done initing { opts.nameOfTheProject}");

            return null;
        }

        #endregion



        #region aeselect command ***************************************************************************

        static string GetProjectsDisplay()
        {
            string disp = "";
            foreach (var proj in AEInitializing.GetAllCurrentAEProjects())
            {
                disp += "=============================================================="; disp += "\n";
                disp += $"ProjectName: {proj.Name}"; disp += "\n";
                disp += $"ProjectDirectory: {proj.DirectoryOfLibrary}"; disp += "\n";
                disp += $"ProjectTestsToChoose: "; disp += "\n";
                foreach (var test in proj.ListOfTests)
                {
                    disp += $"  {test}"; disp += "\n";
                }
            }

            return disp;
        }

        static string GetProjectTestsDisplay(AEProject proj)
        {
            string disp = "";
            disp += "=============================================================="; disp += "\n";
            disp += $"ProjectName: {proj.Name}"; disp += "\n";
            disp += $"ProjectDirectory: {proj.DirectoryOfLibrary}"; disp += "\n";
            disp += $"ProjectTestsToChoose: "; disp += "\n";
            foreach (var test in proj.ListOfTests)
            {
                disp += $"  {test}"; disp += "\n";
            }

            return disp;
        }

        static ParserResult<object> aeselect(aeselectOptions opts)
        {

            ProblemHandle prob = new ProblemHandle();

            //if projectName is null, return back a list of possible projects you can select
            if (opts.projectNameSelection == null)
            {
                string disp = "You did not provide projectNameSelection"; disp += "\n";
                disp += "Here is a list of projects to choose from"; disp += "\n";
                disp += GetProjectsDisplay();

                Console.WriteLine(disp);
                return null;
            }


            //projectNameSelection and is valid project provided but not projectSelected
            var projectSelected = AEInitializing.GetProjectIfNameExists(opts.projectNameSelection);
            if (opts.projectNameSelection == null && projectSelected != null)
            {
                string disp = $"{opts.projectNameSelection} is valid but no tests selected "; disp += "\n";
                disp += $"Here is a list of test from chosen project {opts.projectNameSelection}"; disp += "\n";
                disp += GetProjectTestsDisplay(projectSelected);
                Console.WriteLine(disp);
                return null;
            }

            //projectNameSelection provided but is NOT valid
            if (projectSelected == null)
            {
                string disp = $"No such project of name {opts.projectNameSelection} exists."; disp += "\n";
                disp += "Here is a list of projects to choose from"; disp += "\n";
                disp += GetProjectsDisplay();
                Console.WriteLine(disp);
                return null;
            }

            //projectNameSelection provided  and is valid but opts.projectEXETestSelection is NOT valid
            string TestSelected = projectSelected.ListOfTests.FirstOrDefault(s => s == opts.projectEXETestSelection);

            string dispp = opts.projectEXETestSelection == null ? "You did not provide projectEXETestSelection" : ""; dispp += "\n";
            if (projectSelected != null && (TestSelected == null || string.IsNullOrWhiteSpace(TestSelected)))
            {
                dispp += $"No such test of name {opts.projectEXETestSelection} exists for project named {opts.projectNameSelection} ."; dispp += "\n";
                dispp += $"Here is a list of test from chosen project {opts.projectNameSelection}"; dispp += "\n";
                dispp += GetProjectTestsDisplay(projectSelected);
                Console.WriteLine(dispp);
                return null;
            }
            opts.projectEXETestSelection = TestSelected;



            //everything is valid from here, start the process of changing the project chosen
            //step1: set the AETarget.cmake file
            //step2: set the IntegTestPipeline.h file 
            //step3: init the project just in case
            //step4: generate AEConfig TODO

            AEInitializing aEInitializing = new AEInitializing();

            //step1: set the AETarget.cmake file
            aEInitializing.WriteFileContents_FromCGENMMFile_ToFullPath(
                "AERTOS\\AETarget",
                Path.Combine(@"C:/AERTOS/AERTOS", $"AETarget.cmake"),//
                true, false,
                 new MacroVar() { MacroName = "ProjectName", VariableValue = projectSelected.Name },
                 new MacroVar() { MacroName = "ProjectDir", VariableValue = projectSelected.DirectoryOfLibrary },
                 new MacroVar() { MacroName = "TestChosen", VariableValue = TestSelected }
                 );

            //step2: set the IntegTestPipeline.h file
            aEInitializing.WriteFileContents_FromCGENMMFile_ToFullPath(
                "AERTOS\\IntegTestPipeline",
                Path.Combine(projectSelected.DirectoryOfLibrary, $"IntegTestPipeline.h"),
                true, false,
                 new MacroVar() { MacroName = "ProjectName", VariableValue = projectSelected.Name }
                 );



            //step3: init the project just in case
            aeinitOptions aeinitOptions = new aeinitOptions() { nameOfTheProject = projectSelected.Name };
            _aeinitProjectFileStructure(aeinitOptions, aEInitializing, projectSelected, projectSelected.DirectoryOfLibrary);


            //step4: generate AEConfig TODO


            return null;
        }


        #endregion



        #region aebuild command ***************************************************************************

        public static bool IsRunning(string name) => Process.GetProcessesByName(name).Length > 0;

        static ParserResult<object> aebuild(aebuildOptions opts)
        {

            ProblemHandle prob = new ProblemHandle();


            //string pathForProcessRunningFile = $"{PATHTOCMAKEGUI}/IsBuilding.txt";
            //var tt = Process.GetProcesses();

            //if (File.Exists(pathForProcessRunningFile) == false)
            //{
            //    File.Create(pathForProcessRunningFile);
            //}
            //var rr = File.ReadAllText(pathForProcessRunningFile);

            //if (rr == "Running")
            //{
            //    prob.ThereisAProblem($"There is already a build process running, wait for that to finish first!");
            //}


            string projectName = GetAEProjectName();
            string projectTestName = GetAEProjectTestName();


            string VISUALGDB_DIR = "";
            string PLAT_TOOLCHAIN = "";
            List<string> BuildTarget = new List<string>();

            //read all contents from the AEbuildinfo.txt
            string contents = File.ReadAllText($"{PATHTOCMAKEGUI}/AEbuildinfo.txt");

            var regex = new Regex(@"VISUALGDB_DIR==(.*)");
            Match match = regex.Match(contents);
            if (match.Success)
            {
                VISUALGDB_DIR = match.Groups[1].Value.Trim();//C:\PROGRA~2\Sysprogs\VISUAL~1/ninja.exe
            }

            regex = new Regex(@"PLAT_TOOLCHAIN==(.*)");
            match = regex.Match(contents);
            if (match.Success)
            {
                PLAT_TOOLCHAIN = match.Groups[1].Value.Trim();
            }

            regex = new Regex(@"BuildTarget==(.*)");
            var matches = regex.Matches(contents);
            if (match.Success)
            {
                foreach (Match m in matches)
                {
                    BuildTarget.Add(m.Groups[1].Value.Trim());
                }
            }


            //now that I have all into needed, I can build all targets in correct order.
            string buildDir = $"C:/AERTOS/AERTOS/build/AE_{PLAT_TOOLCHAIN}/Debug";

            //create to cmdHandlers. one will go into the build directory that will be exectured by visualgdb before
            //the build there. The other will go into the target directory for the reference of the user.
            CMDHandler cMDToBuildTargets = new CMDHandler(buildDir);
            CMDHandler cMDToBuildTargetsForUser = new CMDHandler(buildDir);
            cMDToBuildTargets.SetMultipleCommands($"cd {buildDir}");
            cMDToBuildTargetsForUser.SetMultipleCommands($"cd {buildDir}");




            foreach (var target in BuildTarget)
            {
                Console.WriteLine($"--------------------------------------");
                Console.WriteLine($"target to build: {target}");

                string VISUALGDB_DIR_PATH = PLAT_TOOLCHAIN == "mingw" ? "echo" : $"\"{VISUALGDB_DIR}\"";

                //File.WriteAllText(pathForProcessRunningFile, "Running");
                //cMDToBuildTargets.ExecuteCommand($"\"{VISUALGDB_DIR}\" {target}",false,false);// ("C:\PROGRA~2\Sysprogs\VISUAL~1/ninja.exe AECoreLib");
                cMDToBuildTargets.SetMultipleCommands($"{VISUALGDB_DIR_PATH} {target}");
                cMDToBuildTargetsForUser.SetMultipleCommands($"{VISUALGDB_DIR_PATH} {target}");
                cMDToBuildTargets.SetMultipleCommands($"echo built target {target}.press any key to build next target");
                cMDToBuildTargetsForUser.SetMultipleCommands($"echo built target {target}.press any key to build next target");

                //cMDToBuildTargets.ExecuteMultipleCommands_InSeperateProcess();


                //int numPolled = 0;
                //while (true)
                //{
                //    if (cMDToBuildTargets.IsProcessFinished())
                //    { 
                //        File.WriteAllText(pathForProcessRunningFile, "Done");
                //        break;
                //    }
                //    Thread.Sleep(1000);
                //    numPolled++;
                //    var tts = Process.GetProcesses();
                //    if (numPolled > 10)
                //    {
                //        //force kill the process, something went wrong.  
                //        cMDToBuildTargets.KillProcess();
                //        File.WriteAllText(pathForProcessRunningFile, "Done");
                //        prob.ThereisAProblem($"Something went wrong when building target {target}. Timed out");

                //    }
                //}
                var tta = Process.GetProcesses();


                //Console.WriteLine($"output: {cMDToBuildTargets.Output}");
                //string errorout = $"{ cMDToBuildTargets.Error }" == "" ? "" : $"Error: {cMDToBuildTargets.Error}";
                //Console.WriteLine($"{errorout}");
                //Console.WriteLine($"Done {target}");
                //Console.WriteLine($"--------------------------------------\n\n");
            }

            cMDToBuildTargetsForUser.SetMultipleCommands("pause");
            string projectDir = AEInitializing.GetRunningDirectoryFromProjectName(projectName);
            cMDToBuildTargets.ExecuteMultipleCommands_InItsOwnBatch(buildDir, "AEBuildProject");
            cMDToBuildTargetsForUser.ExecuteMultipleCommands_InItsOwnBatch(projectDir, "AEBuildProject");

            return null;
        }

        #endregion



        #region aegenerate command ***************************************************************************

        static ParserResult<object> aegenerate(aegenerateOptions opts)
        {
            ProblemHandle prob = new ProblemHandle();

            //grab the currently selected
            //set(INTEGRATION_TESTS AESamples)
            //add_compile_definitions(INTEGRATION_TESTS__${ INTEGRATION_TESTS})
            //add_compile_definitions(INTEGRATION_TEST_CHOSEN = "AESamples")
            //set(INTEGRATION_TESTS_FOR_AESamples SPBSamples)

             

            string ProjectName = GetAEProjectName();
            string ProjectTest = GetAEProjectTestName();



            //generate the project.
            AEInitializing aEInitializing = new AEInitializing();
            var projectSelected = AEInitializing.GetProjectIfNameExists(ProjectName);
            aeinitOptions aeinitOptions = new aeinitOptions() { nameOfTheProject = ProjectName };
            _aeinitProjectFileStructure(aeinitOptions, aEInitializing, projectSelected, projectSelected.DirectoryOfLibrary);

            aEInitializing.GenerateProject(ProjectName, ProjectTest);




            return null;
        }

        #endregion


        #region aeserial command ***************************************************************************

        static ParserResult<object> aeserial(aeserialOptions opts)
        {
            ProblemHandle prob = new ProblemHandle();

            string pathToconfexe = Path.Combine(CodeGenerator.Program.PATHTO_SERIAL_UTILITY, "bin", "Debug");//, "HolterMonitorGui.exe");
            CMDHandler cMDHandler = new CMDHandler(pathToconfexe);

            cMDHandler.ExecuteCommand("HolterMonitorGui.exe");



            return null;
        }

        #endregion



        #region CmakeConfig command ***************************************************************************
        //***************************************************************************************************  


        //step 1:
        //Dir_Step1.txt:
        //main CMakeLists.txt will call the cgen_start() function. this function will write to
        //${CMAKE_CURRENT_SOURCE_DIR}/CGensaveFiles/Dir_StepOne.txt
        // it will write the build directory ${CMAKE_CURRENT_BINARY_DIR}

        //step 2:
        //Dir_Step2.txt:
        //cgen cmakegui command will be called which will read the Dir_Step1.txt file, and forward that information into 
        //the Dir_Step2.txt which will be located at this CgenCmakeGui project directory. information forwared will be the 
        // ${CMAKE_CURRENT_BINARY_DIR} and the ${CMAKE_CURRENT_SOURCE_DIR}

        //directory of cgencmakegui project
        string cgencmakeguiProj = "";

        static ParserResult<object> cmakegui(cmakeguiOptions opts)
        {
            string ggg = DIRECTORYOFTHISCG;



            string environcgensaveFile = Path.Combine(envIronDirectory, "CGensaveFiles", "Dir_Step1.txt");
            if (Directory.Exists(Path.GetDirectoryName(environcgensaveFile)) == false)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(environcgensaveFile));
            }
            if (File.Exists(environcgensaveFile) == false)
            {
                File.Create(environcgensaveFile);
            }

            Thread.Sleep(500);
            if (File.Exists(environcgensaveFile) == false)
            {
                File.Create(environcgensaveFile);
            }

            //read all contents of the Dir_Step1 in the current working directory
            string Dir_Step1Contents = File.ReadAllText(envIronDirectory + "/CGensaveFiles/Dir_Step1.txt");

            //forward this content to the PATHTOCMAKEGUI directory
            File.WriteAllText(PATHTOCMAKEGUI + "/Dir_Step2.txt", Dir_Step1Contents);


            return null;
        }

        #endregion


        #region post_compile command ***************************************************************************
        //***************************************************************************************************  
        //this is meant to compile my own custom keywords that will change the code. The keywords must follow some rules.
        //1. They must not change any of the code not generated by this cgen command. This means any code that is from the user.
        //2. the generated code from the command must be identifiably done once. This means, once the keyword is compiled, it can
        //      be checked that the post_compile command has already compiled that keyword.


        static ParserResult<object> post_compile(post_compileOptions opts)
        {
            string ggg = DIRECTORYOFTHISCG;

            // get all intermediary files .ii
            var intermediaryFiles = Directory.GetFiles(envIronDirectory).Where(f => Path.GetExtension(f) == ".ii").ToList();
            foreach (var inter in intermediaryFiles)
            {

            }





            return null;
        }

        #endregion


#if NOTDEPRECATED_GENERATE_AND_DEGENERATE

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
                    projectBuilderForVs.ImportConfigFiles(opts.git);



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
                    p.ThereisAProblem("\n you are building for a platform " + projectBuilderForVs.GetPlatFormThisisSetupFor() + " as stated in your mainCG.cpp file, but " + libraryNameNotSupportingPlat + " does not support the platform you are building for.  \n use cgen configproj -a <nameofScope> \n to add platform to that project scope.");
                }
                configFileBuilder.WriteTempConfigurationToFinalFile();


                //2.  add filters and folders directories from that toplevel project directory as:
                //LibraryDependencies
                //  prefix(of libraries top level uses)
                //      confTypePrefix(for libraries that are same but different template type.)    
                projectBuilderForVs.RecreateLibraryDependenciesFoldersFilters();

                //3. I need to go through each library, git checkout their correct major. (master should be the branch with tag name of major)
                //first grab all lowest level libraries that have no dependencies
                projectBuilderForVs.ImportDependentLibrariesFiles(opts.git);



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
#endif




        #region QRInit command ***************************************************************************
        //***************************************************************************************************  

        // copy directory from cpp template to new directory
        // -delete the git directory, build, install directories
        //-change the module name in CMakeLists.txt, while keeping the old name in memory
        //- change the include directory name to ${MODULE_NAME}_cp.
        //- go through all files in src and include/${MODULE_NAME}_cp
        //replace all instances of the old module name that you find
        //-ourcolcon
        //- make sure everything built



        //-cd rqt
        //- change the include directory name to ${MODULE_NAME}_rqt.
        //- go through all files in src and include/${MODULE_NAME}_rqt
        //    replace all instances of the old module name that you find
        //- delete the install_win and install_lin folders

        //-cd IF
        //- delete the install_win and install_lin folders


        //finally time to build and source everything. do this all in one bash file to keep environment
        //variables

        //- call c:\opt\ros\foxy\x64\setup.bat
        //- cd base directory of your module
        //- call ourcolcon
        //- call oursource

        //- cd rqt/IF
        //- call ourcolcon
        //- call oursource

        //- cd ..
        //- call ourcolcon
        //- call oursource


        private static void CloneDirectory(string root, string dest, List<string> neglectedDirs = null)
        {
            if (neglectedDirs == null)
            {
                neglectedDirs = new List<string>();
            }

            foreach (string directory in Directory.GetDirectories(root))
            {
                if (!neglectedDirs.Contains(new DirectoryInfo(directory).Name))
                {
                    string dirName = Path.GetFileName(directory);
                    if (!Directory.Exists(Path.Combine(dest, dirName)))
                    {
                        Directory.CreateDirectory(Path.Combine(dest, dirName));
                    }
                    CloneDirectory(directory, Path.Combine(dest, dirName));
                }
            }

            foreach (var file in Directory.GetFiles(root))
            {
                File.Copy(file, Path.Combine(dest, Path.GetFileName(file)));
            }
        }

        private static void SetAttributesNormal(DirectoryInfo dir)
        {

            foreach (var subDir in dir.GetDirectories())
                SetAttributesNormal(subDir);
            foreach (var file in dir.GetFiles())
            {
                file.Attributes = FileAttributes.Normal;
            }
        }


        private static void DeleteDirectoryIfExists_RemoveReadonly(string dir)
        {

            if (Directory.Exists(dir))
            {
                //remove readonly crap
                SetAttributesNormal(new System.IO.DirectoryInfo(dir));

                //delete directory recursively
                Directory.Delete(dir, true);
            }

        }


        private static void CreateQTCreatorOpenBatch(CMDHandlerVSDev cmdvs, string pathToBaseMod)
        {
            cmdvs.SetMultipleCommands(@"call c:\opt\ros\foxy\x64\setup.bat");
            cmdvs.SetMultipleCommands(@"cd " + pathToBaseMod);
            cmdvs.SetMultipleCommands("call oursource");
            cmdvs.SetMultipleCommands(@"C:\Qt\Tools\QtCreator\bin\qtcreator.exe");
            cmdvs.ExecuteMultipleCommands_InItsOwnBatch(pathToBaseMod, "OpenQTCreatorHere");
        }



        private static void PromptAQuestionToContinue(string q, Action whatToDoIfNo = null)
        {
            do
            {
                Console.WriteLine(q + " \n  \'y\' or \'n\'");
                var mm = Console.ReadKey();
                if (mm.KeyChar == 'n' || mm.KeyChar == 'N')
                {
                    if (whatToDoIfNo != null)
                    {
                        whatToDoIfNo();
                    }
                    return;
                }
                else if (mm.KeyChar == 'y' || mm.KeyChar == 'Y')
                {
                    break;
                }
            } while (true);

        }



        static ParserResult<object> QRInit(QRInitOptions opts)
        {
            Console.WriteLine(envIronDirectory);



            //if they didnt provide a name for the module
            if (opts.name == null)
            {
                ProblemHandle p = new ProblemHandle();
                p.ThereisAProblem("you did not provide a name for the module. do that with QRinit <name>");
                return null;
            }

            string pathToBaseMod = $"{envIronDirectory}\\{opts.name}";




            //if they tried to init the project in a directory that already exists
            if (Directory.Exists($"{envIronDirectory}\\{opts.name}") && (opts.isToRename == false))
            {
                ProblemHandle p = new ProblemHandle();
                p.ThereisAProblem($"you tried to initialize a module named {opts.name} that already exists");
                return null;
            }

            if (opts.isToRename == false)
            {

                //----------------------------------------------------------------------------------------
                Console.WriteLine("---cloning base template from the QR_sync/cpp_template directory");
                CloneDirectory(@"C:/QR_sync/cpp_template", $"{envIronDirectory}\\{opts.name}", new List<string>() { @".git" });
                Console.WriteLine("---finished cloning");

                Console.WriteLine("---deleting the git repo.");
                DeleteDirectoryIfExists_RemoveReadonly(pathToBaseMod + @"/.git");

            }


            //----------------------------------------------------------------------------------------
            Console.WriteLine("---deleting build and install directories");
            DeleteDirectoryIfExists_RemoveReadonly(pathToBaseMod + @"/build");
            DeleteDirectoryIfExists_RemoveReadonly(pathToBaseMod + @"/install_win");
            DeleteDirectoryIfExists_RemoveReadonly(pathToBaseMod + @"/install_lin");
            DeleteDirectoryIfExists_RemoveReadonly(pathToBaseMod + @"/log");


            //----------------------------------------------------------------------------------------
            Console.WriteLine("---changing the module name in CMakeLists.txt and in the config/module_name.cmake, while keeping the old name in memory");
            //get old name
            string contents = File.ReadAllText(pathToBaseMod + @"/CMakeLists.txt");
            string contents2 = File.ReadAllText(pathToBaseMod + @"/config/module_name.cmake");
            var maches = Regex.Matches(contents, @"QR_module\((.*)\)");
            string oldName = "qwertasd_module";//maches[0].Groups[1].Value;
            string contentsReplace = Regex.Replace(contents, oldName, opts.name);
            string contentsReplace2 = Regex.Replace(contents2, oldName, opts.name);
            File.WriteAllText(pathToBaseMod + @"/CMakeLists.txt", contentsReplace);
            File.WriteAllText(pathToBaseMod + @"/config/module_name.cmake", contentsReplace2);

            Console.WriteLine("---going through all files package.xml files replacing all instances of the old module name that you find");
            var fileIncpp = pathToBaseMod + @"/package.xml";
            var fileInrqt = pathToBaseMod + @"/rosqt/package.xml";
            var fileInIF = pathToBaseMod + @"/rosqt/IF/package.xml";
            List<string> allFilesToChangexml = new List<string>();
            allFilesToChangexml.Add(fileIncpp); allFilesToChangexml.Add(fileInrqt); allFilesToChangexml.Add(fileInIF);
            foreach (var filetochange in allFilesToChangexml)
            {
                Console.WriteLine($"    changing all occurences of {oldName} with {opts.name} in file {filetochange}");
                contents = File.ReadAllText(filetochange);
                string contentrp = Regex.Replace(contents, oldName, opts.name);
                File.WriteAllText(filetochange, contentrp);
            }



            //----------------------------------------------------------------------------------------
            Console.WriteLine("---change the include directory name to ${MODULE_NAME}_cp.");
            if (!Directory.Exists(pathToBaseMod + @"/include/" + oldName + "_cp"))
            {
                Console.WriteLine("WARNING: could not find directory include/" + oldName + "");
            }
            else
            {
                Directory.Move(pathToBaseMod + @"/include/" + oldName + "_cp", pathToBaseMod + @"/include/" + opts.name + "_cp");
            }


            //----------------------------------------------------------------------------------------
            //-cd IF 
            Console.WriteLine("deleting the build, install_win, and install_lin folders in the IF folder");
            DeleteDirectoryIfExists_RemoveReadonly(pathToBaseMod + @"/rosqt/IF/build");
            DeleteDirectoryIfExists_RemoveReadonly(pathToBaseMod + @"/rosqt/IF/install_win");
            DeleteDirectoryIfExists_RemoveReadonly(pathToBaseMod + @"/rosqt/IF/install_lin");
            DeleteDirectoryIfExists_RemoveReadonly(pathToBaseMod + @"/rosqt/IF/log");

            Console.WriteLine("build the interface project. this is the first one that is built because everthing in this module depends on this one");
            CMDHandlerVSDev cmdvs = new CMDHandlerVSDev(pathToBaseMod, pathToBaseMod);
            cmdvs.SetMultipleCommands(@"call c:\opt\ros\foxy\x64\setup.bat");
            cmdvs.SetMultipleCommands(@"cd C:\QR_sync\QR_core");
            cmdvs.SetMultipleCommands("call oursource");
            //go to the IF directory to build
            cmdvs.SetMultipleCommands(@"cd " + pathToBaseMod + "/rosqt/IF");
            //build the interface project
            cmdvs.SetMultipleCommands(@"call ourcolcon");
            //source the Interface project
            cmdvs.SetMultipleCommands(@"call oursource");
            cmdvs.SetMultipleCommands("echo Press any key to exit . . .");
            cmdvs.SetMultipleCommands(@"pause>nul");


            cmdvs.ExecuteMultipleCommands_InSeperateProcess();


            PromptAQuestionToContinue("did it colcon build right?", () =>
            {
                //delete dir
                DeleteDirectoryIfExists_RemoveReadonly($"{envIronDirectory}\\{opts.name}");
                System.Environment.Exit(1);
            });

            //----------------------------------------------------------------------------------------
            Console.WriteLine("---going through all files in src and include /${ MODULE_NAME}_cp and" +
                " replacing all instances of the old module name that you find");
            var filesInSrc = Directory.GetFiles(pathToBaseMod + "/src").ToList();
            var filesInInc = Directory.GetFiles(pathToBaseMod + "/include/" + opts.name + "_cp").ToList();
            var filesInSrcUnit = Directory.GetFiles(pathToBaseMod + "/unit_tests/src").ToList();
            var filesInIncUnit = Directory.GetFiles(pathToBaseMod + "/unit_tests/include").ToList();
            List<string> allFilesToChange = new List<string>();
            allFilesToChange.AddRange(filesInSrc); allFilesToChange.AddRange(filesInInc); allFilesToChange.AddRange(filesInSrcUnit); allFilesToChange.AddRange(filesInIncUnit);
            foreach (var filetochange in allFilesToChange)
            {
                Console.WriteLine($"    changing all occurences of {oldName} with {opts.name} in file {filetochange}");
                contents = File.ReadAllText(filetochange);
                string contentrp = Regex.Replace(contents, oldName, opts.name);
                File.WriteAllText(filetochange, contentrp);
            }


            //----------------------------------------------------------------------------------------
            Console.WriteLine("--- running ourcolcon for the cpp project portion of your module");

            //first source QR_core
            cmdvs.SetMultipleCommands(@"call c:\opt\ros\foxy\x64\setup.bat");
            cmdvs.SetMultipleCommands(@"cd C:\QR_sync\QR_core");
            cmdvs.SetMultipleCommands("call oursource");
            cmdvs.SetMultipleCommands(@"cd " + pathToBaseMod + "/rosqt/IF");
            //source the Interface project
            cmdvs.SetMultipleCommands(@"call oursource");
            cmdvs.SetMultipleCommands(@"cd " + pathToBaseMod);
            cmdvs.SetMultipleCommands("call ourcolcon");
            cmdvs.SetMultipleCommands("call oursource");
            cmdvs.SetMultipleCommands("echo Press any key to exit . . .");
            cmdvs.SetMultipleCommands(@"pause>nul");


            //cmdvs.SetMultipleCommands("call oursource.bat");
            //cmdvs.SetMultipleCommands(@"cd ../"+opts.name);
            //cmdvs.SetMultipleCommands("call ourcolcon.bat");
            cmdvs.ExecuteMultipleCommands_InSeperateProcess();



            PromptAQuestionToContinue("did it colcon build right?", () =>
            {
                //delete dir
                DeleteDirectoryIfExists_RemoveReadonly($"{envIronDirectory}\\{opts.name}");
                System.Environment.Exit(1);
            });


            //creating batch file for opening up qt creator with cp sourced
            CreateQTCreatorOpenBatch(cmdvs, pathToBaseMod);


            Console.WriteLine("\n\n--- done running ourcolcon fpr cpp project");

            //----------------------------------------------------------------------------------------
            Console.WriteLine("--- switching to the rosqt portion of your module");
            pathToBaseMod = pathToBaseMod + "/rosqt";


            //----------------------------------------------------------------------------------------
            Console.WriteLine("--- change the include directory name to ${MODULE_NAME}_rqt.");
            if (!Directory.Exists(pathToBaseMod + "/include/" + oldName + "_cp"))
            {
                Console.WriteLine("WARNING: could not find directory rosqt/include/" + oldName + "");
            }
            else
            {
                Directory.Move(pathToBaseMod + @"include/" + oldName + "_cp", pathToBaseMod + @"include/" + opts.name + "_cp");
            }


            //----------------------------------------------------------------------------------------
            Console.WriteLine("---deleting build and install directories");
            DeleteDirectoryIfExists_RemoveReadonly(pathToBaseMod + @"/build");
            DeleteDirectoryIfExists_RemoveReadonly(pathToBaseMod + @"/install_win");
            DeleteDirectoryIfExists_RemoveReadonly(pathToBaseMod + @"/install_lin");
            DeleteDirectoryIfExists_RemoveReadonly(pathToBaseMod + @"/log");

            //----------------------------------------------------------------------------------------
            Console.WriteLine("---changing the module name in CMakeLists.txt, ");
            contents = File.ReadAllText(pathToBaseMod + @"/CMakeLists.txt");
            contentsReplace = Regex.Replace(contents, oldName, opts.name);
            File.WriteAllText(pathToBaseMod + @"/CMakeLists.txt", contentsReplace);



            //----------------------------------------------------------------------------------------
            Console.WriteLine("---change the include directory name to ${MODULE_NAME}_rqt.");
            if (!Directory.Exists(pathToBaseMod + @"/include/" + oldName + "_rqt"))
            {
                Console.WriteLine("WARNING: could not find directory include/" + oldName + "");
            }
            else
            {
                Directory.Move(pathToBaseMod + @"/include/" + oldName + "_rqt", pathToBaseMod + @"/include/" + opts.name + "_rqt");
            }


            //----------------------------------------------------------------------------------------
            Console.WriteLine("---going through all files in src and include /${ MODULE_NAME}_cp and" +
                " replacing all instances of the old module name that you find");
            filesInSrc = Directory.GetFiles(pathToBaseMod + "/src").ToList();
            filesInInc = Directory.GetFiles(pathToBaseMod + "/include/" + opts.name + "_rqt").ToList();
            allFilesToChange = new List<string>(); allFilesToChange.AddRange(filesInSrc); allFilesToChange.AddRange(filesInInc);
            foreach (var filetochange in allFilesToChange)
            {
                Console.WriteLine($"    changing all occurences of {oldName} with {opts.name} in file {filetochange}");
                contents = File.ReadAllText(filetochange);
                string contentrp = Regex.Replace(contents, oldName, opts.name);
                File.WriteAllText(filetochange, contentrp);
            }





            //----------------------------------------------------------------------------------------
            Console.WriteLine("--- finally time to build and source everything. do this all in one bash file to keep environment");
            cmdvs.SetWorkingDirectory(pathToBaseMod);
            cmdvs.SetMultipleCommands(@"call c:\opt\ros\foxy\x64\setup.bat");
            cmdvs.SetMultipleCommands(@"cd C:\QR_sync\QR_core");
            cmdvs.SetMultipleCommands("call oursource");
            //go to the IF directory to build
            cmdvs.SetMultipleCommands(@"cd " + pathToBaseMod + "/IF");
            //source the Interface project
            cmdvs.SetMultipleCommands(@"call oursource");
            //go back to the cpp project
            cmdvs.SetMultipleCommands(@"cd ../..");
            //just source this one
            cmdvs.SetMultipleCommands(@"call oursource");
            //go to the rosqt again
            cmdvs.SetMultipleCommands(@"cd rosqt");
            //build and source this one
            cmdvs.SetMultipleCommands(@"call ourcolcon");
            cmdvs.SetMultipleCommands(@"call oursource");
            cmdvs.SetMultipleCommands("echo Press any key to exit . . .");
            cmdvs.SetMultipleCommands(@"pause>nul");

            cmdvs.ExecuteMultipleCommands_InSeperateProcess();

            PromptAQuestionToContinue("did it colcon build right?", () =>
            {
                //delete dir
                DeleteDirectoryIfExists_RemoveReadonly($"{envIronDirectory}\\{opts.name}");
                System.Environment.Exit(1);
            });


            CreateQTCreatorOpenBatch(cmdvs, pathToBaseMod);

            Console.WriteLine("\n\n--- done running ourcolcon for rosqt");


            Console.WriteLine("\n\n--- finished initializing everything. wanna open qt creator for the cp project?");
            return null;
        }
        #endregion













        #region helper static functions  ***************************************************************************
        //***************************************************************************************************  




        public static bool isSubDirOfPath(string ParentDir, string SubDir)
        {
            DirectoryInfo di1 = new DirectoryInfo(ParentDir);
            DirectoryInfo di2 = new DirectoryInfo(SubDir);
            bool isParent = false;
            while (di2.Parent != null)
            {
                if (di2.Parent.FullName == di1.FullName)
                {
                    isParent = true;
                    break;
                }
                else di2 = di2.Parent;
            }

            return isParent;
        }


        public static string GetAEProjectName()
        {
            string AETargetContents = File.ReadAllText(@"C:/AERTOS/AERTOS/AETarget.cmake");

            Regex re = new Regex(@"\s*set\(\s*INTEGRATION_TESTS\s+(?<ProjectName>.*)\s*\)\s*\n");
            string ProjectName = re.Match(AETargetContents).Groups["ProjectName"].Value;

            return ProjectName;
        }
        public static string GetAEProjectTestName()
        {
            string AETargetContents = File.ReadAllText(@"C:/AERTOS/AERTOS/AETarget.cmake");

            string ProjectName = GetAEProjectName();
            Regex re2 = new Regex(@"\s*set\(\s*INTEGRATION_TESTS_FOR_" + ProjectName + @"\s+(?<ProjectTest>.*)\s*\)\s*\n?");
            string ProjectTest = re2.Match(AETargetContents).Groups["ProjectTest"].Value;

            return ProjectTest;
        }

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

        #if NOTDEPRECATED_GENERATE_AND_DEGENERATE
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
#endif

#endregion



    }


}
