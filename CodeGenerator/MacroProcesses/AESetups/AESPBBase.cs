using CodeGenerator.MacroProcesses;
using CodeGenerator.ProblemHandler;
using System.Collections.Generic;
using System.Linq;

namespace CgenMin.MacroProcesses
{
    public class SizeOfSPBOutput//_IfUserDefined SizeOfSPBOutput
    {
        public bool IsUserDefined { get; protected set; }
        public int SizeOfOutput { get; protected set; }
        public SizeOfSPBOutput(int sizeOfOutput, bool isUserDefined)
        {
            SizeOfOutput = sizeOfOutput;
            IsUserDefined = isUserDefined;
        }
    }






    public abstract class AESPBBase_VariableNumOfChannels_AllSameCountBufferSizes : AESPBBase
    {
        protected AESPBBase_VariableNumOfChannels_AllSameCountBufferSizes(string fromLibraryName, string nameOfSPB, StyleOfSPB styleOfSPB, string templateType, string templateargs, bool isSubscribable, SizeOfSPBOutput sizeOfSPBOutput, int numOfChannels, int AllchannelsCountBufferSizes)
            : base(fromLibraryName, nameOfSPB)//, styleOfSPB, templateType, templateargs, isSubscribable,  sizeOfSPBOutput,  channels.Cast<SPBChannelBase>().ToList()
        {

            List<SPBChannelUserDefinedCountBuffer> sPBChannelUserDefinedCountBuffer = new List<SPBChannelUserDefinedCountBuffer>();
            for (int i = 0; i < numOfChannels; i++)
            {
                sPBChannelUserDefinedCountBuffer.Add(new SPBChannelUserDefinedCountBuffer(AllchannelsCountBufferSizes));
            }

            init(styleOfSPB, templateType, templateargs, isSubscribable, sizeOfSPBOutput, sPBChannelUserDefinedCountBuffer.Cast<SPBChannelBase>().ToList());
            IsNumOFInputChannelsUserDefined = true;
        }
    }

    public abstract class AESPBBase_VariableNumOfChannels_VariableCountBufferSizes : AESPBBase
    {
        protected AESPBBase_VariableNumOfChannels_VariableCountBufferSizes(string fromLibraryName, string nameOfSPB, StyleOfSPB styleOfSPB, string templateType, string templateargs, bool isSubscribable, SizeOfSPBOutput sizeOfSPBOutput, params SPBChannelUserDefinedCountBuffer[] channels)
            : base(fromLibraryName, nameOfSPB)//, styleOfSPB, templateType, templateargs, isSubscribable,  sizeOfSPBOutput,  channels.Cast<SPBChannelBase>().ToList()
        {

            init(styleOfSPB, templateType, templateargs, isSubscribable, sizeOfSPBOutput, channels.Cast<SPBChannelBase>().ToList());
            IsNumOFInputChannelsUserDefined = true;
        }
    }

    public abstract class AESPBBase_VariableNumOfChannels : AESPBBase
    {
        protected AESPBBase_VariableNumOfChannels(string fromLibraryName, string nameOfSPB, StyleOfSPB styleOfSPB, string templateType, string templateargs, bool isSubscribable, SizeOfSPBOutput sizeOfSPBOutput, params SPBChannelBase[] channels)
            : base(fromLibraryName, nameOfSPB)//, styleOfSPB, templateType, templateargs, isSubscribable,  sizeOfSPBOutput,  channels.Cast<SPBChannelBase>().ToList()
        {

            init(styleOfSPB, templateType, templateargs, isSubscribable, sizeOfSPBOutput, channels.Cast<SPBChannelBase>().ToList());
            IsNumOFInputChannelsUserDefined = true;
        }
    }

