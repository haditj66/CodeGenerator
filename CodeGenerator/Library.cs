using CodeGenerator.IDESettingXMLs;
using CodeGenerator.IDESettingXMLs.VisualStudioXMLs;
using CodeGenerator.IDESettingXMLs.VisualStudioXMLs.Filters;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

namespace CodeGenerator
{
    public class Library
    {

        static public string TopLevelDir { get; private set; }
        public bool IsTopLevel { get; private set; }
        static public MyFilter LibraryDependencyFilter = new MyFilter("LibraryDependencies");
        public MyFilter LibraryBaseFilter { get; private set; }
        public  Config config { get; private set; } 
        public MySettingsBase settings { get; protected set; }
        public List<Library> LibrariesIDependOn { get; protected set; }

        public Library(Config config, MySettingsBase settings)
        {
            this.config = config;
            LibrariesIDependOn = new List<Library>();
            if (config.IsTopLevel == "true")
            {
                TopLevelDir = Directory.GetParent(Path.GetDirectoryName(config.ConfigFileFullPath)).FullName;
                TopLevelDir = Path.Combine(TopLevelDir, "LibraryDependencies"); 
                IsTopLevel = true;
                settings.Initiate(); 
                
            }
            else
            {
                //add the filters here.
                if (config.ClassName != "GlobalBuildConfig")
                {
                    
                    //any libraries that depend on other libraries will not be with that library's filter but rather have its own
                    MyFilter prefixFilter = LibraryDependencyFilter.AddChildFilter(config.Prefix);
                    LibraryBaseFilter = prefixFilter.AddChildFilter(config.ConfTypePrefix);

                    settings.Initiate();

                    //because this is not top level, I need to change all cCompiles and Cincludes to have LibraryBaseFilter
                    settings.CLCompileFiles.ForEach((MyCLCompileFile cCompile)=> { cCompile.FilterIBelongTo = LibraryBaseFilter; }); 
                    settings.CLIncludeFiles.ForEach((MyCLIncludeFile cinc) => { cinc.FilterIBelongTo = LibraryBaseFilter; });

                }
            } 
            this.settings = settings;


        }



        public void SetLibrariesIDependOn(List<Library> FromTheseLibraries)
        {
            //if this library is top level, look for all non top level libraries that dont have depends in their configs.
            if (this.IsTopLevel == true)
            {
                foreach (var lib in FromTheseLibraries)
                {
                    if (lib.IsTopLevel == false && lib.config.Depends.Depend.Count == 0 && lib.config.ClassName != "GlobalBuildConfig")
                    {
                        //then add this library as a depend to the top level library
                        this.LibrariesIDependOn.Add(lib);
                    }
                }
            }
            //if it is not top level than ignore top level library and set depends for all in its config depends that matches
            else
            {
                var LibrarysThatStillNeedInitializing = FromTheseLibraries.Where((Library lib) => { return lib.IsTopLevel == false && lib.config.Depends.Depend.Count != 0 && lib.config.ClassName != "GlobalBuildConfig"; });

                 

                foreach (var libstill in LibrarysThatStillNeedInitializing)
                {

                    //get its depends
                    foreach (var d in libstill.config.Depends.Depend)
                    { 
                        //now iterate through the LibrarysThatStillNeedInitializing again and add any that match that depend
                        foreach (var libToCheck in LibrarysThatStillNeedInitializing)
                        {
                            if (d.ModeOfDepend == libToCheck.config.Major && d.NameOfDepend == libToCheck.config.ClassName && d.TypePrefOfDepend == libToCheck.config.ConfTypePrefix)
                            {
                                libstill.LibrariesIDependOn.Add(libToCheck);
                            }  
                        } 
                    }  
                } 
            } 
        }

        public string  GetFullPrefix()
        {
            return config.Prefix + config.Major + config.ConfTypePrefix ;
        }

        public string GetPathToProjectAsADependent()
        {
            return Path.Combine(config.Prefix,config.ConfTypePrefix);
            //return Path.Combine(config.Prefix, config.Major, config.ConfTypePrefix);
        }

        public bool IsEqual(Library toThisLibrary)
        {
            //if major is the same, ClassName is the same, and conftypeprefix is the same.
            if (toThisLibrary.config.ConfTypePrefix == this.config.ConfTypePrefix && toThisLibrary.config.Major== this.config.Major && toThisLibrary.config.ClassName == this.config.ClassName)
            {
                return true;
            }

            return false;
        }


        public void CreateDirectoryForLibraryPath()
        {
            //add the filters here.
            if (config.ClassName != "GlobalBuildConfig" && this.IsTopLevel == false)
            {
                //over here use TopLevelDir
                Directory.CreateDirectory(Path.Combine(TopLevelDir, config.Prefix));
                Directory.CreateDirectory(Path.Combine(TopLevelDir, config.Prefix, config.ConfTypePrefix));
            }
        } 

        public void AddAdditionalIncludes(params string[] includes )
        {
            //add the additional incldue directories to the top level library
            foreach (var include in includes)
            {
                settings.AddAdditionalInclude(include);
            }

        }

        public void AddAdditionalLibraries(params string[] Libraries)
        {
            //add the additional incldue directories to the top level library
            foreach (var lib in Libraries)
            {
                settings.AddAdditionalLibrary(lib);
            }

        }

        public List<string> GetAllAdditionalLibraries()
        {
            return settings.StringLibraries;
        }


        public List<string> GetAllAdditionalIncludes()
        {
            return settings.StringIncludes;
        }


        public List<MyFilter> GetAllFitlers()
        {
            return settings.myFilters;
        }

        public List<MyCLIncludeFile> GetAllCincludes()
        {
            return settings.CLIncludeFiles;
        }

        public List<MyCLCompileFile> GetAllCCompile()
        {
            return settings.CLCompileFiles;
        }

        public void AddFilter(MyFilter filterToAdd)
        {

            settings.AddFilter(filterToAdd);
        }

        public void AddCCompileFile(MyCLCompileFile MyCcompileToAdd)
        {
            settings.AddCLCompileFile(MyCcompileToAdd);
        }

        public void AddCIncludeFile(MyCLIncludeFile MyCIncludeToAdd)
        {
            settings.AddCLIncludeFile(MyCIncludeToAdd);
        }

        public void GenerateXMLSettings(string baseDirectoryForProject)
        {
            settings.Save(baseDirectoryForProject); 

            /*
            if (settings.XmlFilterClass != null)
            {
                settings.XmlSettings[0].GenerateXMLSetting();
            }

            if (settings.XmlProjectClass != null)
            {
                settings.XmlSettings[1].GenerateXMLSetting();
            }*/

        }

        public void SetPrefixToCLCompileFiles(string prefixToAddToAllFilesNames)
        {
            settings.SetPrefixToCLCompileFiles(prefixToAddToAllFilesNames);
        }

        public void SetPrefixToCLIncFiles(string prefixToAddToAllFilesNames)
        {
            settings.SetPrefixToCLIncFiles(prefixToAddToAllFilesNames);
        }


    }



}