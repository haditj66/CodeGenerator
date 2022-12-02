using System.Collections.Generic;
using System.Linq;

namespace CgenMin.MacroProcesses
{

    //===================================================================================
    //Module: CGENTest
    //===================================================================================


    //example: multiple user defined channels. NOTE all channels must be of same countbuffer or size consumption
    public class AdderSPB : AESPBBase
    {
        public AdderSPB(string nameOfSPB, bool isSubscribable,int numOfChannels, SPBChannelUserDefinedCountBuffer channelAllSame)
            : base("CGENTest",nameOfSPB, "AdderSPB", "", "", isSubscribable, new SizeOfOutput_IfUserDefined(1,false), numOfChannels, channelAllSame)
        {

        }


    }


    public class AverageSPB : AESPBBase
    {
        public AverageSPB(string nameOfSPB,  string templateType, bool isSubscribable,  SPBChannelUserDefinedCountBuffer ch1)
            : base("CGENTest", nameOfSPB, "AverageSPB", "", templateType, isSubscribable,  new SizeOfOutput_IfUserDefined(1,false), ch1)
        {

        }
    }


    public class ThreeDimensionalVector : AESPBBase
    {
        public ThreeDimensionalVector(string nameOfSPB, bool isSubscribable, int countBufferForAll3Channels)
            : base("CGENTest", nameOfSPB, "ThreeDimensionalVector", "", "", isSubscribable, new SizeOfOutput_IfUserDefined(4, false), new SPBChannelUserDefinedCountBuffer(countBufferForAll3Channels), new SPBChannelUserDefinedCountBuffer(countBufferForAll3Channels), new SPBChannelUserDefinedCountBuffer(countBufferForAll3Channels))
        {

        }
    }


}
