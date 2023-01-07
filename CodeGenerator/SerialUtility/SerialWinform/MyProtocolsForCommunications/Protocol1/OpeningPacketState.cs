using HolterMonitorGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProtocolsForCommunications.Protocol1
{
    public class OpeningPacketState : ProtocolState
    {
        private SequenceFinder sequenceFinderFor_finished;


        public bool FirstPacket;

        public OpeningPacketState()
        { 
            sequenceFinderFor_finished = new SequenceFinder();
            sequenceFinderFor_finished.CurrentSequence = "finished";
        }

        public override bool InputData(ref DataRecievedWrapperType data)
        { 

            sequenceFinderFor_finished.Reset();

            //check for if there is a "finished" sequence  
            bool seqFound = sequenceFinderFor_finished.LookForSequence(data.DataAsString);
            if (seqFound)
            {
                //if a sequence was found then remove the data all before that sequence, 
                // data handle that and then change states to UploadingDataState
                var data2 = sequenceFinderFor_finished.GetAllBeforeFoundSequence(data);
                _context.InsidePacketDataHandler(data2, FirstPacket);
                FirstPacket = false;

                //remove all handled data and the sequence found.
                data = sequenceFinderFor_finished.RemoveFoundSequenceAndEverythingBeforeIt(data);

                //change state
                _context.ChangeState(new UploadingDataState());

                //send a signal that it is done with the packet
                _context.SendDataRecievedHandler();
                 
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
            //if there was no sequence found but there was something started to be found near the end of the data
            else if (sequenceFinderFor_finished.EndOfSequenceStartingAMatch)
            {
                //get all data before the sequence getting started and handle that data
                var data2 = sequenceFinderFor_finished.GetAllBeforeFoundSequence(data);
                _context.InsidePacketDataHandler(data2, FirstPacket);
                FirstPacket = false;

                //remove everything before sequence being started but NOT the sequence itself
                data = sequenceFinderFor_finished.RemoveOnlyEverythingBeforeSequence(data);

                return true;
            }
            //if no sequence was found at all, handle all the data and then remove all data
            else
            {
                _context.InsidePacketDataHandler(data, FirstPacket);
                FirstPacket = false;

                data.Clear();
                return true;
            }

             
        }
    }
}
