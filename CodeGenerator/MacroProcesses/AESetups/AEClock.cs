using System;
using System.Collections.Generic;
using System.Linq;

namespace CgenMin.MacroProcesses
{
    public class AEClock : AO
    {
        public string NameOfClock { get; private set; }
        public int FrequencyOfClock { get; private set; }
        public string NameOfClockCallBack { get; private set; }
        public bool TicksFromRTOSTimer { get; private set; }


        List<Tuple<AESensor, AEClock_PrescalerEnum>> SensorsIFlowTo;
        List<Tuple<AEUtilityService, AEClock_PrescalerEnum>> TdusIFlowTo;

        static int NumOfClocksMadeSoFar = 0;
        int clockidNum;
        public AEClock(string nameOfClock, int frequencyOfClock, string nameOfClockCallBack, bool ticksFromRTOSTimer = true)
        : base($"clock{NumOfClocksMadeSoFar.ToString()}", AOTypeEnum.Clock)
        {
            _Init(nameOfClock, frequencyOfClock, nameOfClockCallBack, ticksFromRTOSTimer);

        }

        public AEClock(string nameOfClock, int frequencyOfClock)
: base($"clock{NumOfClocksMadeSoFar.ToString()}", AOTypeEnum.Clock)
        {
            _Init(nameOfClock, frequencyOfClock, "", false);

        }

        private void _Init(string nameOfClock, int frequencyOfClock, string nameOfClockCallBack, bool ticksFromRTOSTimer)
        {
            SensorsIFlowTo = new List<Tuple<AESensor, AEClock_PrescalerEnum>>();
            TdusIFlowTo = new List<Tuple<AEUtilityService, AEClock_PrescalerEnum>>();
            NameOfClock = nameOfClock;
            FrequencyOfClock = frequencyOfClock;
            NameOfClockCallBack = nameOfClockCallBack;

            NumOfClocksMadeSoFar++;
            clockidNum = NumOfClocksMadeSoFar;
            TicksFromRTOSTimer = ticksFromRTOSTimer;
        }


        public AESensor FlowIntoSensor(AESensor sensorToFlowTo, AEClock_PrescalerEnum prescaler)
        {
            SensorsIFlowTo.Add(new Tuple<AESensor, AEClock_PrescalerEnum>(sensorToFlowTo, prescaler));
            sensorToFlowTo.ClockIAmFrom = this;

            return sensorToFlowTo;
        }


        public void FlowIntoTDU(AEUtilityService UtilityOfTdu, AEClock_PrescalerEnum prescaler)
        {

            TdusIFlowTo.Add(new Tuple<AEUtilityService, AEClock_PrescalerEnum>(UtilityOfTdu, prescaler));
            //TdusIFlowTo.ClockIAmFrom = this;

        }


        protected override string _GenerateAEConfigSection(int numOfAOOfThisSameTypeGeneratesAlready)
        {
            string ret = $"#define ClockType{clockidNum} AEClock{GetTemplateType} \n";
            ret += $"#define ClockName{clockidNum} {NameOfClock} \n";

            return ret;
        }
        public override string GenerateMainHeaderSection()
        {
            string ret = "";
            if (TicksFromRTOSTimer == true)
            {
                ret += $"static void {NameOfClockCallBack}(TimerHandle_t xTimerHandle);" + "\n";
            }

            return ret;
        }
        public override string GenerateMainInitializeSection()
        {
            string ret = "";

            if (TicksFromRTOSTimer == true)
            {
                ret += $"static AEClock{GetTemplateType} {NameOfClock}L({FrequencyOfClock.ToString()}, {NameOfClockCallBack});" + "\n";
                ret += $"{NameOfClock} = &{NameOfClock}L;" + "\n";
            }

            return ret;
        }
        public override string GenerateMainClockSetupsSection()
        {

            string ret = "";

            foreach (var item in this.SensorsIFlowTo)
            {
                ret += $"{NameOfClock}->SetObservorToClock({item.Item1.InstanceName}, AEClock_PrescalerEnum::PRESCALER{((int)item.Item2).ToString()});\n";
            }

            foreach (var item in this.TdusIFlowTo)
            {
                ret += $"{NameOfClock}->SetTDUToClock({item.Item1.InstanceName}, AEClock_PrescalerEnum::PRESCALER{((int)item.Item2).ToString()});\n";
            }

            return ret;
        }
        public override string GenerateMainLinkSetupsSection()
        {
            return "";
        }
        public override string GenerateFunctionDefinesSection()
        {
            string ret = "";
            if (TicksFromRTOSTimer == true)
            {
                ret += "static void " + NameOfClockCallBack + "(TimerHandle_t xTimerHandle) {  \n  ";
                ret += $"##UserCode_{NameOfClock}before \n" + "  \n " + NameOfClock + "->Tick(); \n  " + $"##UserCode_{NameOfClock}after \n" + "}";
            }

            return ret;
        }



        public string GetTemplateType
        {
            get
            {
                int s1 = SensorsIFlowTo.Count(s => s.Item2 == AEClock_PrescalerEnum.PRESCALER1);
                int s2 = SensorsIFlowTo.Count(s => s.Item2 == AEClock_PrescalerEnum.PRESCALER2);
                int s3 = SensorsIFlowTo.Count(s => s.Item2 == AEClock_PrescalerEnum.PRESCALER4);
                int s4 = SensorsIFlowTo.Count(s => s.Item2 == AEClock_PrescalerEnum.PRESCALER8);
                int s5 = SensorsIFlowTo.Count(s => s.Item2 == AEClock_PrescalerEnum.PRESCALER16);
                int s6 = SensorsIFlowTo.Count(s => s.Item2 == AEClock_PrescalerEnum.PRESCALER32);
                int s7 = SensorsIFlowTo.Count(s => s.Item2 == AEClock_PrescalerEnum.PRESCALER64);

                int t1 = TdusIFlowTo.Count(s => s.Item2 == AEClock_PrescalerEnum.PRESCALER1);
                int t2 = TdusIFlowTo.Count(s => s.Item2 == AEClock_PrescalerEnum.PRESCALER2);
                int t3 = TdusIFlowTo.Count(s => s.Item2 == AEClock_PrescalerEnum.PRESCALER4);
                int t4 = TdusIFlowTo.Count(s => s.Item2 == AEClock_PrescalerEnum.PRESCALER8);
                int t5 = TdusIFlowTo.Count(s => s.Item2 == AEClock_PrescalerEnum.PRESCALER16);
                int t6 = TdusIFlowTo.Count(s => s.Item2 == AEClock_PrescalerEnum.PRESCALER32);
                int t7 = TdusIFlowTo.Count(s => s.Item2 == AEClock_PrescalerEnum.PRESCALER64);

                string sensordummy = SensorsIFlowTo.Count > 0 ? "AEObservorSensor" : "AEObservorSensorDUMMY";
                //string tdudummy = TdusIFlowTo.Count > 0 ? "AEObservorInterpretorBase" : "AEObservorInterpretorBaseDUMMY";

                string ret = $"<{sensordummy}, AEObservorInterpretorBaseDUMMY, {s1}, 0, {t1}, {s2}, 0, {t2},{s3}, 0, {t3},{s4}, 0, {t4},{s5}, 0, {t5},{s6}, 0, {t6},{s7}, 0, {t7}>";


                return ret;
            }
        }

    }
}
