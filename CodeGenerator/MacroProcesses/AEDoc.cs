using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CgenMin.MacroProcesses
{

    public class AEDocument
    {
        public static string PathToAERTOS = @"C:\AERTOS";



        public static List<AEDocument> aEDocuments = new List<AEDocument>();


        public static string TableOfContents
        {
            get
            {
                

                string ret = "";
                foreach (var doc in aEDocuments)
                {
                    int countRelative = doc.RelativeLocation.Count(c => c == '\\' || c == '/');
                    string tabs = "";
                    for (int i = 0; i < countRelative; i++)  { tabs += "    ";  }

                    //- [Overview](https://github.com/haditj66/AERTOSCopy)
                    string _GetFullGitPathOfFileWithFILE = doc.RelativeLocation == "" ? $"{doc.GetFullGitPathOfFile}" : $"{doc.GetFullGitPathOfFile}/{doc.FileName}.md";
                    ret += $"{tabs}- [{doc.PageName}]({_GetFullGitPathOfFileWithFILE})"; ret += "\n";
                }

                return ret;
            }
        }

        public string GetFullPathOfFile { get { return Path.Combine(PathToAERTOS, RelativeLocation); } }

        public string GetFullGitPathOfFile
        {
            get
            {
                if (RelativeLocation == "")
                {
                    return "https://github.com/haditj66/AERTOSCopy";
                }
                string _RelativeLocation = RelativeLocation != "" ? Path.Combine("blob/master", RelativeLocation) : "";
                string ret =  Path.Combine("https://github.com/haditj66/AERTOSCopy", _RelativeLocation); 
                return ret.Replace("\\", "/"); ;
            }
        }


        public string GetSectionsDec
        {
            get
            {
                string ret = "";
                foreach (var sec in this.Sections)
                {
                    //[Third Example](#third-example)
                    string uncapital = sec.ToLower().Replace(" ", "-");
                    ret += $"- [{sec}](#{uncapital})"; ret += "\n"; ret += "\n";
                }

                return ret;
            }
        }

        public string GetSectionsDef
        {
            get
            {
                string ret = "";
                foreach (var sec in this.Sections)
                {
                    //## Third Example
                    //##UserCode_uncapital
                    string uncapital = sec.ToLower().Replace(" ", "-");
                    ret += $"## {sec}"; ret += "\n";
                    ret += GetUserCode(uncapital); ret += "\n";
                }

                return ret;
            }
        }


        public static string GetUserCode(string name)
        {
            name = name.Replace("-", "");
            return $"<!--  \n ##UserCode_{name}\n-->";
        }

        public string FileName { get; }
        public string PageName { get; }
        public string RelativeLocation { get; }
        public string[] Sections { get; }




        public AEDocument(string fileName, string pageName, string relativeLocation, params string[] sections)
        {
            FileName = fileName;
            PageName = pageName;
            RelativeLocation = relativeLocation;
            Sections = sections;
            aEDocuments.Add(this);

        }

    }


    class AEDoc : MacroProcess
    {



        public override void RunProcess()
        {


            var readme = new AEDocument("README", "AERTOS", "", "Overview", "Example", "License");
            var Installation = new AEDocument("Installation", "Installation", "doc", "Prerequisites", "Installing visualgdb", "installing CGEN", "Installing AERTOS", "Installing AERTOS projects");
            var Creating_an_AERTOS_project = new AEDocument("Creating_an_AERTOS_project", "Creating an AERTOS project", "doc", "Overview", "CGEN commands for AERTOS", "The Config file", "AEConfig","ProjectTests", "Declaring events in project", "peripheral declaration", "Libraries project depends on", "additional include and src files", "write your own cmake for a project", "the cmakegui for cmake options");            
            var AERTOS_concepts = new AEDocument("AERTOS_concepts", "AERTOS concepts", "doc", "Overview");
            var AOs = new AEDocument("AOs", "Active Objects (AO)", "doc/concepts", "What are Active Objects", "Priorities");
            var Events = new AEDocument("Events", "Events", "doc/concepts", "What are events", "signal_event vs event", "Publishing an event", "Subcribing to an event", "Create your own event");
            var LoopObject = new AEDocument("LoopObject", "LoopObject", "doc/concepts", "What are LoopObjects", "how to subscribe to events", "Create your own LoopObject");
            var AEClock = new AEDocument("AEClock", "AEClock", "doc/concepts", "What are AEClocks", "how to set them", "how to flow to other AOs");
            var Observers = new AEDocument("Observers", "Observers", "doc/concepts", "Overview");
            var Sensors = new AEDocument("Sensors", "Sensors", "doc/concepts/observers", "What are sensors", "how to set them" );
            var filters = new AEDocument("Filters", "Filters", "doc/concepts/observers", "What are filters ", "how to set them", "create your own filter");
            var SPB = new AEDocument("SPB", "SPB", "doc/concepts/observers", "What are SPBs ", "Sampling frequency reduces", "Pass by reference or copy","Channel connect limitations", "style of spb efficiencies", "create your own spb");
            var Utilities = new AEDocument("Utilities", "Utilities", "doc/concepts", "What are Utilities", "Service types", "Set to clock or spb", "Waiting for event", "Create your own utility");
            var FSM = new AEDocument("FSM", "Finite State Machine", "doc/concepts", "What are FSMs", "SubStateMachine feature",  "Create your own FSM", "View FSM graph generated");
            var Target_PC_Or_Embed = new AEDocument("Target_PC_Or_Embed", "Target PC or embedded device", "doc/concepts", "Overview");
            var AEHal = new AEDocument("AEHal", "AEHal", "doc/concepts", "Overview", "How it works and File structure", "Using a Hal peripheral in an AO", "function demos");
            var IntegrationTesting = new AEDocument("IntegrationTesting", "Integration Testing Debugging", "doc/concepts", "Overview", "How to use integration functions",   "Log timers", "Other helpful functions");
            var UnitTesting = new AEDocument("UnitTesting", "Unit Testing", "doc/concepts", "Overview");
            var Examples = new AEDocument("Examples", "Example Projects", "doc", "Overview");
            var blinky = new AEDocument("blinky", "Example blinky", "doc/example", "Overview", "step1 init project", "step2 config project", "step3 AERTOS project code", "step4 Run your application");
            var motor_speed_controller = new AEDocument("motor_speed_controller", "Advanced Example Motor speed controller", "doc/example", "Overview" );            
            var AERTOS_TOOLS = new AEDocument("AERTOS_TOOLS", "AERTOS Tools", "doc", "Overview");
            var UploadDataToPC = new AEDocument("UploadDataToPC", "Upload Data To PC", "doc/tools", "Overview");
            


            foreach (var doc in AEDocument.aEDocuments)
            {
                WriteFileContents_FromCGENMMFile_ToFullPath(
"AERTOS\\AEDoc",
Path.Combine(doc.GetFullPathOfFile, doc.FileName + ".md"),
true, true,false,
new MacroVar() { MacroName = "PageName", VariableValue = "# "+doc.PageName },
new MacroVar() { MacroName = "TableOfContents", VariableValue = AEDocument.TableOfContents },
new MacroVar() { MacroName = "SectionsDec", VariableValue = doc.GetSectionsDec },
new MacroVar() { MacroName = "SectionsDef", VariableValue = doc.GetSectionsDef } 
);

            }




            return;
        }


        Dictionary<string, string> varList = new Dictionary<string, string>(){
            {"posx","int"},
            {"posy","float"}};

        public string Section2()
        {

            StringBuilder sb = new StringBuilder();
            foreach (var item in varList)
            {
                string gout = GenerateFileOut(@"inputs\SetPosxInput",
                    new MacroVar { MacroName = "VarName", VariableValue = item.Key },
                    new MacroVar { MacroName = "VarType", VariableValue = item.Value });

                sb.Append(gout + "\n");

            }

            return sb.ToString();
        }

        public string SomeDynamicStuff()
        {
            return "int somestuff = 4;";
        }

    }
}
