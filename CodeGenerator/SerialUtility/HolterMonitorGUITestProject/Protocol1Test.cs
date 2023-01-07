using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyProtocolsForCommunications.Protocol1;
using HolterMonitorGui;

namespace HolterMonitorGUITestProject
{

    /// <summary>
    /// Summary description for Protocol1Test
    /// </summary>
    [TestClass]
    public class Protocol1Test
    {
        private Protocol1 protocol1;


        public Protocol1Test()
        {
            IdleBuffer =new DataRecievedWrapperType();
            PacketBuffer = new DataRecievedWrapperType();

            protocol1 = new Protocol1(IdleDataHandler, PacketDataHandler, RecieveHandler);

        }


        private DataRecievedWrapperType IdleBuffer;
        private DataRecievedWrapperType PacketBuffer;
        private int countOfRecieverSent;

        private void IdleDataHandler(DataRecievedWrapperType str)
        {
            IdleBuffer = IdleBuffer.AppendData(str);
        }
        private void PacketDataHandler(DataRecievedWrapperType str, bool isFirstPacket)
        {
            PacketBuffer = PacketBuffer.AppendData(str); 
        }

        private void RecieveHandler()
        {
            countOfRecieverSent++;
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion


        //test that 
        [TestMethod]
        public void Test_BasicStringOfdataToStartUploadingWithAPacket()
        {
            string data =
                "bbbbbbbbbbbbbbbbbbbbbbbbbbbbbstartingthisisdatafrompacketfinishedqqqqqqqqqqqqqqqqqqqqqqqqqqqq";
            DataRecievedWrapperType dataa = new DataRecievedWrapperType();
            dataa.SetNewData(Encoding.ASCII.GetBytes(data));


            protocol1.InputNewData(dataa);

            Assert.IsTrue(PacketBuffer.DataAsString == "thisisdatafrompacket");
        }


        //test that 
        [TestMethod]
        public void Test_BasicStringOfdataToStartUploadingWithAPacket2()
        {
            string data =
                "bbbbbbbbbbbbbbbbbbbbbbbbbbbbbstartingthisisdatafrompacketfinisheddfvd" +
                "startingthisisdatafrompacket2finisheddvqqqqqqqqqqqqqqqqqqqqqqqqqqqq";
            DataRecievedWrapperType dataa = new DataRecievedWrapperType();
            dataa.SetNewData(Encoding.ASCII.GetBytes(data));


            protocol1.InputNewData(dataa);

            Assert.IsTrue(PacketBuffer.DataAsString == "thisisdatafrompacketthisisdatafrompacket2");
            //Assert.IsTrue(countOfRecieverSent == 2);
        }

        //test that 
        [TestMethod]
        public void Test_BasicStringOfdataAllChoppedUp()
        {
            string data1 =
                "bbbbbbbbbbbbbbb";
            string data2 = "bbbbbbbbbbbbbbstarti";
            string data3 = "ngthisisdatafrompacketfi";
            string data4 = "nishedqqqqqqqqqq";
            string data5 = "qqqqqqqqqqqqqqqqqq";

            DataRecievedWrapperType dataa1 = new DataRecievedWrapperType();
            dataa1.SetNewData(Encoding.ASCII.GetBytes(data1));
            DataRecievedWrapperType dataa2 = new DataRecievedWrapperType();
            dataa2.SetNewData(Encoding.ASCII.GetBytes(data2));
            DataRecievedWrapperType dataa3 = new DataRecievedWrapperType();
            dataa3.SetNewData(Encoding.ASCII.GetBytes(data3));
            DataRecievedWrapperType dataa4 = new DataRecievedWrapperType();
            dataa4.SetNewData(Encoding.ASCII.GetBytes(data4));
            DataRecievedWrapperType dataa5 = new DataRecievedWrapperType();
            dataa5.SetNewData(Encoding.ASCII.GetBytes(data5));


            protocol1.InputNewData(dataa1);
            protocol1.InputNewData(dataa2);
            protocol1.InputNewData(dataa3);
            protocol1.InputNewData(dataa4);
            protocol1.InputNewData(dataa5);

            Assert.IsTrue(PacketBuffer.DataAsString == "thisisdatafrompacket");
            //Assert.IsTrue(countOfRecieverSent == 1);
        }

        //test that 
        [TestMethod]
        public void Test_BasicStringOfdataAllChoppedUp2()
        {
            string data1 =
                "sccfbbbbbbbbbbbbbbb";
            string data2 = "bbbbbbbbbbbbbbdvsstarti";
            string data3 = "ngthisisdatafrompacketf";
            string data4 = "inishedvvdqqqqqqqqqq";
            string data5 = "qqqqqqqqqqqqqqqqqq";
            DataRecievedWrapperType dataa1 = new DataRecievedWrapperType();
            dataa1.SetNewData(Encoding.ASCII.GetBytes(data1));
            DataRecievedWrapperType dataa2 = new DataRecievedWrapperType();
            dataa2.SetNewData(Encoding.ASCII.GetBytes(data2));
            DataRecievedWrapperType dataa3 = new DataRecievedWrapperType();
            dataa3.SetNewData(Encoding.ASCII.GetBytes(data3));
            DataRecievedWrapperType dataa4 = new DataRecievedWrapperType();
            dataa4.SetNewData(Encoding.ASCII.GetBytes(data4));
            DataRecievedWrapperType dataa5 = new DataRecievedWrapperType();
            dataa5.SetNewData(Encoding.ASCII.GetBytes(data5));
             
            protocol1.InputNewData(dataa1);
            protocol1.InputNewData(dataa2);
            protocol1.InputNewData(dataa3);
            protocol1.InputNewData(dataa4);
            protocol1.InputNewData(dataa5);


            Assert.IsTrue(PacketBuffer.DataAsString == "thisisdatafrompacket");
        }


        //test that 
        [TestMethod]
        public void Test_BasicStringOfdataAllChoppedUp3()
        {
            string data1 =
                "startbbbbbbbbbccfbbbbbbbbbbbbbbb";
            string data2 = "bbbbbbbbbbbbbbdvsssstarting";
            string data3 = "thisisdatafffinisfrompacketf";
            string data4 = "fffinishedvvdqqqqqqqqqq";
            string data5 = "qqqqqqqqqqqqqqqqqq";
            DataRecievedWrapperType dataa1 = new DataRecievedWrapperType();
            dataa1.SetNewData(Encoding.ASCII.GetBytes(data1));
            DataRecievedWrapperType dataa2 = new DataRecievedWrapperType();
            dataa2.SetNewData(Encoding.ASCII.GetBytes(data2));
            DataRecievedWrapperType dataa3 = new DataRecievedWrapperType();
            dataa3.SetNewData(Encoding.ASCII.GetBytes(data3));
            DataRecievedWrapperType dataa4 = new DataRecievedWrapperType();
            dataa4.SetNewData(Encoding.ASCII.GetBytes(data4));
            DataRecievedWrapperType dataa5 = new DataRecievedWrapperType();
            dataa5.SetNewData(Encoding.ASCII.GetBytes(data5));

            protocol1.InputNewData(dataa1);
            protocol1.InputNewData(dataa2);
            protocol1.InputNewData(dataa3);
            protocol1.InputNewData(dataa4);
            protocol1.InputNewData(dataa5);

            Assert.IsTrue(PacketBuffer.DataAsString == "thisisdatafffinisfrompacketfff");
        }

        //test that 
        [TestMethod]
        public void Test_BasicStringOfdataAllChoppedUp4()
        {
            string dat1 =
                "startbbbbbbbbbccfbbbbbbbbbbbbbbb";
            string dat21 = "bbbbbbbbbbbbbbdvsssstar";
            string dat22 = "t";
            string dat23 = "i";
            string dat24 = "n";
            string dat25 = "g";
            string dat31 = "d";
            string dat32 = "thisisdatafffinisfrompacketf";
            string dat41 = "ff";
            string dat42 = "f";
            string dat43 = "i";
            string dat44 = "nishedvvdqqqqqqqqqq";
            string dat5 = "qqqqqqqqqqqqqqqqqq";

            DataRecievedWrapperType data1 = new DataRecievedWrapperType();
            data1.SetNewData(Encoding.ASCII.GetBytes(dat1));
            DataRecievedWrapperType data21 = new DataRecievedWrapperType();
            data21.SetNewData(Encoding.ASCII.GetBytes(dat21));
            DataRecievedWrapperType data22 = new DataRecievedWrapperType();
            data22.SetNewData(Encoding.ASCII.GetBytes(dat22));
            DataRecievedWrapperType data23 = new DataRecievedWrapperType();
            data23.SetNewData(Encoding.ASCII.GetBytes(dat23));
            DataRecievedWrapperType data24 = new DataRecievedWrapperType();
            data24.SetNewData(Encoding.ASCII.GetBytes(dat24));
            DataRecievedWrapperType data25 = new DataRecievedWrapperType();
            data25.SetNewData(Encoding.ASCII.GetBytes(dat25));
            DataRecievedWrapperType data31 = new DataRecievedWrapperType();
            data31.SetNewData(Encoding.ASCII.GetBytes(dat31));
            DataRecievedWrapperType data32 = new DataRecievedWrapperType();
            data32.SetNewData(Encoding.ASCII.GetBytes(dat32));
            DataRecievedWrapperType data41 = new DataRecievedWrapperType();
            data41.SetNewData(Encoding.ASCII.GetBytes(dat41));
            DataRecievedWrapperType data42 = new DataRecievedWrapperType();
            data42.SetNewData(Encoding.ASCII.GetBytes(dat42));
            DataRecievedWrapperType data43 = new DataRecievedWrapperType();
            data43.SetNewData(Encoding.ASCII.GetBytes(dat43));
            DataRecievedWrapperType data44 = new DataRecievedWrapperType();
            data44.SetNewData(Encoding.ASCII.GetBytes(dat44));
            DataRecievedWrapperType data5 = new DataRecievedWrapperType();
            data5.SetNewData(Encoding.ASCII.GetBytes(dat5)); 


            protocol1.InputNewData(data1);
            protocol1.InputNewData(data21);
            protocol1.InputNewData(data22);
            protocol1.InputNewData(data23);
            protocol1.InputNewData(data24);
            protocol1.InputNewData(data25);
            protocol1.InputNewData(data31);
            protocol1.InputNewData(data32);
            protocol1.InputNewData(data41);
            protocol1.InputNewData(data42);
            protocol1.InputNewData(data43);
            protocol1.InputNewData(data44);
            protocol1.InputNewData(data5);

            Assert.IsTrue(PacketBuffer.DataAsString == "dthisisdatafffinisfrompacketfff");
        }


        //test that 
        [TestMethod]
        public void Test_BasicStringOfdataAllChoppedUp5()
        {
            /*
            string data1 = 
                "startbbbbbbbbbccfbbbbbbbbbbbbbbb"; 
            string data11 = "b";
            string data12 = "b";
            string data13 = "b";
            string data14 = "b";
            string data15 = "b";
            string data16 = "b";
            string data17 = "b";
            string data18 = "b";
            string data19 = "b";
            string data110 = "b";
            string data111 = "b";
            string data112 = "b";
            string data113 = "b";
            string data114 = "b";
            string data2 = "dvsssstarting";
            string data3 = "thisisdatafffinisfrompacketf";
            string data4 = "fffinishedvvdqqqqqqqqqq";
            string data5 = "qqqqqqqqqqqqqqqqqq";
            protocol1.InputNewData(data1);
            protocol1.InputNewData(data11);
            protocol1.InputNewData(data12);
            protocol1.InputNewData(data13);
            protocol1.InputNewData(data14);
            protocol1.InputNewData(data15);
            protocol1.InputNewData(data16);
            protocol1.InputNewData(data17);
            protocol1.InputNewData(data18);
            protocol1.InputNewData(data19);
            protocol1.InputNewData(data110);
            protocol1.InputNewData(data111);
            protocol1.InputNewData(data112);
            protocol1.InputNewData(data113);
            protocol1.InputNewData(data114);
            protocol1.InputNewData(data2);
            protocol1.InputNewData(data3);
            protocol1.InputNewData(data4);
            protocol1.InputNewData(data5);

            Assert.IsTrue(PacketBuffer == "thisisdatafffinisfrompacketfff");
        */

        }
       
    }
}