    public abstract class AESPBBase : AOObserver, IPartOfAEDefines//<Tch1, Tch2, Tch3, Tch4, Tch5>  : AOObserver 
                                                                  //where Tch1 : SPBChannelBase, new()
                                                                  //where Tch2 : SPBChannelBase, new()
                                                                  //where Tch3 : SPBChannelBase, new()
                                                                  //where Tch4 : SPBChannelBase, new()
                                                                  //where Tch5 : SPBChannelBase, new()
    {
        //Tch1 Channel1;
        //Tch2 Channel2;
        //Tch3 Channel3;
        //Tch4 Channel4;
        //Tch5 Channel5;

        //public string NameOfSPB { get; protected set; }
        public string TemplateType { get; protected set; }
        public string Templateargs { get; protected set; }
        public bool IsSubscribable { get; protected set; }
        public List<SPBChannelBase> Channels { get; protected set; }
        public SizeOfSPBOutput SizeOfOutput { get; protected set; }
        public bool IsNumOFInputChannelsUserDefined { get; protected set; }
        public StyleOfSPB TheStyleOfSPB { get; protected set; }

        public bool FlowsIntoTDU { get; protected set; }
        List<AEUtilityService> TDUsToFlowTo = new List<AEUtilityService>();


        public AESPBBase FlowIntoSPB(AESPBBase spbToFlowTo, SPBChannelNum toChannel, LinkTypeEnum linkType)
        {
            NumSPBSIPointTo++;

            spbToFlowTo.Channels[(int)toChannel].AOThatLinksToThisChannel = this;
            spbToFlowTo.Channels[(int)toChannel].AOFilterID_ThatLinksToThisChannel = 0;
            spbToFlowTo.Channels[(int)toChannel].LinkType = linkType;

            return this;
        }

        public void FlowIntoTDU(AEUtilityService tduToFlowTo, int fromFilter = 0)
        {
            tduToFlowTo.FilterNumIFlowFrom = fromFilter;
            tduToFlowTo.FlowsFromSPB = true;
            FlowsIntoTDU = true;
            TDUsToFlowTo.Add(tduToFlowTo);
        }


        public string Get_AESPBObservorOutputType_TemplateArgsValues()
        {
            //       template < uint32_t OUTPUTSIZE = 1, uint16_t NUMOFINPUTSIGNALS = 1, bool IsOutputSubscribeAble = false, uint8_t NUMOFFILTERS = 0,  class TFilter1 = AENullClass, class TFilter2 = AENullClass, uint8_t Filter2LinksFrom = 0, class TFilter3 = AENullClass, uint8_t Filter3LinksFrom = 0, class TFilter4 = AENullClass, uint8_t Filter4LinksFrom = 0, class TFilter5 = AENullClass, uint8_t Filter5LinksFrom = 0,
            //uint32_t TheParameterNOTone1 = 1, bool isParameterNOToneInputSize1 = true,
            //uint32_t TheParameterNOTone2 = 1, bool isParameterNOToneInputSize2 = true,
            //uint32_t TheParameterNOTone3 = 1, bool isParameterNOToneInputSize3 = true,
            //uint32_t TheParameterNOTone4 = 1, bool isParameterNOToneInputSize4 = true,
            //uint32_t TheParameterNOTone5 = 1, bool isParameterNOToneInputSize5 = true,
            //uint32_t TheParameterNOTone6 = 1, bool isParameterNOToneInputSize6 = true,
            //uint32_t TheParameterNOTone7 = 1, bool isParameterNOToneInputSize7 = true,
            //uint32_t TheParameterNOTone8 = 1, bool isParameterNOToneInputSize8 = true,
            //uint32_t TheParameterNOTone9 = 1, bool isParameterNOToneInputSize9 = true,
            //uint32_t TheParameterNOTone10 = 1, bool isParameterNOToneInputSize10 = true,
            //uint32_t TheParameterNOTone11 = 1, bool isParameterNOToneInputSize11 = true,
            //uint32_t TheParameterNOTone12 = 1, bool isParameterNOToneInputSize12 = true

            //everything depends on what arguments are locked in and what are user defined. locked in args need to have values defined here.
            string ret = "";


            //template < uint32_t OUTPUTSIZE = 1, uint16_t NUMOFINPUTSIGNALS = 1, bool IsOutputSubscribeAble = false,
            string inputChnum = this.IsNumOFInputChannelsUserDefined ? "NUMOFINPUTSIGNALS" : Channels.Count.ToString();
            if (this.SizeOfOutput.IsUserDefined)
            {
                ret += $"AESPBObservorOutputType <OUTPUTSIZE, {inputChnum}, isSubscribable,"; ret += "\n";
            }
            else
            {
                ret += $"AESPBObservorOutputType <{SizeOfOutput.SizeOfOutput.ToString()}, {inputChnum}, isSubscribable,"; ret += "\n";
            }


            //assume all filters ARE user definable
            //uint8_t NUMOFFILTERS = 0,  class TFilter1 = AENullClass, class TFilter2 = AENullClass, uint8_t Filter2LinksFrom = 0, class TFilter3 = AENullClass, uint8_t Filter3LinksFrom = 0, class TFilter4 = AENullClass, uint8_t Filter4LinksFrom = 0, class TFilter5 = AENullClass, uint8_t Filter5LinksFrom = 0,
            ret += $"TEMPLATESPB_FilterParams,"; ret += "\n";

            //uint32_t TheParameterNOTone1 = 1, bool isParameterNOToneInputSize1 = true,
            for (int i = 1; i <= 12; i++)
            {
                string chanNumStr = this.IsNumOFInputChannelsUserDefined ? "" : $"{i.ToString()}";

                if (Channels.Count >= i || this.IsNumOFInputChannelsUserDefined == true)
                {
                    var ch = this.IsNumOFInputChannelsUserDefined ? Channels[0] : Channels[i - 1];

                    //if (this.IsNumOFInputChannelsUserDefined)
                    //{
                    //    templateType =
                    //                this.Channels[0].isUserDefinedCountBuffer ?
                    //                templateType + $", {uint32Type} CHANNELCOUNTBUFFER" :
                    //                templateType + $", {uint32Type} CHANNEL_CONSUMPTION_SIZE";
                    //}
                    if (ch.isUserDefined)
                    {
                        if (ch.isUserDefinedCountBuffer)
                        {
                            ret += $"CHANNELCOUNTBUFFER{chanNumStr}, false,"; ret += "\n";
                        }
                        else if (ch.isUserDefinedConsumptionSize)
                        {
                            ret += $"CHANNEL_CONSUMPTION_SIZE{chanNumStr}, true,"; ret += "\n";
                        }
                    }
                    else
                    {
                        if (ch.ChannelCountBuffer == 1 && ch.SizeOfDataTheChannelExpectsToConsume == 1)
                        {
                            ret += $"{ch.ChannelCountBuffer.ToString()}, false,"; ret += "\n";
                        }
                        else if (ch.ChannelCountBuffer > 1)
                        {
                            ret += $"{ch.ChannelCountBuffer.ToString()}, false,"; ret += "\n";
                        }
                        else if (ch.SizeOfDataTheChannelExpectsToConsume > 1)
                        {
                            ret += $"{ch.SizeOfDataTheChannelExpectsToConsume.ToString()}, true,"; ret += "\n";
                        }
                    }

                }
                else
                {
                    ret += $"";
                }

            }

            //Remove last comma
            ret = ret.Remove(ret.Length - 2, 2);
            ret += ">";

            return ret;
        }

        private string _GetFullTemplateArgs(bool withTypes)
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

            //create the template here based on the user defined countbuffers 
            //issubscrbable is always user defined. 
            string comma = this.Templateargs.Trim() == string.Empty ? "" : ",";
            string outputsize = this.SizeOfOutput.IsUserDefined == true ? $" {uint32Type} OUTPUTSIZE, " : "";
            string inputChNum = this.IsNumOFInputChannelsUserDefined == true ? $" {uint16Type} NUMOFINPUTSIGNALS, " : "";
            string templateType = $"{this.TemplateType}{comma}{outputsize}{inputChNum}{boolType} isSubscribable";
            int ind = 1;
            if (this.IsNumOFInputChannelsUserDefined)
            {
                templateType =
                            this.Channels[0].isUserDefinedCountBuffer ?
                            templateType + $", {uint32Type} CHANNELCOUNTBUFFER" :
                            templateType + $", {uint32Type} CHANNEL_CONSUMPTION_SIZE";
            }
            else
            {
                foreach (SPBChannelBase ch in this.Channels)
                {
                    //if CHANNELCOUNTBUFFER1 us user defined
                    if (ch.isUserDefined)
                    {
                        templateType =
                            ch.isUserDefinedCountBuffer ?
                            templateType + $", {uint32Type} CHANNELCOUNTBUFFER{ind}" :
                            templateType + $", {uint32Type} CHANNEL_CONSUMPTION_SIZE{ind}";
                    }

                    ind++;
                }
            }

            return templateType;
        }
        public override string GetFullTemplateArgs()
        {
            string templateType = _GetFullTemplateArgs(false) + ", TEMPLATESPB_FilterParams";

            return templateType;
        }

