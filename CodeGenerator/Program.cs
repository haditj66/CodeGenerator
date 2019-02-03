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
using CodeGenerator.GitHandlerForLibraries;
using CodeGenerator.ProblemHandler;
using CPPParserLibClang;

namespace CodeGenerator
{


    

    public class Program
    {



        //Verbs help delineate and separate options and values for multiple commands within a single app
        [Verb("generate", HelpText = "generate the code")]
        public class GenerateOptions
        {
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

        public static string DIRECTORYOFTHISCG = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string CGSAVEFILESBASEDIRECTORY = "CGensaveFiles";
        public static string CGCONFCOMPILATOINSBASEDIRECTORY = "ConfigCompilations";
        public static string PATHTOCONFIGTEST = @"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\ConfigTest";

#if TESTING
        //public static string envIronDirectory = @"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\Module1AA";//
        public static string envIronDirectory =   @"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\Module1A";//
        //public static string envIronDirectory = @"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\Module1B";//
#else
        public static string envIronDirectory = Environment.CurrentDirectory;
#endif

        public static SaveFilecgenProject savefileProjLocal = new SaveFilecgenProject();
        public static SaveFilecgenConfig saveFilecgenConfigLocal = new SaveFilecgenConfig();
        //public static ProjectBuilderVS projectBuilderForVs;

        static string[] command;


        static void Main(string[] args)
        {

            test t = new test();

            //string e = Environment.CurrentDirectory;
            //Console.WriteLine(e);
            //ProjectVSTest();
            //CreateProjectBuilder(); this should not be done here as a saveddata.xml may not even be created yet

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

            //command = "generate -r fiile.txt oubnfe.tct --aienabled=true".Split(' '); //values should be called LOWER CASED
            //command = "degenerate -r fiile.txt oubnfe.tct ".Split(' ');
            //command = "init moda".Split(' ');
            //command = ("config --directoryofconfig " +  @"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\ConfigTest").Split(' ');
            //command = "".Split(' ');
            command = "generate".Split(' ');
            //command = "init ModuleB".Split(' ');

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
                    SaveFilecgenProject savefileAtDir = new SaveFilecgenProject(envIronDirectory + "\\" + CGSAVEFILESBASEDIRECTORY);

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
            if (Directory.Exists(envIronDirectory + "\\" + CGSAVEFILESBASEDIRECTORY))
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

            return (new SaveFilecgenProject(envIronDirectory + "\\" + CGSAVEFILESBASEDIRECTORY));

        }


        static ParserResult<object> Config(ConfigOptions opts)
        {
            //if there is no cgenXMLSaves directory than this must be the first config run. build all the
            //config files needed here.
            if (!Directory.Exists(CGSAVEFILESBASEDIRECTORY))
            {
                Directory.CreateDirectory(CGSAVEFILESBASEDIRECTORY);
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
                //get project settings for this.
                MySettingsVS settings = MySettingsVS.CreateMySettingsVS(envIronDirectory);
                settings.RecreateConfigurationFilterFolderIncludes(GetSaveFilecgenProjectAtEnvironDirectory().CgenProjects.Projects.First().NameOfProject);
                //projectBuilderForVs.RecreateConfigurationFilterFolderIncludes(GetSaveFilecgenProjectAtEnvironDirectory().CgenProjects.Projects.First().NameOfProject, saveFilecgenConfigLocal.CgenConfig.DirectoryOfConfig);
                Console.WriteLine(GetSaveFilecgenProjectAtEnvironDirectory().CgenProjects.Projects.First().NameOfProject + " already exists as a project here");
                return null;
            }
            else if (opts.name == null || opts.name == "")
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


                //then I need to create a new project as none exist and user wants to create one 
                try
                {
                    Directory.CreateDirectory(envIronDirectory + "\\" + CGSAVEFILESBASEDIRECTORY);
                    //CREATE ALL THE FILES THAT ARE NEEDED FOR PROJECT INITIATION!
                    // creation of the SaveFilecgenProject file
                    SaveFilecgenProject saveFilecgenProject = new SaveFilecgenProject(envIronDirectory + "\\" + CGSAVEFILESBASEDIRECTORY);
                    cgenProjects projs = saveFilecgenProject.CgenProjects;
                    cgenProject projToAdd = new cgenProject();
                    projToAdd.NameOfProject = opts.name;
                    projToAdd.PathOfProject = envIronDirectory + "\\" + CGSAVEFILESBASEDIRECTORY;
                    projToAdd.UniqueIdentifier = cgenXMLMemeberCreationHelper.UniqueIdentifierCreator();
                    //also add the project to the localProjSettings
                    savefileProjLocal.CgenProjects.Projects.Add(projToAdd);
                    projs.Projects.Add(projToAdd);

                    //save created files
                    saveFilecgenProject.Save();
                    savefileProjLocal.Save();
                    settings.RecreateConfigurationFilterFolderIncludes(opts.name);
                    //projectBuilderForVs.RecreateConfigurationFilterFolderIncludes(opts.name, saveFilecgenConfigLocal.CgenConfig.DirectoryOfConfig);

                    Console.WriteLine("Project successfully created");

                }
                catch (Exception e)
                {
                    Console.WriteLine("There was a problem with project creation");
                    Console.WriteLine(e);
                }
                // so we have a name and no project exists here. create one 


            }


