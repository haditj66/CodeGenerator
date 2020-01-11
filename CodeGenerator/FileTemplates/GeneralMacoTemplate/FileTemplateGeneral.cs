using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeGenerator.FileTemplates.Files
{
     

    public class FileTemplateGeneral : FileTemplateBase
    {
        private List<int> LoopIncrement = new List<int>(){ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        private List<string> generalMacros = new List<string>(){"", "", "", "", "", "", "", "", "", ""};


        public FileTemplateGeneral(string templateOutputDestination,string nameOfcGenMacroFile, string nameOfOutputTemplateFile) : base(templateOutputDestination, "General.txt", nameOfOutputTemplateFile)
        {

            string pathTofile = Path.Combine(templateOutputDestination, nameOfcGenMacroFile);
            string contents = File.ReadAllText(pathTofile);

            //get all ##LoopIncrement1 values if any
            for (int i = 1; i <= 10; i++)
            {
                string patternForLoopIncrement = @"##LoopIncrement"+i+@"\s+(.*)";
                Match mmm = Regex.Match(contents, patternForLoopIncrement, RegexOptions.Multiline);
                if (mmm.Success)
                {
                    try
                    {
                        int valueOfInc = Convert.ToInt32(mmm.Groups[1].Value);
                        LoopIncrement[i-1] = valueOfInc;
                    }
                    catch (Exception e)
                    { 
                    }  
                }
            }

            //get all ##macrogroup values if any
            for (int i = 1; i <= 10; i++)
            {
                string patternForLoopIncrement = @"##Macro" + i + @"\s+(.*)";
                Match mmm = Regex.Match(contents, patternForLoopIncrement, RegexOptions.Multiline);
                if (mmm.Success)
                {
                    try
                    {
                        generalMacros[i - 1] = mmm.Groups[1].Value.Trim(); 
                    }
                    catch (Exception e)
                    {
                    }
                }
            }

            /* 
##ToFile testForGeneral.h 
 
hi, macro 2 is <#Macro2#>
<#Macro3#>*/

            /*
            //get all contents after the ##ToFile stuff
            string patternForValuesDone = @"##ToFile\s+(.*)";//@"##EndValues##";//@"[.|\s]*(##############################)";
            Match mm = Regex.Match(contents, patternForValuesDone, RegexOptions.Multiline);
            string macroContents = contents.Substring(mm.Index + mm.Length, contents.Length - (mm.Index + mm.Length));
           
            //write that in General.txt  
            string pathToGeneraltxt = Path.Combine(PathTOFileTemplate, NameOfTemplateFile);
            macroContents = "//generated file: " + NameOfOutputTemplateFile + "\n" + macroContents;
            File.WriteAllText(pathToGeneraltxt, macroContents);
            */


            var macroLoopGroup1 = new MacroGroupLoop("1", "i");
            var macroLoopGroup2 = new MacroGroupLoop("2", "i");
            var macroLoopGroup3 = new MacroGroupLoop("3", "i");
            var macroLoopGroup4 = new MacroGroupLoop("4", "i");
            var macroLoopGroup5 = new MacroGroupLoop("5", "i");
            var macroLoopGroup6 = new MacroGroupLoop("6", "i");
            var macroLoopGroup7 = new MacroGroupLoop("7", "i");
            var macroLoopGroup8 = new MacroGroupLoop("8", "i");
            var macroLoopGroup9 = new MacroGroupLoop("9", "i");
            var macroLoopGroup10 = new MacroGroupLoop("10", "i");

            var Macro1 = new Macro("Macro1", generalMacros[0]);
            var Macro2 = new Macro("Macro2", generalMacros[1]);
            var Macro3 = new Macro("Macro3", generalMacros[2]);
            var Macro4 = new Macro("Macro4", generalMacros[3]);
            var Macro5 = new Macro("Macro5", generalMacros[4]);
            var Macro6 = new Macro("Macro6", generalMacros[5]);
            var Macro7 = new Macro("Macro7", generalMacros[6]);
            var Macro8 = new Macro("Macro8", generalMacros[7]);
            var Macro9 = new Macro("Macro9", generalMacros[8]);
            var Macro10 = new Macro("Macro10", generalMacros[9]);

            Macros = new List<Macro>();
            Macros.Add(Macro1);
            Macros.Add(Macro2);
            Macros.Add(Macro3);
            Macros.Add(Macro4);
            Macros.Add(Macro5);
            Macros.Add(Macro6);
            Macros.Add(Macro7);
            Macros.Add(Macro8);
            Macros.Add(Macro9);
            Macros.Add(Macro10);
        

            MacroLoopGroups.Add(macroLoopGroup1);
            MacroLoopGroups.Add(macroLoopGroup2);
            MacroLoopGroups.Add(macroLoopGroup3);
            MacroLoopGroups.Add(macroLoopGroup4);
            MacroLoopGroups.Add(macroLoopGroup5);
            MacroLoopGroups.Add(macroLoopGroup6);
            MacroLoopGroups.Add(macroLoopGroup7);
            MacroLoopGroups.Add(macroLoopGroup8);
            MacroLoopGroups.Add(macroLoopGroup9);
            MacroLoopGroups.Add(macroLoopGroup10);

            for (int j = 0; j < 10; j++)
            {
                for (int i = LoopIncrement[j]; i > 0; i--)
                {
                    MacroLoopGroups[j].AddNewGroup(i.ToString());
                } 
            }

            //now define the macro groups


            /*
                       //first get the information in UserCode1 and put that in the General.txt
                           string pathTofile = Path.Combine(templateOutputDestination, nameOfOutputTemplateFile);
                       string contents = File.ReadAllText(pathTofile);
           
                        
                       string patternForFirstComment = @"##UserCode_main##((.|\n)*?)##END##";
                       Match m = Regex.Match(contents, patternForFirstComment, RegexOptions.Multiline);
                       if (m.Success)
                       {
                           string contentsOfUserCode = m.Groups[1].Value;
                           //write that in General.txt but only AFTER the UserCode_main
                           string pathToGeneraltxt = Path.Combine(PathTOFileTemplate,NameOfTemplateFile);
           
                           string patternForWholeUserCodemain = @"\/\*\n*.*##UserCode_main##.*##END##.*\n*\*\/";
                           Match mm = Regex.Match(contents, patternForWholeUserCodemain, RegexOptions.Multiline);
                           if (mm.Success)
                           {
           
                               File.WriteAllText(pathToGeneraltxt, contentsOfUserCode+ mm.Length);
                           }
                           else
                           {
                               Debug.Assert((this.MacroLoopGroups.Count != 0), "You need a ##UserCode_main## comment in your file.");
                           }
                           
                       }
                       else
                       {
                           Debug.Assert((this.MacroLoopGroups.Count != 0), "You need a ##UserCode_main## comment in your file.");
                       }
                       */


        }
    }
}