        public string GetFullTemplateTypeNoDefaults()
        {
            string templateType = _GetFullTemplateArgs(false) + ", TEMPLATESPB_FiltersFunctionParams";

            return templateType;
        }

        public string GetFullTemplateTypeNoDefaultsArgs()
        {
            string templateType = _GetFullTemplateArgs(true) + ", TEMPLATESPB_FiltersFunctionParams";

            return templateType;
        }

        public override string GetFullTemplateType()
        {

            string templateType = _GetFullTemplateArgs(true) + ", TEMPLATESPB_Filters";

            return templateType;
        }


        public override string GetFullTemplateArgsValues()
        {

            string comma = this.TemplateType == string.Empty ? "" : ",";

            //create the template arguments based on what was user defined.
            string outputsize = this.SizeOfOutput.IsUserDefined == true ? $" {this.SizeOfOutput.ToString()}, " : "";
            string inputChNum = this.IsNumOFInputChannelsUserDefined == true ? $" {this.Channels.Count.ToString()}, " : "";
            string issub = this.IsSubscribable ? "true" : "false";
            string templateArgs = $"{outputsize}{inputChNum}{this.TemplateType}{comma} {issub}";

            if (this.IsNumOFInputChannelsUserDefined)
            {
                templateArgs =
                            this.Channels[0].isUserDefinedCountBuffer ?
                            templateArgs + $", {Channels[0].ChannelCountBuffer}" :
                            templateArgs + $", {Channels[0].SizeOfDataTheChannelExpectsToConsume}";
            }
            else
            {
                foreach (SPBChannelBase ch in this.Channels)
                {
                    //if CHANNELCOUNTBUFFER1 us user defined
                    if (ch.isUserDefined)
                    {
                        templateArgs =
                            ch.isUserDefinedCountBuffer ? templateArgs + $",  {ch.ChannelCountBuffer}" :
                            templateArgs + $",  {ch.SizeOfDataTheChannelExpectsToConsume}";
                    }
                }
            }



            templateArgs = this.GetFilterTemplateArgsValues() == string.Empty ? templateArgs : templateArgs + ", " + GetFilterTemplateArgsValues();

            //get filters
            //if (this.FiltersIflowTo.Count == 0)
            //{

            //}
            //else
            //{
            //    //treat the first filter differently as it does not have a second param
            //    if (this.FiltersIflowTo.Count > 0)
            //    {
            //        templateArgs += $",  {FiltersIflowTo.Count.ToString()}, {FiltersIflowTo[0].HowItShowsUpInTemplateArg}";
            //    }

            //    for (int i = +1; i < this.FiltersIflowTo.Count - 1; i++)
            //    {

            //        string secondArg = FiltersIflowTo[i].FilterICameFrom == null ? "0" : FiltersIflowTo[i].FilterICameFrom.FilterId.ToString();
            //        templateArgs += $",{FiltersIflowTo[i].HowItShowsUpInTemplateArg} ,{secondArg}  ";
            //    }
            //}

            return templateArgs + "";
        }


