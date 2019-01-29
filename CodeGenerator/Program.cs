#define TESTING 

using System;
using System.Collections.Generic;
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

namespace CodeGenerator
{




    public class Program
    {



        //Verbs help delineate and separate options and values for multiple commands within a single app
        [Verb("generate", HelpText = "generate the code")]
        public class GenerateOptions
        {
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

        }

        [Verb("degenerate", HelpText = "degenerate the code")]
        public class DegenerateOptions
        {

            [Option('r', Separator = ' ')]
            public IEnumerable<string> IncludeFiles { get; set; }

        }

        [Verb("init", HelpText = "create a new CG project")]
        public class InitOptions
        {
            //[CommandLine.Value(1), help]
            [Value(0, HelpText = "name of the project you want to create")]
            public string name { get; set; }


        }

        [Verb("config", HelpText = "Configure Code Generator")]
        public class ConfigOptions
        {
            //[CommandLine.Value(1), help]
            [Option(HelpText = "Directory of the Config")]
            public string directoryofconfig { get; set; }


        }


        public static string CGProjectBaseDirectory = "CGensaveFiles";
#if TESTING
        public static string envIronDirectory = @"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\Module1AA";//
        //public static string envIronDirectory =   @"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\Module1A";//

#else
        public static string envIronDirectory = Environment.CurrentDirectory;
#endif
        public static SaveFilecgenProject savefileProjLocal = new SaveFilecgenProject();
        public static SaveFilecgenConfig saveFilecgenConfigLocal = new SaveFilecgenConfig();
        public static ProjectBuilderVS projectBuilderForVs;

        static string[] command;


