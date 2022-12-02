using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CgenMin.MacroProcesses
{
    class AEServiceMacro : MacroProcess
    {

         
        List<UtilityService> utServices;


        public override void RunProcess()
        {
            //grab all files in the include directory.
            var filesToSearch = Directory.GetFiles(Path.Combine(this.EvironmentDirectory, "include"));
            foreach (var fil in filesToSearch)
            {
                var cont = File.ReadAllText(fil);

                if (cont.Contains("ActionRequestObjectArg"))
                {

                    //strip out all comments from the file.
                    cont = Regex.Replace(cont, @"\/\/.*\n","", RegexOptions.Multiline);

                    Regex regclass = new Regex(@"class\s+(?<UtilityClassName>\w+)\s*:\s*public\s*AEService");


                    utServices = new List<UtilityService>();


                    //find  utility service classes. 
                    MatchCollection reg = regclass.Matches(cont);
                    foreach (Match mat in reg)
                    {
                        UtilityService utServ = new UtilityService()
                        {
                            NameOfUtility = mat.Groups["UtilityClassName"].Value
                        };
                        utServices.Add(utServ);

                        Regex regArgsContents = new Regex(@"class\s+" + utServ.NameOfUtility + @"\s*:\s*public\s+AEService\s*<(?<ArgReqContents>[A-Z a-z 0-9 , \s \t \n <>_* \r ]*>?\s*){"); 
                        var ArgsContessvants = regArgsContents.Match(cont);
                        string ArgsContents = regArgsContents.Match(cont).Groups["ArgReqContents"].Value;

                        //grab any action req that are not tdus, rmove the ones you got from the contents
                        Regex regArgNotTDU = new Regex(@"ActionRequestObjectArg\d.*" + utServ.NameOfUtility + @"\s*>\s*");
                        int nonTdusFound = 1;
                        while (regArgNotTDU.IsMatch(ArgsContents))
                        {
                            string mmm = regArgNotTDU.Match(ArgsContents).Value;
                            ActionRequest ar = new ActionRequest(cont,mmm, nonTdusFound, utServ.NameOfUtility);
                            utServ.requests.Add(ar);

                            ArgsContents = ArgsContents.Replace(mmm, "");
                            nonTdusFound++;
                        }

                        Regex regArgTDU = new Regex(@"ActionRequestObjectArgTDU\d.*" + utServ.NameOfUtility + @"\s*>\s*"); 
                        int TdusFound = 4;
                        while (regArgTDU.IsMatch(ArgsContents))
                        {
                            string mmm = regArgTDU.Match(ArgsContents).Value;
                            ActionRequest ar = new ActionRequest(cont, mmm, TdusFound, utServ.NameOfUtility);
                            utServ.requests.Add(ar);

                            ArgsContents = ArgsContents.Replace(mmm, "");
                            TdusFound++;
                        }
                         
                    }
                     
                }

                WriteUtilityPublicFuncContents(utServices, this,  $@"{this.EvironmentDirectory}/include/{Path.GetFileNameWithoutExtension(fil)}_ServiceGen");
                 

            } 

            return;
        }
         

        public static void WriteUtilityPublicFuncContents(List<UtilityService> allServices, MacroProcess macroProc, string fileDirWithFileWITHOUT_EXT)
        {

            string UtilityDefines = "";
            string allDefinesCtor = "";
            string allDefinesServices = "";
             


            foreach (var utility in allServices)
            {


                var currentUtility = utility;
                allDefinesCtor = $"#define {utility.NameOfUtility}_CTOR \\\n";
                allDefinesServices = $"#define {utility.NameOfUtility}_Service \\\n";


                foreach (var req in utility.requests)
                {
                    var currentRequest = req;
                     
                    //contructor section
                    currentUtility = utility;
                    UtilityDefines += macroProc.GenerateFileOut("AERTOS/UtilityCTOR",
                                new MacroVar() { MacroName = "UtilityName", VariableValue = currentUtility.NameOfUtility },
                                new MacroVar() { MacroName = "ServiceName", VariableValue = currentRequest.ServiceName },
                                new MacroVar() { MacroName = "FuncArguments", VariableValue = currentRequest.GetFuncArguments(true) },
                                new MacroVar() { MacroName = "Arguments", VariableValue = currentRequest.GetFuncArguments(false) },
                                new MacroVar() { MacroName = "ServiceId", VariableValue = currentRequest.ArgIdNum.ToString() });
                    UtilityDefines += "\n";
                    if (currentRequest.ArgIdNum > 3)//then it is an update
                    {
                        UtilityDefines += macroProc.GenerateFileOut("AERTOS/UtilityUpdateCTOR",
                        new MacroVar() { MacroName = "UtilityName", VariableValue = currentUtility.NameOfUtility },
                        new MacroVar() { MacroName = "ServiceName", VariableValue = currentRequest.ServiceName },
                        new MacroVar() { MacroName = "FuncArguments", VariableValue = currentRequest.GetFuncArguments(true) },
                        new MacroVar() { MacroName = "Arguments", VariableValue = currentRequest.GetFuncArguments(false) },
                        new MacroVar() { MacroName = "ServiceId", VariableValue = currentRequest.ArgIdNum.ToString() }
                        );
                    }
                    UtilityDefines += "\n";




                    UtilityDefines += $"\n\n//{req.ServiceName}----------------------------------------------\n\n";

                    //now that I have all utility services captured, I can generate the code for them
                    UtilityDefines += macroProc.GenerateFileOut("AERTOS/UtilityService",
                        new MacroVar() { MacroName = "UtilityName", VariableValue = utility.NameOfUtility },
                        new MacroVar() { MacroName = "ServiceName", VariableValue = req.ServiceName },
                        new MacroVar() { MacroName = "FuncArguments", VariableValue = req.GetFuncArguments(true) },
                        new MacroVar() { MacroName = "Arguments", VariableValue = req.GetFuncArguments(false) },
                        new MacroVar() { MacroName = "ServiceId", VariableValue = req.ArgIdNum.ToString() },
                        new MacroVar() { MacroName = "TemplateContents", VariableValue = req.TemplateContents },
                        new MacroVar() { MacroName = "TDUNum", VariableValue = (currentRequest.ArgIdNum - 3).ToString() }

                        );

                    allDefinesCtor += $"{utility.NameOfUtility}_{req.ServiceName}_CTOR \\\n";
                    allDefinesServices += $"{utility.NameOfUtility}_{req.ServiceName}_Service \\\n";
                }

                //get all defines and write it in one define file.
                UtilityDefines += $"\n//Alldefines ------------------\n";
                allDefinesCtor = allDefinesCtor.Remove(allDefinesCtor.Length - 2, 2);
                allDefinesServices = allDefinesServices.Remove(allDefinesServices.Length - 2, 2);
                UtilityDefines += allDefinesCtor;
                UtilityDefines += "\n";
                UtilityDefines += allDefinesServices;

            }


            //UtilityDefines += $"\n//Alldefines ------------------\n";
            //UtilityDefines += allDefinesCtor;

            if (allServices.Count > 0)
            {
                macroProc.WriteFileContents(UtilityDefines, fileDirWithFileWITHOUT_EXT, "h" );
            }
        }



        
        
         


    }
}
