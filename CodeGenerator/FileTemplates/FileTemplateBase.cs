using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeGenerator.FileTemplates
{
    public abstract class FileTemplateBase
    {
        private string v1;
        private string v2;

        protected List<Macro> Macros { get; set; }
        protected List<MacroGroupLoop> MacroLoopGroups { get; set; }
        protected string PathTOFileTemplate { get; set; }
        protected string TemplateOutputDestination { get; set; }
        public string NameOfTemplateFile { get; }
        public string NameOfOutputTemplateFile { get; }

        public FileTemplateBase(string templateOutputDestination, string nameOfTemplateFile, string nameOfOutputTemplateFile)
        {
            Macros = new List<Macro>();
            MacroLoopGroups = new List<MacroGroupLoop>();//new List<List<Macro>>();

            PathTOFileTemplate = @"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\CodeGenerator\FileTemplates\Files";
            TemplateOutputDestination = templateOutputDestination;
            Debug.Assert(nameOfTemplateFile.Contains("."), "name of file needs an extension");
            NameOfTemplateFile = nameOfTemplateFile;
            Debug.Assert(nameOfOutputTemplateFile.Contains("."), "name of file needs an extension");
            NameOfOutputTemplateFile = nameOfOutputTemplateFile;
        }


        public void CreateTemplate()
        {
            //read from the template file
            string templateFileContents = "";
            string FullPathToTemplate = Path.Combine(PathTOFileTemplate, NameOfTemplateFile);
            using (StreamReader sr = new StreamReader(FullPathToTemplate))
            {
                templateFileContents = sr.ReadToEnd();
            }

            //look for any macro loops first and replace these first.
            string patternForLoop = @"##LOOP_(.*?)##((.|\n)*?)##END##";
            Match m = Regex.Match(templateFileContents, patternForLoop, RegexOptions.Multiline);
            while (m.Success)
            {
                //go through each loop section found 
                int index = m.Groups[0].Index;
                int lengthofSubstring = m.Groups[0].Length;
                string subLoopContents = m.Groups[2].Value;

                //find the macro group associated with this loop section
                Debug.Assert((this.MacroLoopGroups.Count != 0), "you need to create a macro loop that matches macros for the loop section");
                int MacroLoopGroupToUseIndex = 0;
                MacroGroupLoop macroGroupForThisSection = MacroLoopGroups.FirstOrDefault(macroGroup =>
               {
                   return m.Groups[1].Value == macroGroup.NameOfLoop;
                        //return !macroGroup.Any(macro =>  !Regex.Match(subLoopContents, @"<#\s*" + macro.NameInFile + @"\s*#>")); 
                    });
                Debug.Assert((macroGroupForThisSection != null), "you need to create a macro loop that matches macros for the loop section");

                //insert new macro filled in snippet AFTER the loop section
                foreach (var macroGroup in macroGroupForThisSection.MacroGroups)
                {
                    int indexAdjustedForLOOPLength = index + 9 + macroGroupForThisSection.NameOfLoop.Length;
                    int countAdjustedForEndLength = lengthofSubstring - 7 - 9 - macroGroupForThisSection.NameOfLoop.Length;
                    templateFileContents = InsertMacrosWithinContents(templateFileContents, macroGroup, indexAdjustedForLOOPLength, countAdjustedForEndLength, index + lengthofSubstring);

                }
                //remove the loop section I just finished doing
                templateFileContents = templateFileContents.Remove(index, lengthofSubstring);

                m = Regex.Match(templateFileContents, patternForLoop, RegexOptions.Multiline);

            }

            //get that and replace all macros with their approprieate values 
            templateFileContents = ReplaceMacrosWithinContents(templateFileContents, Macros);


            //now write this into the destination File
            //this will overwrite the file itf it exists file does not exist
            string FullFilePathDestination = Path.Combine(TemplateOutputDestination, NameOfOutputTemplateFile);
            File.WriteAllText(FullFilePathDestination, templateFileContents);
        }


        private static string ReplaceMacrosWithinContents(string contentWithMacros, List<Macro> macros, int startindex = 0, int count = 0)
        {
            if (count == 0)
            {
                count = contentWithMacros.Length;
            }
            //first remove the contents from that range
            string substringToReplace = contentWithMacros.Substring(startindex, count);
            string contentsWithRemovedSubstring = contentWithMacros.Remove(startindex, count);

            //get that and replace all macros with their approprieate values
            foreach (var macro in macros)
            {
                substringToReplace = Regex.Replace(substringToReplace, @"<#\s*" + macro.NameInFile + @"\s*#>", macro.ValueToReplace);
            }

            return contentsWithRemovedSubstring.Insert(startindex, substringToReplace);
        }

        private static string InsertMacrosWithinContents(string contentWithMacros, List<Macro> macros, int startindex, int count, int indexOfInsertion)
        {
            //first remove the contents from that range
            string substringToReplace = contentWithMacros.Substring(startindex, count);

            //get that and replace all macros with their approprieate values
            foreach (var macro in macros)
            {
                substringToReplace = Regex.Replace(substringToReplace, @"<#\s*" + macro.NameInFile + @"\s*#>", macro.ValueToReplace);
            }

            return contentWithMacros.Insert(indexOfInsertion, substringToReplace);
        }


    }




    public class Macro
    {
        public string NameInFile;
        public string ValueToReplace;
        public Macro(string NameInFile, string ValueToReplace)
        {
            this.NameInFile = NameInFile;
            this.ValueToReplace = ValueToReplace;
        }

    }

    public class MacroGroupLoop
    {
        public string NameOfLoop { get; }
        private readonly string[] _macroNames; 
        public List<List<Macro>> MacroGroups;


        public MacroGroupLoop(string nameOfLoop, params string[] MacroNames)
        {
            MacroGroups = new List<List<Macro>>();
            NameOfLoop = nameOfLoop;
            _macroNames = MacroNames;
        }

        public void AddNewGroup(params string[] MacroValues)
        {
            List<Macro> macroGrouptoAdd = new List<Macro>();
            for (int i = 0; i < _macroNames.Length; i++)
            {
                Macro macro = new Macro(_macroNames[i], MacroValues[i]);
                macroGrouptoAdd.Add(macro);
            }
            MacroGroups.Add(macroGrouptoAdd);

        }

    }
}
