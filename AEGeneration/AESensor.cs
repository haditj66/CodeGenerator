﻿using CodeGenerator.ProblemHandler;
using System.Collections.Generic;

namespace CgenMin.MacroProcesses
{
    public class AESensor : AOObserver
    {
        //public string NameOfSensor { get; }
        public SensorResolution TheSensorResolution { get; }
        public SensorDataType TheSensorDataType { get; }
        public float MapsToAFLoatOfLowerBound { get; }
        public float MapsToAFLoatOfUpperBound { get; }

        public string TheSensorResolutionStr { get
            {
                return
                    TheSensorResolution == SensorResolution.Resolution0Bit ? "Resolution0Bit" :
                    TheSensorResolution == SensorResolution.Resolution8Bit ? "Resolution8Bit" :
                    TheSensorResolution == SensorResolution.Resolution12Bit ? "Resolution12Bit" :
                    TheSensorResolution == SensorResolution.Resolution16Bit ? "Resolution16Bit" :
                    TheSensorResolution == SensorResolution.Resolution32Bit ? "Resolution32Bit" :
                    TheSensorResolution == SensorResolution.Resolution64Bit ? "Resolution64Bit" : "";
            }
        }

        public string SensorDataTypeSTR
        {
            get
            {
                return
                    TheSensorDataType == SensorDataType.int32_T ? "int32_t" :
                    TheSensorDataType == SensorDataType.int16_T ? "int16_t" :
                    TheSensorDataType == SensorDataType.int8_T ?  "int8_t" :
                    TheSensorDataType == SensorDataType.uint32_T ? "uint32_t" :
                    TheSensorDataType == SensorDataType.uint16_T ? "uint16_t" :
                    TheSensorDataType == SensorDataType.uint8_T ? "uint8_t" :  "";
            }
        } 


        
        public AEClock ClockIAmFrom { get; internal set; }
        public string ADC_IMSetTo;


        public static int numOfSensorsSoFar = 0;
        public int SensorId = 0;

        public AESensor(string nameOfSensor, SensorResolution sensorResolution, SensorDataType theSensorDataType,
        float mapsToAFLoatOfLowerBound = 0,
        float mapsToAFLoatOfUpperBound = 0) : base(AEInitializing.RunningProjectName, nameOfSensor, AOTypeEnum.Sensor)//"AEObservorSensorFilterOut",
        {  
            TheSensorResolution = sensorResolution;
            TheSensorDataType = theSensorDataType;
            MapsToAFLoatOfLowerBound = mapsToAFLoatOfLowerBound;
            MapsToAFLoatOfUpperBound = mapsToAFLoatOfUpperBound;
            ADC_IMSetTo = "";
            numOfSensorsSoFar++;
            SensorId = numOfSensorsSoFar;

             
        }
        public AESensor(string nameOfSensor, IADC adcImSetTo,
        float mapsToAFLoatOfLowerBound = 0,
        float mapsToAFLoatOfUpperBound = 0) : this(nameOfSensor, SensorResolution.Resolution16Bit, SensorDataType.uint16_T, mapsToAFLoatOfLowerBound, mapsToAFLoatOfUpperBound)
        { 
            ADC_IMSetTo = adcImSetTo.ADCInstName;
        }


        public AESensor FlowIntoSPB(AESPBBase spbToFlowTo, SPBChannelNum toChannel, LinkTypeEnum linkType)
        {
            NumSPBSIPointTo++;

            ProblemHandle problemHandle = new ProblemHandle();
            if ((int)toChannel >= spbToFlowTo.Channels.Count )
            { 
                problemHandle.ThereisAProblem($"you tried to connect to a channel that spb {spbToFlowTo.ClassName} does not have");
            }

            spbToFlowTo.Channels[(int)toChannel].AOThatLinksToThisChannel = this;
            spbToFlowTo.Channels[(int)toChannel].AOFilterID_ThatLinksToThisChannel = 0;
            spbToFlowTo.Channels[(int)toChannel].LinkType = linkType;

            return this;
        }



        protected override string _GenerateAEConfigSection(int numOfAOOfThisSameTypeGeneratesAlready)
        {
            //#define SensorName1 sensor1
            //#define SensorName2 sensor2
            //#define SensorName3 sensor3
            string ret = "";
            ret += $"#define SensorName{SensorId.ToString()} {this.InstanceName}"; ret += "\n"; 
            

            return ret;
        }
         
        public override string GenerateMainHeaderSection()
        {
            //static uint32_t sensordata1[1];

            string ret = "";

            if (string.IsNullOrEmpty(ADC_IMSetTo))
            {
                ret += $"static {SensorDataTypeSTR} {InstanceName}_data[1];"; ret += "\n";
            }


            return ret;
        }
        public override string GenerateMainInitializeSection()
        {
            //static AEObservorSensorFilterOut<0> ECGDataL((uint32_t*) sensordata1, SensorResolution::Resolution16Bit);
            //sensor1 = &ECGDataL;
            string ret = "";

            string maplowerBoundStr = MapsToAFLoatOfLowerBound == 0 && MapsToAFLoatOfUpperBound == 0 ? "" : $", {MapsToAFLoatOfLowerBound.ToString()}";
            string mapupperBoundStr = MapsToAFLoatOfLowerBound == 0 && MapsToAFLoatOfUpperBound == 0 ? "" : $", {MapsToAFLoatOfUpperBound.ToString()}";

            if (string.IsNullOrEmpty(ADC_IMSetTo))
            {
                ret += $"static AEObservorSensorFilterOut<{this.GetFilterTemplateArgsValues()}> {InstanceName}L(({SensorDataTypeSTR}*) {InstanceName}_data, SensorResolution::{TheSensorResolutionStr} {maplowerBoundStr} {mapupperBoundStr});"; ret += "\n";
            }
            else
            {
                ret += $"static AEObservorSensorFilterOut<{this.GetFilterTemplateArgsValues()}> {InstanceName}L(({SensorDataTypeSTR}*) {ADC_IMSetTo}->GetADCDataAddress(), SensorResolution::{TheSensorResolutionStr} {maplowerBoundStr} {mapupperBoundStr});"; ret += "\n";
            }
            ret += $"{InstanceName} = &{InstanceName}L;"; ret += "\n";

            return ret;
        }
        public override string GenerateMainClockSetupsSection()
        {
            //nothing
            return "";
        }
        public override string GenerateMainLinkSetupsSection()
        {
            //nothing
            return "";
        }
        public override string GenerateFunctionDefinesSection()
        {
            //nothing
            return "";
        }

        protected override List<RelativeDirPathWrite> _WriteTheContentedToFiles()
        {
            return null;
        }

        public override string GetFullTemplateType()
        {
            throw new System.NotImplementedException();
        }

        public override string GetFullTemplateArgsValues()
        {
            return this.GetFilterTemplateArgsValues();
            
        }

        public override string GetFullTemplateArgs()
        {
            throw new System.NotImplementedException();
        }
    }


}
