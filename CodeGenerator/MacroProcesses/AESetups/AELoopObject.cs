using System.Collections.Generic;

namespace CgenMin.MacroProcesses
{


    public abstract class AELoopObject : AOWritableConstructible
    {
        public AELoopObject(string fromLibrary,  string instanceNameOfloopObject, AEPriorities priority, int frequencyOfLoop, CppFunctionArgs constructorArgs = null)
            : base(fromLibrary,   instanceNameOfloopObject, AOTypeEnum.LoopObject, constructorArgs)
        {

            Priority = priority;
            FrequencyOfLoop = frequencyOfLoop;
        }

        public AEPriorities Priority { get; }
        public int FrequencyOfLoop { get; }

        public string PriorityStr
        {
            get
            {
                return
Priority == AEPriorities.LowPriority ? "LowPriority" :
Priority == AEPriorities.MediumPriority ? "MediumPriority" :
Priority == AEPriorities.HighPriority ? "HighPriority" : "";
            }
        }


        public override string GenerateFunctionDefinesSection()
        {
            string ret = "";
            //nothing
            return ret;
        }

        public override string GenerateMainClockSetupsSection()
        {
            string ret = "";
            //nothing
            return ret;
        }

        public override string GenerateMainHeaderSection()
        {
            //static AELoopObject1Test * objectTest;
            //#include "LoopObjeect1Test.h"
            string ret = "";
            ret += $"#include \"{ClassName}.h\""; ret += "\n";
            ret += $"static {ClassName}* {InstanceName};"; ret += "\n"; 
            return ret;
        }

        public override string GenerateMainInitializeSection()
        {

            //static AELoopObject1Test objectTest_l;
            //objectTest = &objectTest_l;
            //objectTest->InitObject(2, AEPriorities::MediumPriority);

            string ret = "";
            ret += $"static {ClassName} {InstanceName}_l;"; ret += "\n";
            ret += $"{InstanceName} = &{InstanceName}_l;"; ret += "\n";
            ret += $"{InstanceName}->InitObject({FrequencyOfLoop.ToString()}, AEPriorities::{PriorityStr});"; ret += "\n";
            return ret;
        }

        public override string GenerateMainLinkSetupsSection()
        {
            string ret = "";
            //nothing
            return ret;
        }

        public override string GetFullTemplateArgs()
        {
            string ret = "";
            //nothing
            return ret;
        }

        public override string GetFullTemplateArgsValues()
        {
            string ret = "";
            //nothing
            return ret;
        }

        public override string GetFullTemplateType()
        {
            string ret = "";
            //nothing
            return ret;
        }

        protected override string _GenerateAEConfigSection(int numOfAOOfThisSameTypeGeneratesAlready)
        {
            //DONT DO THIS!!!!!
            //#define AnyOtherNeededIncludes2 LoopObjeect1Test 
            string ret = "";
            //nothing
            return ret;
        }

        protected override List<RelativeDirPathWrite> _WriteTheContentedToFiles()
        {
            List<RelativeDirPathWrite> relativeDirPathWrites = new List<RelativeDirPathWrite>();


            string contentesOut = AEInitializing.TheMacro2Session.GenerateFileOut("AERTOS/LoopObjectClass",
    new MacroVar() { MacroName = "ClassName", VariableValue = $"{ClassName}" },
    new MacroVar() { MacroName = "InitFunction", VariableValue = $"{GetInitializationFunction()}" }
    );
            relativeDirPathWrites.Add(new RelativeDirPathWrite($"{ClassName}", ".h", "include", contentesOut, true));
            return relativeDirPathWrites;
        }
    }




}
