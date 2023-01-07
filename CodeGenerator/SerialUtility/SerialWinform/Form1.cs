using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports; 

   

namespace WindowsFormsApplication1
{


    public partial class Form1 : Form
    {

        byte[][] buffers = new byte[10000][];

        SerialPort ComPort;
        string name = "";

        public Form1()
        {
            InitializeComponent();
            button1.Text = "Send To File";

            ComPort = new SerialPort("COM4");
            
            ComPort.BaudRate = 115200;
            ComPort.Parity = Parity.None;
            ComPort.StopBits = StopBits.One;
            ComPort.DataBits = 8;

            ComPort.Open();
            ComPort.DataReceived += ComPort_DataReceived;

            for (int i = 0; i < 10000; i++)
            {
                buffers[i] = new byte[1000];
            }

            

            //Task ds = new Task(Bla);
            //ds.Start();
        }

        /*int t = 0;
        private void  Bla()
        {
            while (true)
            {
                t++; 
            } 
        }*/


        byte[] buffer = new byte[10000];
        int readSoFar = 0;
        int readLastTime= 0;
        int numOfMsgs = 0;
        int numofBytesToRead;

        public void ComPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        { 
            readSoFar = 0;
            //if (e.EventType != System.IO.Ports.SerialData.Chars) return;
            numofBytesToRead = ComPort.BytesToRead;
            //var d = new ArraySegment<byte>(buffer, readLastTime, readLastTime + numofBytes).ToArray();
            //while ((readSoFar += ComPort.Read(buffers[numOfMsgs], readSoFar, numofBytes - readSoFar)) != numofBytes) ;
            //while ((readSoFar += ComPort.Read( d, readSoFar, numofBytes - readSoFar)) != numofBytes) ;
            while ((readSoFar += ComPort.Read(buffer, readSoFar, numofBytesToRead - readSoFar)) != numofBytesToRead) ; 
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
            

            richTextBox1.Invoke(new MethodInvoker(delegate { richTextBox1.Text+= uartFloatMsg; }));
            


            //this.Invoke(new MethodInvoker(delegate {  }));
            //richTextBox1.Text = System.Text.Encoding.UTF8.GetString(buffer) + u;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //write all to the txt file. 
            System.IO.File.WriteAllText("C:\\Users\\Hadi\\OneDrive\\Documents\\VisualStudioprojects\\Projects\\cSharp\\Serial stuff\\serial try winform\\testWrite.txt", richTextBox1.Text);

            //clear the text that I write to the file.
            richTextBox1.Text = "";

            //richTextBox1.Invoke(new MethodInvoker(delegate { name = richTextBox1.Text; }));
            /*
            int uu6 = ComPort.BytesToRead;
            while ((readSoFar += ComPort.Read(buffer, readSoFar, buffer.Length - readSoFar)) != buffer.Length) ;
             
           string u = ComPort.ReadExisting();//this is so that it can read all the rest that needs to be read
            int uu = ComPort.BytesToRead;
            richTextBox1.Text = System.Text.Encoding.UTF8.GetString(buffer) + u;
            */
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ComPort.WriteLine( writeTextBox1.Text);
            writeTextBox1.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
