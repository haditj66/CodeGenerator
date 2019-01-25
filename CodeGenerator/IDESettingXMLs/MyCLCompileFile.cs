using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.IDESettingXMLs
{
    public class MyCLCompileFile
    {
        public MyCLCompileFile(MyFilter filterIBelongTo, string name, string locationOfFile = "")
        {
            FilterIBelongTo = filterIBelongTo;
            Name = name;
            LocationOfFile = locationOfFile;
        }

        public MyFilter FilterIBelongTo { get; set; }
        public string Name { get; set; }
        public string LocationOfFile { get; set; }
        public string FullFilterName
        {
            get
            {
                string ffn = FilterIBelongTo.GetFullAddress() == "" ?
                Name :
                FilterIBelongTo.GetFullAddress() + "\\" + Name;
                return ffn;
            }
        }
        public string FullLocationName
        {
            get
            {
                string fln = LocationOfFile == "" ?
                Name :
                LocationOfFile + "\\" + Name;
                return fln;
            }
        }
    }
}