        protected AESPBBase(string fromLibraryName, string nameOfSPB)
             : base(fromLibraryName, nameOfSPB, AOTypeEnum.SPB)
        {

        }


        /// <summary>
        /// sv
        /// </summary>
        /// <param name="nameOfSPB">s</param>
        /// <param name="typeOfSPB"> the typeOfSPB is the name of the spb class. For example "AverageSPB"</param>
        /// <param name="templateType"> these are additional template types the spb uses. </param>
        /// <param name="templateargs"> these are additional template args the spb uses. </param>
        /// <param name="sizeOfOutput"> this is the output size of the spb. for example a pose filter might have a size three for eulter angle outputs theta, beta, gamma</param>
        public AESPBBase(string fromLibraryName, string nameOfSPB, StyleOfSPB styleOfSPB, string templateType, string templateargs, bool isSubscribable, SizeOfSPBOutput sizeOfOutput, SPBChannelBase channel1, params SPBChannelBase[] channels)
        : base(fromLibraryName, nameOfSPB, AOTypeEnum.SPB)
        {
            Channels = new List<SPBChannelBase>();
            Channels.Add(channel1);
            foreach (var item in channels)
            {
                Channels.Add(item);
            }

            this.init(styleOfSPB, templateType, templateargs, isSubscribable, sizeOfOutput, Channels);
            IsNumOFInputChannelsUserDefined = false;

        }


