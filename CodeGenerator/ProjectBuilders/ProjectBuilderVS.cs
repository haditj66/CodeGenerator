using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGenerator.IDESettingXMLs;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using CodeGenerator.cgenXMLSaves;
using CodeGenerator.cgenXMLSaves.SaveFiles;
using CodeGenerator.CMD_Handler;
using CodeGenerator.IDESettingXMLs.VisualStudioXMLs;
using ExtensionMethods;
using CodeGenerator.FileTemplates;
using CodeGenerator.GitHandlerForLibraries;
using CodeGenerator.ProblemHandler;
using CodeGenerator.ProjectBuilders.FileDependentImporters;
using ConsoleApp2.CPPRefactoring;

namespace CodeGenerator.ProjectBuilders
{
    public class ProjectBuilderVS : ProjectBuilderBase
    {
        public ProjectBuilderVS(IDESetting configSettings) : base(configSettings)
        {

        }


        protected override MySettingsBase GetSettingsOVERRIDE(string pathToProjectSettings)
        {

            return MySettingsVS.CreateMySettingsVS(pathToProjectSettings);
        }

        public override void ImportDependentSettingsLibrariesCincAndCcompAndAdditional(SaveFilecgenProjectGlobal saveProjGlob)
        {

            //5. I need to get all the cIncludes, cClompiles, additionalincludes from the other libraries and add to top level
            // to the top level library. 

            string plaformOfSetup = GetPlatFormThisisSetupFor();

            //getting and adding all additional includes
            AllNotTopAndNotGlobal
                .ForEach((Library lib) =>
                {
                    var includes = saveProjGlob.CgenProjects.Projects.First(p =>  p.PathOfProject == lib.settings.PATHOfProject)
                        .PlatFormsInScope.GetIncludesFromPlatForm(plaformOfSetup);
                    includes.ForEach((string inc) =>
                    {
                        LibTop.AddAdditionalIncludes(inc);
                    });
                    /*
                    lib.GetAllAdditionalIncludes()
                    .ForEach((string inc) =>
                    {
                        LibTop.AddAdditionalIncludes(inc);
                    });
                    */
                });

            //getting and adding all additional libraries
            AllNotTopAndNotGlobal
                .ForEach((Library lib) =>
                {
                    var additlibraries = saveProjGlob.CgenProjects.Projects.First(p => p.PathOfProject == lib.settings.PATHOfProject)
                        .PlatFormsInScope.GetAdditionalLibrariesFromPlatForm(plaformOfSetup);
                    additlibraries.ForEach((string adlib) =>
                    {
                        LibTop.AddAdditionalLibraries(adlib);
                    });
                    /*
                    lib.GetAllAdditionalIncludes()
                    .ForEach((string inc) =>
                    {
                        LibTop.AddAdditionalIncludes(inc);
                    });
                    */
                });

            //List<myc> cCompToExcludeFromImporting = new List<string>();
            //cCompToExcludeFromImporting.Add();

            //CLCompile
            AllNotTopAndNotGlobal
            .ForEach((Library lib) =>
            {
                lib.GetAllCCompile()
                .ForEach((MyCLCompileFile inc) =>
                {
                    //exclude files that are in the Config filter of that project or the LibraryDependencies. exclude main.cpp and as well
                    if (IsCCompDependencyAbleForImporting(inc, lib.config.Prefix+"_", lib.config.Prefix + "_"+ lib.config.ConfTypePrefix+"_"))
                    {
                        
                        //change it so that the location of these files will be the same as the filters they are set in
                        inc.LocationOfFile = Path.GetDirectoryName(inc.FullFilterName);

                        LibTop.AddCCompileFile(inc);
                    }

                });

            });

            //CLinclude
            AllNotTopAndNotGlobal
            .ForEach((Library lib) =>
            {
                lib.GetAllCincludes()
                .ForEach((MyCLIncludeFile inc) =>
                {
                    //exclude files that are in the Config filter of that project or the LibraryDependencies
                    if (IsCIncDependencyAbleForImporting(inc))
                    {
                        //change it so that the location of these files will be the same as the filters they are set in
                        inc.LocationOfFile = Path.GetDirectoryName(inc.FullFilterName);

                        LibTop.AddCIncludeFile(inc);
                    }
                });
            });

        }

        public string GetPlatFormThisisSetupFor()
        {
            string plaformOfSetup = Libraries.First(l => l.config.ClassName == "GlobalBuildConfig")
                .config.Defines.Define.First(d => d.DefineName == "PLATFORM").Value;
            int plaformOfSetupint = Convert.ToInt32(plaformOfSetup);
            plaformOfSetup = Libraries.First(l => l.config.ClassName == "GlobalBuildConfig")
                .config.EnumsDefinintions.EnumDefinition.First(en =>
                {
                    return en.Name.Contains("PLATFORM_") && Convert.ToInt32(en.Value) == plaformOfSetupint;
                }).Name;
            plaformOfSetup = Regex.Replace(plaformOfSetup, "PLATFORM_", "");

            return plaformOfSetup;
        }

