using System;
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
        public XMLSetting ConfigSettings { get; set; }
        public List<Library> Libraries { get; set; }

        public ProjectBuilderBase(XMLSetting configSettings)
        { 
            ConfigSettings = configSettings;
            Libraries = new List<Library>();
            //2: get the libraries settings xml file (they will be in the same directory as the config 
            //file path )converted to the settingxml  
            //go through each lirary config
            foreach (var config in ConfigSettings.RootOfSetting.Configs.Config)
            { 
                MySettingsBase Settings = CreateMySettingsBase(config);
                Libraries.Add(new Library(config, Settings)); 
            }
             
            //initialize all dependent libraries
            foreach (Library lib in Libraries)
            {
                lib.SetLibrariesIDependOn(Libraries);
            }


        }

        public void RecreateLibraryDependenciesFolders()
        {
            List<Library> allNotTopAndNotGlobal = Libraries.Where((Library lib) => { return lib.TopLevel == false && lib.config.ClassName != "GlobalBuildConfig"; }).ToList();
            Library libTop = Libraries.Where((Library lib) => { return lib.TopLevel; }).First();

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

        public abstract MySettingsBase CreateMySettingsBase(Config configClass);

         
    }
}