        public AESPBBase(string fromLibraryName, string nameOfSPB, StyleOfSPB styleOfSPB, string templateType, string templateargs, bool isSubscribable, SizeOfSPBOutput sizeOfOutput, List<SPBChannelBase> channels)
        : base(fromLibraryName, nameOfSPB, AOTypeEnum.SPB)
        {
            init(styleOfSPB, templateType, templateargs, isSubscribable, sizeOfOutput, channels);
            IsNumOFInputChannelsUserDefined = true;
        }

        public AESPBBase(string fromLibraryName, string nameOfSPB, StyleOfSPB styleOfSPB, string templateType, string templateargs, bool isSubscribable, SizeOfSPBOutput sizeOfOutput, int numOfOfChannels, SPBChannelUserDefinedCountBuffer channelsAllSame)
        : base(fromLibraryName, nameOfSPB, AOTypeEnum.SPB)
        {
            List<SPBChannelBase> channels = new List<SPBChannelBase>();
            channels.Add(channelsAllSame);
            for (int i = 1; i < numOfOfChannels; i++)
            {
                channels.Add(new SPBChannelBase(channelsAllSame));
            }

            init(styleOfSPB, templateType, templateargs, isSubscribable, sizeOfOutput, channels.Cast<SPBChannelBase>().ToList());
            IsNumOFInputChannelsUserDefined = true;
        }
        public AESPBBase(string fromLibraryName, string nameOfSPB, StyleOfSPB styleOfSPB, string templateType, string templateargs, bool isSubscribable, SizeOfSPBOutput sizeOfOutput, int numOfOfChannels, SPBChannelUserDefinedChannelConsumptionSize channelsAllSame)
        : base(fromLibraryName, nameOfSPB, AOTypeEnum.SPB)
        {
            List<SPBChannelBase> channels = new List<SPBChannelBase>();
            channels.Add(channelsAllSame);
            for (int i = 1; i < numOfOfChannels; i++)
            {
                channels.Add(new SPBChannelBase(channelsAllSame));
            }
            init(styleOfSPB, templateType, templateargs, isSubscribable, sizeOfOutput, channels.Cast<SPBChannelBase>().ToList());
            IsNumOFInputChannelsUserDefined = true;
        }
        public AESPBBase(string fromLibraryName, string nameOfSPB, StyleOfSPB styleOfSPB, string templateType, string templateargs, bool isSubscribable, SizeOfSPBOutput sizeOfOutput, int numOfOfChannels, SPBChannelLockedInCountBuffer channelsAllSame = null)
        : base(fromLibraryName, nameOfSPB, AOTypeEnum.SPB)
        {
            List<SPBChannelBase> channels = new List<SPBChannelBase>();
            channels.Add(channelsAllSame);
            for (int i = 1; i < numOfOfChannels; i++)
            {
                channels.Add(new SPBChannelBase(channelsAllSame));
            }
            init(styleOfSPB, templateType, templateargs, isSubscribable, sizeOfOutput, channels.Cast<SPBChannelBase>().ToList());
            IsNumOFInputChannelsUserDefined = true;
        }
        public AESPBBase(string fromLibraryName, string nameOfSPB, StyleOfSPB styleOfSPB, string templateType, string templateargs, bool isSubscribable, SizeOfSPBOutput sizeOfOutput, int numOfOfChannels, SPBChannelLockedInChannelConsumptionSize channelsAllSame = null)
            : base(fromLibraryName, nameOfSPB, AOTypeEnum.SPB)
        {
            List<SPBChannelBase> channels = new List<SPBChannelBase>();
            channels.Add(channelsAllSame);
            for (int i = 1; i < numOfOfChannels; i++)
            {
                channels.Add(new SPBChannelBase(channelsAllSame));
            }
            init(styleOfSPB, templateType, templateargs, isSubscribable, sizeOfOutput, channels.Cast<SPBChannelBase>().ToList());
            IsNumOFInputChannelsUserDefined = true;
        }

        protected void init(StyleOfSPB styleOfSPB, string templateType, string templateargs, bool isSubscribable, SizeOfSPBOutput sizeOfOutput, List<SPBChannelBase> channels)
        {
            //NameOfSPB = nameOfSPB;
            TemplateType = templateType;
            Templateargs = templateargs;
            IsSubscribable = isSubscribable;
            Channels = channels;

            TheStyleOfSPB = styleOfSPB;

            SizeOfOutput = sizeOfOutput;

            //if (Channel1 != null)
            //{
            //    Channels.Add(Channel1);
            //}
            //if (Channel2 != null)
            //{
            //    Channels.Add(Channel2);
            //}
            //if (Channel3 != null)
            //{
            //    Channels.Add(Channel3);
            //}
            //if (Channel4 != null)
            //{
            //    Channels.Add(Channel4);
            //}
            //if (Channel5 != null)
            //{
            //    Channels.Add(Channel5);
            //}

        }




