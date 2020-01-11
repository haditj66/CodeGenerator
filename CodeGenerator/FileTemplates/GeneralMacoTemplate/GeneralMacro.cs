using CodeGenerator.FileTemplates.Files;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeGenerator.FileTemplates.GeneralMacoTemplate
{
    public class GeneralMacro
    { 

        public string PathtoTemplateFileAndOutputFiles { get; }
        public string NameOfcGenMacroFile { get; }

        FileTemplateGeneral GeneralTemplate;

        public GeneralMacro(string pathtoTemplateFileAndOutputFiles, string nameOfcGenMacroFile )
        {
            PathtoTemplateFileAndOutputFiles = pathtoTemplateFileAndOutputFiles;
            NameOfcGenMacroFile = nameOfcGenMacroFile;
        }


        public void CreateTemplate()
        {

            //get the ##ToFile value 
            string pathTofile = Path.Combine(PathtoTemplateFileAndOutputFiles, NameOfcGenMacroFile);
            string contents = File.ReadAllText(pathTofile);
            string patternForToFile = @"##ToFile\s+(.*)";
            MatchCollection matches = Regex.Matches(contents, patternForToFile, RegexOptions.Multiline);

            //macke sure there was at least one match
            if (matches.Count == 0)
            {
                Debug.Assert(false, "you need a ##ToFile with the file name of your destination file that the macro will generate code to");
            }



            while (matches.Count > 0)
            {
                var mm = matches[matches.Count - 1];
                GeneralTemplate = new FileTemplateGeneral(PathtoTemplateFileAndOutputFiles, NameOfcGenMacroFile, mm.Groups[1].Value.Trim());

                //get all contents after the ##ToFile stuff
                string macroContents = contents.Substring(mm.Index + mm.Length, contents.Length - (mm.Index + mm.Length));

                //write that in General.txt  
                string pathToGeneraltxt = Path.Combine(GeneralTemplate.PathTOFileTemplate, GeneralTemplate.NameOfTemplateFile);
                string macroContents_gen = "//generated file: " + GeneralTemplate.NameOfOutputTemplateFile + "\n" + macroContents;
                File.WriteAllText(pathToGeneraltxt, macroContents_gen);

                //generate template
                GeneralTemplate.CreateTemplate();

                //remove the contents generated so far
                contents = contents.Remove(mm.Index, contents.Length - (mm.Index));

                //get the next matches
                matches = Regex.Matches(contents, patternForToFile, RegexOptions.Multiline);
            }

        }
    }
}
