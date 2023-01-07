//#define TESTING

using System.IO;
using System.Linq;
using ClassLibrary2;
using System.Windows;
using System.Windows.Input;
using System;
using System.Collections.Generic;
using System.Text;
using MyProtocolsForCommunications;
using MyProtocolsForCommunications.Protocol1;
using MatFileHandler;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace HolterMonitorGui
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {



        private IProtocol protocol;
        private bool isUploading; 
        private static MyUARTSerialPortWrapper Uart;


#if TESTING
        private static string DirOfProject = @"C:\CodeGenerator\CodeGenerator\SerialUtility\SerialWinform"; 
        private static string ProjectName = @"TestProj";
        private static string ProjectTestName = @"defaultTest";
        private static string RunName = @"Run1";

        private static int sizeOfVar1 = 1;
        private static int sizeOfVar2 = 20;
        private static int sizeOfVar3 = 1;
        private static int sizeOfVar4 = 1;
        private static int sizeOfVar5 = 1;
        private static int sizeOfVar6 = 1;
        private static int sizeOfVar7 = 1;
        private static int sizeOfVar8 = 1;

        private int NumOfVars = 8;

        private int ComPort = 3;
        private int BufferThisManyBeforeCallback = 17;
        private int BaudRate = 115200;


#else
        private static string DirOfProject;
        private static string ProjectName;
        private static string ProjectTestName;
        private static string RunName;

        private static int sizeOfVar1;
        private static int sizeOfVar2;
        private static int sizeOfVar3;
        private static int sizeOfVar4;
        private static int sizeOfVar5;
        private static int sizeOfVar6;
        private static int sizeOfVar7;
        private static int sizeOfVar8;

        private int NumOfVars;

        private int ComPort;
        private int BufferThisManyBeforeCallback;
        private int BaudRate;
#endif

        private static string DirOfFileToWrite  ;

        private MyDeSerializer1<float> des; 
        private MyDeSerializer8<float, float, float, float, float, float, float, float> des8; 
        private MyDeSerializer7<float, float, float, float, float, float, float> des7; 
        private MyDeSerializer6<float, float, float, float, float, float> des6; 
        private MyDeSerializer5<float, float, float, float, float> des5; 
        private MyDeSerializer4<float, float, float, float> des4; 
        private MyDeSerializer3<float, float, float> des3; 
        private MyDeSerializer2<float, float> des2; 
        private MyDeSerializer1<float> des1; 



        //example 2
        /*private MyDeSerializer5<float, float, Int16, UInt32, float> des;
        MyDeSerializerContainer<float> var1;
        MyDeSerializerContainer<float> var2;
        MyDeSerializerContainer<Int16> var3;
        MyDeSerializerContainer<UInt32> var4;
        MyDeSerializerContainer<float> var5;*/

        //example 3
        //private MyDeSerializer3<float, float, float> des;
        //MyDeSerializerContainer<float> var1;
        //MyDeSerializerContainer<float> var2; 
        //MyDeSerializerContainer<float> var3;


        void initDeserializer()
        {
            //example 1

            MyDeSerializerContainer<float> var1 = new MyDeSerializerContainer<float>("var1", sizeOfVar1);
            MyDeSerializerContainer<float> var2 = new MyDeSerializerContainer<float>("var2", sizeOfVar2);
            MyDeSerializerContainer<float> var3 = new MyDeSerializerContainer<float>("var3", sizeOfVar3);
            MyDeSerializerContainer<float> var4 = new MyDeSerializerContainer<float>("var4", sizeOfVar4);
            MyDeSerializerContainer<float> var5 = new MyDeSerializerContainer<float>("var5", sizeOfVar5);
            MyDeSerializerContainer<float> var6 = new MyDeSerializerContainer<float>("var6", sizeOfVar6);
            MyDeSerializerContainer<float> var7 = new MyDeSerializerContainer<float>("var7", sizeOfVar7);
            MyDeSerializerContainer<float> var8 = new MyDeSerializerContainer<float>("var8", sizeOfVar8);

            des1 = new MyDeSerializer1<float>(
                var1
                );
            des2 = new MyDeSerializer2<float, float>(
                var1,
                var2
                );
            des3 = new MyDeSerializer3<float, float, float>(
                var1,
                var2,
                var3
                );
            des4 = new MyDeSerializer4<float, float, float, float>(
                var1,
                var2,
                var3,
                var4
                );
            des5 = new MyDeSerializer5<float, float, float, float, float>(
                var1,
                var2,
                var3,
                var4,
                var5
                );
            des6 = new MyDeSerializer6<float, float, float, float, float, float>(
                var1,
                var2,
                var3,
                var4,
                var5,
                var6 
                );
            des7 = new MyDeSerializer7<float, float, float, float, float, float, float>(
                var1,
                var2,
                var3,
                var4,
                var5,
                var6,
                var7 
                );
            des8 = new MyDeSerializer8<float, float, float, float, float, float, float, float>(
                var1,
                var2,
                var3,
                var4,
                var5,
                var6,
                var7,
                var8
                );


            //example 2
            /*
            MyDeSerializerContainer<float> var1 = new MyDeSerializerContainer<float>("var1", 1);
            MyDeSerializerContainer<float> var2 = new MyDeSerializerContainer<float>("var2", 1);
            MyDeSerializerContainer<Int16> var3 = new MyDeSerializerContainer<Int16>("var3", 1);
            MyDeSerializerContainer<UInt32> var4 = new MyDeSerializerContainer<UInt32>("var4", 1);
            MyDeSerializerContainer<float> var5 = new MyDeSerializerContainer<float>("var5", 1);
            des = new MyDeSerializer5<float, float, Int16, UInt32, float>(var1, var2, var3, var4, var5);
            */

            //example 3 
            //MyDeSerializerContainer<float> var1 = new MyDeSerializerContainer<float>("var1", 700);
            //MyDeSerializerContainer<float> var2 = new MyDeSerializerContainer<float>("var2", 700);
            //MyDeSerializerContainer<float> var3 = new MyDeSerializerContainer<float>("var3", 700); 
            //des = new MyDeSerializer3<float, float, float>(var1, var2, var3); 

        }



        private DataRecievedWrapperType dataBuffer;
        

        private List<char> readBufferIfNot4Bytes;
        private int BytesSoFar;


        private void SetValueFromConf(string contentsOfConfFile, string nameOFConfVar, ref string varToSet)
        {
            var regex = new Regex($"{nameOFConfVar}: (.*)");
            var match = regex.Match(contentsOfConfFile);
            if (match.Success)
            {
                varToSet = match.Groups[1].Value.Trim();
            }
        }

        private void SetValueFromConf(string contentsOfConfFile, string nameOFConfVar, ref int varToSet)
        {
            var regex = new Regex($"{nameOFConfVar}: (.*)");
            var match = regex.Match(contentsOfConfFile);
            if (match.Success)
            {
                varToSet = Int32.Parse(match.Groups[1].Value.Trim());
            }
        }

        public MainWindow()
        {

            InitializeComponent();


            //grab all the needed vars from a text file that will be generated from the aegenerate command from the uploadtdu class.
#if !TESTING

            string ss = File.ReadAllText("SerialUtilConf.txt");

  
            SetValueFromConf(ss, "DirOfProject", ref DirOfProject);
            SetValueFromConf(ss, "ProjectName", ref ProjectName);
            SetValueFromConf(ss, "ProjectTestName", ref ProjectTestName);
            SetValueFromConf(ss, "RunName", ref RunName);

            SetValueFromConf(ss, "sizeOfVar1", ref sizeOfVar1);
            SetValueFromConf(ss, "sizeOfVar2", ref sizeOfVar2);
            SetValueFromConf(ss, "sizeOfVar3", ref sizeOfVar3);
            SetValueFromConf(ss, "sizeOfVar4", ref sizeOfVar4);
            SetValueFromConf(ss, "sizeOfVar5", ref sizeOfVar5);
            SetValueFromConf(ss, "sizeOfVar6", ref sizeOfVar6);
            SetValueFromConf(ss, "sizeOfVar7", ref sizeOfVar7);
            SetValueFromConf(ss, "sizeOfVar8", ref sizeOfVar8);

            SetValueFromConf(ss, "NumOfVars", ref NumOfVars); 
             

            SetValueFromConf(ss, "ComPort", ref ComPort); 
            SetValueFromConf(ss, "BufferThisManyBeforeCallback", ref BufferThisManyBeforeCallback); 
            SetValueFromConf(ss, "BaudRate", ref BaudRate); 
             
#endif

            DirOfFileToWrite = Path.Combine(DirOfProject, "UploadedFiles", ProjectTestName, RunName);
            if (Directory.Exists(DirOfFileToWrite) == false)
            {
                Directory.CreateDirectory(DirOfFileToWrite);
                Directory.CreateDirectory(Path.Combine(DirOfFileToWrite, "Matlab"));
            }

            DeleteAllDirsIn(DirOfFileToWrite);
            Directory.CreateDirectory(Path.Combine(DirOfFileToWrite, "Matlab")); 

             
            
            BytesSoFar = 0;
            isUploading = false;
            Uart = new MyUARTSerialPortWrapper(BufferThisManyBeforeCallback, BaudRate, "myPort", ComPort, UartRecievedCallback);//460800
            readBufferIfNot4Bytes = new List<char>();

            protocol = new Protocol1(HandlerForDisplayingData, PacktDataHandler, SendAImDoneSignal);

            dataBuffer = new DataRecievedWrapperType();

            initDeserializer(); 


        }



        void UartRecievedCallback(DataRecievedWrapperType strData, int numOfBytes)
        {

            protocol.InputNewData(strData);

           




            /*
            //if the numOfBytes is not divisible by 4, then just store the
            // readBuffer and wait until it is
            bool isdivisibleBy4 = !((numOfBytes % 4 != 0) || (numOfBytes < 4));


            
            //get the char to use as i might want to use the buffer storing data

            if (isdivisibleBy4 && (readBufferIfNot4Bytes.Count == 0))
            {  

                SendCharListDataToFile(floatData);
            }
           
            else
            {
                readBufferIfNot4Bytes.AddRange(readBuffer.ToList());
                //if the data being stored is divisible by 4 then I can send that data to file
                //if the readBufferIfNot4Bytes count is divisible by four 
                if (!((readBufferIfNot4Bytes.Count % 4 != 0) || (readBufferIfNot4Bytes.Count < 4)))
                {
                    SendCharListDataToFile(readBufferIfNot4Bytes);
                    readBufferIfNot4Bytes.Clear();
                }
              
            }
             */



        }

        

        private void PacktDataHandler(DataRecievedWrapperType data, bool isFirstPacket )
        {
            //if this is the fist packet, i want to clear the databuffer so to start over with new fresh data
            //this is only for this use-case assuming all needed data will be sent in one go. If you send all needed
            //data in sections, this wont work.
            //this will ensure data does not become offset
            bool wasSerialized= false;
            if (isFirstPacket == true)
            { 
                dataBuffer.Clear();
            } 

            dataBuffer = dataBuffer.AppendData(data);


            if (NumOfVars == 8)
            {
                wasSerialized = des8.Deserialize(dataBuffer.DataAsBytesBuffer);
            }
            else if (NumOfVars == 7)
            {
                wasSerialized = des7.Deserialize(dataBuffer.DataAsBytesBuffer);
            }
            else if (NumOfVars == 6)
            {
                wasSerialized = des6.Deserialize(dataBuffer.DataAsBytesBuffer);
            }
            else if (NumOfVars == 5)
            {
                wasSerialized = des5.Deserialize(dataBuffer.DataAsBytesBuffer);
            }
            else if (NumOfVars == 4)
            {
                wasSerialized = des4.Deserialize(dataBuffer.DataAsBytesBuffer);
            }
            else if (NumOfVars == 3)
            {
                wasSerialized = des3.Deserialize(dataBuffer.DataAsBytesBuffer);
            }
            else if (NumOfVars == 2)
            {
                wasSerialized = des2.Deserialize(dataBuffer.DataAsBytesBuffer);
            }
            else if (NumOfVars == 1)
            {
                wasSerialized = des1.Deserialize(dataBuffer.DataAsBytesBuffer);
            }

            if (wasSerialized)
            {

                if (NumOfVars >= 8)
                {
                    ConvertMyDeserializeVarsToMatFile(des8);
                }
                else if (NumOfVars >= 7)
                {
                    ConvertMyDeserializeVarsToMatFile(des7);
                }
                else if (NumOfVars >= 6)
                {
                    ConvertMyDeserializeVarsToMatFile(des6);
                }
                else if (NumOfVars >= 5)
                {
                    ConvertMyDeserializeVarsToMatFile(des5);
                }
                else if (NumOfVars >= 4)
                {
                    ConvertMyDeserializeVarsToMatFile(des4);
                }
                else if (NumOfVars >= 3)
                {
                    ConvertMyDeserializeVarsToMatFile(des3);
                }
                else if (NumOfVars >= 2)
                {
                    ConvertMyDeserializeVarsToMatFile(des2);
                }
                else if (NumOfVars >= 1)
                {
                    ConvertMyDeserializeVarsToMatFile(des1);
                }

                
                if (NumOfVars >= 8)
                { 
                    SendCharListDataToFile(des8.Variable8.Values, 8);
                    SendCharListDataToFile(des8.Variable7.Values, 7);
                    SendCharListDataToFile(des8.Variable6.Values, 6);
                    SendCharListDataToFile(des8.Variable5.Values, 5);
                    SendCharListDataToFile(des8.Variable4.Values, 4);
                    SendCharListDataToFile(des8.Variable3.Values, 3);
                    SendCharListDataToFile(des8.Variable2.Values, 2);
                    SendCharListDataToFile(des8.Variable1.Values, 1);
                }
                else if (NumOfVars >= 7)
                { 
                    SendCharListDataToFile(des7.Variable7.Values, 7);
                    SendCharListDataToFile(des7.Variable6.Values, 6);
                    SendCharListDataToFile(des7.Variable5.Values, 5);
                    SendCharListDataToFile(des7.Variable4.Values, 4);
                    SendCharListDataToFile(des7.Variable3.Values, 3);
                    SendCharListDataToFile(des7.Variable2.Values, 2);
                    SendCharListDataToFile(des7.Variable1.Values, 1);
                }
                else if (NumOfVars >= 6)
                { 
                    SendCharListDataToFile(des6.Variable6.Values, 6);
                    SendCharListDataToFile(des6.Variable5.Values, 5);
                    SendCharListDataToFile(des6.Variable4.Values, 4);
                    SendCharListDataToFile(des6.Variable3.Values, 3);
                    SendCharListDataToFile(des6.Variable2.Values, 2);
                    SendCharListDataToFile(des6.Variable1.Values, 1);
                }
                else if (NumOfVars >= 5)
                { 
                    SendCharListDataToFile(des5.Variable5.Values, 5);
                    SendCharListDataToFile(des5.Variable4.Values, 4);
                    SendCharListDataToFile(des5.Variable3.Values, 3);
                    SendCharListDataToFile(des5.Variable2.Values, 2);
                    SendCharListDataToFile(des5.Variable1.Values, 1);
                }
                else if (NumOfVars >= 4)
                { 
                    SendCharListDataToFile(des4.Variable4.Values, 4);
                    SendCharListDataToFile(des4.Variable3.Values, 3);
                    SendCharListDataToFile(des4.Variable2.Values, 2);
                    SendCharListDataToFile(des4.Variable1.Values, 1);
                }
                else if (NumOfVars >= 3)
                { 
                    SendCharListDataToFile(des3.Variable3.Values, 3);
                    SendCharListDataToFile(des3.Variable2.Values, 2);
                    SendCharListDataToFile(des3.Variable1.Values, 1);
                }
                else if (NumOfVars >= 2)
                { 
                    SendCharListDataToFile(des2.Variable2.Values, 2);
                    SendCharListDataToFile(des2.Variable1.Values, 1);
                }
                else if (NumOfVars >= 1)
                { 
                    SendCharListDataToFile(des1.Variable1.Values, 1); ;
                }

            }
            

            /*if (dataBuffer)
            {
                dataBuffer
            }*/

            //store data into the float cache
            //Uart.StoreDataInFloatCache(data.DataAsString);

            //if the controller is uploading data, I need to send all data
            //to a file instead of outputting it
            //SendCharListDataToFile(des.Variable1.Values);


            /*this.Dispatcher.Invoke(() =>
            {
                //remove everything after a \0 char  

                ReceiveTextBox.Text += dataAsStr;
                ReceiveTextBox.ScrollToEnd();
                //.Where((char c) => { return c.Equals(''); }).ToString();
            });*/
            //File.AppendAllText(@"HolterData.txt", dataAsStr);

        }


        private void SendCharListDataToFile(List<float> dataAsChar, int forVar)
        {
            /*
            //first convert the char array to a float array 
            float[] data = new float[1000];

            Encoding encoding = System.Text.Encoding.ASCII;
            byte[] bytesData = encoding.GetBytes(new string(dataAsChar.ToArray())); //Convert.fr(readBuffer, 0, numOfBytes);
            int i;
            for (i = 0; i < dataAsChar.Count/4; i ++)
            {
                data[i] = BitConverter.ToSingle(bytesData, i*4);

            }

            float[] data2 = new float[dataAsChar.Count / 4];
            Array.Copy(data, data2, dataAsChar.Count / 4);
            */


            string filePathToWrite = Path.Combine(DirOfFileToWrite, "DataVar" + forVar + ".txt");
             
            if (File.Exists(filePathToWrite) == false)
            {
                File.Create(filePathToWrite).Close();
            }
            File.AppendAllLines(filePathToWrite, dataAsChar.Select(f => f.ToString())); 
        }


        private void HandlerForDisplayingData(DataRecievedWrapperType dataToDisplay2)
        {
            this.Dispatcher.Invoke(() =>
            { 
                //remove everything after a \0 char 
                string dataToDisplay = Uart.RemoveTrailingZeros(dataToDisplay2.DataAsString);
                if (dataToDisplay.Contains("PC are you there"))
                {
                    SendAImDoneSignal();

                }
                ReceiveTextBox.Text += dataToDisplay;
                ReceiveTextBox.ScrollToEnd();
                //.Where((char c) => { return c.Equals(''); }).ToString();
            });
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {

            this.Dispatcher.Invoke(() =>
            {
                SendAImDoneSignal();
            });

        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                Uart.WriteLine("2");
            });
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {

            this.Dispatcher.Invoke(() =>
            {
                Uart.WriteLine("3");
            });

        }


        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                Uart.WriteLine("4");
            });
        }

        private void Button5_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                Uart.WriteLine("5");
            });
        }


        private void SendAImDoneSignal()
        {
            Uart.WriteLine("1");
        }
        private void SendNothingSignal()
        {
            // Uart.WriteLine("1");
        }




        private static int  indexOfWrites = 0;
        private static void ConvertMyDeserializeVarsToMatFile(IMyDeSerializer des)
        {
            /*
            var builder = new DataBuilder();

            var floattest = builder.NewArray<float>(1, des.Variable1.Length);
            Array.Copy(des.Variable1.Values.ToArray(), floattest.Data, des.Variable1.Length);
            //var array = builder.NewArray<double>(1, 2);
            //array[0] = -14.5;
            //array[1] = 247.0;
            //var variable = builder.NewVariable("test", array);
            var floattestvariable = builder.NewVariable("floattest", floattest);
            var actual = builder.NewFile(new[] { floattestvariable });
            */

            //Console.WriteLine("queue is length: " + queueOfDeserialiers.Count.ToString());

            indexOfWrites++;

            //buffer creating files ahead of time


            var matFile = des.ConvertMyDeserializeVarsToMatFile();

            
            string filePathToWrite = Path.Combine(DirOfFileToWrite, $"Matlab", $"MatDataVar{indexOfWrites.ToString()}.mat");

            if (File.Exists(filePathToWrite) == false)
            {
                File.Create(filePathToWrite).Close();
            }

            //for (int i = indexOfWrites; i < indexOfWrites + 10; i++)
            //{
            //    string ff = Path.Combine(DirOfFileToWrite, $"Matlab", $"MatDataVar{i.ToString()}.mat");
            //    if (File.Exists(ff) == false)
            //    {
            //        File.Create(ff).Close();
            //    }
            //}


            using (var fileStream = new System.IO.FileStream(filePathToWrite, System.IO.FileMode.Create))
            {
                var writer = new MatFileWriter(fileStream);
                writer.Write(matFile);
            }
            //SendCharListDataToFile(des.Variable1.Values);

            //indexOfFile = indexOfFile >= 10 ? 1 : indexOfFile + 1;
        }


        public static void DeleteAllDirsIn(string  strdirectory)
        {
            System.IO.DirectoryInfo directory = new DirectoryInfo(strdirectory);
            foreach (System.IO.FileInfo file in directory.GetFiles()) file.Delete();
            foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
        }


        /*
private void SendTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
{
   if (e.Key == Key.Enter)
   {
       Uart.WriteLine(SendTextBox.Text);
   }


}*/

    }
}