        protected override string _GenerateAEConfigSection(int numOfAOOfThisSameTypeGeneratesAlready)
        {

            return this.SetAEConfig(numOfAOOfThisSameTypeGeneratesAlready);
        }
        public override string GenerateMainHeaderSection()
        {
            //nothing
            return "";
        }
        public override string GenerateMainInitializeSection()
        {
            //static ThreeDimensionalVector<0> velocitySPBL; velocitySPBL.InitSPBObserver(); 
            //velocitySPB = &velocitySPBL;

            string ret = "";


            ret += $"static {this.ClassName}<{GetFullTemplateArgsValues()}> {this.InstanceName}L; {this.InstanceName}L.InitSPBObserver(StyleOfSPB::{AO.GetStyleOfSPBStr(this.TheStyleOfSPB)}); "; ret += "\n";
            ret += $"{this.InstanceName} = &{this.InstanceName}L;"; ret += "\n";

            return ret;
        }
        public override string GenerateMainClockSetupsSection()
        {
            //nothing
            return "";
        }
        public override string GenerateMainLinkSetupsSection()
        {
            //static float chBuffer1[1];
            //static float chBuffer2[1];
            //static float chBuffer3[1];
            //velocitySPB->AddSignalFlowLinkToChannelWithCopy1(sensor1, chBuffer1, 1);
            //velocitySPB->AddSignalFlowLinkToChannelWithCopy2(sensor2, chBuffer2, 1);
            //velocitySPB->AddSignalFlowLinkToChannelWithCopy3(sensor3, chBuffer3, 1);
            //averageSPB->AddSignalFlowLinkToChannelWithReference((AESPBObservor*)sensor1, 1, 0); 

            string ret = "";

            int ind = 1;
            foreach (SPBChannelBase ch in this.Channels)
            {
                //get the name of the originating AO that links to this one.
                string originatingAOName = ch.OriginatingAO.InstanceName;


                if (ch.LinkType == LinkTypeEnum.Copy)
                {
                    ret += $"static float {this.InstanceName}chBuffer{ind.ToString()}[{ch.ChannelCountBuffer}];"; ret += "\n";
                    ret += $"{this.InstanceName}->AddSignalFlowLinkToChannelWithCopy{ind.ToString()}({originatingAOName}, {this.InstanceName}chBuffer{ind.ToString()}, {ch.AOFilterID_ThatLinksToThisChannel});"; ret += "\n";

                    ind++;
                }
                else if (ch.LinkType == LinkTypeEnum.Reference)
                {
                    ret += $"{this.InstanceName}->AddSignalFlowLinkToChannelWithReference((AESPBObservor*){originatingAOName},  {ind.ToString()}, {ch.AOFilterID_ThatLinksToThisChannel});"; ret += "\n";
                }
            }

            foreach (var tdu in TDUsToFlowTo)
            {
                
                   ret += $"{tdu.InstanceName}->SetToSPBTick({this.InstanceName}, {tdu.FilterNumIFlowFrom});"; ret += "\n";
            }

            return ret;
        }
        public override string GenerateFunctionDefinesSection()
        {
            //nothing
            return "";
        }



