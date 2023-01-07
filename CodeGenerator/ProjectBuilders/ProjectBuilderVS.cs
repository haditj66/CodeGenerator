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
//using ConsoleApp2.CPPRefactoring;


#if NOTDEPRECATED_GENERATE_AND_DEGENERATE


namespace CodeGenerator.ProjectBuilders
{
    public class ProjectBuilderVS : ProjectBuilderBase
    {
        public ProjectBuilderVS(IDESetting configSettings) : base(configSettings)
        {

        }


        protected override MyMainSettingsBase GetSettingsOVERRIDE(string pathToProjectSettings)
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
                    var includes = saveProjGlob.CgenProjects.Projects.First(p => p.PathOfProject == lib.settings.PATHOfProject)
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
                    if (IsCCompDependencyAbleForImporting(inc, lib.config.Prefix + "_", lib.config.Prefix + "_" + lib.config.ConfTypePrefix + "_"))
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

        public override void ImportDependentLibrariesFiles(bool usingGit)
        {
            //steps 
            //1. check out git tags for ALL dependent libraries
            //2. build configuration for that dependent library in temp folder
            //2.5 send that configurationCG.h file to final librarydependencies/xx/xx folder
            //3. get all files that are able to be imported in from dependent library (NOT main.cpp, ModuleConfig.h etc)
            //4. import those files from dependent library to main library.
            //5. git library back to commit it was before checked out



            //go through lowest level libraries first to import in      
            foreach (var libraryToImp in LibTop.LibrariesIDependOn)
            {

                //1. check out git tags for ALL dependent libraries
                if (usingGit)
                {
                    CheckoutLibraryToCorrectMajor(libraryToImp);
                }


                //2. ---------------------
                //create Configuration.h in temporary folder but dont put it in project so to not change anything 
                ConfigurationFileBuilder configFileBuilder = new ConfigurationFileBuilder(libraryToImp.settings, Program.CGCONFCOMPILATOINSBASEDIRECTORY, Program.DIRECTORYOFTHISCG, Program.PATHTOCONFIGTEST, ProblemHandle);
                configFileBuilder.CreateConfigurationToTempFolder();


                //create a configuration.h myinclude for the dependent library so to include that in the importing
                var filtForConfiguration_h = libraryToImp.GetAllFitlers().Where((MyFilter filt) =>
                {
                    return filt.GetFullAddress() == "Config";//Path.Combine("LibraryDependencies", lowestLevelLibrary.GetPathToProjectAsADependent());
                }).First();
                //todo the path here should be to the temp folder?
                string e = libraryToImp.GetPathToProjectAsADependent();
                MyCLIncludeFile clincConfig = new MyCLIncludeFile(filtForConfiguration_h, "ConfigurationCG.h", Path.Combine("LibraryDependencies", libraryToImp.GetPathToProjectAsADependent()));


                //2.5 send that configurationCG.h file to final librarydependencies/xx/xx folder. but first inherit values
                configFileBuilder.InheritFromTopConfig(libraryToImp.config);
                configFileBuilder.WriteTempConfigurationToFinalFile(Path.Combine(LibTop.settings.PATHOfProject, clincConfig.LocationOfFile, clincConfig.Name));

                //3. --------------------- 
                //grab all files that are qualified to be imported in and send them through  (NOT main.cpp, ModuleConfig.h etc)
                var CcompsToImport = libraryToImp.GetAllCCompile().Where((MyCLCompileFile ccom) => { return IsCCompDependencyAbleForImporting(ccom); }).ToList();
                var CIncToImport = libraryToImp.GetAllCincludes().Where((MyCLIncludeFile cinc) => { return IsCIncDependencyAbleForImporting(cinc); }).ToList();
                //CIncToImport.Add(clinc); dont do this yet as you only want to refactor this, not import it.



                //4. ---------------------import those files from dependent library to main library.
                FileDepedentsImporter FileImporter = new FileDepedentsImporter(libraryToImp.GetFullPrefix(), CcompsToImport, CIncToImport, libraryToImp.settings.PATHOfProject);
                string pathToOutput = Path.Combine(LibTop.settings.PATHOfProject, "LibraryDependencies", libraryToImp.GetPathToProjectAsADependent());
                FileImporter.ImportFilesToPath(pathToOutput);

                CIncToImport.Add(clincConfig);//the configuration that was written in needs to be added now
                libraryToImp.AddCIncludeFile(clincConfig);
                List<string> allFilePaths = new List<string>();
                allFilePaths.AddRange(CcompsToImport.Select(cinc => Path.Combine(pathToOutput, cinc.Name)));
                allFilePaths.AddRange(CIncToImport.Select(cinc => Path.Combine(pathToOutput, cinc.Name)));


                //refactor files -------------------------------------------------------------------------
                var isStaticdef = libraryToImp.config.Defines.Define.FirstOrDefault(d => d.DefineName == "STATIC");
                bool isStatic = isStaticdef == null ? true : Convert.ToInt32(isStaticdef.Value) == 0 ? false : true;

                CppRefactorer refactorer = new CppRefactorer(allFilePaths);
                List<string> filesInScopeToRefactorCopy = new List<string>(refactorer.FilesInScopeToRefactor);
                string prefixToAddToAllFilesNames = string.IsNullOrEmpty(libraryToImp.config.ConfTypePrefix)
                        ? libraryToImp.config.Prefix + "_"
                        : libraryToImp.config.Prefix + "_" + libraryToImp.config.ConfTypePrefix + "_";



                if (isStatic)
                {

                    //change names of ONLY the configuration file
                    foreach (var File in filesInScopeToRefactorCopy)
                    {
                        if (Path.GetFileName(File) == "ConfigurationCG.h")
                        {
                            refactorer.ChangeNameOfFile(Path.GetFileName(File),
                                Path.Combine(pathToOutput, prefixToAddToAllFilesNames + Path.GetFileName(File)), false);
                        }
                    }
                    refactorer.ReloadRefactorer();
                }

                if (!isStatic)
                {
                    var definesDict = refactorer.GetAllDefines(clincConfig.Name);
                    foreach (var file in filesInScopeToRefactorCopy)
                    {
                        foreach (var define in definesDict)
                        {
                            refactorer.RenameDefine(file, define.Key, prefixToAddToAllFilesNames + define.Key);
                        }
                    }


                    /*
                    //replace the files defines from the configuration folder values. otherwise the define names will conflict with the main program configuration defines.
                        foreach (var file in filesInScopeToRefactorCopy)
                    {
                        string configContents = File.ReadAllText(Path.Combine(LibTop.settings.PATHOfProject, clincConfig.LocationOfFile, clincConfig.Name));
                        refactorer.ReplaceDefinesWithDefineValueFile(file, configContents);
                    }*/

                    refactorer.ReloadRefactorer();
                    //change names of all imported files 
                    foreach (var File in filesInScopeToRefactorCopy)
                    {
                        refactorer.ChangeNameOfFile(Path.GetFileName(File),
                            Path.Combine(pathToOutput, prefixToAddToAllFilesNames + Path.GetFileName(File)), false);

                    }


                    refactorer.ReloadRefactorer();



                    //add the prefix as a namespace to all files as well. however remove the last character if it is a "_" 
                    string namespaceprefix = prefixToAddToAllFilesNames.Last().Equals('_')
                        ? prefixToAddToAllFilesNames.Remove(prefixToAddToAllFilesNames.Length - 1, 1)
                        : prefixToAddToAllFilesNames;
                    refactorer.InsertNamespaceIntoAllFiles(namespaceprefix);
                    refactorer.ReloadRefactorer();
                }



                //change #include mentions of librarydependencies to have just the file name
                foreach (var file in refactorer.FilesInScopeToRefactor)
                {
                    string fullPath = refactorer.GetFullFilePathFromFileNameInScope(file);
                    string[] contents = File.ReadAllLines(fullPath);
                    string[] contentsNew = File.ReadAllLines(fullPath);
                    int index = 0;

                    foreach (var line in contents)
                    {


                        if (Regex.IsMatch(line, @"#include.*LibraryDependencies"))
                        {
                            if (!isStatic)
                            {
                                //get the include and replace it with just the file name
                                Match m = Regex.Match(line, @"#include\s*""(.*)""");
                                contentsNew[index] =
                                    contentsNew[index].Remove(m.Groups[1].Index, m.Groups[1].Length);
                                contentsNew[index] = contentsNew[index].Insert(m.Groups[1].Index,
                                    Path.GetFileName(m.Groups[1].Value));
                            }
                            else
                            {
                                //if it is a static, then only change  ConfigurationCG.h
                                if (line.Contains("ConfigurationCG.h"))
                                {
                                    Match m = Regex.Match(line, @"#include\s*""(.*)""");
                                    contentsNew[index] =
                                        contentsNew[index].Remove(m.Groups[1].Index, m.Groups[1].Length);
                                    contentsNew[index] = contentsNew[index].Insert(m.Groups[1].Index,
                                        Path.GetFileName(m.Groups[1].Value));
                                }

                            }
                        } 


                        index++;
                    }



                    File.WriteAllLines(fullPath, contentsNew);
                }



                /* DONT
                //remove contents of configuration
                foreach (var file in refactorer.FilesInScopeToRefactor)
                {
                    if (file.Contains("_ConfigurationCG.h"))
                    {
                        string fullPath = refactorer.GetFullFilePathFromFileNameInScope(file); 

                        File.WriteAllText(fullPath, "");
                    } 
                }*/

                if (!isStatic)
                {
                    //finally change the names of all the myincludes and myccompiles
                    libraryToImp.SetPrefixToCLCompileFiles(prefixToAddToAllFilesNames);
                    libraryToImp.SetPrefixToCLIncFiles(prefixToAddToAllFilesNames);
                }
                else
                {
                    //if it is not static, just set prefix to configurationfile
                    libraryToImp.SetPrefixToCLIncFiles(prefixToAddToAllFilesNames,"ConfigurationCG.h");
                }

                //5. --------------------- 
                //revert it back to its previous state
                if (usingGit)
                {
                    LibGitCleanUp.UncheckoutLibraryCheckedOutSoFar(libraryToImp);
                }

                Console.WriteLine("Files have been imported for library " + libraryToImp.config.ClassName + "\n");
            }



        }

