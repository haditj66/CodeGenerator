using CodeGenerator.MacroProcesses;
using CodeGenerator.ProblemHandler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CgenMin.MacroProcesses
{

    public enum ServiceType
    {
        Normal,
        TDU
    }

    public class ActionRequest
    {



        public ActionRequest(string serviceName, ServiceType serviceType, string returnType,
            string argType1, string argName1,
            string argType2, string argName2,
            string argType3, string argName3,
            string argType4, string argName4,
            string argType5, string argName5
            )
        {
            NumOfArgs = 5;// NumOfArgs == 0 ? 5 : NumOfArgs;
            ServiceName = serviceName;

            TheServiceType = serviceType;
            ReturnType = returnType;
            ArgType1 = argType1;
            ArgType2 = argType2;
            ArgType3 = argType3;
            ArgType4 = argType4;
            ArgType5 = argType5;

            ArgName1 = argName1;
            ArgName2 = argName2;
            ArgName3 = argName3;
            ArgName4 = argName4;
            ArgName5 = argName5;

            SetAllArgTypes();
        }

        public ActionRequest(string serviceName, ServiceType serviceType, string returnType,
    string argType1, string argName1,
    string argType2, string argName2,
    string argType3, string argName3,
    string argType4, string argName4) :
            this(serviceName, serviceType, returnType,
        argType1, argName1,
        argType2, argName2,
        argType3, argName3,
        argType4, argName4,
        "", "")
        {
            NumOfArgs = 4;//NumOfArgs == 0 ? 4 : NumOfArgs;
        }

        public ActionRequest(string serviceName, ServiceType serviceType, string returnType,
    string argType1, string argName1,
    string argType2, string argName2,
    string argType3, string argName3) :
            this(serviceName, serviceType, returnType,
        argType1, argName1,
        argType2, argName2,
        argType3, argName3,
        "", "",
        "", "")
        {
            NumOfArgs = 3;// NumOfArgs == 0 ? 3 : NumOfArgs;
        }

        public ActionRequest(string serviceName, ServiceType serviceType, string returnType,
        string argType1, string argName1,
        string argType2, string argName2) :
            this(serviceName, serviceType, returnType,
        argType1, argName1,
        argType2, argName2,
        "", "",
        "", "",
        "", "")
        {
            NumOfArgs = 2;// NumOfArgs == 0 ? 2 : NumOfArgs;
        }
        public ActionRequest(string serviceName, ServiceType serviceType, string returnType,
        string argType1, string argName1) :
            this(serviceName, serviceType, returnType,
        argType1, argName1,
        "", "",
        "", "",
        "", "",
        "", "")
        {
            NumOfArgs = 1; // NumOfArgs == 0 ? 1 : NumOfArgs;
        }


        /// <summary>
        /// from file contents to instance
        /// </summary>
        /// <param name="allContentsOfFile"></param>
        /// <param name="strContentsOfRequest"></param>
        /// <param name="argIdNum"></param>
        /// <param name="nameofUtilityIBelongTo"></param>
        public ActionRequest(string allContentsOfFile, string strContentsOfRequest, int argIdNum, string nameofUtilityIBelongTo)
        {
            ArgIdNum = argIdNum;
            //first get the nuber of arguments there are
            Regex numArgs1 = new Regex(@"(?<numOfArgs>\d)<");
            NumOfArgs = Int32.Parse(numArgs1.Match(strContentsOfRequest).Groups["numOfArgs"].Value);

            Regex regForArgs =
                NumOfArgs == 1 ? regArgs1 :
                NumOfArgs == 2 ? regArgs2 :
                NumOfArgs == 3 ? regArgs3 :
                NumOfArgs == 4 ? regArgs4 :
                NumOfArgs == 5 ? regArgs5 : null;


            var mat = regForArgs.Match(strContentsOfRequest);

            ArgType1 = mat.Groups["ArgType1"].Value;
            ArgType2 = mat.Groups["ArgType2"].Value;
            ArgType3 = mat.Groups["ArgType3"].Value;
            ArgType4 = mat.Groups["ArgType4"].Value;
            ArgType5 = mat.Groups["ArgType5"].Value;
            ReturnType = mat.Groups["RetType"].Value;




            Regex serviceNameReg = new Regex($@"{nameofUtilityIBelongTo}\s*\(\s*\)[\S\s]*ActionReq{ArgIdNum}.ServiceName\s*=\s*""(?<Service>.*)""");

            ServiceName = serviceNameReg.Match(allContentsOfFile).Groups["Service"].Value;


            ArgName1 = $"{ServiceName}Arg1";
            ArgName2 = $"{ServiceName}Arg2";
            ArgName3 = $"{ServiceName}Arg3";
            ArgName4 = $"{ServiceName}Arg4";
            ArgName5 = $"{ServiceName}Arg5";

            SetAllArgTypes();


            TemplateContents = strContentsOfRequest.Replace($"ActionRequestObjectArg{NumOfArgs}", "");
            TemplateContents = TemplateContents.Replace($"ActionRequestObjectArgTDU{NumOfArgs}", "");

        }


        private void SetAllArgTypes()
        {
            AllArgsTypes = new List<string>();
            AllArgsNames = new List<string>();
            if (ArgType1 != "")
            {
                AllArgsTypes.Add(ArgType1);
            }
            if (ArgType2 != "")
            {
                AllArgsTypes.Add(ArgType2);
            }
            if (ArgType3 != "")
            {
                AllArgsTypes.Add(ArgType3);
            }
            if (ArgType4 != "")
            {
                AllArgsTypes.Add(ArgType4);
            }
            if (ArgType5 != "")
            {
                AllArgsTypes.Add(ArgType5);
            }

            if (ArgName1 != "")
            {
                AllArgsNames.Add(ArgName1);
            }
            if (ArgName2 != "")
            {
                AllArgsNames.Add(ArgName2);
            }
            if (ArgName3 != "")
            {
                AllArgsNames.Add(ArgName3);
            }
            if (ArgName4 != "")
            {
                AllArgsNames.Add(ArgName4);
            }
            if (ArgType5 != "")
            {
                AllArgsNames.Add(ArgName5);
            }

        }


        static Regex regArgs1 = new Regex(@"1<(?<ArgType1>.+)\s*,\s*(?<RetType>.+)\s*,.*,.*>");
        static Regex regArgs2 = new Regex(@"2<(?<ArgType1>.+)\s*,\s*(?<ArgType2>.+)\s*,\s*(?<RetType>.+)\s*,.*,.*>");
        static Regex regArgs3 = new Regex(@"3<(?<ArgType1>.+)\s*,\s*(?<ArgType2>.+)\s*,\s*(?<ArgType3>.+)\s*,\s*(?<RetType>.+)\s*,.*,.*>");
        static Regex regArgs4 = new Regex(@"4<(?<ArgType1>.+)\s*,\s*(?<ArgType2>.+)\s*,\s*(?<ArgType3>.+)\s*,\s*(?<ArgType4>.+)\s*,\s*(?<RetType>.+)\s*,.*,.*>");
        static Regex regArgs5 = new Regex(@"5<(?<ArgType1>.+)\s*,\s*(?<ArgType2>.+)\s*,\s*(?<ArgType3>.+)\s*,\s*(?<ArgType4>.+)\s*,\s*(?<ArgType5>.+)\s*,\s*(?<RetType>.+)\s*,.*,.*>");

        public string ServiceName;
        public int ArgIdNum;
        public int NumOfArgs = 0;
        public string ReturnType;
        public string ArgType1;
        public string ArgType2;
        public string ArgType3;
        public string ArgType4;
        public string ArgType5;
        public string TemplateContents;

        public string ArgName1;
        public string ArgName2;
        public string ArgName3;
        public string ArgName4;
        public string ArgName5;
        public ServiceType TheServiceType;

        private List<string> AllArgsTypes;
        private List<string> AllArgsNames;

        public string GetFuncArguments(bool withTypes)
        {

            string ret = "";

            int ind = 0;
            foreach (var argtype in AllArgsTypes)
            {
                string argtype_l = withTypes ? argtype : "";

                if (ind != 0)
                {
                    ret += $", ";
                }
                ind++;
                ret += $"{argtype_l} {AllArgsNames[ind - 1]}";// Arg{ind.ToString()}";

            }

            return ret;
        }

        public string GetArguments()
        {

            string ret = "";

            int ind = 0;
            foreach (var argtype in AllArgsTypes)
            {
                if (ind != 0)
                {
                    ret += $", ";
                }
                ind++;
                ret += $"{AllArgsNames[ind - 1]}";

            }

            return ret;
        }

        public string GetFullTemplateType(int ServiceBuffer, string utilityClassName)
        {
            string ret = "";
            ret += TheServiceType == ServiceType.Normal ? $"ActionRequestObjectArg{this.NumOfArgs.ToString()}" :
                        $"ActionRequestObjectArgTDU{this.NumOfArgs.ToString()}";

            ret += $"<";
            int ind = 0;
            foreach (var argtype in AllArgsTypes)
            {
                if (ind != 0)
                {
                    ret += $", ";
                }
                ind++;
                ret += $"{argtype} ";// Arg{ind.ToString()}";

            }


            //ret += $"<{GetFuncArguments(false)}";

            ret += $", {ReturnType}, {ServiceBuffer}, {utilityClassName}>";

            return ret;
        }

        public string GetFullTemplateArgs(int ServiceBuffer, string utilityClassName)
        {
            string ret = "";
            ret += TheServiceType == ServiceType.Normal ? $"ActionRequestObjectArg{ArgIdNum}" :
                        $"ActionRequestObjectArgTDU{ArgIdNum}";
            ret += $"<{GetFuncArguments(false)}";

            ret += $", {ReturnType}, {ServiceBuffer}, {utilityClassName}>,";

            return ret;
        }
    }



    public class UtilityService
    {
        public List<ActionRequest> requests = new List<ActionRequest>();

        public string NameOfUtility;
        public UtilityService()
        {
        }

        public UtilityService(string className)
        {
            NameOfUtility = className;
        }


        //public static UtilityService GetAllUtilitiesFromFile(string fromFile, string className)
        /// <summary>
        /// from file to instance
        /// </summary>
        /// <param name="fromFile"></param>
        /// <param name="className"></param>
        public UtilityService(string fromFile, string className)
        {
            UtilityService utService = null;

            //var filesToSearch = Directory.GetFiles(Path.Combine(fromDirectory));
            //foreach (var fil in filesToSearch)
            //{
            var cont = File.ReadAllText(fromFile);

            if (cont.Contains("ActionRequestObjectArg"))
            {

                //strip out all comments from the file.
                cont = Regex.Replace(cont, @"\/\/.*\n", "", RegexOptions.Multiline);

                Regex regclass = new Regex(@"class\s+" + className + @"\s*:\s*public\s*AEService");




                //find  utility service classes. 
                MatchCollection reg = regclass.Matches(cont);
                foreach (Match mat in reg)
                {
                    //utService = new UtilityService()
                    //{
                    this.NameOfUtility = className;// mat.Groups["UtilityClassName"].Value
                    //};

                    Regex regArgsContents = new Regex(@"class\s+" + this.NameOfUtility + @"\s*:\s*public\s+AEService\s*<(?<ArgReqContents>[A-Z a-z 0-9 , \s \t \n <>_* \r ]*>?\s*){");
                    var ArgsContessvants = regArgsContents.Match(cont);
                    string ArgsContents = regArgsContents.Match(cont).Groups["ArgReqContents"].Value;

                    //grab any action req that are not tdus, rmove the ones you got from the contents
                    Regex regArgNotTDU = new Regex(@"ActionRequestObjectArg\d.*" + this.NameOfUtility + @"\s*>\s*");
                    int nonTdusFound = 1;
                    while (regArgNotTDU.IsMatch(ArgsContents))
                    {
                        string mmm = regArgNotTDU.Match(ArgsContents).Value;
                        ActionRequest ar = new ActionRequest(cont, mmm, nonTdusFound, this.NameOfUtility);
                        this.requests.Add(ar);

                        ArgsContents = ArgsContents.Replace(mmm, "");
                        nonTdusFound++;
                    }

                    Regex regArgTDU = new Regex(@"ActionRequestObjectArgTDU\d.*" + this.NameOfUtility + @"\s*>\s*");
                    int TdusFound = 4;
                    while (regArgTDU.IsMatch(ArgsContents))
                    {
                        string mmm = regArgTDU.Match(ArgsContents).Value;
                        ActionRequest ar = new ActionRequest(cont, mmm, TdusFound, this.NameOfUtility);
                        this.requests.Add(ar);

                        ArgsContents = ArgsContents.Replace(mmm, "");
                        TdusFound++;
                    }

                }


            }

            //return utService;
        }

    }


        public abstract class AEUtilityService : AOWritableConstructible
    {
        public bool FlowsFromSPB { get; internal set; }




        //protected UtilityService utService;
        protected List<string> HeaderIncludesFromLibrary = new List<string>();

        /// <summary>
        /// Set all additional includes that this utility uses. It needs to be done here so that it can be included in the additionalincludes in the AEConfig file.
        /// </summary>

        protected virtual List<string> GetHeaderIncludesFromLibrary() { return new List<string>(); }

        protected List<ActionRequest> requests = new List<ActionRequest>();


        public bool isSetToClock()
        {
            var allClocks = AO.AllInstancesOfAO.Where(a => a.AOType == AOTypeEnum.Clock).Cast<AEClock>().ToList();
            if (allClocks.Count > 0)
            {
                foreach (var cl in allClocks)
                {
                    cl.GetTdusIFlowTo().FirstOrDefault(t => t.Item1.InstanceName == this.InstanceName);
                    if (cl != null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        public ActionRequest GetRequestOfName(string requestName)
        {
            return requests.FirstOrDefault(r => r.ServiceName == requestName);
        }

        public string FromFileThisComesFrom { get; }
        public AEPriorities Priority { get; }
        public string PriorityStr
        {
            get
            {
                return
Priority == AEPriorities.LowPriority ? "LowestPriority" :
Priority == AEPriorities.MediumPriority ? "MediumPriority" :
Priority == AEPriorities.HighPriority ? "HighestPriority" : "";
            }
        }

        public int ServiceBufferArgPoolSize { get; protected set; }
        public int FilterNumIFlowFrom { get;   set; }

        //public AEUtilityService(string instanceNameOfTDU, string className, string fromFileThisComesFrom) : base(className, instanceNameOfTDU, AOTypeEnum.UtilityService)
        //{

        //    FromFileThisComesFrom = fromFileThisComesFrom;
        //    utService = new UtilityService(fromFileThisComesFrom,className);
        //}

        public AEUtilityService(string fromLibrary, string instanceNameOfTDU, AEPriorities priority, int serviceBuffer,  CppFunctionArgs cppFunctionArgs = null, params ActionRequest[] actionRequests) :
            base(fromLibrary, instanceNameOfTDU, AOTypeEnum.UtilityService, cppFunctionArgs)
        {
            requests = actionRequests.ToList();
            FromFileThisComesFrom = "";
            ServiceBufferArgPoolSize = serviceBuffer;

            HeaderIncludesFromLibrary = GetHeaderIncludesFromLibrary();

            Priority = priority;

            int indUT = 1;
            int indTDU = 4;
            foreach (var req in requests)
            {
                if (req.TheServiceType == ServiceType.Normal)
                {
                    req.ArgIdNum = indUT;
                    indUT++;
                }
                if (req.TheServiceType == ServiceType.TDU)
                {
                    req.ArgIdNum = indTDU;
                    indTDU++;
                }


            }
        }



        /// <summary>
        /// from file to instance
        /// </summary>
        /// <param name="fromFile"></param>
        /// <param name="className"></param>
        public AEUtilityService(string fromLibrary, string instanceNameOfTDU, AEPriorities priority,  string fromFile) 
            : base(fromLibrary,  instanceNameOfTDU, AOTypeEnum.UtilityService)
        {
            FromFileThisComesFrom = fromFile;

            HeaderIncludesFromLibrary = GetHeaderIncludesFromLibrary();

            //UtilityService utService = null;

            //var filesToSearch = Directory.GetFiles(Path.Combine(fromDirectory));
            //foreach (var fil in filesToSearch)
            //{
            var cont = File.ReadAllText(fromFile);

            if (cont.Contains("ActionRequestObjectArg"))
            {

                //strip out all comments from the file.
                cont = Regex.Replace(cont, @"\/\/.*\n", "", RegexOptions.Multiline);

                Regex regclass = new Regex(@"class\s+" + ClassName + @"\s*:\s*public\s*AEService");




                //find  utility service classes. 
                MatchCollection reg = regclass.Matches(cont);
                foreach (Match mat in reg)
                {
                    //utService = new UtilityService()
                    //{
                    //this.NameOfUtility = className;// mat.Groups["UtilityClassName"].Value
                    //};

                    Regex regArgsContents = new Regex(@"class\s+" + this.ClassName + @"\s*:\s*public\s+AEService\s*<(?<ArgReqContents>[A-Z a-z 0-9 , \s \t \n <>_* \r ]*>?\s*){");
                    var ArgsContessvants = regArgsContents.Match(cont);
                    string ArgsContents = regArgsContents.Match(cont).Groups["ArgReqContents"].Value;

                    //grab any action req that are not tdus, rmove the ones you got from the contents
                    Regex regArgNotTDU = new Regex(@"ActionRequestObjectArg\d.*" + this.ClassName + @"\s*>\s*");
                    int nonTdusFound = 1;
                    while (regArgNotTDU.IsMatch(ArgsContents))
                    {
                        string mmm = regArgNotTDU.Match(ArgsContents).Value;
                        ActionRequest ar = new ActionRequest(cont, mmm, nonTdusFound, this.ClassName);
                        this.requests.Add(ar);

                        ArgsContents = ArgsContents.Replace(mmm, "");
                        nonTdusFound++;
                    }

                    Regex regArgTDU = new Regex(@"ActionRequestObjectArgTDU\d.*" + this.ClassName + @"\s*>\s*");
                    int TdusFound = 4;
                    while (regArgTDU.IsMatch(ArgsContents))
                    {
                        string mmm = regArgTDU.Match(ArgsContents).Value;
                        ActionRequest ar = new ActionRequest(cont, mmm, TdusFound, this.ClassName);
                        this.requests.Add(ar);

                        ArgsContents = ArgsContents.Replace(mmm, "");
                        TdusFound++;
                    }

                }


            }

            //return utService;
        }







        public override string GenerateFunctionDefinesSection()
        {
            string ret = "";
            //nothing
            return ret;
        }
        public override string GenerateMainHeaderSection()
        {
            string ret = "";
            //nothing
            return ret;
        }


        public override string GenerateMainClockSetupsSection()
        {


            //assert that it is set to a clock or a spb if it has any tdus
            if ((this.FlowsFromSPB || this.isSetToClock()) == false)
            {
                var tt = this.requests.FirstOrDefault(r => r.TheServiceType == ServiceType.TDU);
                if (tt != null)
                {
                    ProblemHandle problemHandle = new ProblemHandle();
                    problemHandle.ThereisAProblem($"The utility object of instance name {this.InstanceName} is not set to a clock or spb even though it has a tdu service type");
                }
            } 

            string ret = "";
            //nothing
            return ret;
        }



        public override string GenerateMainInitializeSection()
        {
            //static UUartDriverTDU uartDriver_L; uartDriver_L.Init(AEPriorities::MediumPriority);
            //uartDriverTDU = &uartDriver_L;
            string ret = "";

            ret += $"static {ClassName} {InstanceName}_L; {InstanceName}_L.Init(AEPriorities::{PriorityStr});"; ret += "\n";
            ret += $"{InstanceName} = &{InstanceName}_L;"; ret += "\n";

            //if no arguments for user init contructor, call it here
            string cppfuncStr = $"{InstanceName}->UserInitialize();";
            if (this.TheCppFunctionArgs != null)
            {
                if (this.TheCppFunctionArgs.TheCppFunctionArgs.Length == 0)
                { 
                     ret += cppfuncStr; ret += "\n";
                }
                else
                {
                    ret += ""; ret += "\n";

                }
            }
            else
            {
                ret += ""; ret += "\n";
            }


            return ret;
        }

        public override string GenerateMainLinkSetupsSection()
        {
            string ret = "";
            //nothing
            return ret;
        }

        protected override string _GenerateAEConfigSection(int numOfAOOfThisSameTypeGeneratesAlready)
        {
            string ret = "";

            ret += this.SetAEConfig(numOfAOOfThisSameTypeGeneratesAlready) + "\n";
            foreach (var item in HeaderIncludesFromLibrary)
            { 
                ret += GetAdditionalIncludeInAEConfig(item) + "\n";
            }


            return ret;// this.SetAEConfig(numOfAOOfThisSameTypeGeneratesAlready);
        }

        public string GetFullTemplateTypeForBase()
        {

            //                public AEService<10,
            //   ActionRequestObjectArg1<char const*, bool, 10, UUartDriverTDU>, 
            //AENullActionRequest, 
            //AENullActionRequest, 
            //ActionRequestObjectArgTDU1<char*, int8_t,10 , UUartDriverTDU>>


            string ret = "";

            ret += $"{ServiceBufferArgPoolSize} ";

            for (int i = 0; i < 6; i++)
            {
                ActionRequest req = requests.FirstOrDefault(a => a.ArgIdNum == i + 1);

                if (req != null)
                {
                    ret += ", ";
                    ret += req.GetFullTemplateType(ServiceBufferArgPoolSize, ClassName);
                }
                else
                {
                    ret += ", AENullActionRequest";
                }
            }

            ret += $"";
            return ret;


        }

        public override string GetFullTemplateArgsValues()
        {
            string ret = "";


            return ret;
        }


        public override string GetFullTemplateType()
        {
            string ret = "";


            return ret;
        }
        public override string GetFullTemplateArgs()
        {
            string ret = "";


            return ret;

        }

        protected override List<RelativeDirPathWrite> _WriteTheContentedToFiles()
        {
            List<RelativeDirPathWrite> relativeDirPathWrites = new List<RelativeDirPathWrite>();

            var serv1 = this.requests.Where(a => a.ArgIdNum == 1).FirstOrDefault();
            var serv2 = this.requests.Where(a => a.ArgIdNum == 2).FirstOrDefault();
            var serv3 = this.requests.Where(a => a.ArgIdNum == 3).FirstOrDefault();
            var serv4 = this.requests.Where(a => a.ArgIdNum == 4).FirstOrDefault();
            var serv5 = this.requests.Where(a => a.ArgIdNum == 5).FirstOrDefault();
            var serv6 = this.requests.Where(a => a.ArgIdNum == 6).FirstOrDefault();


            string ServiceName1 = serv1 == null ? "" : serv1.ServiceName;
            string ServiceName2 = serv2 == null ? "" : serv2.ServiceName;
            string ServiceName3 = serv3 == null ? "" : serv3.ServiceName;
            string ServiceName4 = serv4 == null ? "" : serv4.ServiceName;
            string ServiceName5 = serv5 == null ? "" : serv5.ServiceName;
            string ServiceName6 = serv6 == null ? "" : serv6.ServiceName;

            string Service1NameDef = serv1 == null ? "" : $"ActionReq1.ServiceName = \"{ServiceName1}\";";
            string Service2NameDef = serv2 == null ? "" : $"ActionReq2.ServiceName = \"{ServiceName2}\";";
            string Service3NameDef = serv3 == null ? "" : $"ActionReq3.ServiceName = \"{ServiceName3}\";";
            string Service4NameDef = serv4 == null ? "" : $"ActionReq4.ServiceName = \"{ServiceName4}\";";
            string Service5NameDef = serv5 == null ? "" : $"ActionReq5.ServiceName = \"{ServiceName5}\";";
            string Service6NameDef = serv6 == null ? "" : $"ActionReq6.ServiceName = \"{ServiceName6}\";";


            string updateFunction4 = serv4 == null ? "" : $"    bool _{ServiceName4}Update({serv4.GetFullTemplateType(this.ServiceBufferArgPoolSize, this.ClassName)}* request, {serv4.GetFuncArguments(true)}) \n" + "   {\n" + $"\n         ##UserCode_{ServiceName4}u \n" + "    return true; \n    }\n";
            string updateFunction5 = serv5 == null ? "" : $"    bool _{ServiceName5}Update({serv5.GetFullTemplateType(this.ServiceBufferArgPoolSize, this.ClassName)}* request, {serv5.GetFuncArguments(true)}) \n" + "   {\n" + $"\n         ##UserCode_{ServiceName5}u \n" + "    return true; \n    }\n";
            string updateFunction6 = serv6 == null ? "" : $"    bool _{ServiceName6}Update({serv6.GetFullTemplateType(this.ServiceBufferArgPoolSize, this.ClassName)}* request, {serv6.GetFuncArguments(true)}) \n" + "   {\n" + $"\n         ##UserCode_{ServiceName6}u \n" + "    return true; \n    }\n";

            string functionService1 = serv1 == null ? "" : $"void _{ServiceName1}({serv1.GetFuncArguments(true)}) \n" + "   {\n" + $"\n         ##UserCode_{ServiceName1} \n" + "   }\n";
            string functionService2 = serv2 == null ? "" : $"void _{ServiceName2}({serv2.GetFuncArguments(true)}) \n" + "   {\n" + $"\n         ##UserCode_{ServiceName2} \n" + "   }\n";
            string functionService3 = serv3 == null ? "" : $"void _{ServiceName3}({serv3.GetFuncArguments(true)}) \n" + "   {\n" + $"\n         ##UserCode_{ServiceName3} \n" + "   }\n";
            string functionService4 = serv4 == null ? "" : $"void _{ServiceName4}({serv4.GetFuncArguments(true)}) \n" + "   {\n" + $"\n         ##UserCode_{ServiceName4} \n" + "   }\n\n" ;
            string functionService5 = serv5 == null ? "" : $"void _{ServiceName5}({serv5.GetFuncArguments(true)}) \n" + "   {\n" + $"\n         ##UserCode_{ServiceName5} \n" + "   }\n\n" ;
            string functionService6 = serv6 == null ? "" : $"void _{ServiceName6}({serv6.GetFuncArguments(true)}) \n" + "   {\n" + $"\n         ##UserCode_{ServiceName6} \n" + "   }\n\n" ;

            functionService4 = serv4 == null ? "" : functionService4 + $"void _{ServiceName4}CancelCleanup() \n" + "   {\n" + $"\n         ##UserCode_{ServiceName4}cancel \n" + "   }\n\n" + updateFunction4;
            functionService5 = serv5 == null ? "" : functionService5 + $"void _{ServiceName5}CancelCleanup() \n" + "   {\n" + $"\n         ##UserCode_{ServiceName5}cancel \n" + "   }\n\n" + updateFunction5;
            functionService6 = serv6 == null ? "" : functionService6 + $"void _{ServiceName6}CancelCleanup() \n" + "   {\n" + $"\n         ##UserCode_{ServiceName6}cancel \n" + "   }\n\n" + updateFunction6;


            //all includes
            string AllIncludesHeaders = "";
            foreach (var inc in HeaderIncludesFromLibrary)
            {
                AllIncludesHeaders += $"#include \"{inc}.h\"" + "\n";
            }


            string contentesOut = AEInitializing.TheMacro2Session.GenerateFileOut("AERTOS/UtilityServiceClass",
    new MacroVar() { MacroName = "UtilityName", VariableValue = ClassName },
    new MacroVar() { MacroName = "PoolSize", VariableValue = this.ServiceBufferArgPoolSize.ToString() },
    new MacroVar() { MacroName = "BaseTemplate", VariableValue = $"{ GetFullTemplateTypeForBase() }" },
    new MacroVar() { MacroName = "AllIncludesHeaders", VariableValue = $"{AllIncludesHeaders}" },
    new MacroVar() { MacroName = "Service1NameDef", VariableValue = $"{Service1NameDef}" },
    new MacroVar() { MacroName = "Service2NameDef", VariableValue = $"{Service2NameDef}" },
    new MacroVar() { MacroName = "Service3NameDef", VariableValue = $"{Service3NameDef}" },
    new MacroVar() { MacroName = "Service4NameDef", VariableValue = $"{Service4NameDef}" },
    new MacroVar() { MacroName = "Service5NameDef", VariableValue = $"{Service5NameDef}" },
    new MacroVar() { MacroName = "Service6NameDef", VariableValue = $"{Service6NameDef}" },
    new MacroVar() { MacroName = "Service1Func", VariableValue = $"{functionService1}" },
    new MacroVar() { MacroName = "Service2Func", VariableValue = $"{functionService2}" },
    new MacroVar() { MacroName = "Service3Func", VariableValue = $"{functionService3}" },
    new MacroVar() { MacroName = "Service4Func", VariableValue = $"{functionService4}" },
    new MacroVar() { MacroName = "Service5Func", VariableValue = $"{functionService5}" },
    new MacroVar() { MacroName = "Service6Func", VariableValue = $"{functionService6}" },
    new MacroVar() { MacroName = "InitFunction", VariableValue = $"{GetInitializationFunction()}" }
    );
            relativeDirPathWrites.Add(new RelativeDirPathWrite($"{ClassName}", ".h", "include", contentesOut, true)); 
            relativeDirPathWrites.Add(new RelativeDirPathWrite($"{ClassName}_ServiceGen", ".h", "include", GetUtilityPublicFuncContents(AEInitializing.TheMacro2Session), false));

            

            return relativeDirPathWrites;
        }





        public string GetUtilityPublicFuncContents(MacroProcess macroProc)//, string fileDirWithFileWITHOUT_EXT)
        {

            string UtilityDefines = "";
            string allDefinesCtor = "";
            string allDefinesServices = "";



            //foreach (var utility in this.requests)
            //{


            var currentUtility = this;
            allDefinesCtor = $"#define {ClassName}_CTOR \\\n";
            allDefinesServices = $"#define {ClassName}_Service \\\n";


            foreach (var req in this.requests)
            {
                var currentRequest = req;

                //contructor section
                currentUtility = this;
                UtilityDefines += "\n";
                UtilityDefines += macroProc.GenerateFileOut("AERTOS/UtilityCTOR",
                            new MacroVar() { MacroName = "UtilityName", VariableValue = ClassName },
                            new MacroVar() { MacroName = "ServiceName", VariableValue = currentRequest.ServiceName },
                            new MacroVar() { MacroName = "FuncArguments", VariableValue = currentRequest.GetFuncArguments(true) },
                            new MacroVar() { MacroName = "Arguments", VariableValue = currentRequest.GetFuncArguments(false) },
                            new MacroVar() { MacroName = "ServiceId", VariableValue = currentRequest.ArgIdNum.ToString() });
                //UtilityDefines += "\n";
                if (currentRequest.ArgIdNum > 3)//then it is an update
                {
                    UtilityDefines += macroProc.GenerateFileOut("AERTOS/UtilityUpdateCTOR",
                    new MacroVar() { MacroName = "UtilityName", VariableValue = ClassName },
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
                    new MacroVar() { MacroName = "UtilityName", VariableValue = ClassName },
                    new MacroVar() { MacroName = "ServiceName", VariableValue = req.ServiceName },
                    new MacroVar() { MacroName = "FuncArguments", VariableValue = req.GetFuncArguments(true) },
                    new MacroVar() { MacroName = "Arguments", VariableValue = req.GetFuncArguments(false) },
                    new MacroVar() { MacroName = "ServiceId", VariableValue = req.ArgIdNum.ToString() },
                    new MacroVar() { MacroName = "TemplateContents", VariableValue = req.TemplateContents },
                    new MacroVar() { MacroName = "TDUNum", VariableValue = (currentRequest.ArgIdNum - 3).ToString() }

                    );

                allDefinesCtor += $"{ClassName}_{req.ServiceName}_CTOR \\\n";
                allDefinesServices += $"{ClassName}_{req.ServiceName}_Service \\\n";
            }

            //get all defines and write it in one define file.
            UtilityDefines += $"\n//Alldefines ------------------\n";
            allDefinesCtor = allDefinesCtor.Remove(allDefinesCtor.Length - 2, 2);
            allDefinesServices = allDefinesServices.Remove(allDefinesServices.Length - 2, 2);
            UtilityDefines += allDefinesCtor;
            UtilityDefines += "\n";
            UtilityDefines += allDefinesServices;
            UtilityDefines += "\n";

            //}


            //UtilityDefines += $"\n//Alldefines ------------------\n";
            //UtilityDefines += allDefinesCtor;

            return UtilityDefines;

            //if (allServices.Count > 0)
            //{
            //    string oldCont = "";
            //    //get current contents 
            //    if (File.Exists(fileDirWithFileWITHOUT_EXT + ".h"))
            //    {
            //        oldCont = File.ReadAllText(fileDirWithFileWITHOUT_EXT + ".h");
            //    }
            //    if (oldCont != UtilityDefines)
            //    {
            //        macroProc.WriteFileContents(UtilityDefines, fileDirWithFileWITHOUT_EXT, "h");
            //    }

            //}
        }








    }


    //public class AETDUService
    //{
    //    public AETDUService(string nameOfTDU)
    //    {
    //        NameOfTDU = nameOfTDU;
    //    }

    //    public string NameOfTDU { get; }
    //}

}
