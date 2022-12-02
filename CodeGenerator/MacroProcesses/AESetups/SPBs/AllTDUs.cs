using CgenMin.MacroProcesses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.MacroProcesses.AESetups.SPBs
{
    class UARTDriver : AEUtilityService
    {
        public UARTDriver(  string instanceNameOfTDU, int serviceBuffersize, AEPriorities priority) 
            : base(  "CGENTest", instanceNameOfTDU, priority, serviceBuffersize, "UUartDriver", 
                  new ActionRequest("Transmit", ServiceType.Normal, "bool", "char const*", "msg")
                  )
        {
        }
    }

    class UARTDriverTDU : AEUtilityService
    {
        public UARTDriverTDU( string instanceNameOfTDU, int serviceBuffersize, AEPriorities priority)
            : base("CGENTest", instanceNameOfTDU, priority, serviceBuffersize, "UUartDriverTDU",
                  new ActionRequest("Transmit", ServiceType.Normal, "bool", "char const*", "msg"),
                  new ActionRequest("TransmitTDU", ServiceType.TDU, "int8_t", "char*", "msg")
                  )
        {
        }


    }

}
