using CodeGenerator.ProblemHandler;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CgenMin.MacroProcesses
{



    //====================================================================================================
    //ADC
    //====================================================================================================
    public interface IADC
    {
        Portenum PortofAdc { get; }
        PinEnum PinOfEnum { get; }
        string ADCInstName { get; }
    }

    public abstract class ADCPERIPHERALBase<TDerived> : AEHalWithChannel<TDerived>, IADC
        where TDerived : ADCPERIPHERALBase<TDerived>, new()
    {

        protected ADCPERIPHERALBase(int peripheralNum, int channel)
            : base("ADCPERIPHERAL", "ADCPeripheral", peripheralNum, channel)
        {
        }


        public Portenum PortofAdc { get; protected set; }
        public PinEnum PinOfEnum { get; protected set; }
        public string ADCInstName { get; set; }

        public static TDerived Init(Portenum portofAdc, PinEnum pinOfEnum)
        {
            TDerived inst = _PInit();
            inst.PortofAdc = portofAdc;
            inst.PinOfEnum = pinOfEnum;
            inst.ADCInstName = inst.InstanceName;
            //inst.FromChannel = channel;
            //inst.InstanceName = $"{inst.PeripheralDefineName}_inst{inst.PeripheralNum}_ch{channel}";
            return inst;
        }

        public override string GetTemplateArgsForSingleChannel()
        {
            //WhichInstanceOfADC, ADC_ch1_Port, ADC_ch1_Pin
            string ret = "";

            ret = $"{PortofAdc.ToString()}, {PinOfEnum.ToString()}";

            return ret;
        }
    }


    public class ADCPERIPHERAL1_CH1 : ADCPERIPHERALBase<ADCPERIPHERAL1_CH1>
    {
        public ADCPERIPHERAL1_CH1()
            : base(1, 1)
        { }

    }

    public class ADCPERIPHERAL1_CH2 : ADCPERIPHERALBase<ADCPERIPHERAL1_CH2>
    {
        public ADCPERIPHERAL1_CH2()
            : base(1, 2)
        { }

    }
    public class ADCPERIPHERAL1_CH3 : ADCPERIPHERALBase<ADCPERIPHERAL1_CH3>
    {
        public ADCPERIPHERAL1_CH3()
            : base(1, 3)
        { }

    }
    public class ADCPERIPHERAL1_CH4 : ADCPERIPHERALBase<ADCPERIPHERAL1_CH4>
    {
        public ADCPERIPHERAL1_CH4()
            : base(1, 4)
        { }

    }

    public class ADCPERIPHERAL1_CH5 : ADCPERIPHERALBase<ADCPERIPHERAL1_CH5>
    {
        public ADCPERIPHERAL1_CH5()
            : base(1, 5)
        { }

    }
    public class ADCPERIPHERAL1_CH6 : ADCPERIPHERALBase<ADCPERIPHERAL1_CH6>
    {
        public ADCPERIPHERAL1_CH6()
            : base(1, 6)
        { }

    }
    public class ADCPERIPHERAL1_CH7 : ADCPERIPHERALBase<ADCPERIPHERAL1_CH7>
    {
        public ADCPERIPHERAL1_CH7()
            : base(1, 7)
        { }

    }
    public class ADCPERIPHERAL1_CH8 : ADCPERIPHERALBase<ADCPERIPHERAL1_CH8>
    {
        public ADCPERIPHERAL1_CH8()
            : base(1, 8)
        { }

    }

    //====================================================================================================
    //PWM
    //====================================================================================================

    public interface IPWM
    {
        Portenum PortofPWM { get; }
        PinEnum PinOfPWM { get; }
        string InstName { get; }
    }



    public class PWMPERIPHERALBase<TDerived> : AEHalWithOutChannel<TDerived>, IPWM
        where TDerived : PWMPERIPHERALBase<TDerived>, new()
    {
        public PWMPERIPHERALBase(int peripheralNum)
            : base($"PWMPERIPHERAL", "PWMPeripheral", peripheralNum)
        {
            InstName = this.InstanceName;
        }

        public Portenum PortofPWM { get; protected set; }
        public PinEnum PinOfPWM { get; protected set; }
        public string InstName { get; protected set; }

        public static TDerived Init(Portenum portofPWM, PinEnum pinOfPWM)
        {
            TDerived inst = _PInit();
            inst.PortofPWM = portofPWM;
            inst.PinOfPWM = pinOfPWM;
            return inst;
        }

        public override string GetTemplateArgValues()
        {
            //WhichInstanceOfPWM, PWM_PwmPinOut_Port, PWM_PwmPinOut_Pin
            string ret = "";

            ret = $"{PeripheralNum}, {PortofPWM.ToString()}, {PinOfPWM.ToString()}";

            return ret;
        }
    }




    public class PWMPERIPHERAL1 : PWMPERIPHERALBase<PWMPERIPHERAL1>
    {
        public PWMPERIPHERAL1()
            : base(1)
        {
        }

    }

    public class PWMPERIPHERAL2 : PWMPERIPHERALBase<PWMPERIPHERAL2>
    {
        public PWMPERIPHERAL2()
            : base(2)
        {
        }
    }
    public class PWMPERIPHERAL3 : PWMPERIPHERALBase<PWMPERIPHERAL3>
    {
        public PWMPERIPHERAL3()
            : base(3)
        {
        }
    }
    public class PWMPERIPHERAL4 : PWMPERIPHERALBase<PWMPERIPHERAL4>
    {
        public PWMPERIPHERAL4()
            : base(4)
        {
        }
    }
    public class PWMPERIPHERAL5 : PWMPERIPHERALBase<PWMPERIPHERAL5>
    {
        public PWMPERIPHERAL5()
            : base(5)
        {
        }
    }



    //====================================================================================================
    //UART
    //====================================================================================================


    public interface IUART
    {
          BaudRatesEnum BaudRate { get;   }
            int ReceiveMsgSizeInBytes { get;  }
          Portenum UART_TX_Port { get;   }
          PinEnum UART_TX_Pin { get;   }
          Portenum UART_RX_Port { get;  }
          PinEnum UART_RX_Pin { get;  }
        string InstName { get; }
    }

    //BaudRate, ReceiveMsgSize, UART_TX_Port, UART_TX_Pin, UART_RX_Port, UART_RX_Pin
    //#define UARTPERIPHERAL2 UARTPeripheral<115200, 2, PortD, PIN5, PortD, PIN6>// PortA, PIN3>////460800 * 2
    //#define UARTPERIPHERAL2_Name uart1
    public enum BaudRatesEnum
    {
        T_110 = 110, T_300 = 300, T_600 = 600, T_1200 = 1200, T_2400 = 2400, T_4800 = 4800, T_9600 = 9600, T_14400 = 14400, T_19200 = 19200, T_38400 = 38400, T_57600 = 57600, T_115200 = 115200, T_128000 = 128000, T_256000 = 256000, T_460800 = 460800, T_921600 = 921600
    }

    public class UARTPERIPHERALBase<TDerived> : AEHalWithOutChannel<TDerived>, IUART
        where TDerived : UARTPERIPHERALBase<TDerived>, new()
    {
        public UARTPERIPHERALBase(int peripheralNum)
            : base($"UARTPERIPHERAL", "UARTPeripheral", peripheralNum)
        {
            InstName = InstanceName;
        }

        public BaudRatesEnum BaudRate { get; protected set; }
        public int ReceiveMsgSizeInBytes { get; protected set; }
        public Portenum UART_TX_Port { get; protected set; }
        public PinEnum UART_TX_Pin { get; protected set; }
        public Portenum UART_RX_Port { get; protected set; }
        public PinEnum UART_RX_Pin { get; protected set; }
        public string InstName { get; }
        public static TDerived Init(BaudRatesEnum baudRate, int receiveMsgSizeInBytes, Portenum uART_TX_Port, PinEnum uART_TX_Pin, Portenum uART_RX_Port, PinEnum uART_RX_Pin)
        {
            TDerived inst = _PInit();

            inst.BaudRate = baudRate;
            inst.ReceiveMsgSizeInBytes = receiveMsgSizeInBytes;
            inst.UART_TX_Port = uART_TX_Port;
            inst.UART_TX_Pin = uART_TX_Pin;
            inst.UART_RX_Port = uART_RX_Port;
            inst.UART_RX_Pin = uART_RX_Pin;
            return inst;
        }

        public override string GetTemplateArgValues()
        {
            //WhichInstanceOfPWM, PWM_PwmPinOut_Port, PWM_PwmPinOut_Pin
            string ret = "";

            ret = $"{(int)BaudRate}, {ReceiveMsgSizeInBytes}, {UART_TX_Port}, {UART_TX_Pin}, {UART_RX_Port}, {UART_RX_Pin}";

            return ret;
        }
    }

    public class UARTPERIPHERAL1 : UARTPERIPHERALBase<UARTPERIPHERAL1>
    {
        public UARTPERIPHERAL1()
            : base(1)
        {
        }

    }
    public class UARTPERIPHERAL2 : UARTPERIPHERALBase<UARTPERIPHERAL2>
    {
        public UARTPERIPHERAL2()
            : base(2)
        {
        }

    }
    public class UARTPERIPHERAL3 : UARTPERIPHERALBase<UARTPERIPHERAL3>
    {
        public UARTPERIPHERAL3()
            : base(3)
        {
        }

    }
    public class UARTPERIPHERAL4 : UARTPERIPHERALBase<UARTPERIPHERAL4>
    {
        public UARTPERIPHERAL4()
            : base(4)
        {
        }

    }
    public class UARTPERIPHERAL5 : UARTPERIPHERALBase<UARTPERIPHERAL5>
    {
        public UARTPERIPHERAL5()
            : base(5)
        {
        }

    }



    //====================================================================================================
    //SPI
    //====================================================================================================
    //#define templateargsForSPI WhichInstanceOfSPI, SLAVEMODE, SPI_SCK_Port, SPI_SCK_Pin, SPI_MISO_Port, SPI_MISO_Pin, SPI_MOSI_Port, SPI_MOSI_Pin, SPI_NSS_Port, SPI_NSS_Pin



    public class SPIPERIPHERALBase<TDerived> : AEHalWithOutChannel<TDerived>
        where TDerived : SPIPERIPHERALBase<TDerived>, new()
    {
        public SPIPERIPHERALBase(int peripheralNum)
            : base($"SPIPERIPHERAL", "SPIPeripheral", peripheralNum)
        {
        }

        public bool SLAVEMODE { get; protected set; }
        public Portenum SPI_SCK_Port { get; protected set; }
        public PinEnum SPI_SCK_Pin { get; protected set; }
        public Portenum SPI_MISO_Port { get; protected set; }
        public PinEnum SPI_MISO_Pin { get; protected set; }
        public Portenum SPI_MOSI_Port { get; protected set; }
        public PinEnum SPI_MOSI_Pin { get; protected set; }
        public Portenum SPI_NSS_Port { get; protected set; }
        public PinEnum SPI_NSS_Pin { get; protected set; }
        public static TDerived Init(bool sLAVEMODE,
            Portenum sPI_SCK_Port, PinEnum sPI_SCK_Pin,
            Portenum sPI_MISO_Port, PinEnum sPI_MISO_Pin,
            Portenum sPI_MOSI_Port, PinEnum sPI_MOSI_Pin,
            Portenum sPI_NSS_Port, PinEnum sPI_NSS_Pin
            )
        {
            TDerived inst = _PInit();

            inst.SLAVEMODE = sLAVEMODE;
            inst.SPI_SCK_Port = sPI_SCK_Port;
            inst.SPI_SCK_Pin = sPI_SCK_Pin;

            inst.SPI_MISO_Port = sPI_MISO_Port;
            inst.SPI_MISO_Pin = sPI_MISO_Pin;

            inst.SPI_MOSI_Port = sPI_MOSI_Port;
            inst.SPI_MOSI_Pin = sPI_MOSI_Pin;

            inst.SPI_NSS_Port = sPI_NSS_Port;
            inst.SPI_NSS_Pin = sPI_NSS_Pin;
            return inst;
        }

        public override string GetTemplateArgValues()
        {
            //WhichInstanceOfSPI, SLAVEMODE, SPI_SCK_Port, SPI_SCK_Pin, SPI_MISO_Port, SPI_MISO_Pin, SPI_MOSI_Port, SPI_MOSI_Pin, SPI_NSS_Port, SPI_NSS_Pin
            string ret = "";
            string SLAVEMODESTR = SLAVEMODE ? "true" : "false";
            ret = $"{PeripheralNum}, {SLAVEMODESTR}, {SPI_SCK_Port}, {SPI_SCK_Pin}, {SPI_MISO_Port}, {SPI_MISO_Pin}, {SPI_MOSI_Port}, {SPI_MOSI_Pin}, {SPI_NSS_Port}, {SPI_NSS_Pin}";

            return ret;
        }
    }

    public class SPIPERIPHERAL1 : SPIPERIPHERALBase<SPIPERIPHERAL1>
    {
        public SPIPERIPHERAL1()
            : base(1)
        { }
    }
    public class SPIPERIPHERAL2 : SPIPERIPHERALBase<SPIPERIPHERAL2>
    {
        public SPIPERIPHERAL2()
            : base(2)
        { }
    }
    public class SPIPERIPHERAL3 : SPIPERIPHERALBase<SPIPERIPHERAL3>
    {
        public SPIPERIPHERAL3()
            : base(3)
        { }
    }
    public class SPIPERIPHERAL4 : SPIPERIPHERALBase<SPIPERIPHERAL4>
    {
        public SPIPERIPHERAL4()
            : base(4)
        { }
    }
    public class SPIPERIPHERAL5 : SPIPERIPHERALBase<SPIPERIPHERAL5>
    {
        public SPIPERIPHERAL5()
            : base(5)
        { }
    }



    //====================================================================================================
    //SPI
    //====================================================================================================
    //#define templateargsForI2C WhichInstanceOfI2C, I2C_SCL_Port, I2C_SCL_Pin, I2C_SDA_Port, I2C_SDA_Pin, Clockspeed



 


    public interface II2C
    {
        string InstName { get; }
        int ClockSpeedFreq { get; }
        Portenum DataLine_Port { get; }
        PinEnum DataLine_Pin { get; }
        Portenum Clock_Port { get; }
        PinEnum Clock_Pin { get; }
    }

    public class I2CPERIPHERALBase<TDerived> : AEHalWithOutChannel<TDerived>, II2C
        where TDerived : I2CPERIPHERALBase<TDerived>, new()
    {
        public I2CPERIPHERALBase(int peripheralNum)
            : base($"I2CPERIPHERAL", "I2CPeripheral", peripheralNum)
        {
            InstName = this.InstanceName;
        }

        public string InstName { get; protected set; }
        public int ClockSpeedFreq { get; protected set; }
        public Portenum DataLine_Port { get; protected set; }
        public PinEnum DataLine_Pin { get; protected set; }
        public Portenum Clock_Port { get; protected set; }
        public PinEnum Clock_Pin { get; protected set; }
        public static TDerived Init(int clockSpeedFreq,
            Portenum clock_Port, PinEnum clock_Pin,
            Portenum dataLine_Port, PinEnum dataLine_Pin
            )
        {
            TDerived inst = _PInit();

            inst.ClockSpeedFreq = clockSpeedFreq;
            inst.DataLine_Port = dataLine_Port;
            inst.DataLine_Pin = dataLine_Pin;
            inst.Clock_Port = clock_Port;
            inst.Clock_Pin = clock_Pin;
            return inst;
        }

        public override string GetTemplateArgValues()
        {
            //WhichInstanceOfI2C, I2C_SCL_Port, I2C_SCL_Pin, I2C_SDA_Port, I2C_SDA_Pin, Clockspeed
            string ret = "";

            ret = $"{PeripheralNum}, {Clock_Port}, {Clock_Pin}, {DataLine_Port}, {DataLine_Pin}, {ClockSpeedFreq}";

            return ret;
        }
    }

    public class I2CPERIPHERAL1 : I2CPERIPHERALBase<I2CPERIPHERAL1>
    {
        public I2CPERIPHERAL1()
            : base(1)
        { }
    }
    public class I2CPERIPHERAL2 : I2CPERIPHERALBase<I2CPERIPHERAL2>
    {
        public I2CPERIPHERAL2()
            : base(2)
        { }
    }
    public class I2CPERIPHERAL3 : I2CPERIPHERALBase<I2CPERIPHERAL3>
    {
        public I2CPERIPHERAL3()
            : base(3)
        { }
    }
    public class I2CPERIPHERAL4 : I2CPERIPHERALBase<I2CPERIPHERAL4>
    {
        public I2CPERIPHERAL4()
            : base(4)
        { }
    }
    public class I2CPERIPHERAL5 : I2CPERIPHERALBase<I2CPERIPHERAL5>
    {
        public I2CPERIPHERAL5()
            : base(5)
        { }
    }











    //====================================================================================================
    //GPIO as an output
    //====================================================================================================


    public interface IGPIO
    {
        Portenum Gpio_Port { get; }
        PinEnum Gpio_Pin { get; }
        string InstName { get; }
    }

    public class GPIOPERIPHERALBase<TDerived> : AEHalWithOutChannel<TDerived>, IGPIO
        where TDerived : GPIOPERIPHERALBase<TDerived>, new()
    {
        public GPIOPERIPHERALBase(int peripheralNum)
            : base($"GPIOPERIPHERAL", "GPIOPeripheral", peripheralNum)
        {
            InstName = InstanceName;
        }

        public Portenum Gpio_Port { get; protected set; }
        public PinEnum Gpio_Pin { get; protected set; }
        public string InstName { get; }

        public static TDerived Init(
            Portenum gpio_Port, PinEnum gpio_Pin
            )
        {
            TDerived inst = _PInit();

            inst.Gpio_Port = gpio_Port;
            inst.Gpio_Pin = gpio_Pin;
            return inst;
        }

        public override string GetTemplateArgValues()
        {
            //<PortD, PIN14>
            string ret = "";

            ret = $" {Gpio_Port}, {Gpio_Pin}";

            return ret;
        }


    }

    public class GPIOPERIPHERAL1 : GPIOPERIPHERALBase<GPIOPERIPHERAL1>
    {
        public GPIOPERIPHERAL1()
            : base(1)
        { }
    }
    public class GPIOPERIPHERAL2 : GPIOPERIPHERALBase<GPIOPERIPHERAL2>
    {
        public GPIOPERIPHERAL2()
            : base(2)
        { }
    }
    public class GPIOPERIPHERAL3 : GPIOPERIPHERALBase<GPIOPERIPHERAL3>
    {
        public GPIOPERIPHERAL3()
            : base(3)
        { }
    }
    public class GPIOPERIPHERAL4 : GPIOPERIPHERALBase<GPIOPERIPHERAL4>
    {
        public GPIOPERIPHERAL4()
            : base(4)
        { }
    }
    public class GPIOPERIPHERAL5 : GPIOPERIPHERALBase<GPIOPERIPHERAL5>
    {
        public GPIOPERIPHERAL5()
            : base(5)
        { }
    }
    public class GPIOPERIPHERAL6 : GPIOPERIPHERALBase<GPIOPERIPHERAL6>
    {
        public GPIOPERIPHERAL6()
            : base(6)
        { }
    }
    public class GPIOPERIPHERAL7 : GPIOPERIPHERALBase<GPIOPERIPHERAL7>
    {
        public GPIOPERIPHERAL7()
            : base(7)
        { }
    }
    public class GPIOPERIPHERAL8 : GPIOPERIPHERALBase<GPIOPERIPHERAL8>
    {
        public GPIOPERIPHERAL8()
            : base(8)
        { }
    }
    public class GPIOPERIPHERAL9 : GPIOPERIPHERALBase<GPIOPERIPHERAL9>
    {
        public GPIOPERIPHERAL9()
            : base(9)
        { }
    }
    public class GPIOPERIPHERAL10 : GPIOPERIPHERALBase<GPIOPERIPHERAL10>
    {
        public GPIOPERIPHERAL10()
            : base(10)
        { }
    }







    //====================================================================================================
    //GPIO as an Input
    //====================================================================================================


    public class GPIOPERIPHERALInputBase<TDerived> : AEHalWithOutChannel<TDerived>
        where TDerived : GPIOPERIPHERALInputBase<TDerived>, new()
    {
        public GPIOPERIPHERALInputBase(int peripheralNum)
            : base($"GPIOInputPERIPHERAL", "GPIOInputPeripheral", peripheralNum)
        {
        }

        public Portenum Gpio_Port { get; protected set; }
        public PinEnum Gpio_Pin { get; protected set; }
        public static TDerived Init(
            Portenum gpio_Port, PinEnum gpio_Pin
            )
        {
            TDerived inst = _PInit();

            inst.Gpio_Port = gpio_Port;
            inst.Gpio_Pin = gpio_Pin;
            return inst;
        }

        public override string GetTemplateArgValues()
        {
            //<PortD, PIN14>
            string ret = "";

            ret = $" {Gpio_Port}, {Gpio_Pin}";

            return ret;
        }


    }

    public class GPIOPERIPHERAL_INPUT1 : GPIOPERIPHERALInputBase<GPIOPERIPHERAL_INPUT1>
    {
        public GPIOPERIPHERAL_INPUT1()
            : base(1)
        { }
    }
    public class GPIOPERIPHERAL_INPUT2 : GPIOPERIPHERALInputBase<GPIOPERIPHERAL_INPUT2>
    {
        public GPIOPERIPHERAL_INPUT2()
            : base(2)
        { }
    }
    public class GPIOPERIPHERAL_INPUT3 : GPIOPERIPHERALInputBase<GPIOPERIPHERAL_INPUT3>
    {
        public GPIOPERIPHERAL_INPUT3()
            : base(3)
        { }
    }
    public class GPIOPERIPHERAL_INPUT4 : GPIOPERIPHERALInputBase<GPIOPERIPHERAL_INPUT4>
    {
        public GPIOPERIPHERAL_INPUT4()
            : base(4)
        { }
    }
    public class GPIOPERIPHERAL_INPUT5 : GPIOPERIPHERALInputBase<GPIOPERIPHERAL_INPUT5>
    {
        public GPIOPERIPHERAL_INPUT5()
            : base(5)
        { }
    }
    public class GPIOPERIPHERAL_INPUT6 : GPIOPERIPHERALInputBase<GPIOPERIPHERAL_INPUT6>
    {
        public GPIOPERIPHERAL_INPUT6()
            : base(6)
        { }
    }
    public class GPIOPERIPHERAL_INPUT7 : GPIOPERIPHERALInputBase<GPIOPERIPHERAL_INPUT7>
    {
        public GPIOPERIPHERAL_INPUT7()
            : base(7)
        { }
    }
    public class GPIOPERIPHERAL_INPUT8 : GPIOPERIPHERALInputBase<GPIOPERIPHERAL_INPUT8>
    {
        public GPIOPERIPHERAL_INPUT8()
            : base(8)
        { }
    }
    public class GPIOPERIPHERAL_INPUT9 : GPIOPERIPHERALInputBase<GPIOPERIPHERAL_INPUT9>
    {
        public GPIOPERIPHERAL_INPUT9()
            : base(9)
        { }
    }
    public class GPIOPERIPHERAL_INPUT10 : GPIOPERIPHERALInputBase<GPIOPERIPHERAL_INPUT10>
    {
        public GPIOPERIPHERAL_INPUT10()
            : base(10)
        { }
    }
}
