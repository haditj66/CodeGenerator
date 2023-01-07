using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HolterMonitorGui;
 

namespace MyProtocolsForCommunications.Protocol1
{
    public class Idle : ProtocolState
    {
        private SequenceFinder sequenceFinderForIdle;

        private bool FirstPacket;

        public Idle()
        {
            FirstPacket = true;

            sequenceFinderForIdle = new SequenceFinder();
            sequenceFinderForIdle.CurrentSequence = "bbbbbbbbbbbbbbbbbbbbbbbbbbbb";

        }



        public override bool InputData(ref DataRecievedWrapperType data)
        { 

            sequenceFinderForIdle.Reset();

            //check for if there is a "bbbbbbb" sequence found OR at the end of the data
            bool seqFound = sequenceFinderForIdle.LookForSequence(data.DataAsString);
            if (seqFound)
            {
                //if a sequence was found then remove the data all before that sequence, 
                // data handle that and then change states to UploadingDataState
                var data2 = sequenceFinderForIdle.GetAllBeforeFoundSequence(data);
                _context.IdleDataHandler(data2);

                //remove all handled data and the sequence found.
                data = sequenceFinderForIdle.RemoveFoundSequenceAndEverythingBeforeIt(data);

                //change state
                var tger = new UploadingDataState();
                tger.FirstPacket = this.FirstPacket;
                _context.ChangeState(tger);
                this.FirstPacket = false;

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
            else if (sequenceFinderForIdle.EndOfSequenceStartingAMatch)
            {
                //get all data before the sequence getting started and handle that data
                var data2 = sequenceFinderForIdle.GetAllBeforeFoundSequence(data);
                _context.IdleDataHandler(data2);

                //remove everything before sequence being started but NOT the sequence itself
                data = sequenceFinderForIdle.RemoveOnlyEverythingBeforeSequence(data);

                //done with data
                return true;
            }
            //if no sequence was found at all, handle all the data and then remove all data
            else
            {
                _context.IdleDataHandler(data);

                data.Clear();

                //done with data
                return true;
            }

             
        }


    }
}