        static void Main(string[] args)
        {

            //string e = Environment.CurrentDirectory;
            //Console.WriteLine(e);
            //ProjectVSTest();
            CreateProjectBuilder();

            Action RunParser = () =>
            {
                Parser.Default.ParseArguments<GenerateOptions, DegenerateOptions, InitOptions, ConfigOptions>(command)
.WithParsed<GenerateOptions>(opts => Generate(opts))
.WithParsed<DegenerateOptions>(opts => Degenerate(opts))
.WithParsed<InitOptions>(opts => Init(opts))
.WithParsed<ConfigOptions>(opts => Config(opts));
            };

#if !TESTING
            command = args;
              
            if (command.Count() != 0)
#else

            command = "generate -r fiile.txt oubnfe.tct --aienabled=true".Split(' '); //values should be called LOWER CASED
            //command = "degenerate -r fiile.txt oubnfe.tct ".Split(' ');
            //command = "init moda".Split(' ');
            //command = ("config --directoryofconfig " +  @"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\ConfigTest").Split(' ');
            //command = "".Split(' ');
            //command = "generate".Split(' ');
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
                    SaveFilecgenProject savefileAtDir = new SaveFilecgenProject(envIronDirectory + "\\" + CGProjectBaseDirectory);

                    var projHere = savefileProjLocal.CgenProjects.Projects.Where((cgenProject cgenproj) => cgenproj.UniqueIdentifier == savefileAtDir.CgenProjects.Projects.FirstOrDefault().UniqueIdentifier).First();
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



        static bool IsProjectExistsAtEnvironDirectory()
        {
            if (Directory.Exists(envIronDirectory + "\\" + CGProjectBaseDirectory))
            {
                return true;
            }
            return false;
        }

        static SaveFilecgenProject GetSaveFilecgenProjectAtEnvironDirectory()
        {
            if (!IsProjectExistsAtEnvironDirectory())
            {
                throw new Exception("there is no such project here yet");
            }

            return (new SaveFilecgenProject(envIronDirectory + "\\" + CGProjectBaseDirectory));

        }


        static ParserResult<object> Config(ConfigOptions opts)
        {
            //if there is no cgenXMLSaves directory than this must be the first config run. build all the
            //config files needed here.
            if (!Directory.Exists(CGProjectBaseDirectory))
            {
                Directory.CreateDirectory(CGProjectBaseDirectory);
            }

            if (opts.directoryofconfig != null || opts.directoryofconfig != "")
            {
                saveFilecgenConfigLocal.CgenConfig.DirectoryOfConfig = opts.directoryofconfig;
                saveFilecgenConfigLocal.Save();
            }


            return null;
        }



        static ParserResult<object> Init(InitOptions opts)
        {


            //get the project builder for Visual Studio, as VS is right now the only supporting starting project build
            //IDESetting settingConfig = new IDESetting(@"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\ConfigTest", ".xml", typeof(Root));
            //ProjectBuilderBase projectBuilderForVs = new ProjectBuilderVS(settingConfig);


            //check that there isnt already a project here.  
            if (IsProjectExistsAtEnvironDirectory())
            {
                projectBuilderForVs.RecreateConfigurationFilterFolderIncludes(GetSaveFilecgenProjectAtEnvironDirectory().CgenProjects.Projects.First().NameOfProject, saveFilecgenConfigLocal.CgenConfig.DirectoryOfConfig);
                Console.WriteLine(GetSaveFilecgenProjectAtEnvironDirectory().CgenProjects.Projects.First().NameOfProject + " already exists as a project here");
                return null;
            }
            else if (opts.name == null || opts.name == "")
            {
                Console.WriteLine("no project exists here");
            }
            else
            {
                //todo say that you can not put numbers in the name!! 
                if (Regex.IsMatch(opts.name, @"\d"))
                {
                    Console.WriteLine("you cant have a number in the name of the project");
                    return null;
                }


                //then I need to create a new project as none exist and user wants to create one 
                try
                {
                    Directory.CreateDirectory(envIronDirectory + "\\" + CGProjectBaseDirectory);
                    //CREATE ALL THE FILES THAT ARE NEEDED FOR PROJECT INITIATION!
                    // creation of the SaveFilecgenProject file
                    SaveFilecgenProject saveFilecgenProject = new SaveFilecgenProject(envIronDirectory + "\\" + CGProjectBaseDirectory);
                    cgenProjects projs = saveFilecgenProject.CgenProjects;
                    cgenProject projToAdd = new cgenProject();
                    projToAdd.NameOfProject = opts.name;
                    projToAdd.PathOfProject = envIronDirectory + "\\" + CGProjectBaseDirectory;
                    projToAdd.UniqueIdentifier = cgenXMLMemeberCreationHelper.UniqueIdentifierCreator();
                    //also add the project to the localProjSettings
                    savefileProjLocal.CgenProjects.Projects.Add(projToAdd);
                    projs.Projects.Add(projToAdd);

                    //save created files
                    saveFilecgenProject.Save();
                    savefileProjLocal.Save();

                    projectBuilderForVs.RecreateConfigurationFilterFolderIncludes(opts.name, saveFilecgenConfigLocal.CgenConfig.DirectoryOfConfig);

                    Console.WriteLine("Project successfully created");

                }
                catch (Exception)
                {
                    Console.WriteLine("There was a problem with project creation");
                }
                // so we have a name and no project exists here. create one 


            }


            return null;
        }




        private static void GetEnvironmentVariables(ProcessStartInfo processInfo)
        {

        }

        static ParserResult<object> Generate(GenerateOptions opts)
        {

            CLCommandBuilder cl = new CLCommandBuilder("configGen");
            //get all .cpp files that are in the top level library and are only in the Config Filter
            var ccompiles = projectBuilderForVs.LibTop.GetAllCCompile().GetCCompilesFromFilter("Config");

            string ss = cl.GetCompileCommand();

            CMDHandler cmdHandler = new CMDHandler(CMDTYPE.VS);
            string buildCommand = @"cl    /EHsc /MDd   /Fo""Debug\Dd""\ /I ""C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\ConfigTest"" /I ""C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\Module1AA"" main.cpp   /link ConfigTest.lib   /libpath:""C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\ConfigTest""    /out:Debug/Dd/configGen.exe";
            cmdHandler.SetWorkingDirectory(@"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\Module1A");
            cmdHandler.ExecuteCommand(buildCommand);
            /* 
            ExecuteCommand("GetVSEnironmentVariables.bat");
            //change directories from processInfo.workingdireectory
            //cl    /EHsc /MDd   /Fo"Debug\Dd"\ /I “C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\ConfigTest” /I “C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\Module1AA” main.cpp   /link ConfigTest.lib   /libpath:"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\ConfigTest"    /out:Debug/Dd/configGen.exe
            string buildCommand = @"cl    /EHsc /MDd   /Fo""Debug\Dd""\ /I ""C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\ConfigTest"" /I ""C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\Module1AA"" main.cpp   /link ConfigTest.lib   /libpath:""C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\ConfigTest""    /out:Debug/Dd/configGen.exe";
            string cdd = @"cd C:\Users\Hadi";
            string g = @"cd C:\Program Files (x86)\Microsoft Visual Studio 14.0\VC";
                       //@"cd C:\Program Files (x86)\Microsoft Visual Studio 14.0\VC"
            string StartCLBat = @"CALL ""C:\Program Files (x86)\Microsoft Visual Studio 14.0\VC\vcvarsall.bat"" x86 " ;
            //generate the batch commands to create the configtest.exe to get the config files.
            ExecuteCommand("echo testing");
            //ExecuteCommand(g);
            //ExecuteCommand("vcvarsall.bat  x86");
            //ExecuteCommand(StartCLBat);
            //ExecuteCommand("cl");
            //ExecuteCommand(@"cd C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\Module1A");
            ExecuteCommand(buildCommand);
            */
            Console.WriteLine(string.Join(" ", command));

            foreach (var item in opts.IncludeFiles)
            {
                Console.WriteLine(item);
            }
            /*if (o.Verbose)
            {
                Console.WriteLine($"Verbose output enabled. Current Arguments: -v {o.Verbose}");
                Console.WriteLine("Quick Start Example! App is in Verbose mode!");
            }
            else
            {
                Console.WriteLine($"Current Arguments: -v {o.Verbose}");
                Console.WriteLine("Quick Start Example!");
            }*/
            return null;
        }

        static ParserResult<object> Degenerate(DegenerateOptions opts)
        {



            Console.WriteLine(string.Join(" ", command));

            foreach (var item in opts.IncludeFiles)
            {
                Console.WriteLine(item);
            }


            return null;
        }

        public static void CreateProjectBuilder()
        {

            //DONT WORRY ABOUT LIBRARIES THAT DEPEND ON LIBRARIES FOR NOW JUST GET THIS MUCH TO WORK.
            // steps to import a new library would be
            //1. get the libraries config xmlclasses created. (located in configTest/savedData.xml)
            IDESetting settingConfig = new IDESetting(@"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\ConfigTest", ".xml", typeof(Root));


            //2: get the libraries settings xml file (they will be in the same directory as the config 
            //file path )converted to the settingxml   
            
            projectBuilderForVs = new ProjectBuilderVS(settingConfig);
            
        }

        static void ProjectVSTest()
        {


            //3. determine who is top level. use its filtersettingxml for that and create new 
            //directories from that toplevel project directory as:
            //LibraryDependencies
            //  prefix(of libraries top level uses)
            //      confTypePrefix(for libraries that are same but different template type.)  
            //get the top level library
            List<Library> libraries = projectBuilderForVs.Libraries; 
            projectBuilderForVs.RecreateLibraryDependenciesFolders();
            List<Library> allNotTopAndNotGlobal = libraries.Where((Library lib) => { return lib.IsTopLevel == false && lib.config.ClassName != "GlobalBuildConfig"; }).ToList();
            Library libTop = libraries.Where((Library lib) => { return lib.IsTopLevel; }).First();


            //4. add the filters from other libraries to toplevel library for this as well that matches the directories created from LibraryDependencies.
            //these are already created in Library constructor as static LibraryDependencyFilter
            libTop.AddFilter(Library.LibraryDependencyFilter);



            //5. I need to get all the cIncludes, cClompiles, additionalincludes from the other libraries and add to top level
            // to the top level library.

            //getting and adding all additional includes
            allNotTopAndNotGlobal
                .ForEach((Library lib) =>
                {
                    lib.GetAllAdditionalIncludes()
                    .ForEach((string inc) =>
                    {
                        libTop.AddAdditionalIncludes(inc);
                    });
                });

            //CLCompile
            allNotTopAndNotGlobal
            .ForEach((Library lib) =>
            {
                lib.GetAllCCompile()
                .ForEach((MyCLCompileFile inc) =>
                {
                    //change it so that the location of these files will be the same as the filters they are set in
                    inc.LocationOfFile = Path.GetDirectoryName(inc.FullFilterName);

                    libTop.AddCCompileFile(inc);
                });
            });

            //CLinclude
            allNotTopAndNotGlobal
            .ForEach((Library lib) =>
            {
                lib.GetAllCincludes()
                .ForEach((MyCLIncludeFile inc) =>
                {
                    //change it so that the location of these files will be the same as the filters they are set in
                    inc.LocationOfFile = Path.GetDirectoryName(inc.FullFilterName);

                    libTop.AddCIncludeFile(inc);
                });
            });



            //6. Finally recreate the xml settings files. in this case there will be two .xproj and .filters
            libTop.GenerateXMLSettings();



        }

    }
}
