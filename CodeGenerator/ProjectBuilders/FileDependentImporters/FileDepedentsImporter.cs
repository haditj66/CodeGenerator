using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CodeGenerator.IDESettingXMLs;
using ConsoleApp2.CPPRefactoring;

namespace CodeGenerator.ProjectBuilders.FileDependentImporters
{
    public class FileDepedentsImporter
    {
        public static string PathToTempfolder = "FileImporterTemp";
        protected string PrefixToAdd { get; }
        protected List<MyCLCompileFile> ClCompFilesToImport { get; }
        protected List<MyCLIncludeFile> ClIncFilesToImport { get; }
        public string BaseLocationOfFiles { get; }

        public FileDepedentsImporter(string prefixToAdd, List<MyCLCompileFile> clCompFilesToImport, List<MyCLIncludeFile> clIncFilesToImport, string baseLocationOfFiles)
        {
            PrefixToAdd = prefixToAdd;
            ClCompFilesToImport = clCompFilesToImport;
            ClIncFilesToImport = clIncFilesToImport;
            BaseLocationOfFiles = baseLocationOfFiles;
        }
         

        public void ImportFilesToPath(string PathToOutput)
        {

            //first erase all files in the temp FileImporter directory
            if (Directory.Exists(PathToTempfolder))
            {
                Directory.Delete(PathToTempfolder,true);
            } 
            Directory.CreateDirectory(PathToTempfolder);

            //I need to get all files, change their names with the prefix, put a copy of all files in the temp folder,
            //and change all mentions of their //header #include "name" to that name
            List<string> allFilePaths = new List<string>();// = ClCompFilesToImport.Select((MyCLCompileFile comp) => comp.FullLocationName).ToList();
            //allFilePaths.AddRange(ClIncFilesToImport.Select((MyCLIncludeFile comp) => comp.FullLocationName).ToList());
            //create copy temp files 
            foreach (var clComp in ClCompFilesToImport)
            {
                string toFullPathName = Path.Combine(PathToOutput, clComp.Name);
                allFilePaths.Add(toFullPathName);
                
                File.Copy(Path.Combine(BaseLocationOfFiles,clComp.FullLocationName) , toFullPathName);

            }
            foreach (var CLinc in ClIncFilesToImport)
            {
                string toFullPathName = Path.Combine(PathToOutput, CLinc.Name);
                allFilePaths.Add(toFullPathName);
                File.Copy(Path.Combine(BaseLocationOfFiles, CLinc.FullLocationName), toFullPathName);
            }



            //allFilePaths.ForEach((string pathstr) => { pathstr = PrefixToAdd+pathstr; });




            //change namespace for all files with the 

            //put the namespace in there. since this is a .cpp file. it wont have #pragma once or a header gaurd. 


        }


    }
}
