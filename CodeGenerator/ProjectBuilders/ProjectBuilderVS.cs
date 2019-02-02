using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGenerator.IDESettingXMLs;
using System.IO;
using CodeGenerator.CMD_Handler;
using CodeGenerator.IDESettingXMLs.VisualStudioXMLs;
using ExtensionMethods;
using CodeGenerator.FileTemplates;
using CodeGenerator.GitHandlerForLibraries;
using CodeGenerator.ProblemHandler;
using CodeGenerator.ProjectBuilders.FileDependentImporters;

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

        public override void ImportDependentLibrariesCincAndCcompAndAdditional()
        {

            //5. I need to get all the cIncludes, cClompiles, additionalincludes from the other libraries and add to top level
            // to the top level library. 

            //getting and adding all additional includes
            AllNotTopAndNotGlobal
                .ForEach((Library lib) =>
                {
                    lib.GetAllAdditionalIncludes()
                    .ForEach((string inc) =>
                    {
                        LibTop.AddAdditionalIncludes(inc);
                    });
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
                    if (IsCCompDependencyAbleForImporting(inc))
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

        public override void CreateCCompCincDependencyFiles()
        { 
            var lowestLevelLibraries = LibTop.LibrariesIDependOn.Where((Library lib) =>
            {
                return lib.LibrariesIDependOn == null || lib.LibrariesIDependOn.Count == 0;
            }).ToList();
            //go through each of these and check out their tags. make a list of libraries already checked out.      
            foreach (var lowestLevelLibrary in lowestLevelLibraries)
            {
                CheckoutLibraryToCorrectMajor(lowestLevelLibrary);

                //grab all files that are qualified to be imported in and send them through the 
                var CcompsToImport = lowestLevelLibrary.GetAllCCompile().Where((MyCLCompileFile ccom) => { return IsCCompDependencyAbleForImporting(ccom);}).ToList();
                var CIncToImport = lowestLevelLibrary.GetAllCincludes().Where((MyCLIncludeFile cinc) => { return IsCIncDependencyAbleForImporting(cinc); }).ToList();
                FileDepedentsImporter FileImporter = new FileDepedentsImporter(lowestLevelLibrary.GetFullPrefix(), CcompsToImport, CIncToImport);
                FileImporter.ImportFilesToPath(Path.Combine(LibTop.settings.PATHOfProject + lowestLevelLibrary.GetPathToProjectAsADependent()));

                //revert it back to its previous state
                LibGitCleanUp.UncheckoutLibraryCheckedOutSoFar(lowestLevelLibrary);
            }
        }




         
    }
}
