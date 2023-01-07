using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using HolterMonitorGui;

namespace MyProtocolsForCommunications.Protocol1
{
    public class UploadingDataState : ProtocolState
    {

        private SequenceFinder sequenceFinderFor_starting;
        private SequenceFinder sequenceFinderFor_qqqqqqqq;

        public bool FirstPacket;

        public UploadingDataState()
        { 
            sequenceFinderFor_starting = new SequenceFinder();
            sequenceFinderFor_starting.CurrentSequence = "starting";
            sequenceFinderFor_qqqqqqqq = new SequenceFinder();
            sequenceFinderFor_qqqqqqqq.CurrentSequence = "qqqqqqqqqqqqqqqqqqqqqqqqqqqq";
        }



        public override bool InputData(ref DataRecievedWrapperType data)
        {

            sequenceFinderFor_starting.Reset();
            sequenceFinderFor_qqqqqqqq.Reset();

            // if both sequences were found within the same string, then I need to handle the one that happened
            //first
            bool seqFoundStarting = sequenceFinderFor_starting.LookForSequence(data.DataAsString);
            bool seqFounddddddddd = sequenceFinderFor_qqqqqqqq.LookForSequence(data.DataAsString);
            if (seqFoundStarting && seqFounddddddddd)
            {
                if (sequenceFinderFor_starting.EndingIndex < sequenceFinderFor_qqqqqqqq.EndingIndex)
                {
                    HandlingstartingFound(ref data);
                }
                else
                {
                    HandlingddddddddFound(ref data);
                }
                return false;

            }
            //if nothing was found for any of them.
            else if (!sequenceFinderFor_starting.EndOfSequenceStartingAMatch &&
                     !sequenceFinderFor_qqqqqqqq.EndOfSequenceStartingAMatch &&
                     !seqFoundStarting &&
                     !seqFounddddddddd)
            {
                //trash all data
                data.Clear();
                return true;
            }
            // handle whichever one had a full sequence happen
            else if (seqFoundStarting)
            {
                HandlingstartingFound(ref data);
                if (data.Length > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else if (sequenceFinderFor_starting.EndOfSequenceStartingAMatch)
            {
                HandlingstartingFound(ref data);
                return true;
            }
            else if (sequenceFinderFor_qqqqqqqq.EndOfSequenceStartingAMatch)
            {
                 HandlingddddddddFound(ref data);
                return true;
            }
            else if (seqFounddddddddd)
            {
                 HandlingddddddddFound(ref data);
                if (data.Length > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            //if there is anymore data, then return that I am not done with the data yet
            if (data.Length > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        private void HandlingddddddddFound(ref DataRecievedWrapperType data)
        {
             

            if (sequenceFinderFor_qqqqqqqq.FullSequenceFound)
            {
                //if a sequence was found then remove the data all before that sequence, 
                // and Dont handle that data as it is trash. 
                data = sequenceFinderFor_qqqqqqqq.RemoveFoundSequenceAndEverythingBeforeIt(data);

                //change state to the idle state
                _context.ChangeState(new Idle());

                //send a signal that it is done with the packet
                _context.SendDataRecievedHandler();

                return  ;
            }
            //if an end sequence is being started.
            else if (sequenceFinderFor_qqqqqqqq.EndOfSequenceStartingAMatch)
            {
                //trash all data before the data being started,
                data = sequenceFinderFor_qqqqqqqq.RemoveOnlyEverythingBeforeSequence(data);

                return  ;
            }
            else
            { 
                return  ;
            }
             

        }

        private void HandlingstartingFound(ref DataRecievedWrapperType data)
        { 
            if (sequenceFinderFor_starting.FullSequenceFound)
            {
                //if a sequence was found then remove the data all before that sequence, 
                // and Dont handle that data as it is trash. 
                data = sequenceFinderFor_starting.RemoveFoundSequenceAndEverythingBeforeIt(data);

                //change state to the OpeningPacketState
                var cscsc = new OpeningPacketState();
                cscsc.FirstPacket = this.FirstPacket;
                _context.ChangeState(cscsc);
                FirstPacket = false;

                return;
            }
            //if an end sequence is being started.
            else if(sequenceFinderFor_starting.EndOfSequenceStartingAMatch)
            {
                //trash all data before the data being started,
                data = sequenceFinderFor_starting.RemoveOnlyEverythingBeforeSequence(data);

                return;
            }
            else
            { 
                return;
            }

             
        }

    }
}
