using ClassLibrary2;
using MatFileHandler;
using MyProtocolsForCommunications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mytest
{ 

    class Program
    {  
        
        private IProtocol protocol;
    private bool isUploading;
    private static MyUARTSerialPortWrapper Uart;
         
        static void Main(string[] args)
        {

            var builder = new DataBuilder();
            var array = builder.NewArray<double>(1, 2);
            array[0] = -14.5;
            array[1] = 347.0;
            var variable = builder.NewVariable("test", array);
            var actual = builder.NewFile(new[] { variable });

            using (var fileStream = new System.IO.FileStream("output22.mat", System.IO.FileMode.Create))
            {
                var writer = new MatFileWriter(fileStream);
                writer.Write(actual);
            }

        }
    }
}
