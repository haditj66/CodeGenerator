using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGenerator.IDESettingXMLs;
using System.IO;
using CodeGenerator.cgenXMLSaves.SaveFiles;
using CodeGenerator.CMD_Handler;
using CodeGenerator.GitHandlerForLibraries;
using CodeGenerator.ProblemHandler;

namespace CodeGenerator.ProjectBuilders
{
    public abstract class ProjectBuilderBase
    {
        public string BaseDirectoryForProject { get; protected set; }
        private IDESetting ConfigSettings { get; set; }
        public List<Library> Libraries { get; set; }
        public Library LibTop { get; private set; }
        public List<Library> AllNotTopAndNotGlobal { get; private set; }

        protected CMDHandler Cmd { get; private set; }
        protected GitHandlerForLibrary GitHandlerforLib { get; private set; }
        protected LibrariesForCheckedOutGit LibGitCleanUp { get; private set; }

        public ProblemHandle ProblemHandle { get; protected set; }

        public ProjectBuilderBase(IDESetting configSettings)
        {
            Cmd = new CMDHandler("");
            GitHandlerforLib = new GitHandlerForLibrary(Cmd);
            LibGitCleanUp = new LibrariesForCheckedOutGit(Cmd);

            ProblemHandle = new ProblemHandle(LibGitCleanUp);

            ConfigSettings = configSettings;
            Libraries = new List<Library>();
            //2: get the libraries settings xml file (they will be in the same directory as the config 
            //file path )converted to the settingxml  
            //go through each lirary config
            foreach (Config config in ConfigSettings.RootOfSetting.Configs.Config)
            {
                //change the configs file path string to take out any path/config at end
                string PathToProjectSettings = Path.GetDirectoryName(config.ConfigFileFullPath);
                bool isConfigLast = Path.GetFileName(PathToProjectSettings).ToLower() == "config";
                if (isConfigLast)
                {
                    PathToProjectSettings = Directory.GetParent(PathToProjectSettings).FullName;//Path.Combine( Directory.GetParent(Path.GetDirectoryName(config.ConfigFileFullPath)).Name, Path.GetFileName(config.ConfigFileFullPath));

                }
                BaseDirectoryForProject = config.IsTopLevel == "true" ? PathToProjectSettings : BaseDirectoryForProject;


                MySettingsBase Settings = GetSettingsOVERRIDE(PathToProjectSettings);
                Libraries.Add(new Library(config, Settings));
            }

            //initialize all dependent libraries
            foreach (Library lib in Libraries)
            {
                lib.SetLibrariesIDependOn(Libraries);
            }


            LibTop = Libraries.Where((Library lib) => { return lib.IsTopLevel == true; }).First();
            AllNotTopAndNotGlobal = Libraries.Where((Library lib) => { return lib.IsTopLevel == false && lib.config.ClassName != "GlobalBuildConfig"; }).ToList();
        }




        public static bool IsCCompDependencyAbleForImporting(MyCLCompileFile CComp, params string[] possiblePrefixs)
        {
            if ((CComp.LocationOfFile == "Config"))
            {
                return false;
            }

            if ((CComp.LocationOfFile.Contains("LibraryDependencies")))
            {
                return false;
            }

            if ((CComp.Name == "main.cpp")   )
            {
                return false;
            }

            foreach (string possibleprefix in possiblePrefixs)
            {
                if ((CComp.Name == possibleprefix + "main.cpp"))
                {
                    return false;
                } 
            }

            return true;
        }

        public static bool IsCIncDependencyAbleForImporting(MyCLIncludeFile Cinc)
        {


            if (Cinc.Name.Contains("_ConfigurationCG"))
            {
                return true;
            }
            if ((Cinc.LocationOfFile == "Config"))
            {
                return false;
            }

            if ((Cinc.LocationOfFile.Contains("LibraryDependencies")))
            {
                return false;
            }

            return true;
        }

        public void RecreateLibraryDependenciesFoldersFilters()
        { 
            if (Directory.Exists(Path.GetFullPath(Library.TopLevelDir)))
            {
                //first erase any directories past the LibraryDependencies
                Directory.Delete(Library.TopLevelDir, true);


            }
            //now recreate LibraryDependencies
            Directory.CreateDirectory(Library.TopLevelDir);

            //go through each config, and create the directories from them. 
            Libraries.ForEach((Library library) => { library.CreateDirectoryForLibraryPath(); });


            // I need to create additional includes that point to the end of the library directories. for example
            foreach (MyFilter libDepFilters in Library.LibraryDependencyFilter)
            {
                //only for last child filters 
                if (libDepFilters.ChildrenFilters.Count == 0)
                {
                    LibTop.AddAdditionalIncludes(libDepFilters.GetFullAddress());
                }
            }

            //add the librarydependency filter
            LibTop.AddFilter(Library.LibraryDependencyFilter);
        }


        protected void CheckoutLibraryToCorrectMajor(Library libraryToCheckout)
        {
            string tagToCheckoutTo;
            if (!GitHandlerforLib.DoesLibraryContainsGitRepoAndTagForMajor(libraryToCheckout, out tagToCheckoutTo))
            {
                //ProblemHandle.ThereisAProblem("this library located \n" + lowestLevelLibrary.settings.PATHOfProject + "\n has no git repo.");
                ProblemHandle.ThereisAProblem("this library located \n" + libraryToCheckout.settings.PATHOfProject +
                                              "\n either does not contain a git repo or does not have a proper tag of pattern \n Vx.y.z where x matches major version that a library depends on."
                    );
            }
            else
            {
                //since it has the needed tag. stash everything, checkout to that tag 
                GitHandlerforLib.StashAndCheckoutTag(libraryToCheckout, tagToCheckoutTo, ProblemHandle);
                LibGitCleanUp.AddLibraryCheckedOutSoFar(libraryToCheckout);
            }
        }

        protected abstract MySettingsBase GetSettingsOVERRIDE(string pathToProjectSettings);

        /// <summary>
        /// this imports settings from dependent libraries into the settings of your project program
        /// </summary>
        public abstract void ImportDependentSettingsLibrariesCincAndCcompAndAdditional(SaveFilecgenProjectGlobal saveProjGlob);

        /// <summary>
        /// this will import all physical files from libraries your top library depends on.
        /// </summary>
        public abstract void ImportDependentLibrariesFiles();
        //public abstract void RecreateConfigurationFilterFolderIncludes(string NameOfCGenProject, string pathOfConfigTestDir);


    }
}
