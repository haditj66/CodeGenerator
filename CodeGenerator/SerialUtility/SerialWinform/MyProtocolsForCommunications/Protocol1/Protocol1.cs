using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using HolterMonitorGui;
using MyProtocolsForCommunications.StatePattern;

namespace MyProtocolsForCommunications.Protocol1
{
    public class Protocol1 : Context<ProtocolState>, IProtocol
    {
        public Action<DataRecievedWrapperType> IdleDataHandler { get; protected set; }
        public Action<DataRecievedWrapperType, bool > InsidePacketDataHandler { get; protected set; } 
        public Action SendDataRecievedHandler { get; protected set; }

        private DataRecievedWrapperType LeftOverDataForNextInput;

        public Protocol1(Action<DataRecievedWrapperType> idleDataHandler, Action<DataRecievedWrapperType, bool> insidePacketDataHandler, Action sendDataRecievedHandler) : base(new Idle())
        {
            InsidePacketDataHandler = insidePacketDataHandler;
            IdleDataHandler = idleDataHandler;
            SendDataRecievedHandler = sendDataRecievedHandler;

            LeftOverDataForNextInput = new DataRecievedWrapperType();
        }



        public override void InitCrossReference()
        {
            MyCurrentState._context = this;
        }


        /// <summary>
        /// sends data through the protocol until the protocol returns left over data it wont deal with anymore
        /// </summary>
        /// <param name="Data"></param>
        public void InputNewData(DataRecievedWrapperType Data)
        {

            if (Data.Contains("qwetyufghjklgyucrrthkupliby"))
            {
                SendDataRecievedHandler();
            }

            //append to the last left over data
            Data = LeftOverDataForNextInput.AppendData(Data);


            bool doneWithData = false;
            while (!doneWithData)
            {
                doneWithData = MyCurrentState.InputData(ref Data);
            }

            //if there is any left over data, leave it in a cache for next time
            LeftOverDataForNextInput = Data;

        }
    }
}
