


using Microsoft.VisualStudio.TestTools.UnitTesting; 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    [TestClass]
    public class Test
    {

        [TestMethod]
        public void bla()
        {
            var file = new FileInfo(Path.Combine(@"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\CodeGenerator\ConsoleApp2\bin\Debug", "test2" + ".h"));


        }

 
    }
}
