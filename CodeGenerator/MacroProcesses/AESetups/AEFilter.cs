using CodeGenerator.ProblemHandler;
using System.Collections.Generic;

namespace CgenMin.MacroProcesses
{

    public abstract class AEFilter_ConstructorArg<TargTyp1, TargTyp2, TargTyp3> : AEFilter
    {
        CppFunctionArgsWithValue<TargTyp1, TargTyp2, TargTyp3> cppFunctionArgs;


        protected AEFilter_ConstructorArg(
            string nameOfConstructorArg1, TargTyp1 arg1Value,
            string nameOfConstructorArg2, TargTyp2 arg2Value,
            string nameOfConstructorArg3, TargTyp3 arg3Value,
            string fromeLibraryName, int filterSamplingNum)//, bool isUserInputedFilterSampling)
            : base(fromeLibraryName, filterSamplingNum)//isUserInputedFilterSampling)
        {
            ProblemHandle problemHandle = new ProblemHandle();
            //if (typeof(TargTyp1) == typeof(float) || typeof(TargTyp2) == typeof(float) || typeof(TargTyp3) == typeof(float))
            //{
            //    problemHandle.ThereisAProblem("the argument type of the tempalte arg type cannot be a float.");
            //}
            cppFunctionArgs = new CppFunctionArgsWithValue<TargTyp1, TargTyp2, TargTyp3>(
               nameOfConstructorArg1, arg1Value, 
               nameOfConstructorArg2, arg2Value,  
               nameOfConstructorArg3, arg3Value, false, false, false);
        }
        public override string GetAdditionalTemplateArgs()
        {
            return cppFunctionArgs.GetContructorArgsWithoutTypes();
        }

        public override string GetAdditionalTemplateTypes()
        {
            return cppFunctionArgs.GetContructorArgs();
        }
        public override string GetAdditionalTemplateValues()
        {
            var rr1 = ((CppFunctionArgWithValue<TargTyp1>)(cppFunctionArgs.TheCppFunctionArgs[0])).ValueOfArg1;
            var rr2 = ((CppFunctionArgWithValue<TargTyp2>)(cppFunctionArgs.TheCppFunctionArgs[1])).ValueOfArg1;
            var rr3 = ((CppFunctionArgWithValue<TargTyp3>)(cppFunctionArgs.TheCppFunctionArgs[2])).ValueOfArg1;
            return $"{rr1}, {rr2}, {rr3}";
        }

        public override string GetFloatWorkAroundDec()
        {
            if (cppFunctionArgs == null)
            { return ""; }

            string FloatWorkAroundDec = "";
            string FloatWorkAroundDef = "";
            foreach (var cpparg in this.cppFunctionArgs.TheCppFunctionArgs)//TheCppFunctionArgs.TheCppFunctionArgs)
            {
                if (cpparg.Type == CppTypeEnum.float_t)
                {
                    // float G_name;
                    FloatWorkAroundDec += $"float G_{cpparg.Name}; \n";
                    // _name T_name;
                    // G_name =  T_name.GetValue();
                    FloatWorkAroundDef += $"_{cpparg.Name} T_{cpparg.Name}; \n G_{cpparg.Name} =  T_{cpparg.Name}.GetValue(); \n";
                }
            }
            return FloatWorkAroundDec;
        }

        public override string GetFloatWorkAroundDef()
        {
            if (cppFunctionArgs == null)
            { return ""; }

            string FloatWorkAroundDec = "";
            string FloatWorkAroundDef = "";
            foreach (var cpparg in this.cppFunctionArgs.TheCppFunctionArgs)//TheCppFunctionArgs.TheCppFunctionArgs)
            {
                if (cpparg.Type == CppTypeEnum.float_t)
                {
                    // float G_name;
                    FloatWorkAroundDec += $"float G_{cpparg.Name}; \n";
                    // _name T_name;
                    // G_name =  T_name.GetValue();
                    FloatWorkAroundDef += $"_{cpparg.Name} T_{cpparg.Name}; \n G_{cpparg.Name} =  T_{cpparg.Name}.GetValue(); \n";
                }
            }
            return FloatWorkAroundDef;
        }
    }

    public abstract class AEFilter_ConstructorArg<TargTyp1, TargTyp2 > : AEFilter
    {
        CppFunctionArgsWithValue<TargTyp1, TargTyp2 > cppFunctionArgs;


