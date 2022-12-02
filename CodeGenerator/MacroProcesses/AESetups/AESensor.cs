using System.Collections.Generic;

namespace CgenMin.MacroProcesses
{
    public class AESensor : AOObserver
    {
        //public string NameOfSensor { get; }
        public SensorResolution TheSensorResolution { get; }
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
        public AEClock ClockIAmFrom { get; internal set; }
        public string ADC_IMSetTo;


        public static int numOfSensorsSoFar = 0;
        public int SensorId = 0;

        public AESensor(string nameOfSensor, SensorResolution sensorResolution,
        float mapsToAFLoatOfLowerBound = 0,
        float mapsToAFLoatOfUpperBound = 0) : base("nothing","AEObservorSensorFilterOut", nameOfSensor, AOTypeEnum.Sensor)
        {  
            TheSensorResolution = sensorResolution;
            MapsToAFLoatOfLowerBound = mapsToAFLoatOfLowerBound;
            MapsToAFLoatOfUpperBound = mapsToAFLoatOfUpperBound;
            ADC_IMSetTo = "";
            numOfSensorsSoFar++;
            SensorId = numOfSensorsSoFar;
        }

        public void SetSensorToADC(string adcName)
        {
            ADC_IMSetTo = adcName;
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
                ret += $"static uint32_t {InstanceName}_data[1];"; ret += "\n";
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
                ret += $"static AEObservorSensorFilterOut<{this.GetFilterTemplateArgsValues()}> {InstanceName}L((uint32_t*) {InstanceName}_data, SensorResolution::{TheSensorResolutionStr} {maplowerBoundStr} {mapupperBoundStr});"; ret += "\n";
            }
            else
            {
                ret += $"static AEObservorSensorFilterOut<{this.GetFilterTemplateArgsValues()}> {InstanceName}L((uint16_t*) {ADC_IMSetTo}->GetADCDataAddress(), SensorResolution::{TheSensorResolutionStr} {maplowerBoundStr} {mapupperBoundStr});"; ret += "\n";
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
