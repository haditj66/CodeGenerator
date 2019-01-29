using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.IDESettingXMLs
{
    public class MyStaticLib
    {
        public MyStaticLib(string name, string FullLocation)
        {
            Name = name;
            this.FullLocation = FullLocation;
        }

        public string Name { get; }
        public string FullLocation { get; }
    }
}