        protected AEFilter_ConstructorArg(
            string nameOfConstructorArg1, TargTyp1 arg1Value,
            string nameOfConstructorArg2, TargTyp2 arg2Value, 
            string fromeLibraryName, int filterSamplingNum)//, bool isUserInputedFilterSampling), bool isUserInputedFilterSampling)
            : base(fromeLibraryName, filterSamplingNum)//, isUserInputedFilterSampling)
        {
            ProblemHandle problemHandle = new ProblemHandle();
            //if (typeof(TargTyp1) == typeof(float) || typeof(TargTyp2) == typeof(float)  )
            //{
            //    problemHandle.ThereisAProblem("the argument type of the tempalte arg type cannot be a float.");
            //}
            cppFunctionArgs = new CppFunctionArgsWithValue<TargTyp1, TargTyp2 >(
               nameOfConstructorArg1, arg1Value,
               nameOfConstructorArg2, arg2Value, false, false);
        }
        public override string GetAdditionalTemplateArgs()
        {
            return cppFunctionArgs.GetContructorArgsWithoutTypes();
        }

        public override string GetAdditionalTemplateTypes()
        {
            return cppFunctionArgs.GetContructorArgs();
        }
        public override string GetAdditionalTemplateValues()
        {
            var rr1 = ((CppFunctionArgWithValue<TargTyp1>)(cppFunctionArgs.TheCppFunctionArgs[0])).ValueOfArg1;
            var rr2 = ((CppFunctionArgWithValue<TargTyp2>)(cppFunctionArgs.TheCppFunctionArgs[1])).ValueOfArg1; 
            return $"{rr1}, {rr2} ";
        }

        public override string GetFloatWorkAroundDec()
        {
            if (cppFunctionArgs == null)
            { return ""; }

            string FloatWorkAroundDec = "";
            string FloatWorkAroundDef = "";
            foreach (var cpparg in this.cppFunctionArgs.TheCppFunctionArgs)//TheCppFunctionArgs.TheCppFunctionArgs)
            {
                if (cpparg.Type == CppTypeEnum.float_t)
                {
                    // float G_name;
                    FloatWorkAroundDec += $"float G_{cpparg.Name}; \n";
                    // _name T_name;
                    // G_name =  T_name.GetValue();
                    FloatWorkAroundDef += $"_{cpparg.Name} T_{cpparg.Name}; \n G_{cpparg.Name} =  T_{cpparg.Name}.GetValue(); \n";
                }
            }
            return FloatWorkAroundDec;
        }

        public override string GetFloatWorkAroundDef()
        {
            if (cppFunctionArgs == null)
            { return ""; }

            string FloatWorkAroundDec = "";
            string FloatWorkAroundDef = "";
            foreach (var cpparg in this.cppFunctionArgs.TheCppFunctionArgs)//TheCppFunctionArgs.TheCppFunctionArgs)
            {
                if (cpparg.Type == CppTypeEnum.float_t)
                {
                    // float G_name;
                    FloatWorkAroundDec += $"float G_{cpparg.Name}; \n";
                    // _name T_name;
                    // G_name =  T_name.GetValue();
                    FloatWorkAroundDef += $"_{cpparg.Name} T_{cpparg.Name}; \n G_{cpparg.Name} =  T_{cpparg.Name}.GetValue(); \n";
                }
            }
            return FloatWorkAroundDef;
        }
    }

    public abstract class AEFilter_ConstructorArg<TargTyp1> : AEFilter
    {
        CppFunctionArgsWithValue<TargTyp1> cppFunctionArgs;
         

        protected AEFilter_ConstructorArg(string nameOfConstructorArg1, TargTyp1 arg1Value, string fromeLibraryName, int filterSamplingNum)//, bool isUserInputedFilterSampling)
            : base(fromeLibraryName, filterSamplingNum)//, isUserInputedFilterSampling)
        {
            ProblemHandle problemHandle = new ProblemHandle();
            //if (typeof(TargTyp1) == typeof(float))
            //{
            //    problemHandle.ThereisAProblem("the argument type of the tempalte arg1 type cannot be a float.");
            //}
             cppFunctionArgs = new CppFunctionArgsWithValue<TargTyp1>(
                nameOfConstructorArg1, arg1Value, false);
        } 
        public override string GetAdditionalTemplateArgs()
        {
            return cppFunctionArgs.GetContructorArgsWithoutTypes();
        }

        public override string GetAdditionalTemplateTypes()
        {
            string ret = cppFunctionArgs.GetContructorArgs(); 
            return ret.Replace("float","class");
        }
        public override string GetAdditionalTemplateValues()
        {
            var rr = ((CppFunctionArgWithValue<TargTyp1>)(cppFunctionArgs.TheCppFunctionArgs[0])).GetValueOfArgStr();



            return $"{rr}";
        }

