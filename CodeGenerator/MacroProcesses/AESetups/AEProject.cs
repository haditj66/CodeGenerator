using CodeGenerator.MacroProcesses.AESetups;
using CodeGenerator.ProblemHandler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        public List<AEHal> PeripheralsInLibrary { get { return _GetPeripheralsInLibrary(); } }
       
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
        protected abstract List<AEHal> _GetPeripheralsInLibrary();

        protected abstract List<AEProject> _GetLibrariesIDependOn();// where T : AELibrary;


    }
}
