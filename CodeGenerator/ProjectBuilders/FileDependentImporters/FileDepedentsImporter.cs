using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CodeGenerator.IDESettingXMLs;

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
              
            foreach (var clcomp in ClCompFilesToImport)
            {
                //read all contents of the ccomp
                List<string> contents = File.ReadAllLines(Path.Combine(BaseLocationOfFiles,clcomp.FullLocationName)).ToList();


                //put the namespace in there. since this is a .cpp file. it wont have #pragma once or a header gaurd. 
                //so look through and put the namespace after the last mention of a #include 
                contents = SetNameSPaceForCPPContents(contents);

            }

        }

        private static List<string> SetNameSPaceForCPPContents(List<string> contents)
        {
            int index = 0;
            bool previousLineWasInclude = true;
            foreach (var content in contents)
            {
                //ignore comment lines and #include lines
                if (content.Contains("#include") || Regex.IsMatch(content, @"^\s*//"))
                {
                    
                } 
                index++;
            }

            return null;
        }
         
    }
}
