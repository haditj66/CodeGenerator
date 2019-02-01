﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGenerator.IDESettingXMLs;
using System.IO;

namespace CodeGenerator.ProjectBuilders
{
    public abstract class ProjectBuilderBase  
    {
        public string BaseDirectoryForProject { get; protected set; }
        private IDESetting ConfigSettings { get; set; }
        public List<Library> Libraries { get; set; }
        public Library LibTop { get; private set; }
        public List<Library> AllNotTopAndNotGlobal { get; private set; }


        public ProjectBuilderBase(IDESetting configSettings)
        { 
            ConfigSettings = configSettings;
            Libraries = new List<Library>();
            //2: get the libraries settings xml file (they will be in the same directory as the config 
            //file path )converted to the settingxml  
            //go through each lirary config
            foreach (Config config in ConfigSettings.RootOfSetting.Configs.Config)
            {
                //change the configs file path string to take out any path/config at end
                string PathToProjectSettings = Path.GetDirectoryName(config.ConfigFileFullPath);
                bool isConfigLast = Path.GetFileName(PathToProjectSettings).ToLower() == "config" ;
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


        public static bool ConditionForImportingDependencyCComp(MyCLCompileFile CComp)
        {
            if ((CComp.LocationOfFile == "Config"))
            {
                return false;
            }

            if ((CComp.LocationOfFile.Contains("LibraryDependencies")))
            {
                return false;
            }

            if ((CComp.Name == "main.cpp"))
            {
                return false;
            }

            return true;
        }

        public static bool ConditionForImportingDependencyCInc(MyCLIncludeFile Cinc)
        { 
            if ((Cinc.LocationOfFile == "Config"))
            {
                return  false;
            }

            if ((Cinc.LocationOfFile.Contains("LibraryDependencies")))
            {
                return false;
            }
             
            return true;
        }

        public void RecreateLibraryDependenciesFolders()
        {
            List<Library> allNotTopAndNotGlobal = Libraries.Where((Library lib) => { return lib.IsTopLevel == false && lib.config.ClassName != "GlobalBuildConfig"; }).ToList();
            Library libTop = Libraries.Where((Library lib) => { return lib.IsTopLevel; }).First();

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
                    libTop.AddAdditionalIncludes(libDepFilters.GetFullAddress());
                }
            }
        }

        protected abstract MySettingsBase GetSettingsOVERRIDE(string pathToProjectSettings);

        public abstract void ImportDependentLibrariesCincAndCcompAndAdditional();
        //public abstract void RecreateConfigurationFilterFolderIncludes(string NameOfCGenProject, string pathOfConfigTestDir);


    }
}
