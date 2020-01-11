using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using CodeGenerator.ProblemHandler;
using CommandLine;
using CPPParser;
using Microsoft.SqlServer.Server;

namespace CodeGenerator.FileTemplates
{
    public abstract class FileTemplateBase
    {
        private string v1;
        private string v2;

        protected List<Macro> Macros { get; set; }
        protected List<MacroGroupLoop> MacroLoopGroups { get; set; }
        public string PathTOFileTemplate { get; protected set; }
        protected string TemplateOutputDestination { get; set; }
        public string NameOfTemplateFile { get; }
        public string NameOfOutputTemplateFile { get; set; }

        private List<UserCode> UserCodes;

        public FileTemplateBase(string templateOutputDestination, string nameOfTemplateFile, string nameOfOutputTemplateFile)
        {
            Macros = new List<Macro>();
            MacroLoopGroups = new List<MacroGroupLoop>();//new List<List<Macro>>();
            UserCodes = new List<UserCode>();

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


            //read all usercode sections from the generated file and save it  
            string generatedContent = "";
            string pathToGeneratedFile = Path.Combine(TemplateOutputDestination, NameOfOutputTemplateFile);
            //if that file does not exist, then create the file
            if (!File.Exists(pathToGeneratedFile))
            {
                File.Create(pathToGeneratedFile);

            }
            //wait for file to be ccreated
            for (int i = 0; i < 2000; i++)
            {
                if ((!File.Exists(pathToGeneratedFile)))
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
                else
                {
                    break;
                }
                ProblemHandle problem = new ProblemHandle();
                problem.ThereisAProblem("the file "+ pathToGeneratedFile + "in the macro needs to be created first as it does not exist yet");
            } 
            try
            {
                using (StreamReader sr = new StreamReader(pathToGeneratedFile))
                {
                    generatedContent = sr.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                ProblemHandle problem = new ProblemHandle();
                problem.ThereisAProblem("the file " + pathToGeneratedFile + "is not able to be accessed yet. make sure it is not open and is created");
            }

            string patternUserCode = @"\/\/UserCode_Section(.*)((.|\n)*)\/\/UserCode_Section\1_end";
            var matches = Regex.Matches(generatedContent, patternUserCode, RegexOptions.Multiline);
            foreach (Match mmat in matches)
            {
                string prefix = mmat.Groups[1].Value;
                UserCode userCode = new UserCode(mmat.Groups[2].Value.Trim(), mmat.Groups[1].Value);
                UserCodes.Add(userCode);
            }


            //look for any macro loops first and replace these first.
            string patternForLoop = @"##LOOP_(.*?)##((.|\n)*?)##END##";
            Match m = Regex.Match(templateFileContents, patternForLoop, RegexOptions.Multiline);
            int indexOfLoop = 1;
            while (m.Success)
            {
                //go through each loop section found 
                int index = m.Groups[0].Index;
                int lengthofSubstring = m.Groups[0].Length;
                string subLoopContents = m.Groups[2].Value;
                Debug.Assert((this.MacroLoopGroups.Count != 0), "you need to create a macro loop that matches macros for the loop section");



                //find the macro group associated with this loop section

                int MacroLoopGroupToUseIndex = 0;
                MacroGroupLoop macroGroupForThisSection = MacroLoopGroups.FirstOrDefault(macroGroup =>
               {
                   return m.Groups[1].Value == macroGroup.NameOfLoop;
                   //return !macroGroup.Any(macro =>  !Regex.Match(subLoopContents, @"<#\s*" + macro.NameInFile + @"\s*#>")); 
               });
                Debug.Assert((macroGroupForThisSection != null), "you need to create a macro loop that matches macros for the loop section");

                //insert new macro filled in snippet AFTER the loop section
                int totalLoopUnfoldedCount = 0;
                int countAdjustedForEndLength = 0;
                int indexAdjustedForLOOPLength = 0;
                int totalReplaced;
                foreach (var macroGroup in macroGroupForThisSection.MacroGroups)
                {
                    indexAdjustedForLOOPLength = index + 9 + macroGroupForThisSection.NameOfLoop.Length;
                    countAdjustedForEndLength = lengthofSubstring - 7 - 9 - macroGroupForThisSection.NameOfLoop.Length;

                    templateFileContents = InsertMacrosWithinContents(templateFileContents, macroGroup, indexAdjustedForLOOPLength, countAdjustedForEndLength, index + lengthofSubstring, out totalReplaced);
                    totalLoopUnfoldedCount += totalReplaced;


                }
                //remove the loop section I just finished doing
                templateFileContents = templateFileContents.Remove(index, lengthofSubstring);

                //replace ##UserCode. prefix  will be indexOfMacro_numofLoop_Iteration 
                int adjustedindex = index;
                int countReplaced;
                for (int i = 0; i < macroGroupForThisSection.MacroGroups.Count; i++)
                {
                    string substringOfUnfoldedLoop = templateFileContents.Substring(adjustedindex, totalLoopUnfoldedCount / macroGroupForThisSection.MacroGroups.Count);

                    substringOfUnfoldedLoop = ReplaceUserCodeMacroWith(substringOfUnfoldedLoop, "_Loop" + indexOfLoop + "_Iteration" + (i + 1), out countReplaced);
                    templateFileContents = templateFileContents.Remove(adjustedindex, totalLoopUnfoldedCount / macroGroupForThisSection.MacroGroups.Count);
                    templateFileContents = templateFileContents.Insert(adjustedindex, substringOfUnfoldedLoop);

                    adjustedindex += substringOfUnfoldedLoop.Length;
                }


                m = Regex.Match(templateFileContents, patternForLoop, RegexOptions.Multiline);
                indexOfLoop++;

            }


            int countReplaced2 = 0;
            templateFileContents = ReplaceUserCodeMacroWith(templateFileContents, "", out countReplaced2);


            //get that and replace all macros with their approprieate values 
            templateFileContents = ReplaceMacrosWithinContents(templateFileContents, Macros);


            //go through each user code found and replace contents 
            foreach (var userCode in UserCodes)
            {
                var mU = Regex.Match(templateFileContents, userCode.FullBeginName+"$", RegexOptions.Multiline);

                Debug.Assert(mU.Success, "could not find the UserCode section " + userCode.FullBeginName + "for file " + NameOfOutputTemplateFile + ". did you maybe change the template file where you removed that ##UserCode? if you did, you need to delete that usercode generated in the output file.");

                if (!userCode.Contents.IsAnEmptyLine())
                {
                    templateFileContents = templateFileContents.Insert(mU.Index + mU.Length, "\n" + userCode.Contents);
                }
                
            }

            //now write this into the destination File
            //this will overwrite the file itf it exists file does not exist
            string FullFilePathDestination = Path.Combine(TemplateOutputDestination, NameOfOutputTemplateFile);
            File.WriteAllText(FullFilePathDestination, templateFileContents);
        }


        private static string ReplaceUserCodeMacroWith(string content, string prefix, out int countReplaced)
        {
            string strToReturn = content;

            //replace all UserCode macros with a comment and a prefix for the sextion of usercode
            string patternForUserCode = @"##UserCode";
            var mm = Regex.Match(strToReturn, patternForUserCode, RegexOptions.Multiline);
            int indexforUserCode = 0;
            while (mm.Success)
            {
                strToReturn = strToReturn.Remove(mm.Index, mm.Length);
                strToReturn = strToReturn.Insert(mm.Index, "//UserCode_Section" + indexforUserCode + prefix + "\n" + "//UserCode_Section" + indexforUserCode + prefix + "_end");
                mm = Regex.Match(strToReturn, patternForUserCode, RegexOptions.Multiline);
                indexforUserCode++;

            }

            countReplaced = strToReturn.Length;
            return strToReturn;
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

        private static string InsertMacrosWithinContents(string contentWithMacros, List<Macro> macros, int startindex, int count, int indexOfInsertion, out int countReplaced)
        {
            //first remove the contents from that range
            string substringToReplace = contentWithMacros.Substring(startindex, count);

            //get that and replace all macros with their approprieate values
            foreach (var macro in macros)
            {
                substringToReplace = Regex.Replace(substringToReplace, @"<#\s*" + macro.NameInFile + @"\s*#>", macro.ValueToReplace);
            }

            countReplaced = substringToReplace.Length;
            return contentWithMacros.Insert(indexOfInsertion, substringToReplace);
        }


    }



    public class UserCode
    {
        public string Contents { get; set; }
        public string Prefix { get; }
        public string FullBeginName { get; }
        public string FullEndName { get; }

        public UserCode(string contents, string fullPrefix)
        {
            Contents = contents;
            Prefix = fullPrefix;

            FullBeginName = @"//UserCode_Section" + fullPrefix;
            FullEndName = @"//UserCode_Section" + fullPrefix + "_end";
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
