using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGenerator.IDESettingXMLs;

namespace CodeGenerator.CMD_Handler
{
    class CLCommandBuilderForConfigTest : CLCommandBuilder
    {
        public CLCommandBuilderForConfigTest(string name, string PathOfConfigTest, params MyCLCompileFile[] filesCComp) : base(name, filesCComp)
        {
            //add additional includes needed for configtest
            AdditionalIncludes.Add(PathOfConfigTest);

            //add .lib for configTest
            MyStaticLib lib = new MyStaticLib("ConfigTest.lib" , PathOfConfigTest);

        }
    }
}
