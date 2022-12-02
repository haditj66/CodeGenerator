using CodeGenerator.MacroProcesses.AESetups;
using CodeGenerator.MacroProcesses.AESetups.SPBs;
using CodeGenerator.ProblemHandler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CgenMin.MacroProcesses
{


    public abstract class AEProject
    {

        public static string BaseAEDir = "";

        public static readonly List<string> ListOfBoardTargets = new List<string>() {
        "mingw",
        "STM32F411RE"
        };

        public List<AEProject> _LibrariesIDependOn { get; protected set; }
        public List<AEProject> LibrariesIDependOn
        {
            get
            {
                return _LibrariesIDependOn;
            }
        }
        public List<string> LibrariesIDependOnStr_LIB { get { return LibrariesIDependOn.Select(a => a.Name + "_lib").ToList(); } }
        public List<AEEvent> EventsInLibrary { get { return _GetEventsInLibrary(); } }
        public List<string> ListOfTests { get { return _GetListOfTests(); } }
        public string DirectoryOfLibrary
        {
            get
            {
                string _DirectoryOfLibrary = _GetDirectoryOfLibrary();
                //_DirectoryOfLibrary = AEInitializing.GetRunningDirectoryFromProjectName(Name);

                _DirectoryOfLibrary =
                    Path.IsPathRooted(_DirectoryOfLibrary) == false ? _DirectoryOfLibrary = Path.Combine(BaseAEDir, _DirectoryOfLibrary)
                    : _DirectoryOfLibrary;

                return _DirectoryOfLibrary;
            }
        }

        public string Name { get { return this.GetType().Name; } }

        public AEProject()
        {
            _LibrariesIDependOn = new List<AEProject>();
        }

        public void Init()
        {

            _LibrariesIDependOn = new List<AEProject>();
            try
            {
                _LibrariesIDependOn = _GetLibrariesIDependOn();

                foreach (var item in LibrariesIDependOn)
                {
                    item.Init();
                }
            }
            catch (System.StackOverflowException e)
            {
                ProblemHandle problemHandle = new ProblemHandle();
                problemHandle.ThereisAProblem("there must have been a circular dependency on your libraries");
                throw;
            }

        }
         

        private List<string> _ListOfTests = null;
        protected  List<string> _GetListOfTests()
        {
            if (_ListOfTests == null)
            {
               _ListOfTests = new List<string>();

                var type = typeof(AEProject);
                var typeProcessToRun = AppDomain.CurrentDomain.GetAssemblies()
              .SelectMany(s => s.GetTypes())
              .Where(p => type.IsAssignableFrom(p))
              .Where(p => p.Name == Name)
              .FirstOrDefault();

                var methodsOfAEEXETest = AppDomain.CurrentDomain.GetAssemblies()
              .SelectMany(s => s.GetTypes())
               .Where(p => type.IsAssignableFrom(typeProcessToRun))
              .SelectMany(t => t.GetMethods())
              .Where(m => m.GetCustomAttributes(typeof(AEEXETest), false).Length > 0)
              .ToArray();

                foreach (var item in methodsOfAEEXETest)
                {
                    _ListOfTests.Add(item.Name);
                }

            }
            return _ListOfTests;
        }

        public AEConfig GenerateTestOfName(string testName)
        {
            var type = typeof(AEProject);
            var typeProcessToRun = AppDomain.CurrentDomain.GetAssemblies()
          .SelectMany(s => s.GetTypes())
          .Where(p => type.IsAssignableFrom(p))
          .Where(p => p.Name == Name)
          .FirstOrDefault();

            var methodsToRun = AppDomain.CurrentDomain.GetAssemblies()
          .SelectMany(s => s.GetTypes())
           .Where(p => type.IsAssignableFrom(typeProcessToRun))
          .SelectMany(t => t.GetMethods())
          .Where(m => m.GetCustomAttributes(typeof(AEEXETest), false).Length > 0)
          .Where(m => m.Name == testName)
          .FirstOrDefault();

           var attr = (AEEXETest)methodsToRun.GetCustomAttributes(typeof(AEEXETest), false).FirstOrDefault();
            

            methodsToRun.Invoke(this,null);
            return attr.AEconfigToUse;
        }

        protected abstract string _GetDirectoryOfLibrary();
        protected abstract List<AEEvent> _GetEventsInLibrary();

        protected abstract List<AEProject> _GetLibrariesIDependOn();// where T : AELibrary;


    }






    public class AEInitializing : MacroProcess
    {

        public static AEInitializing TheMacro2Session;
        public static string RunningProjectName;
        public static string RunningProjectDir;


        public static string GetRunningDirectoryFromProjectName(string projectName)
        {

            // from this library name, I need to get the directory that it belongs to.
            //first grab all the contents of the cmake file in C:/AERTOS/AERTOS/CMakeLists.txt .
            string cmakeCont = File.ReadAllText(@"C:/AERTOS/AERTOS/CMakeLists.txt");

            //    STREQUAL "exeHalTest")
            //set(INTEGRATION_TARGET_DIRECTORY "C:/AERTOS/AERTOS/src/AE/hal/exeHalTest")
            Regex re = new Regex(@"STREQUAL\s*\""" + projectName + @"\""\s*\)\s*set\s*\(\s*INTEGRATION_TARGET_DIRECTORY\s*\""(?<ArgReqContents>.*)\""");
            string _ProjectDirectory = re.Match(cmakeCont).Groups["ArgReqContents"].Value;

            _ProjectDirectory = projectName == "CGENTest" ? @"C:\CodeGenerator\CodeGenerator\macro2Test\CGENTest" : _ProjectDirectory; //for debugging

            return _ProjectDirectory;
        }

        public static string GetRunningProjectNameFromDirectory(string dirOfProject)
        {

            // from this library name, I need to get the directory that it belongs to.
            //first grab all the contents of the cmake file in C:/AERTOS/AERTOS/CMakeLists.txt .
            string cmakeCont = File.ReadAllText(@"C:/AERTOS/AERTOS/CMakeLists.txt").Replace("/", "\\");
            dirOfProject = dirOfProject.Replace("/", "\\").Replace("\\", @"\\");

            //    STREQUAL "exeHalTest")
            //set(INTEGRATION_TARGET_DIRECTORY "C:/AERTOS/AERTOS/src/AE/hal/exeHalTest")
            string pat = @"STREQUAL\s*""(?<ArgProjName>.*)""\s*\)\s*set\s*\(\s*INTEGRATION_TARGET_DIRECTORY\s*""" + dirOfProject + @"""";
            Regex re = new Regex(pat);
            var mat = re.Match(cmakeCont);//.Groups["ArgProjName"].Value;
            if (mat.Success)
            {
                return mat.Groups["ArgProjName"].Value;
            }
            else
            {
                return "CGENTest";
            }

        }

        public static List<AEProject> GetAllCurrentAEProjects()
        {
            var type = typeof(AEProject);
            List<Type> allAEProjectsTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && type != p).ToList();

            var allAEProjects = allAEProjectsTypes.Select(s => Activator.CreateInstance(s)).Cast<AEProject>().ToList();



            return allAEProjects;

            //if (typeProcessToRun == null)
            //{
            //    this.probhandler.ThereisAProblem($"the AEInit class of projectName {RunningProjectName}  does exist");
            //}

            //AEProject aeProject = (AEProject)Activator.CreateInstance(typeProcessToRun);

        }

        private static bool SameDirectory(string path1, string path2)
        {
            var path11 = Path.GetFullPath(path1);
            var path22 = Path.GetFullPath(path2);
            //Console.WriteLine("{0} == {1} ? {2}", path1, path2, string.Equals(path1, path2, StringComparison.OrdinalIgnoreCase));
            return string.Equals(path11, path22, StringComparison.OrdinalIgnoreCase);

        }

        public static AEProject GetProjectIfDirExists(string ofDir)
        {
            var pr = GetAllCurrentAEProjects();

            return pr.Where(p => SameDirectory(ofDir, p.DirectoryOfLibrary)).FirstOrDefault();

        }

        public static AEProject GetProjectIfNameExists(string nameOfProj)
        {
            var pr = GetAllCurrentAEProjects();

            return pr.Where(p => p.Name == nameOfProj).FirstOrDefault();

        }



        public override void RunProcess()
        {

        }

        public void GenerateProject(string projectName, string projectTest)
        {

            //AEUtilityService uts = new AEUtilityService("uartDriver", "UUartDriver",Path.Combine(this.EvironmentDirectory, "include", "UUARTDriver.h"));
            //AEUtilityService utstdu = new AEUtilityService("uartDriver", "UUartDriverTDU",Path.Combine(this.EvironmentDirectory, "include", "UUARTDriver.h"));



            TheMacro2Session = this;

            //string sd = GetRunningProjectNameFromDirectory("C:/AERTOS/AERTOS/src/AE/hal/exeHalTest");
            RunningProjectName = projectName;// GetRunningProjectNameFromDirectory(this.EvironmentDirectory);
            var projToGenerate = GetProjectIfNameExists(RunningProjectName);
            if (projToGenerate == null)
            {
                probhandler.ThereisAProblem($"project of name {projectName} did not exist.");
            }

            RunningProjectDir = projToGenerate.DirectoryOfLibrary;



            //use reflection to get the class with the same RunningProjectName
            var type = typeof(AEProject);
            var typeProcessToRun = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p))
                .Where(p => p.Name == RunningProjectName)
                .FirstOrDefault();

            if (typeProcessToRun == null)
            {
                this.probhandler.ThereisAProblem($"the AEInit class of projectName {RunningProjectName}  does exist");
            }

            AEProject aeProject = (AEProject)Activator.CreateInstance(typeProcessToRun);
            aeProject.Init();
            //aeProject.InitAE();

            //get the project test lists
            var methodsOfAEEXETest = AppDomain.CurrentDomain.GetAssemblies()
                      .SelectMany(s => s.GetTypes())
                       .Where(p => type.IsAssignableFrom(typeProcessToRun))
                      .SelectMany(t => t.GetMethods())
                      .Where(m => m.GetCustomAttributes(typeof(AEEXETest), false).Length > 0)
                      .ToArray(); 


            var tt = aeProject.ListOfTests;
            AEConfig aEConfig = aeProject.GenerateTestOfName(projectTest);

            //string ss = aEClock.GenerateMainInitializeSection();

            //string sss = averageSPB3.GenerateAEConfigSection();
            //string ssss = averageSPB2.GenerateAEConfigSection(); 
            //string ssssa1 = averageSPB2.GenerateMainInitializeSection();
            //string ssssa2 = averageSPB2.GenerateMainLinkSetupsSection();
            //string ssss1 = averageSPB3.GenerateMainInitializeSection();
            //string ssss2 = averageSPB3.GenerateMainLinkSetupsSection();


             string aeconfig = AO.All_GenerateAEConfigSection();
            string mainHeader = AO.All_GenerateMainHeaderSection();
            string clockInit = AO.All_GenerateMainClockSetupsSection();
            string mainInit = AO.All_GenerateMainInitializeSection();
            string linksInit = AO.All_GenerateMainLinkSetupsSection();
            string funcInit = AO.All_GenerateFunctionDefinesSection();



            //AEConfig aEConfig = new AEConfig();
            aEConfig.GenerateFile(RunningProjectDir, this, aeconfig, "");
            //AOWritableToAOClassContents.WriteAllFileContents();

            //string aeConfigOUT = this.GenerateFileOut("AERTOS\\AEConfig",
            //    new MacroVar() { MacroName = "AODefines", VariableValue = aeconfig }
            //    );

            //Console.WriteLine($"generating AEConfig.h ");
            //WriteFileContentsToFullPath(aeConfigOUT, Path.Combine(RunningProjectDir, "conf", "AEConfig.h"), "h", true);



            List<string> AllDepends = new List<string>();
            foreach (var lib in aeProject.LibrariesIDependOn)
            {
                AllDepends.Add(this.GenerateFileOut("AERTOS\\TargetCreationDepend",
                new MacroVar() { MacroName = "DependName", VariableValue = lib.Name },
                new MacroVar() { MacroName = "DependDir", VariableValue = lib.DirectoryOfLibrary },
                new MacroVar() { MacroName = "DependDepends", VariableValue = string.Join(" ", lib.LibrariesIDependOnStr_LIB) }
                ));
            }



            //            rt(CGEN_PROJECT_DIRECTORY "@DependDir@")

            //CREATE_TARGET_INTEGRATIONEXE(NAME_OF_TARGET @DependName@
            //LOCATION_OF_TARGET "@DependDir@"
            //LibrariesToLinkTo AECoreLib @DependDepends@


            Console.WriteLine($"generating TargetCreation.cmake ");
            WriteFileContents_FromCGENMMFile_ToFullPath(
                "AERTOS\\TargetCreation",
                Path.Combine(RunningProjectDir, "cmake", "TargetCreation.cmake"),
                true, false,
                 new MacroVar() { MacroName = "TestNamesList", VariableValue = string.Join(" ", aeProject.ListOfTests) },
                 new MacroVar() { MacroName = "LibrariesIDependOn", VariableValue = string.Join(" ", aeProject.LibrariesIDependOnStr_LIB) },
                 new MacroVar() { MacroName = "DependsInit", VariableValue = string.Join(" ", AllDepends) }
                );


            Console.WriteLine($"generating AEConfigProject.cmake ");
            WriteFileContents_FromCGENMMFile_ToFullPath(
            "AERTOS\\AEConfigProject",
            Path.Combine(RunningProjectDir, "AEConfigProject.cmake"),
            true, false,
             new MacroVar() { MacroName = "TestNamesList", VariableValue = string.Join(" ", aeProject.ListOfTests) },
             new MacroVar() { MacroName = "LibrariesIDependOn", VariableValue = string.Join(" ", aeProject.LibrariesIDependOnStr_LIB) },
             new MacroVar() { MacroName = "DependsInit", VariableValue = string.Join(" ", AllDepends) }
            );



            //create testname.cpp file
            Console.WriteLine($"generating {projectTest}.cpp ");
            WriteFileContents_FromCGENMMFile_ToFullPath(
            "AERTOS\\projectTest",
            Path.Combine(RunningProjectDir, $"{projectTest}.cpp"),
            true, true,
             new MacroVar() { MacroName = "ProjectName", VariableValue =  projectName },
             new MacroVar() { MacroName = "ProjectTest", VariableValue = projectTest },
             new MacroVar() { MacroName = "MainHeader", VariableValue = mainHeader },
             new MacroVar() { MacroName = "ClockInit", VariableValue = clockInit },
             new MacroVar() { MacroName = "MainInit", VariableValue = mainInit } ,
             new MacroVar() { MacroName = "LinksInit", VariableValue = linksInit },
             new MacroVar() { MacroName = "FuncInit", VariableValue = funcInit }
            );


            AOWritableToAOClassContents.WriteAllFileContents();
            //WriteFileContentsToFullPath(this.GenerateFileOut("AERTOS\\TargetCreation",
            //    new MacroVar() { MacroName = "AODefines", VariableValue = aeconfig }
            //    ) 
            //    , Path.Combine(RunningProjectDir, "cmake", "TargetCreation.cmake"), "cmake", false);


            return;
        }




        public string UtilityNameCTORSection()
        {

            return "";
        }


    }
}