            return null;
        }





        static ParserResult<object> Generate(GenerateOptions opts)
        {

            //first make sure that a project exists here
            if (IsProjectExistsAtEnvironDirectory())
            { 

                //get the project settings for the project configs I want to generate. for VS for NOW!
                MySettingsVS VSsetting = MySettingsVS.CreateMySettingsVS(envIronDirectory);

                //create the configuration file configurationCG.h 
                ConfigurationFileBuilder configFileBuilder = new ConfigurationFileBuilder(VSsetting, CGCONFCOMPILATOINSBASEDIRECTORY,DIRECTORYOFTHISCG, PATHTOCONFIGTEST);
                configFileBuilder.CreateConfigurationToTempFolder(); 
                configFileBuilder.WriteTempConfigurationToFinalFile();
                  
                 

                //now that config was created for libtop. create the libtop's librarydepencies filter and folders
                ProjectLibraryDependencyCreate();

                 
            }
            else
            {
                //project does not exist
                Console.WriteLine("A project does not exist here yet.  to create one use \ncgen init <NAMEOFPROJECT>   ");
            }


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

         public static ProjectBuilderVS  CreateProjectBuilderVS()
        {

            //DONT WORRY ABOUT LIBRARIES THAT DEPEND ON LIBRARIES FOR NOW JUST GET THIS MUCH TO WORK.
            // steps to import a new library would be
            //1. get the libraries config xmlclasses created. (located in configTest/savedData.xml)
            IDESetting settingConfig = new IDESetting(PATHTOCONFIGTEST, ".xml", typeof(Root));


            //2: get the libraries settings xml file (they will be in the same directory as the config 
            //file path )converted to the settingxml   

            return new ProjectBuilderVS(settingConfig);

        }

         public static void ProjectLibraryDependencyCreate()
         {

             ProjectBuilderVS projectBuilderForVs = CreateProjectBuilderVS();

            //3.  add filters and folders directories from that toplevel project directory as:
            //LibraryDependencies
            //  prefix(of libraries top level uses)
            //      confTypePrefix(for libraries that are same but different template type.)    
            projectBuilderForVs.RecreateLibraryDependenciesFoldersFilters(); 

            //4. I need to go through each library, git checkout their correct major. (master should be the branch with tag name of major)
            //first grab all lowest level libraries that have no dependencies
            projectBuilderForVs.CreateCCompCincDependencyFiles();


         //4.5 I need to have the projectbuilder import the files from other dependncies to their correct locations while changeing file name and adding a namespace
            //projectBuilderForVs.ImportDependencyFiles();


            //5. I need to get all the cIncludes, cClompiles, additionalincludes from the other libraries and add to top level
            // to the top level library. 
            projectBuilderForVs.ImportDependentLibrariesCincAndCcompAndAdditional();


            //6. Finally recreate the xml settings files. in this case there will be two .xproj and .filters
            projectBuilderForVs.LibTop.GenerateXMLSettings(projectBuilderForVs.BaseDirectoryForProject);



        }

         

    }
      

}