        public override void ImportDependentFilesLibrariesCincAndCcomp()
        { 
            //steps 
            //1. check out git tags for ALL dependent libraries
            //2. build configuration for that dependent library in temp folder
            //2.5 send that configurationCG.h file to final librarydependencies/xx/xx folder
            //3. get all files that are able to be imported in from dependent library (NOT main.cpp, ModuleConfig.h etc)
            //4. import those files from dependent library to main library.
            //5. git library back to commit it was before checked out
            

            var lowestLevelLibraries = LibTop.LibrariesIDependOn.Where((Library lib) =>
            {
                return lib.LibrariesIDependOn == null || lib.LibrariesIDependOn.Count == 0;
            }).ToList();




            //go through lowest level libraries first to import in      
                foreach (var lowestLevelLibrary in lowestLevelLibraries)
            {

                //1. check out git tags for ALL dependent libraries
                CheckoutLibraryToCorrectMajor(lowestLevelLibrary);

                //2. ---------------------
                //create Configuration.h in temporary folder but dont put it in project so to not change anything 
                ConfigurationFileBuilder configFileBuilder = new ConfigurationFileBuilder(lowestLevelLibrary.settings, Program.CGCONFCOMPILATOINSBASEDIRECTORY, Program.DIRECTORYOFTHISCG, Program.PATHTOCONFIGTEST, ProblemHandle);
                configFileBuilder.CreateConfigurationToTempFolder();


                //create a configuration.h myinclude for the dependent library so to include that in the importing
                var filtForConfiguration_h = lowestLevelLibrary.GetAllFitlers().Where((MyFilter filt) =>
                {
                    return filt.GetFullAddress() == "Config";//Path.Combine("LibraryDependencies", lowestLevelLibrary.GetPathToProjectAsADependent());
                }).First();
                //todo the path here should be to the temp folder?
                string e = lowestLevelLibrary.GetPathToProjectAsADependent();
                MyCLIncludeFile clinc = new MyCLIncludeFile(filtForConfiguration_h,"ConfigurationCG.h", Path.Combine("LibraryDependencies", lowestLevelLibrary.GetPathToProjectAsADependent()));


                //2.5 send that configurationCG.h file to final librarydependencies/xx/xx folder. but first inherit values
                configFileBuilder.InheritFromTopConfig(lowestLevelLibrary.config);
                configFileBuilder.WriteTempConfigurationToFinalFile(Path.Combine(LibTop.settings.PATHOfProject ,clinc.LocationOfFile,clinc.Name));

                //3. --------------------- 
                //grab all files that are qualified to be imported in and send them through  (NOT main.cpp, ModuleConfig.h etc)
                var CcompsToImport = lowestLevelLibrary.GetAllCCompile().Where((MyCLCompileFile ccom) => { return IsCCompDependencyAbleForImporting(ccom);}).ToList();
                var CIncToImport = lowestLevelLibrary.GetAllCincludes().Where((MyCLIncludeFile cinc) => { return IsCIncDependencyAbleForImporting(cinc); }).ToList();
                //CIncToImport.Add(clinc); dont do this yet as you only want to refactor this, not import it.



                //4. ---------------------import those files from dependent library to main library.
                FileDepedentsImporter FileImporter = new FileDepedentsImporter(lowestLevelLibrary.GetFullPrefix(), CcompsToImport, CIncToImport, lowestLevelLibrary.settings.PATHOfProject);
                string pathToOutput = Path.Combine(LibTop.settings.PATHOfProject, "LibraryDependencies", lowestLevelLibrary.GetPathToProjectAsADependent());
                FileImporter.ImportFilesToPath(pathToOutput);
                 
                CIncToImport.Add(clinc);//the configuration that was written in needs to be added now
                lowestLevelLibrary.AddCIncludeFile(clinc);
                List<string> allFilePaths = new List<string>();
                allFilePaths.AddRange(CcompsToImport.Select(cinc => Path.Combine(pathToOutput, cinc.Name)));
                allFilePaths.AddRange(CIncToImport.Select(cinc => Path.Combine(pathToOutput, cinc.Name)));

                //change names of all imported files 
                CppRefactorer refactorer = new CppRefactorer(allFilePaths);
                List<string> filesInScopeToRefactorCopy = new List<string>(refactorer.FilesInScopeToRefactor);
                string prefixToAddToAllFilesNames = string.IsNullOrEmpty(lowestLevelLibrary.config.ConfTypePrefix) 
                        ?  lowestLevelLibrary.config.Prefix + "_" 
                        : lowestLevelLibrary.config.Prefix + "_" + lowestLevelLibrary.config.ConfTypePrefix + "_";
                foreach (var File in filesInScopeToRefactorCopy)
                {
                    refactorer.ChangeNameOfFile(Path.GetFileName(File), Path.Combine(pathToOutput , prefixToAddToAllFilesNames + Path.GetFileName(File)),false);

                }
                refactorer.ReloadRefactorer();

                //add the prefix as a namespace to all files as well.
                refactorer.InsertNamespaceIntoAllFiles(prefixToAddToAllFilesNames);
                refactorer.ReloadRefactorer();


                //finally change the names of all the myincludes and myccompiles
                lowestLevelLibrary.SetPrefixToCLCompileFiles(prefixToAddToAllFilesNames);
                lowestLevelLibrary.SetPrefixToCLIncFiles(prefixToAddToAllFilesNames);


                //5. --------------------- 
                //revert it back to its previous state
                LibGitCleanUp.UncheckoutLibraryCheckedOutSoFar(lowestLevelLibrary);
                Console.WriteLine("Files have been imported for library " + lowestLevelLibrary.config.ClassName);
            }



        }


        public string GetLibraryThatDoesNOTSupportPlatform(SaveFilecgenProjectGlobal saveFilecgenProjectGlobal)
        {
            string platformsetup = GetPlatFormThisisSetupFor();

            foreach (var library in Libraries)
            {
                if (library.config.ClassName != "GlobalBuildConfig")
                {
                    cgenProjectGlobal projG = saveFilecgenProjectGlobal.CgenProjects.Projects.First(p => p.PathOfProject == library.settings.PATHOfProject);
                    var anyInScope = projG.PlatFormsInScope.PlatForms.FirstOrDefault(plat=> plat.PlatFormName == platformsetup);
                    if (anyInScope == null)
                    {
                        return projG.NameOfProject;
                    }
                }
            }

            return null;
        }
    }
}