        public override string GetFloatWorkAroundDec()
        {
            if (cppFunctionArgs == null)
            { return ""; }

            string FloatWorkAroundDec = "";
            string FloatWorkAroundDef = "";
            foreach (var cpparg in this.cppFunctionArgs.TheCppFunctionArgs)//TheCppFunctionArgs.TheCppFunctionArgs)
            {
                if (cpparg.Type == CppTypeEnum.float_t)
                {
                    // float G_name;
                    FloatWorkAroundDec += $"float G_{cpparg.Name}; \n";
                    // _name T_name;
                    // G_name =  T_name.GetValue();
                    FloatWorkAroundDef += $"_{cpparg.Name} T_{cpparg.Name}; \n G_{cpparg.Name} =  T_{cpparg.Name}.GetValue(); \n";
                }
            }
            return FloatWorkAroundDec;
        }

        public override string GetFloatWorkAroundDef()
        {
            if (cppFunctionArgs == null)
            { return ""; }

            string FloatWorkAroundDec = "";
            string FloatWorkAroundDef = ""; 
            foreach (var cpparg in this.cppFunctionArgs.TheCppFunctionArgs)//TheCppFunctionArgs.TheCppFunctionArgs)
            {
                if (cpparg.Type == CppTypeEnum.float_t)
                {
                    // float G_name;
                    FloatWorkAroundDec += $"float G_{cpparg.Name}; \n";
                    // _name T_name;
                    // G_name =  T_name.GetValue();
                    FloatWorkAroundDef += $"_{cpparg.Name} T_{cpparg.Name}; \n G_{cpparg.Name} =  T_{cpparg.Name}.GetValue(); \n";
                }
            }
            return FloatWorkAroundDef; 
        }

    }

    public abstract class AEFilter : AOObserver
    {

        public int FilterId { get; set; }
        public string HowItShowsUpInTemplateArg { get { 
                return $"Filter<{ClassName}{GetTemplateArgValues()}, {this.FilterSamplingNum}>"; 
            } 
        }//Filter<DerivativeFilter, 2>

        public int FilterSamplingNum { get; }
        public bool IsUserInputed { get; }

        public AEFilter FilterICameFrom { get; set; }
        static int FiltersCreatedSoFar = 0;