        public void ImportConfigFiles(bool usingGit)
        {
            //steps 
            //1. check out git tags for ALL dependent libraries
            //2. build configuration for that dependent library in temp folder
            //2.5 send that configurationCG.h file to final librarydependencies/xx/xx folder
            //3. get all files that are able to be imported in from dependent library (NOT main.cpp, ModuleConfig.h etc)
            //4. import those files from dependent library to main library.
            //5. git library back to commit it was before checked out 

            //go through lowest level libraries first to import in      
            foreach (var libraryToImp in LibTop.LibrariesIDependOn)
            {

                //1. check out git tags for ALL dependent libraries
                if (usingGit)
                {
                    CheckoutLibraryToCorrectMajor(libraryToImp);
                }


                //2. ---------------------
                //create Configuration.h in temporary folder but dont put it in project so to not change anything 
                ConfigurationFileBuilder configFileBuilder = new ConfigurationFileBuilder(libraryToImp.settings, Program.CGCONFCOMPILATOINSBASEDIRECTORY, Program.DIRECTORYOFTHISCG, Program.PATHTOCONFIGTEST, ProblemHandle);
                configFileBuilder.CreateConfigurationToTempFolder();


                //create a configuration.h myinclude for the dependent library so to include that in the importing
                var filtForConfiguration_h = libraryToImp.GetAllFitlers().Where((MyFilter filt) =>
                {
                    return filt.GetFullAddress() == "Config";//Path.Combine("LibraryDependencies", lowestLevelLibrary.GetPathToProjectAsADependent());
                }).First();

                string e = libraryToImp.GetPathToProjectAsADependent();
                MyCLIncludeFile clincConfig = new MyCLIncludeFile(filtForConfiguration_h, "Configuration.h", Program.CGCONFCOMPILATOINSBASEDIRECTORY);



                //2.5 send that configurationCG.h file to final librarydependencies/xx/xx folder. but first inherit values
                configFileBuilder.InheritFromTopConfig(libraryToImp.config);

                //refactor config in temporary first
                string pathToOutput = Path.Combine(LibTop.settings.PATHOfProject, "LibraryDependencies", libraryToImp.GetPathToProjectAsADependent());
                CppRefactorer refactorer = new CppRefactorer(new List<string>() { Path.Combine(Program.DIRECTORYOFTHISCG, Program.CGCONFCOMPILATOINSBASEDIRECTORY, "Configuration.h") });
                List<string> filesInScopeToRefactorCopy = new List<string>(refactorer.FilesInScopeToRefactor);
                string prefixToAddToAllFilesNames = string.IsNullOrEmpty(libraryToImp.config.ConfTypePrefix)
                    ? libraryToImp.config.Prefix + "_"
                    : libraryToImp.config.Prefix + "_" + libraryToImp.config.ConfTypePrefix + "_";

                //refactor names of defines in config
                var definesDict = refactorer.GetAllDefines(clincConfig.Name);
                foreach (var file in filesInScopeToRefactorCopy)
                {
                    foreach (var define in definesDict)
                    {
                        refactorer.RenameDefine(file, define.Key, prefixToAddToAllFilesNames + define.Key);
                    }
                }

                configFileBuilder.WriteTempConfigurationToFinalFile(Path.Combine(LibTop.settings.PATHOfProject, "LibraryDependencies", libraryToImp.GetPathToProjectAsADependent(), prefixToAddToAllFilesNames + "ConfigurationCG.h"));

                //5. --------------------- 
                //revert it back to its previous state
                if (usingGit)
                {
                    LibGitCleanUp.UncheckoutLibraryCheckedOutSoFar(libraryToImp);
                }

                Console.WriteLine("Configuration has been imported for library " + libraryToImp.config.ClassName);
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
                    var anyInScope = projG.PlatFormsInScope.PlatForms.FirstOrDefault(plat => plat.PlatFormName == platformsetup);
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


#endif