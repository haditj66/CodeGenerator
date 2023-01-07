
using System;

namespace HolterMonitorGui
{
    public class SequenceFinder
    {

        public bool FullSequenceFound { get; private set; }
        public bool EndOfSequenceStartingAMatch { get; set; }
        private bool firstcharFound;
        private int SequenceCharIndexFound;
        private int indexofthe_strToLookAt = 0;
        public bool FirstcharFound
        {
            get { return firstcharFound;}
            set { firstcharFound = value; }
        }
        public int StartingIndex { get; private set; }
        public int EndingIndex { get; private set; } 

        private string _currentSequence;
        public string CurrentSequence
        {
            get { return _currentSequence; }
            set
            {
                SequenceCharIndexFound = 0;
                Reset();
                _currentSequence = value;
            }
        }

        public bool LookForSequence(string strToLookAt)
        {
            FullSequenceFound = false;
            EndOfSequenceStartingAMatch = false;


            //use regular expression to find a full sequence match


            
            //go through character by character and look for a match on the sequence 
            indexofthe_strToLookAt = 0;
            firstcharFound = false;
            foreach (var c in strToLookAt)
            {
                //if there is a match, look for if the next character is a match
                if (c == CurrentSequence[SequenceCharIndexFound])
                {

                    SequenceCharIndexFound++;
                    if (MatchFound())
                    {
                        return true;
                    }
                }
                //if it is not a match, make sure that it is not a mach starting over
                else if(c == CurrentSequence[0])
                {
                    firstcharFound = false;
                    SequenceCharIndexFound = 1; 
                    if (MatchFound())
                    {
                        return true;
                    }
                }
                //if it is not a match, then I need to reset all parameters
                else
                { 
                    Reset();
                    indexofthe_strToLookAt++;
                }


                
            }

            //if the sequence found from the end of strToLookAt, make the startingIndex a negative
            if (StartingIndex > 0 || firstcharFound == true)
            {
                EndOfSequenceStartingAMatch = true;
                StartingIndex = StartingIndex - indexofthe_strToLookAt + 1;
            } 
            

            return false;

        }


        private bool MatchFound()
        {

            //if the sequence is the first find, change the strating sequence
            if ((firstcharFound == false))
            {
                if ((StartingIndex >= 0))
                {
                    StartingIndex = indexofthe_strToLookAt;
                }
                firstcharFound = true;
            }

            //if the char found is the last one, than it is a full sequence found and I can return a found
            if (SequenceCharIndexFound >= CurrentSequence.Length)
            {
                EndingIndex = indexofthe_strToLookAt;
                FullSequenceFound = true;
                return true;
            }
            indexofthe_strToLookAt++;

            return false;
        }



        public DataRecievedWrapperType RemoveOnlyEverythingBeforeSequence(DataRecievedWrapperType data)
        {
            DataRecievedWrapperType strToRemoveSequence = new DataRecievedWrapperType(data);
            if (!EndOfSequenceStartingAMatch)
            { 
                strToRemoveSequence.Remove(0, StartingIndex);
            }
            else
            {
                strToRemoveSequence.Remove(0, strToRemoveSequence.Length + StartingIndex-1);
            }
            return strToRemoveSequence;
        }

        public DataRecievedWrapperType RemoveFoundSequenceAndEverythingBeforeIt(DataRecievedWrapperType data)
        {
            DataRecievedWrapperType strToRemoveSequence = new DataRecievedWrapperType(data);
            if (!EndOfSequenceStartingAMatch)
            {
                //remove everything before the end of found sequence
                strToRemoveSequence.Remove(0,  1+EndingIndex);
            }
            return strToRemoveSequence;
        }

        public DataRecievedWrapperType RemoveFoundSequence(DataRecievedWrapperType data)
        {
            DataRecievedWrapperType strToRemoveSequence = new DataRecievedWrapperType(data);
            if (!EndOfSequenceStartingAMatch)
            {
                strToRemoveSequence.Remove(StartingIndex, 1+EndingIndex - StartingIndex);
            } 
            return strToRemoveSequence;
        }

        public DataRecievedWrapperType GetAllBeforeFoundSequence(DataRecievedWrapperType data)
        {
            DataRecievedWrapperType strToRemoveSequence = new DataRecievedWrapperType(data);
            if (!EndOfSequenceStartingAMatch)
            {
                strToRemoveSequence.Substring(0, StartingIndex);
            }
            else
            {
                strToRemoveSequence.Substring(0, strToRemoveSequence.Length-1 + StartingIndex);
            }
            return strToRemoveSequence;
        }


        /// <summary>
        /// this will apppend the previous search found from the end of a str to the begginging of this one
        /// </summary>
        /// <param name="strToAppend"></param>
        /// <returns></returns>
        /*public string AppendEndOfSequenceSearchFindToStr(string strToAppend)
        {
            return _currentSequence.Substring(0, (-1 * StartingIndex)+1) + strToAppend;
        }*/

        public void Reset()
        {
            StartingIndex = 0;
            EndingIndex = 0; 
            SequenceCharIndexFound = 0;
            firstcharFound = false;
            EndOfSequenceStartingAMatch = false;
        }





        /*
        /// <summary>
        /// this will filter out ANY potential sequence from the end middle or beggining of a string. (begging on
        ///only if the previous ended with the start of a sequence)
        /// </summary>
        /// <param name="strToFilter"></param>
        /// <returns></returns>
        string GetStringWithoutAnyPotentialSequence(string strToFilter)
        {
            //if the previous search returned a sequence started to be found from the end of the str, than I should append

            LookForSequence(strToFilter);


        }*/

    }



}
