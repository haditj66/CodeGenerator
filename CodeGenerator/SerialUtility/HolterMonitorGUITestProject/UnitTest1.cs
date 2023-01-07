using System;
using System.ComponentModel;
using System.Text;
using HolterMonitorGui;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HolterMonitorGUITestProject
{
    [TestClass]
    public class UnitTest1
    {
        private SequenceFinder sequenceFinder;

        public UnitTest1()
        {
            sequenceFinder = new SequenceFinder();
        }

        [TestMethod]
        public void FindSequenceInMidStr()
        {

            sequenceFinder.CurrentSequence = "starting";


            string test = "af;repgpopondstarkmp;omgstartingkmdgstalmh;";

            sequenceFinder.LookForSequence(test);

            Assert.IsTrue(sequenceFinder.EndOfSequenceStartingAMatch == false);
            Assert.IsTrue(sequenceFinder.StartingIndex == 24);
            Assert.IsTrue(sequenceFinder.EndingIndex == 31);
        }

        [TestMethod]
        public void FindSequencefromEndOfOneStrToNext()
        {

            sequenceFinder.CurrentSequence = "starting";

            string test = "af;repgpopondstarkmp;omgstngkmdgsta";
            string test2 = "rtingaf;repgpopondstarkmp;omgstngkmdg";

            bool found1 = sequenceFinder.LookForSequence(test);
            bool found2 = sequenceFinder.LookForSequence(test2);

            Assert.IsTrue(sequenceFinder.EndOfSequenceStartingAMatch == false);
            Assert.IsTrue(sequenceFinder.StartingIndex == -2);
            Assert.IsTrue(sequenceFinder.EndingIndex == 4);
            Assert.IsTrue(!found1);
            Assert.IsTrue(found2);
        }


        [TestMethod]
        public void FindSartOfSequencefromEnd()
        {

            sequenceFinder.CurrentSequence = "starting";

            string test = "af;repgpopondstarkmp;omgstngkmdgsta";

            sequenceFinder.LookForSequence(test);

            Assert.IsTrue(sequenceFinder.EndOfSequenceStartingAMatch == true);
            Assert.IsTrue(sequenceFinder.FirstcharFound == true);
            Assert.IsTrue(sequenceFinder.StartingIndex == -2);
            Assert.IsTrue(sequenceFinder.EndingIndex == 0);
        }

        [TestMethod]
        public void FindSartOfSequencefromEnd2()
        {

            sequenceFinder.CurrentSequence = "starting";

            string test = "bdvsstarti";

            sequenceFinder.LookForSequence(test);

            Assert.IsTrue(sequenceFinder.EndOfSequenceStartingAMatch == true);
            Assert.IsTrue(sequenceFinder.FirstcharFound == true);

        }


        [TestMethod]
        public void FindSartOfSequencefromEnd3()
        {

            sequenceFinder.CurrentSequence = "starting";

            string test = "bdvssssstarti"; 

            DataRecievedWrapperType data = new DataRecievedWrapperType();
            data.SetNewData(Encoding.ASCII.GetBytes(test));

            sequenceFinder.LookForSequence(test);

            Assert.IsTrue(sequenceFinder.EndOfSequenceStartingAMatch == true);
            Assert.IsTrue(sequenceFinder.FirstcharFound == true);


            var data2 = sequenceFinder.RemoveOnlyEverythingBeforeSequence(data);
            Assert.IsTrue(data2.DataAsString == "starti");

            test = "bdvssssstarti";
            data.SetNewData(Encoding.ASCII.GetBytes(test));
            var data3 = sequenceFinder.GetAllBeforeFoundSequence(data);
            Assert.IsTrue(data3.DataAsString == "bdvssss");
        }


        [TestMethod]
        public void FindSartOfSequencefromEnd_where_it_only_catches_the_last_char()
        {

            sequenceFinder.CurrentSequence = "starting";

            string test = "af;repgpopondstarkmp;omgstngkmdgs";

            sequenceFinder.LookForSequence(test);

            Assert.IsTrue(sequenceFinder.EndOfSequenceStartingAMatch == true);
            Assert.IsTrue(sequenceFinder.StartingIndex == 0);
            Assert.IsTrue(sequenceFinder.EndingIndex == 0);
        }

        [TestMethod]
        public void FindSartOfSequencefromEnd_exactly()
        {

            sequenceFinder.CurrentSequence = "starting";

            string test = "af;repgpopondstarkmp;omgstngkmdgstarting";

            sequenceFinder.LookForSequence(test);

            Assert.IsTrue(sequenceFinder.FirstcharFound == true);
            Assert.IsTrue(sequenceFinder.EndOfSequenceStartingAMatch == false);
            Assert.IsTrue(sequenceFinder.StartingIndex == 32);
            Assert.IsTrue(sequenceFinder.EndingIndex == 39);
        }


        /*
        [TestMethod]
        public void AppendPreviousSearchOnNewString()
        {

            sequenceFinder.CurrentSequence = "starting";

            string test = "af;repgpopondstarkmp;omgstngkmdgstart";
            string test2 = "ffklmb";
            DataRecievedWrapperType data = new DataRecievedWrapperType();
            data.SetNewData(Encoding.ASCII.GetBytes(test));
            DataRecievedWrapperType data2 = new DataRecievedWrapperType();
            data2.SetNewData(Encoding.ASCII.GetBytes(test2));


            sequenceFinder.LookForSequence(test);
            sequenceFinder.AppendEndOfSequenceSearchFindToStr(data2);

            Assert.IsTrue(data2 == "startffklmb");

        }*/


        [TestMethod]
        public void RemoveASequenceFound()
        {

            sequenceFinder.CurrentSequence = "starting";

            string test2 = "af;repgpopondstartingkmp;omgstngkmdgstar";
            DataRecievedWrapperType data = new DataRecievedWrapperType();
            data.SetNewData(Encoding.ASCII.GetBytes(test2));



            sequenceFinder.LookForSequence(test2);
            var data2 = sequenceFinder.RemoveFoundSequence(data);

            Assert.IsTrue(data2.DataAsString == "af;repgpopondkmp;omgstngkmdgstar");

            string strData = System.Text.Encoding.ASCII.GetString(data2.DataAsBytesBuffer.ToArray(), 0, data2.DataAsBytesBuffer.Count);
            Assert.IsTrue(strData == "af;repgpopondkmp;omgstngkmdgstar");
        }


        [TestMethod]
        public void Remove_AllBeforeSequenceFoundIt()
        {

            sequenceFinder.CurrentSequence = "starting";

            string test2 = "af;repgpopondstartingkmp;omgstngkmdgstar";
            DataRecievedWrapperType data = new DataRecievedWrapperType();
            data.SetNewData(Encoding.ASCII.GetBytes(test2));

            sequenceFinder.LookForSequence(test2);
            var data11 = sequenceFinder.RemoveOnlyEverythingBeforeSequence(data);

            Assert.IsTrue(data11.DataAsString == "startingkmp;omgstngkmdgstar");

            sequenceFinder.CurrentSequence = "starting";

            string test  = "af;repgpoponkmp;omgstngkmdgstar";
            DataRecievedWrapperType data2 = new DataRecievedWrapperType();
            data2.SetNewData(Encoding.ASCII.GetBytes(test));

            sequenceFinder.LookForSequence(test );
            var data22 = sequenceFinder.RemoveOnlyEverythingBeforeSequence(data2);

            Assert.IsTrue(data22.DataAsString == "star");

        }

        [TestMethod]
        public void GetAllBeforeTheSequenceFound()
        {

            sequenceFinder.CurrentSequence = "starting";

            string test2 = "af;repgpopondstartingkmp;omgstngkmdgstar";
            DataRecievedWrapperType data = new DataRecievedWrapperType();
            data.SetNewData(Encoding.ASCII.GetBytes(test2));

            sequenceFinder.LookForSequence(test2);
            var data2 = sequenceFinder.GetAllBeforeFoundSequence(data);

            Assert.IsTrue(data2.DataAsString == "af;repgpopond");

        }


        [TestMethod]
        public void GetAllBeforeTheSequenceFound2()
        {

            sequenceFinder.CurrentSequence = "finished";

            string test2 = "af;repgpopondsfi";
            DataRecievedWrapperType data = new DataRecievedWrapperType();
            data.SetNewData(Encoding.ASCII.GetBytes(test2));

            sequenceFinder.LookForSequence(test2);
            var data2 = sequenceFinder.GetAllBeforeFoundSequence(data);

            Assert.IsTrue(data2.DataAsString == "af;repgpoponds");

        }
    }
}