        public AEFilter(string fromLibraryName, int filterSamplingNum)//, bool isUserInputedFilterSampling)
            : base(fromLibraryName, $"filter{FiltersCreatedSoFar.ToString()}", AOTypeEnum.Filter)
        {
            //ClassName = filterName;
            FilterSamplingNum = filterSamplingNum;
            IsUserInputed = false;//isUserInputedFilterSampling;

            FilterICameFrom = null;

            FiltersCreatedSoFar++;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="spbToFlowTo"></param>
        /// <param name="toChannel"></param>
        /// <param name="linkType">Copy: all data is copied from the linked AO to this spb's channel.
        /// Reference   // all data is not copied but instead a reference is passed. do this if you dont intend on changing the data that is passed in.</param>
        /// <returns></returns>
        public AEFilter FlowIntoSPB(AESPBBase spbToFlowTo, SPBChannelNum toChannel, LinkTypeEnum linkType)
        {

            NumSPBSIPointTo++;

            spbToFlowTo.Channels[(int)toChannel].AOThatLinksToThisChannel = this;
            spbToFlowTo.Channels[(int)toChannel].AOFilterID_ThatLinksToThisChannel = this.FilterId;
            spbToFlowTo.Channels[(int)toChannel].LinkType = linkType;

            return this;
        }


        public void FlowIntoTDU(AEUtilityService tduToFlowTo )
        {
            ProblemHandle problemHandle = new ProblemHandle();
            if (this.SPBIOriginateFrom == null)
            {
                problemHandle.ThereisAProblem("filter needs to originate from an spb for it to flow into a TDU");

            }

            this.SPBIOriginateFrom.FlowIntoTDU(tduToFlowTo, this.FilterId);
        }


        protected override string _GenerateAEConfigSection(int numOfAOOfThisSameTypeGeneratesAlready)
        {

            return GetAdditionalIncludeInAEConfig(ClassName);
        }

        public override string GenerateMainHeaderSection()
        {
            return "";
        }
        public override string GenerateMainInitializeSection()
        {
            return "";
        }
        public override string GenerateMainClockSetupsSection()
        {
            return "";
        }
        public override string GenerateMainLinkSetupsSection()
        {
            return "";
        }

        public override string GenerateFunctionDefinesSection()
        {
            return "";
        }

        public virtual string GetAdditionalTemplateArgs()
        {
            return "";
        }
        public virtual string GetAdditionalTemplateTypes()
        {
            return "";
        }
        public virtual string GetAdditionalTemplateValues()
        {
            return "";
        }

        public virtual string GetFloatWorkAroundDec()
        {
            return "";
        }

        public virtual string GetFloatWorkAroundDef()
        {
            return "";
        }

        protected override List<RelativeDirPathWrite> _WriteTheContentedToFiles()
        {
            List<RelativeDirPathWrite> ret = new List<RelativeDirPathWrite>();

            string additionaltemplateTypes = GetAdditionalTemplateTypes() == "" ? "" : "," + GetAdditionalTemplateTypes();

            string Template = GetFullTemplateType() == "" && GetAdditionalTemplateTypes() == "" ? "" :
                GetFullTemplateType() != "" && GetAdditionalTemplateTypes() == "" ?
                $"template<{GetFullTemplateType()}>" :
                GetFullTemplateType() == "" && GetAdditionalTemplateTypes() != "" ? 
                $"template<{GetAdditionalTemplateTypes()}>" :
                $"template<{GetFullTemplateType()},  {GetAdditionalTemplateTypes()}>" ;
            //string additionaltemplateArgs = GetAdditionalTemplateArgs() == "" ? "" : "," + GetAdditionalTemplateArgs();


            string TemplateArgs = GetFullTemplateArgs() == "" && GetAdditionalTemplateArgs() == "" ? "" :
                GetFullTemplateArgs() != "" && GetAdditionalTemplateArgs() == "" ?
                $"<{GetFullTemplateArgs()}>" :
                GetFullTemplateArgs() == "" && GetAdditionalTemplateArgs() != "" ?
                $"<{GetAdditionalTemplateArgs()}>" :
                $"<{GetFullTemplateArgs()},  {GetAdditionalTemplateArgs()}>";

            //get cpp args and look for any that were floats
            string FloatWorkAroundDec = GetFloatWorkAroundDec();
            string FloatWorkAroundDef = GetFloatWorkAroundDef();
 
            string IncludeFriendTemplate = "";// TemplateArgs != "" ? "" : "//";

            string contentesOut = AEInitializing.TheMacro2Session.GenerateFileOut("AERTOS/FilterClass",
    new MacroVar() { MacroName = "FilterName", VariableValue = ClassName },
    new MacroVar() { MacroName = "PastBufferSize", VariableValue = FilterSamplingNum.ToString() },
    new MacroVar() { MacroName = "Template", VariableValue = Template },
    new MacroVar() { MacroName = "TemplateArgs", VariableValue = TemplateArgs },
    new MacroVar() { MacroName = "FloatWorkAroundDec", VariableValue = FloatWorkAroundDec },
    new MacroVar() { MacroName = "FloatWorkAroundDef", VariableValue = FloatWorkAroundDef },
    new MacroVar() { MacroName = "IncludeFriendTemplate", VariableValue = IncludeFriendTemplate }
    );
            ret.Add(new RelativeDirPathWrite($"{ClassName}", ".h", "include", contentesOut, true));


            return ret;
        }

        private string _GetFullTemplate(bool withTypes)
        {
            string uint32Type = "";
            string uint16Type = "";
            string boolType = "";
            if (withTypes)
            {
                uint16Type = "uint16_t";
                uint32Type = "uint32_t";
                boolType = "bool";
            }

            string ret = "";
            if (IsUserInputed)
            {
                ret += $"{uint32Type} SampleSize ";
            }

            return ret;
        }

        public string GetTemplateArgValues()
        { 
            string args = "";
            if (IsUserInputed == true)
            {
                args += $"{FilterSamplingNum}";
            }

            args = IsUserInputed == false && GetAdditionalTemplateValues() == "" ? "" :
    IsUserInputed == true && GetAdditionalTemplateValues() == "" ?
    $"{FilterSamplingNum}" :
    IsUserInputed == false && GetAdditionalTemplateValues() != "" ?
    $"{GetAdditionalTemplateValues()}" :
    $"{FilterSamplingNum},  {GetAdditionalTemplateValues()}"; 

            return args == "" ? "" : $"<{args}>";
        }

        public override string GetFullTemplateArgs()
        {

            return _GetFullTemplate(false);
        }
        public override string GetFullTemplateType()
        {
            return _GetFullTemplate(true);
        }
        public override string GetFullTemplateArgsValues()
        {
            return IsUserInputed ? $"{this.FilterSamplingNum}" : "";
        }
    }
}
