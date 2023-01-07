using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HolterMonitorGui;
using MyProtocolsForCommunications.Protocol1;

namespace MyProtocolsForCommunications
{
    public abstract class ProtocolState : StatePattern.State<Protocol1.Protocol1>
    {


        public abstract bool InputData(ref DataRecievedWrapperType data);
    }
}
