using CgenMin.MacroProcesses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.MacroProcesses.AESetups.SPBs
{
    public class I2C_RXCpltEVT : AEEvent
    {
        public I2C_RXCpltEVT(int eventPoolSize) :
            base("CGENTest", "I2C_RXCpltEVT", eventPoolSize,
            "uint8_t forI2C;"
            )
        {

        }
    }

}
