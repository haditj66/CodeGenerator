using CodeGenerator.MacroProcesses.AESetups;
 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CgenMin.MacroProcesses
{


    public abstract class AEProject
    {

        public static string BaseAEDir = "C:/AERTOSProjects";

        public static readonly List<string> ListOfBoardTargets = new List<string>() {
        "mingw",
        "STM32F411RE"
        };

        public List<string> GetAllLibrariesIDependOnFlattenedSTR() {  return GetAllLibrariesIDependOnFlattened().Select(a => a.Name).ToList(); } //+ "_lib"

        public List<AEProject> GetAllLibrariesIDependOnFlattened()
        {
            //get all depending libraries
            List<List<AEProject>> AlldependingProjectsByLayers = GetAllLibrariesIDependOnAsDependencyLayers();
            List<AEProject> ret = new List<AEProject>();
            for (int i = AlldependingProjectsByLayers.Count - 1; i >= 0; i--)
            {
                ret.AddRange(AlldependingProjectsByLayers[i]);
            }

            //remove duplicates
            ret = ret.Distinct().ToList();

            return ret;
        }


        public List<List<AEProject>> GetAllLibrariesIDependOnAsDependencyLayers()
        {
            //get all depending libraries
            List<List<AEProject>> AlldependingProjectsByLayers = new List<List<AEProject>>();
            int layer = 0;
            List<AEProject> libdependLayer0 = this.LibrariesIDependOn;
            AlldependingProjectsByLayers.Add(libdependLayer0);
            if (libdependLayer0.Count != 0)
            {
                for (; ; )
                {
                    layer++;

                    List<AEProject> libdependLayer = new List<AEProject>();
                    foreach (var proj in AlldependingProjectsByLayers[layer - 1])
                    {
                        libdependLayer.AddRange(proj.LibrariesIDependOn);
                    }

                    if (libdependLayer.Count == 0)
                    {
                        break;
                    }
                    else
                    {
                        AlldependingProjectsByLayers.Add(libdependLayer);
                    }
                }
            }
            return AlldependingProjectsByLayers;
        }

        public List<AEProject> _LibrariesIDependOn { get; protected set; }
        public List<AEProject> LibrariesIDependOn
        {
            get
            {
                _LibrariesIDependOn =  _GetLibrariesIDependOn();
                return _LibrariesIDependOn;
            }
        }
        public List<string> LibrariesIDependOnStr { get { return LibrariesIDependOn.Select(a => a.Name ).ToList(); } }//
        public List<string> LibrariesIDependOnStr_LIB { get { return LibrariesIDependOn.Select(a => a.Name + "_lib").ToList(); } }//
        public List<AEEvent> EventsInLibrary { get { return _GetEventsInLibrary(); } }
        public List<AEHal> PeripheralsInLibrary { get { return _GetPeripheralsInLibrary(); } }
       
        
        public string DirectoryOfLibrary
        {
            get
            {
                string _DirectoryOfLibrary = _GetDirectoryOfLibrary();
                //_DirectoryOfLibrary = AEInitializing.GetRunningDirectoryFromProjectName(Name);

                _DirectoryOfLibrary =
                    Path.IsPathRooted(_DirectoryOfLibrary) == false ? _DirectoryOfLibrary = Path.Combine(BaseAEDir, _DirectoryOfLibrary)
                    : _DirectoryOfLibrary;

                _DirectoryOfLibrary = _DirectoryOfLibrary.Replace("\\", @"/");

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


        public List<string> ListOfTests { get { return _GetListOfTests(); } }
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

                var tt = typeProcessToRun.GetMethods();

                var methodsOfAEEXETest = tt
              .Where(m => m.GetCustomAttributes(typeof(AEEXETest), false).Length > 0)
              .ToArray();

                foreach (var item in methodsOfAEEXETest)
                {
                    _ListOfTests.Add(item.Name);
                }

            }
            return _ListOfTests;
        }


        protected abstract List<string> _GetAnyAdditionalIncludeDirs();
        private List<string> _AnyAdditionalIncludeDirs = null;

        protected abstract List<string> _GetAnyAdditionalSRCDirs();
        private List<string> _AnyAdditionalSrcDirs = null;


        private List<string> GetAnyAdditionalIncludeOrSrcDirs(bool ForInc)
        {
            List<string> listToReturn = new List<string>();
            bool isnullList = true;
            if (ForInc)
            {
                isnullList = _AnyAdditionalIncludeDirs == null;
            }
            else
            {
                isnullList = _AnyAdditionalSrcDirs == null; 
            }

            if (isnullList)
            {
                
                if (ForInc)
                {
                    _AnyAdditionalIncludeDirs = new List<string>();

                    _AnyAdditionalIncludeDirs = _GetAnyAdditionalIncludeDirs();
                    listToReturn = _AnyAdditionalIncludeDirs;
                    isnullList = _AnyAdditionalIncludeDirs == null;
                }
                else
                {
                    _AnyAdditionalSrcDirs = new List<string>();

                    _AnyAdditionalSrcDirs = _GetAnyAdditionalSRCDirs();
                    listToReturn = _AnyAdditionalSrcDirs;
                    isnullList = _AnyAdditionalSrcDirs == null;
                }

                if (isnullList == false)
                {
                    List<int> relativeDirs = new List<int>();
                    for (int i = 0; i < listToReturn.Count; i++)
                    {
                        //check if the directory is absolute or relative
                        if (Path.IsPathRooted(listToReturn[i]) == false)
                        {
                            relativeDirs.Add(i);
                        }
                    }

                    foreach (var item in relativeDirs)
                    {
                        listToReturn[item] = Path.Combine(this.DirectoryOfLibrary, listToReturn[item]);
                    }
                }

            }

            for (int i = 0; i < listToReturn.Count; i++)
            {
                listToReturn[i] = listToReturn[i].Replace("\\", "/");
            }

            return listToReturn;
        }


        public List<string> GetAnyAdditionalSRCDirs()
        {
            return GetAnyAdditionalIncludeOrSrcDirs(false);
        }
            public List<string> GetAnyAdditionalIncludeDirs()
        {
            //if (_AnyAdditionalIncludeDirs == null)
            //{
            //    _AnyAdditionalIncludeDirs = new List<string>();

            //    _AnyAdditionalIncludeDirs = _GetAnyAdditionalIncludeDirs();
            //    if (_AnyAdditionalIncludeDirs != null)
            //    {
            //        List<int> relativeDirs = new List<int>();
            //        for (int i = 0; i < _AnyAdditionalIncludeDirs.Count; i++)
            //        {
            //            //check if the directory is absolute or relative
            //            if (Path.IsPathRooted(_AnyAdditionalIncludeDirs[i]) == false)
            //            {
            //                relativeDirs.Add(i);
            //            }
            //        }

            //        foreach (var item in relativeDirs)
            //        {
            //            _AnyAdditionalIncludeDirs[item] = Path.Combine(this.DirectoryOfLibrary, _AnyAdditionalIncludeDirs[item]);
            //        }
            //    }

            //}

            //for (int i = 0; i < _AnyAdditionalIncludeDirs.Count; i++)
            //{
            //    _AnyAdditionalIncludeDirs[i] = _AnyAdditionalIncludeDirs[i].Replace("\\", "/");
            //}

            //return _AnyAdditionalIncludeDirs;
            return GetAnyAdditionalIncludeOrSrcDirs(true);
        }
        


        public AEConfig GenerateTestOfName(string testName )
        {
            var type = typeof(AEProject);
            var typeProcessToRun = AppDomain.CurrentDomain.GetAssemblies()
          .SelectMany(s => s.GetTypes())
          .Where(p => type.IsAssignableFrom(p))
          .Where(p => p.Name == Name)
          .FirstOrDefault();

            var methodsToRun = AppDomain.CurrentDomain.GetAssemblies()
          .SelectMany(s => s.GetTypes())
           .Where(p => p.IsAssignableFrom(typeProcessToRun))
          .SelectMany(t => t.GetMethods())
          .Where(m => m.GetCustomAttributes(typeof(AEEXETest), false).Length > 0)
          .Where(m => m.Name == testName)
          .FirstOrDefault();

           var attr = (AEEXETest)methodsToRun.GetCustomAttributes(typeof(AEEXETest), false).FirstOrDefault();
            

            methodsToRun.Invoke(this, null);
            return attr.AEconfigToUse;
        }

        protected abstract string _GetDirectoryOfLibrary();
        protected abstract List<AEEvent> _GetEventsInLibrary();
        protected abstract List<AEHal> _GetPeripheralsInLibrary();

        protected abstract List<AEProject> _GetLibrariesIDependOn();// where T : AELibrary;


    }
}
