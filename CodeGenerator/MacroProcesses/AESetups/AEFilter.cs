using System.Collections.Generic;

namespace CgenMin.MacroProcesses
{



    public abstract class AEFilter : AOObserver
    {

        public int FilterId { get; set; }
        public string HowItShowsUpInTemplateArg { get { return $"Filter<{ClassName}, {this.FilterSamplingNum}>"; } }//Filter<DerivativeFilter, 2>

        public int FilterSamplingNum { get; }
        public bool IsUserInputed { get; }

        public AEFilter FilterICameFrom { get; set; }
        static int FiltersCreatedSoFar = 0;

        public AEFilter(string fromLibraryName,   int filterSamplingNum, bool isUserInputedFilterSampling) 
            : base(fromLibraryName,  $"filter{FiltersCreatedSoFar.ToString()}", AOTypeEnum.Filter)
        {
            //ClassName = filterName;
            FilterSamplingNum = filterSamplingNum;
            IsUserInputed = isUserInputedFilterSampling;

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
        public AEFilter FlowIntoSPB(AESPBBase spbToFlowTo, int toChannel, LinkTypeEnum linkType)
        {

            spbToFlowTo.Channels[toChannel].AOThatLinksToThisChannel = this;
            spbToFlowTo.Channels[toChannel].AOFilterID_ThatLinksToThisChannel = this.FilterId;
            spbToFlowTo.Channels[toChannel].LinkType = linkType;

            return this;
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

        protected override List<RelativeDirPathWrite> _WriteTheContentedToFiles()
        {
            List<RelativeDirPathWrite> ret = new List<RelativeDirPathWrite>();

            string Template = GetFullTemplateType() == "" ? "" :
                $"template<{GetFullTemplateType()}>";
            string TemplateArgs =  GetFullTemplateArgs() == "" ? "" :
                $"<{GetFullTemplateArgs()}>";  

            string contentesOut = AEInitializing.TheMacro2Session.GenerateFileOut("AERTOS/FilterClass",
    new MacroVar() { MacroName = "FilterName", VariableValue = ClassName },
    new MacroVar() { MacroName = "PastBufferSize", VariableValue = FilterSamplingNum.ToString() },
    new MacroVar() { MacroName = "Template", VariableValue = Template },
    new MacroVar() { MacroName = "TemplateArgs", VariableValue = TemplateArgs }   
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
