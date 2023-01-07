//using CodeGenerator.ProblemHandler;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CgenMin.MacroProcesses
{


    public enum Portenum  
    {
        PortA = 0,
        PortB,
        PortC,
        PortD,
        PortE,
        PortF,
        PortG,
        PortH,
        PortI,
        PortsMaxNum

    }

    public enum PinEnum  
    {
        PIN0 = 0,
        PIN1,
        PIN2,
        PIN3,
        PIN4,
        PIN5,
        PIN6,
        PIN7,
        PIN8,
        PIN9,
        PIN10,
        PIN11,
        PIN12,
        PIN13,
        PIN14,
        PIN15,
        PINMaxNum

    };




    public interface IPeripheralChannel  
    {

        string GetTemplateArgsForSingleChannel();
         int FromChannel { get;  }
         string ChannelInstanceName { get;  }
    }


    public abstract class AEHalWithChannel<TDerived> : AEHalBase<TDerived>, IPeripheralChannel
    where TDerived : AEHalWithChannel<TDerived>, new()
    {

        protected List<IPeripheral> GetAllPeripheralWithChannels { get
            {
                return AO.AllInstancesOfAO.Where(a => a.AOType == AOTypeEnum.AEHal)
                .Cast<IPeripheral>().ToList()
                .Where(a => a.PeripheralDefineName == this.PeripheralDefineName)
                .Cast<IPeripheral>().ToList();
            }
        }

        public string ChannelInstanceName { get; }
        public int FromChannel { get; protected set; }
        protected AEHalWithChannel(string peripheralDefineName, string peripheralClassName, int peripheralNum , int channel)
            : base(peripheralDefineName, peripheralClassName, $"{peripheralDefineName}_inst{peripheralNum}_ch{channel}", peripheralNum)
        {
            FromChannel = channel;
            ChannelInstanceName = InstanceName;
            //AllPeripheralWithChannels.Add(this);
        }

        public abstract string GetTemplateArgsForSingleChannel();

        public override string GetTemplateArgValues()
        {
            //WhichInstanceOfADC, ADC_ch1_Port, ADC_ch1_Pin
            string ret = "";
            ret += $"{PeripheralNum}";
            
            foreach (var aehalCH in GetAllPeripheralWithChannels.OrderBy(a=>a.PeripheralNum).Cast<IPeripheralChannel>().ToList())
            {
                ret += $", {aehalCH.GetTemplateArgsForSingleChannel()}";
            }


            return ret;
        }

        public override List<Tuple<int, string>> GetAllInstanceNamesForPeripheral()
        {
            List<Tuple<int, string>> ret = new List<Tuple<int, string>>();
            foreach (var aehalCH in GetAllPeripheralWithChannels.Where(a => a.PeripheralNum == this.PeripheralNum).Cast<IPeripheralChannel>().ToList())
            { 

                ret.Add(new Tuple<int, string>(aehalCH.FromChannel, aehalCH.ChannelInstanceName));// += $", {aehalCH.GetTemplateArgsForSingleChannel()}";
            }

            return ret;
        }


    }

    public abstract class AEHalWithOutChannel<TDerived> : AEHalBase<TDerived>  
    where TDerived : AEHalWithOutChannel<TDerived>, new()
    {
         
         
        protected AEHalWithOutChannel(string peripheralDefineName, string peripheralClassName, int peripheralNum   )
            : base(peripheralDefineName, peripheralClassName, $"{peripheralDefineName}_inst{peripheralNum}", peripheralNum)
        { 
        } 

        public override List<Tuple<int, string>> GetAllInstanceNamesForPeripheral()
        {
            List<Tuple<int, string>> ret = new List<Tuple<int, string>>();
            ret.Add(new Tuple<int, string>(0, InstanceName));

            return ret;
        }


    }


    public interface IPeripheral
    {
          int PeripheralNum { get; }
          string PeripheralDefineName { get; }
        string PeripheralClassName { get; }
    }

    public abstract class AEHal : AO, IPeripheral
    {
        public string PeripheralDefineName { get; protected set; } 
        public int PeripheralNum { get; protected set; }
        public string PeripheralClassName { get; protected set; }

        protected static bool FromConstructor = true;

        protected AEHal(string peripheralDefineName, string peripheralClassName, string instanceName, int peripheralNum = 0) : base(instanceName, AOTypeEnum.AEHal)
        {
            PeripheralClassName = peripheralClassName;
            PeripheralDefineName = peripheralDefineName;
            //InstanceName = instanceName;
            PeripheralNum = peripheralNum;
            //FromChannel = fromChannel;

            ProblemHandle problemHandle = new ProblemHandle();
            if (peripheralNum <= 0)
            {
                problemHandle.ThereisAProblem($"you chose {peripheralNum} for peripheral {peripheralDefineName}. but it needs to be greater than zero.");
            }
            if (FromConstructor == true)
            {
                problemHandle.ThereisAProblem($"You called peripheral {ClassName} from the contructor. This is not allowed, you need to use the \"Instance\" property as it is a singleton.");
            }
        }


        public override string GenerateFunctionDefinesSection()
        {
            return "";
        }

        public override string GenerateMainClockSetupsSection()
        {
            return "";
        }

        public override string GenerateMainHeaderSection()
        {
            return "";
        }

        public override string GenerateMainInitializeSection()
        {
            return "";
        }

        public override string GenerateMainLinkSetupsSection()
        {
            return "";
        }

        public abstract string GetTemplateArgValues();
        public abstract List<Tuple<int, string>> GetAllInstanceNamesForPeripheral();


        public static List<string> AllGeneratedAlready = new List<string>();
        protected override string _GenerateAEConfigSection(int numOfAOOfThisSameTypeGeneratesAlready)
        {

            //for channels -------------------------------
            //#define ADCPERIPHERAL1 ADCPeripheral<1,PortB, PIN0, PortA, PIN2, PortA, PIN3> 
            //#define ADCPERIPHERAL1_Name_CH1 adc1
            //#define ADCPERIPHERAL1s_Name_CH2 adc2
            //#define ADCPERIPHERAL1_Name_CH3 adc3

            //for nonchannels -----------------------------------
            //#define UARTPERIPHERAL2 UARTPeripheral<115200, 2, PortD, PIN5, PortD, PIN6>// PortA, PIN3>////460800 * 2
            //#define UARTPERIPHERAL2_Name uart1

            string ret = "";

            string peripheralNumstr = PeripheralNum == 0 ? $"{numOfAOOfThisSameTypeGeneratesAlready}" : PeripheralNum.ToString();

            if (AllGeneratedAlready.Contains($"{PeripheralDefineName}{peripheralNumstr}") == false)
            {

                ret += $"#define {PeripheralDefineName}{peripheralNumstr} {PeripheralClassName}<{GetTemplateArgValues()}> "; ret += "\n";

                var allInstanceNames = GetAllInstanceNamesForPeripheral();
                foreach (var instNames in allInstanceNames)
                {
                    string chStr = instNames.Item1 == 0 ? "" : $"_CH{instNames.Item1.ToString()}";
                    ret += $"#define {PeripheralDefineName}{peripheralNumstr}_Name{chStr} {instNames.Item2}"; ret += "\n";
                }


            }

            AllGeneratedAlready.Add($"{PeripheralDefineName}{peripheralNumstr}");
            return ret;
        }

    }



    public abstract class AEHalBase<TDerived> : AEHal
        where TDerived : AEHalBase<TDerived>, new()
    { 


        protected AEHalBase(string peripheralDefineName, string peripheralClassName, string instanceName,int peripheralNum = 0) 
            : base(peripheralDefineName, peripheralClassName, instanceName, peripheralNum)
        { 
        }


        //SingletonStuff------------------
        static TDerived _instance;

        
        private static void _Init( )
        //: base(fromLibrary, ClassName, eventPoolSize, eventDefinition)
        {
            if (_instance == null)
            {
                FromConstructor = false;
                _instance = new TDerived();
                FromConstructor = true;

            }
        }


        protected static TDerived _PInit(   )
        {
            _Init( );
            //_instance.EventPoolSize = eventPoolSize;
            return (TDerived)_instance;

        }

        public static TDerived Instance
        {
            get
            {
                if (_instance == null)
                {
                    ProblemHandle prob = new ProblemHandle();
                    prob.ThereisAProblem($"You never created an instance of  event {typeof(TDerived).ToString()} \n by calling the AEEventFactory() for it in the _GetEventsInLibrary() function for the AEProject");
                }
                return _instance;
            }
        }





    }
         

}
