using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace WindowsFormsApplication1
{


    class UartLibrary
    {

        SerialPort ComPort;

        protected byte[] buffer = new byte[10000];
        protected int readSoFar = 0;
        protected int readLastTime = 0;
        protected int numOfMsgs = 0;
        protected int numofBytes;


        public UartLibrary()
        {

            ComPort = new SerialPort("COM4");

            ComPort.BaudRate = 115200;
            ComPort.Parity = Parity.None;
            ComPort.StopBits = StopBits.One;
            ComPort.DataBits = 8;

            ComPort.Open();
            ComPort.DataReceived += ComPort_DataReceived;
        }




        public void ComPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            readSoFar = 0;
            //if (e.EventType != System.IO.Ports.SerialData.Chars) return;
            numofBytes = ComPort.BytesToRead;
            //var d = new ArraySegment<byte>(buffer, readLastTime, readLastTime + numofBytes).ToArray();
            //while ((readSoFar += ComPort.Read(buffers[numOfMsgs], readSoFar, numofBytes - readSoFar)) != numofBytes) ;
            //while ((readSoFar += ComPort.Read( d, readSoFar, numofBytes - readSoFar)) != numofBytes) ;
            while ((readSoFar += ComPort.Read(buffer, readSoFar, numofBytes - readSoFar)) != numofBytes) ;
            //while ((readSoFar += ComPort.Read(new ArraySegment<byte>(buffer, readLastTime, readLastTime+ numofBytes).ToArray(), readSoFar, numofBytes - readSoFar)) != numofBytes) ;
            numOfMsgs++;
            //readLastTime += numofBytes;

            //string theUartMSG = ComPort.ReadLine();//this can read all but for strings ONLY.

            //make sure that the theUartMSG does not have \0 
            //numofBytes = theUartMSG.Where<char>(x => x != '\0').Count<char>();

            /*
            //convert that msg to a bytes 
            byte[] uartBytes = Encoding.ASCII.GetBytes(theUartMSG);
            */

            /*
            List<float> uartFloatArray = new List<float>(); 
            //convert that bytes to an array of floats
            numofBytes = (int)(numofBytes / 4);  
            for (int i = 0; i < numofBytes; i ++)
            { 
                uartFloatArray.Add((char)BitConverter.ToSingle(buffer2, i*4));
            }
            */




            //convert   bytes to an array of chars
            //List<char> uartCharArray = new List<char>();
            char[] uartCharArray = System.Text.Encoding.UTF8.GetString(buffer).ToCharArray();

            //convert the float array to a string
            string uartFloatMsg = new string(uartCharArray);//.Join(" ", buffer); 


            //richTextBox1.Invoke(new MethodInvoker(delegate { richTextBox1.Text += uartFloatMsg; }));



            //this.Invoke(new MethodInvoker(delegate {  }));
            //richTextBox1.Text = System.Text.Encoding.UTF8.GetString(buffer) + u;
        }



    }
}
