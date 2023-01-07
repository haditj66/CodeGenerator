using HolterMonitorGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProtocolsForCommunications
{
    public interface IProtocol
    {

        void InputNewData(DataRecievedWrapperType Data);
    }
}
