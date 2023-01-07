using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Security.Policy;
using HolterMonitorGui;

namespace ClassLibrary2
{

    public class MyUARTSerialPortWrapper
    {

        SerialPort ComPort;


        private Action<DataRecievedWrapperType, int> CallbackWhenDataRecieved;

        private byte[] readBuffer = new byte[20000];
        private List<byte> readBufferList = new List<byte>();
        private static DataRecievedWrapperType[] dataRecieved;
        private DataRecievedWrapperType[] dataRecievedBuffer;
        private DataRecievedWrapperType dataRecieved2;
        private DataRecievedWrapperType dataRecievedBuffer2;
        private int readSoFar = 0;
        private int readLastTime = 0;
        private int numOfMsgs = 0;
        private int numofBytesToRead;
        private List<byte> dataFloatCache;

        private int BufferThisManyBytesBeforeCallback;
        private int TaskIndex;

        private bool even = true;

        public MyUARTSerialPortWrapper(int bufferThisManyBytesBeforeCallback, int baudRate, string name, int port, Action<DataRecievedWrapperType, int> callbackWhenDataRecieved)
        {
            BufferThisManyBytesBeforeCallback = bufferThisManyBytesBeforeCallback;

            ComPort = new SerialPort("COM" + port.ToString());

            ComPort.BaudRate = baudRate;
            ComPort.Parity = Parity.None;
            ComPort.StopBits = StopBits.One;
            ComPort.DataBits = 8;

            ComPort.Open();
            ComPort.DataReceived += ComPort_DataReceived;

            dataFloatCache = new List<byte>();
            dataRecieved  = new DataRecievedWrapperType[50];
            for (int i = 0; i < dataRecieved.Length; i++)
            {
                dataRecieved[i] = new DataRecievedWrapperType();
            }

            TaskIndex = 0;
             
            dataRecieved2 = new DataRecievedWrapperType();
            dataRecievedBuffer2 = new DataRecievedWrapperType();
            CallbackWhenDataRecieved += callbackWhenDataRecieved;
        }


        public void ComPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            readSoFar = 0;
            numofBytesToRead = ComPort.BytesToRead;

            //read bytes and store in the readBuffer
            while ((readSoFar += ComPort.Read(readBuffer, readSoFar, numofBytesToRead - readSoFar)) != numofBytesToRead) ;

            numOfMsgs++;

            //convert bytes to an array of chars
            //List<char> uartCharArray = new List<char>();
            //char[] uartCharArray = new char[numofBytesToRead];

            //string strData = ""; 
            //strData = System.Text.Encoding.ASCII.GetString(readBuffer,0, numofBytesToRead);

            byte[] sefe = new byte[numofBytesToRead];
            Array.Copy(readBuffer, sefe, numofBytesToRead);
            readBufferList.AddRange(sefe);




            if (readBufferList.Count >= BufferThisManyBytesBeforeCallback)
            { 

                dataRecieved[TaskIndex].SetNewData(readBufferList.ToArray(), readBufferList.Count);
                int forTaskindex = TaskIndex;
                Task.Factory.StartNew(() =>
                    {
                        CallbackWhenDataRecieved(dataRecieved[forTaskindex], dataRecieved[forTaskindex].Length);
                        dataRecieved[forTaskindex].Clear();
                    }
                );
                even = false;

                TaskIndex = TaskIndex >= dataRecieved.Length-1 ? 0 : TaskIndex + 1;


                readBufferList.Clear();

            }




            /*
            string uartStr = new string(uartCharArray, 0, numofBytesToRead);
            int indexOfemptyChars = uartStr.IndexOf('\0');
            if (indexOfemptyChars>0)
            {
                uartStr = uartStr.Remove(indexOfemptyChars, numofBytesToRead - indexOfemptyChars);

            }*/

            //CallbackWhenDataRecieved(dataRecieved, numofBytesToRead);
        }


        /// <summary>
        /// this will store all data that has been read into to float chache.
        /// this way data read that was not divisible by four, will be saved until the
        /// user requests that chache.
        /// </summary>
        public void StoreDataInFloatCache(string dataToStore)
        {
            byte[] dataToAppend = Encoding.ASCII.GetBytes(dataToStore);
            //byte[] dataToAppend = new byte[numofBytesToRead];
            //Array.Copy(readBuffer, dataToAppend, numofBytesToRead); 
            dataFloatCache.AddRange(dataToAppend.ToList());
        }
        public void StoreDataInFloatCache(byte[] dataToStore)
        {
            dataFloatCache.AddRange(dataToStore.ToList());
        }

        /// <summary>
        /// gets all floats in read cache and converts all available floats 
        /// </summary>
        public List<float> GetFloatsInFloatDataChache()
        {
            List<float> floatData = new List<float>();//[numofBytesToRead / 4];

            //get divisibility of four
            int divisibleByFourCount = (int)Math.Floor((float)(dataFloatCache.Count / 4));

            for (int i = 0; i < divisibleByFourCount; i++)
            {
                floatData.Add(System.BitConverter.ToSingle(dataFloatCache.ToArray(), i * 4));

            }

            //remove from the cache  the amount of data turned to a float.
            dataFloatCache.RemoveRange(0, divisibleByFourCount * 4);

            return floatData;
        }


        public char[] RemoveTrailingZeros(char[] charToRemove, int sizeOfarray)
        {
            string uartStr = new string(charToRemove, 0, sizeOfarray);
            int indexOfemptyChars = uartStr.IndexOf('\0');
            if (indexOfemptyChars > 0)
            {
                uartStr = uartStr.Remove(indexOfemptyChars, sizeOfarray - indexOfemptyChars);

            }

            return uartStr.ToCharArray();
        }

        public string RemoveTrailingZeros(string strToRemove)
        {

            int indexOfemptyChars = strToRemove.IndexOf('\0');
            if (indexOfemptyChars > 0)
            {
                strToRemove = strToRemove.Remove(indexOfemptyChars, strToRemove.Length - indexOfemptyChars);

            }

            return strToRemove;
        }

        public void WriteLine(string text)
        {
            ComPort.WriteLine(text);
        }
    }
}
