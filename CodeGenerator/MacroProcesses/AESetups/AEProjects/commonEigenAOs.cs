//############################################### 
//this is an autogenerated file using cgen's macro2 command. Dont modify this file here unless it is in user sections 
//###############################################



using CgenMin.MacroProcesses;
using System.Collections.Generic;

namespace commonEigenAOsProject
{

    class PoseEulerAngles_SPB : AESPBBase
    {
        public PoseEulerAngles_SPB( string nameOfSPB, StyleOfSPB styleOfSPB,  bool isSubscribable  ) 
            : base("commonEigenAOs", nameOfSPB, styleOfSPB, "", "", isSubscribable, new SizeOfSPBOutput(3,false),
                  new SPBChannelLockedInCountBuffer(1),
                  new SPBChannelLockedInCountBuffer(1),
                  new SPBChannelLockedInCountBuffer(1),
                  new SPBChannelLockedInCountBuffer(1),
                  new SPBChannelLockedInCountBuffer(1),
                  new SPBChannelLockedInCountBuffer(1),
                  new SPBChannelLockedInCountBuffer(1),
                  new SPBChannelLockedInCountBuffer(1),
                  new SPBChannelLockedInCountBuffer(1)
                  )
        {
        }

        protected override CppFunctionArgs SetcppConstructorArgs()
        {
            CppFunctionArgs ret = new CppFunctionArgs(
                new CppFunctionArg("ComplFilter*", "compFilt",false)
                );

            return ret;
        }
    }


    public class commonEigenAOs : AEProject
    {
		[AEEXETest]
        public void defaultTest()
        {
            AEClock aEClock = new AEClock("clock1",100,"clock1cb");

            AESensor aESensor1 = new AESensor("sens1",SensorResolution.Resolution16Bit);
            AESensor aESensor2 = new AESensor("sens2",SensorResolution.Resolution16Bit);
            AESensor aESensor3 = new AESensor("sens3",SensorResolution.Resolution16Bit);
            AESensor aESensor4 = new AESensor("sens4",SensorResolution.Resolution16Bit);
            AESensor aESensor5 = new AESensor("sens5",SensorResolution.Resolution16Bit);
            AESensor aESensor6 = new AESensor("sens6",SensorResolution.Resolution16Bit);
            AESensor aESensor7 = new AESensor("sens7",SensorResolution.Resolution16Bit);
            AESensor aESensor8 = new AESensor("sens8",SensorResolution.Resolution16Bit);
            AESensor aESensor9 = new AESensor("sens9",SensorResolution.Resolution16Bit);
            

            PoseEulerAngles_SPB poseEulerAngles_SPB = new PoseEulerAngles_SPB(  "poseEulerAngles_SPB",StyleOfSPB.EachSPBTask, false);

            aEClock.FlowIntoSensor(aESensor1, AEClock_PrescalerEnum.PRESCALER1)
                .FlowIntoSPB(poseEulerAngles_SPB, SPBChannelNum.CH0,LinkTypeEnum.Copy);
            aEClock.FlowIntoSensor(aESensor2, AEClock_PrescalerEnum.PRESCALER1)
                .FlowIntoSPB(poseEulerAngles_SPB, SPBChannelNum.CH1, LinkTypeEnum.Copy);
            aEClock.FlowIntoSensor(aESensor3, AEClock_PrescalerEnum.PRESCALER1)
                .FlowIntoSPB(poseEulerAngles_SPB, SPBChannelNum.CH2, LinkTypeEnum.Copy);
            aEClock.FlowIntoSensor(aESensor4, AEClock_PrescalerEnum.PRESCALER1)
                .FlowIntoSPB(poseEulerAngles_SPB, SPBChannelNum.CH3, LinkTypeEnum.Copy);
            aEClock.FlowIntoSensor(aESensor5, AEClock_PrescalerEnum.PRESCALER1)
                .FlowIntoSPB(poseEulerAngles_SPB, SPBChannelNum.CH4, LinkTypeEnum.Copy);
            aEClock.FlowIntoSensor(aESensor6, AEClock_PrescalerEnum.PRESCALER1)
                .FlowIntoSPB(poseEulerAngles_SPB, SPBChannelNum.CH5, LinkTypeEnum.Copy);
            aEClock.FlowIntoSensor(aESensor7, AEClock_PrescalerEnum.PRESCALER1)
                .FlowIntoSPB(poseEulerAngles_SPB, SPBChannelNum.CH6, LinkTypeEnum.Copy);
            aEClock.FlowIntoSensor(aESensor8, AEClock_PrescalerEnum.PRESCALER1)
                .FlowIntoSPB(poseEulerAngles_SPB, SPBChannelNum.CH7, LinkTypeEnum.Copy);
            aEClock.FlowIntoSensor(aESensor9, AEClock_PrescalerEnum.PRESCALER1)
                .FlowIntoSPB(poseEulerAngles_SPB, SPBChannelNum.CH8, LinkTypeEnum.Copy); 
        }

        protected override string _GetDirectoryOfLibrary()
        {
            return @"commonEigenAOs"; 
        }

        protected override List<AEEvent> _GetEventsInLibrary()
        {
            return new List<AEEvent>() { };
        }

		protected override List<AEHal> _GetPeripheralsInLibrary()
        {
		//ADCPERIPHERAL1_CH1.Init(Portenum.PortB, PinEnum.PIN0)
            return new List<AEHal>() { 
            };
        }

        protected override List<AEProject> _GetLibrariesIDependOn()
        {
            return new List<AEProject>() { };
        }
		
		protected override List<string> _GetAnyAdditionalIncludeDirs()
        {
            return new List<string>() { "eigen3/Eigen", "matlab_common" , "Rot2EulAngle" , "Angle2Quat", "EllipseFit" };
        }

        protected override List<string> _GetAnyAdditionalSRCDirs()
        {
            return new List<string>() { "matlab_common", "Rot2EulAngle", "Angle2Quat", "EllipseFit" };
        }

    }
	
	
	
}