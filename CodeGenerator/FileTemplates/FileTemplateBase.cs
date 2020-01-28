﻿using System;
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
                int loopindex = 1;//Convert.ToInt32(((Macro)(macroGroupForThisSection.MacroGroups[0].Where(macr => macr.NameInFile == "i")).First()).ValueToReplace);
                bool islastIndex = true;
                macroGroupForThisSection.MacroGroups.Reverse();
                foreach (var macroGroup in macroGroupForThisSection.MacroGroups)
                {
                    indexAdjustedForLOOPLength = index + 9 + macroGroupForThisSection.NameOfLoop.Length;
                    countAdjustedForEndLength = lengthofSubstring - 7 - 9 - macroGroupForThisSection.NameOfLoop.Length;
                     
                    string IfLoopContentsFiltered = RemoveContentOfInvalidIfLoop(templateFileContents, loopindex, indexAdjustedForLOOPLength, countAdjustedForEndLength, islastIndex);
                     
                    string MacrosInsertedSubstring = InsertMacrosWithinContents(IfLoopContentsFiltered, macroGroup,0, IfLoopContentsFiltered.Length);// indexAdjustedForLOOPLength, countAdjustedForEndLength, index + lengthofSubstring, out totalReplaced);
                     
                    //append this to the end of the templatecontents AFTER the loop
                    templateFileContents = templateFileContents.Insert(index + lengthofSubstring + totalLoopUnfoldedCount, MacrosInsertedSubstring);
                    totalLoopUnfoldedCount += MacrosInsertedSubstring.Length;

                    islastIndex = false;
                    loopindex++;
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

        private static string InsertMacrosWithinContents(string contentWithMacros, List<Macro> macros, int startindex, int count)
        {
            //first remove the contents from that range
            string substringToReplace = contentWithMacros;//contentWithMacros.Substring(startindex, count);

            //get that and replace all macros with their approprieate values
            foreach (var macro in macros)
            {
                substringToReplace = Regex.Replace(substringToReplace, @"<#\s*" + macro.NameInFile + @"\s*#>", macro.ValueToReplace);
            }

            return substringToReplace;

            //countReplaced = substringToReplace.Length;
            //return contentWithMacros.Insert(indexOfInsertion, substringToReplace);
        }



        private static string RemoveContentOfInvalidIfLoop(string contentsOfLoopSection, int loopnum, int startindex, int count, bool isLastIndex = false)
        {
            //first remove the contents from that range
            string substringToReplace = contentsOfLoopSection.Substring(startindex, count);


            //completely remove any contenets inbetween a ##i1## some content ##i1END## 
            //that dont match the current index of the loop.
            string patternIfIndex = @"<#if(\d+?)#>((.|\n)*?)<#ifEND#>";
            Match mmm = Regex.Match(substringToReplace, patternIfIndex, RegexOptions.Multiline);
            while (mmm.Success)
            {
                int indexofIfIndex = Convert.ToInt32(mmm.Groups[1].Value);
                int startOfMatch = mmm.Groups[0].Index;
                int endofMatch = startOfMatch + mmm.Groups[0].Length;

                int sizeOfFirstTag = 7;
                if (indexofIfIndex > 9)
                {
                    sizeOfFirstTag++;
                }
                if (indexofIfIndex > 99)
                {
                    sizeOfFirstTag++;
                }

                //if Ifindex is less than the current index then remove all contents in there
                if (indexofIfIndex > loopnum)
                {
                    //remove tags first
                    substringToReplace = substringToReplace.Remove(startOfMatch, sizeOfFirstTag);
                    substringToReplace = substringToReplace.Remove(endofMatch - 9 - sizeOfFirstTag, 9);
                    //now remove constents with adjested lengths
                    startOfMatch = startOfMatch - sizeOfFirstTag +5;
                    endofMatch = endofMatch - sizeOfFirstTag ;
                    substringToReplace = substringToReplace.Remove(startOfMatch, mmm.Length - 14);
                }
                //else just remove the <##if##> tag
                else
                {
                    substringToReplace = substringToReplace.Remove(startOfMatch, sizeOfFirstTag);
                    substringToReplace = substringToReplace.Remove(endofMatch - 9 - sizeOfFirstTag, 9);
                    /*
                    //if this is the last loop index, remove all <#!L#>\<#EndL#> tags AND the contents
                    if (isLastIndex == true)
                    {
                        string patternForContents = @"<#!L#>((.|\n)*?)<#EndL#>";
                        substringToReplace = Regex.Replace(substringToReplace, patternForContents, "");
                        substringToReplace = Regex.Replace(substringToReplace, @"<#!L#>", "");
                        substringToReplace = Regex.Replace(substringToReplace, @"<#EndL#>", ""); 
                    }
                    else
                    {
                        //just remove the tags 
                        substringToReplace = Regex.Replace(substringToReplace, @"<#!L#>", "");
                        substringToReplace = Regex.Replace(substringToReplace, @"<#EndL#>", "");
                    }*/
                }

                mmm = Regex.Match(substringToReplace, patternIfIndex, RegexOptions.Multiline);
            }








            //now do the same thing for the NOT if tags
            string patternNotIfIndex = @"<#if!(\d+?)#>((.|\n)*?)<#if!END#>";
            Match mmmm = Regex.Match(substringToReplace, patternNotIfIndex, RegexOptions.Multiline);
            while (mmmm.Success)
            {
                int indexofIfIndex = Convert.ToInt32(mmmm.Groups[1].Value);
                int startOfMatch = mmmm.Groups[0].Index;
                int endofMatch = startOfMatch + mmmm.Groups[0].Length;

                int sizeOfFirstTag = 8;
                if (indexofIfIndex > 9)
                {
                    sizeOfFirstTag++;
                }
                if (indexofIfIndex > 99)
                {
                    sizeOfFirstTag++;
                }

                //if same loop then remove all contents
                if (indexofIfIndex == loopnum)
                {
                    //remove tags first
                    substringToReplace = substringToReplace.Remove(startOfMatch, sizeOfFirstTag);
                    substringToReplace = substringToReplace.Remove(endofMatch - 10 - sizeOfFirstTag, 10);
                    //now remove constents with adjested lengths
                    startOfMatch = startOfMatch - sizeOfFirstTag + 7;
                    endofMatch = endofMatch - sizeOfFirstTag;
                    substringToReplace = substringToReplace.Remove(startOfMatch, mmmm.Length - 16);
                }
                //else just remove the <##if##> tag
                else
                {
                    substringToReplace = substringToReplace.Remove(startOfMatch, sizeOfFirstTag);
                    substringToReplace = substringToReplace.Remove(endofMatch - 10 - sizeOfFirstTag, 10);
                  
                }

                mmmm = Regex.Match(substringToReplace, patternNotIfIndex, RegexOptions.Multiline);
            }



            return substringToReplace;

            //countReplaced = substringToReplace.Length;
            //return contentsOfLoopSection.Insert(indexOfInsertion, substringToReplace);
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