        protected override List<RelativeDirPathWrite> _WriteTheContentedToFiles()
        {
            List<RelativeDirPathWrite> relativeDirPathWrites = new List<RelativeDirPathWrite>();

            string ChInstantiation = "";

            if (this.IsNumOFInputChannelsUserDefined)
            {
                //float* AllChannels[NUMOFINPUTSIGNALS];
                //for (int i = 0; i < NUMOFINPUTSIGNALS; i++)
                //{
                //    AllChannels[i] = this->InputChannels[i]->ChannelSignalBufferSingle;
                //}
                ChInstantiation += $"float* AllChannels[NUMOFINPUTSIGNALS]; \n";
                ChInstantiation += $"       for (int i = 0; i < NUMOFINPUTSIGNALS; i++) \n";
                ChInstantiation += "        {        \n";
                ChInstantiation += $"           AllChannels[i] = this->InputChannels[i]->ChannelSignalBufferSingle; \n";
                ChInstantiation += "        }       \n";
            }
            else
            {
                for (int i = 1; i < this.Channels.Count + 1; i++)
                {
                    ChInstantiation += $"float* ch{i.ToString()} = this->InputChannels[{(i - 1).ToString()}]->ChannelSignalBufferSingle; \n";
                }
            }


            //class .h file   -----------------------------------  

            string contentesOut = AEInitializing.TheMacro2Session.GenerateFileOut("AERTOS/SPBClass",
                new MacroVar() { MacroName = "Template", VariableValue = $"{GetFullTemplateType() }" },
                new MacroVar() { MacroName = "TemplateNoDefaultArgs", VariableValue = $"{GetFullTemplateTypeNoDefaultsArgs() }" },
                new MacroVar() { MacroName = "TemplateArgs", VariableValue = $"{ GetFullTemplateArgs() }" },
                new MacroVar() { MacroName = "BaseTemplate", VariableValue = $"{ Get_AESPBObservorOutputType_TemplateArgsValues() }" },
                new MacroVar() { MacroName = "ClassName", VariableValue = $"{ClassName}" },
                new MacroVar() { MacroName = "OutputSize", VariableValue = $"{SizeOfOutput.SizeOfOutput.ToString()}" },
                new MacroVar() { MacroName = "ChInstantiation", VariableValue = $"{ChInstantiation}" },
                new MacroVar() { MacroName = "InitFunction", VariableValue = $"{GetInitializationFunction()}" },
                new MacroVar() { MacroName = "TemplateArgValues", VariableValue = $"" }
                );
            relativeDirPathWrites.Add(new RelativeDirPathWrite($"{ClassName}", ".h", "include", contentesOut, true));

            return relativeDirPathWrites;
        }


    }



    public class SPBChannelBase
    {

        public AO OriginatingAO
        {
            get
            {
                //get the name of the originating AO that links to this one.

                AOObserver aO = null;
                bool sacv = this.AOThatLinksToThisChannel == null;
                if (sacv)
                {
                    ProblemHandle problemHandle = new ProblemHandle();
                    problemHandle.ThereisAProblem($"you have a spb that has one of its channels not connected to anything");
                }

                if ((AOObserver)this.AOThatLinksToThisChannel.SensorIOriginateFrom != null)
                {
                    aO = this.AOThatLinksToThisChannel.SensorIOriginateFrom;
                }
                else if ((AOObserver)this.AOThatLinksToThisChannel.SPBIOriginateFrom != null)
                {
                    aO = this.AOThatLinksToThisChannel.SPBIOriginateFrom;
                }

                return aO;
            }
        }

        public int ChannelCountBuffer { get; }
        public int SizeOfDataTheChannelExpectsToConsume { get; }
        public AOObserver AOThatLinksToThisChannel { get; set; }
        public int AOFilterID_ThatLinksToThisChannel { get; set; }
        public LinkTypeEnum LinkType { get; set; }

        protected ProblemHandle problem = new ProblemHandle();

        public bool isUserDefined { get; protected set; }
        public bool isUserDefinedCountBuffer { get; protected set; }
        public bool isUserDefinedConsumptionSize { get; protected set; }



        /// <summary>
        /// NOTE: AT LEAST ONE OF THE PARAMETERS BELOW NEED TO BE ONE!
        /// </summary>
        /// <param name="channelCountBuffer">this is the buffer count of the channel. Once this channel is reached, it will trigger the refresh of the spb. </param>
        /// <param name="sizeOfDataTheChannelExpectsToConsume">This is the size of the array the channel gets per refresh.  </param>
        public SPBChannelBase()//(int channelCountBuffer, int sizeOfDataTheChannelExpectsToConsume)
        {
            //    ChannelCountBiffer = channelCountBuffer;
            //    SizeOfDataTheChannelExpectsToConsume = sizeOfDataTheChannelExpectsToConsume;
            AOThatLinksToThisChannel = null;
        }




        public SPBChannelBase(int channelCountBuffer, int sizeOfDataTheChannelExpectsToConsume)
        {
            ChannelCountBuffer = channelCountBuffer;
            SizeOfDataTheChannelExpectsToConsume = sizeOfDataTheChannelExpectsToConsume;
            if (channelCountBuffer > 1 && sizeOfDataTheChannelExpectsToConsume > 1)
            {
                problem.ThereisAProblem("AT LEAST ONE OF THE PARAMETERS channelCountBuffer and sizeOfDataTheChannelExpectsToConsume NEED TO BE ONE!");
            }

            //    ChannelCountBiffer = channelCountBuffer;
            //    SizeOfDataTheChannelExpectsToConsume = sizeOfDataTheChannelExpectsToConsume;
            AOThatLinksToThisChannel = null;
        }


        public SPBChannelBase(SPBChannelBase toCopy)
        {
            ChannelCountBuffer = toCopy.ChannelCountBuffer;
            SizeOfDataTheChannelExpectsToConsume = toCopy.SizeOfDataTheChannelExpectsToConsume;
            AOThatLinksToThisChannel = toCopy.AOThatLinksToThisChannel;
            AOFilterID_ThatLinksToThisChannel = toCopy.AOFilterID_ThatLinksToThisChannel;
            LinkType = toCopy.LinkType;
            isUserDefined = toCopy.isUserDefined;
            isUserDefinedCountBuffer = toCopy.isUserDefinedCountBuffer;
            isUserDefinedConsumptionSize = toCopy.isUserDefinedConsumptionSize;
        }

    }



    //public   class SPBNOTAChannelChannelLockedInBase : SPBChannelBase
    //{
    //    public SPBNOTAChannelChannelLockedInBase()
    //    {

    //    }

    //    public SPBNOTAChannelChannelLockedInBase(int channelCountBuffer, int sizeOfDataTheChannelExpectsToConsume) : base(channelCountBuffer, sizeOfDataTheChannelExpectsToConsume)
    //    {
    //        isUserDefined = false;
    //    }
    //}

    public abstract class SPBChannelLockedInBase : SPBChannelBase
    {


        public SPBChannelLockedInBase(int channelCountBuffer, int sizeOfDataTheChannelExpectsToConsume) : base(channelCountBuffer, sizeOfDataTheChannelExpectsToConsume)
        {
            isUserDefined = false;
        }
    }

    public abstract class SPBChannelUserDefinedBase : SPBChannelBase
    {

        protected SPBChannelUserDefinedBase(int channelCountBuffer, int sizeOfDataTheChannelExpectsToConsume) : base(channelCountBuffer, sizeOfDataTheChannelExpectsToConsume)
        {
            isUserDefined = true;
        }

    }

    public class SPBChannelLockedInCountBuffer : SPBChannelLockedInBase
    {


        public SPBChannelLockedInCountBuffer(int channelCountBuffer) : base(channelCountBuffer, 1)
        {
            isUserDefinedConsumptionSize = true;
        }

    }

    public class SPBChannelLockedInChannelConsumptionSize : SPBChannelLockedInBase
    {

        public SPBChannelLockedInChannelConsumptionSize(int sizeOfDataTheChannelExpectsToConsume) : base(1, sizeOfDataTheChannelExpectsToConsume)
        {
            isUserDefinedConsumptionSize = false;
        }

    }

    public class SPBChannelUserDefinedCountBuffer : SPBChannelUserDefinedBase
    {


        public SPBChannelUserDefinedCountBuffer(int channelCountBuffer) : base(channelCountBuffer, 1)
        {
            isUserDefinedCountBuffer = true;
        }

    }

    public class SPBChannelUserDefinedChannelConsumptionSize : SPBChannelUserDefinedBase
    {


        public SPBChannelUserDefinedChannelConsumptionSize(int sizeOfDataTheChannelExpectsToConsume) : base(1, sizeOfDataTheChannelExpectsToConsume)
        {
            isUserDefinedCountBuffer = false;
        }

    }


    //public abstract class AESPBClassBase : AESPB
    //{ 

    //    public abstract void 
    //}


    //public class SPBCh1UserDefined : AESPBBase<SPBChannelUserDefinedCountBuffer, SPBNOTAChannelChannelLockedInBase, SPBNOTAChannelChannelLockedInBase, SPBNOTAChannelChannelLockedInBase, SPBNOTAChannelChannelLockedInBase>
    //{
    //    public SPBCh1UserDefined(string nameOfSPB, string typeOfSPB, string templateType, int sizeOfOutput, SPBChannelUserDefinedCountBuffer ch1)
    //        : base(nameOfSPB, typeOfSPB, templateType, sizeOfOutput, ch1)
    //    {

    //    }
    //}


}
