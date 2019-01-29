using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.IDESettingXMLs
{
    public class MyCLIncludeFile
    {
        public MyCLIncludeFile(MyFilter filterIBelongTo, string name, string locationOfFile = "")
        {
            //assert that the locationOfFile does not have a ':' as this would mean a global path
            Debug.Assert(!locationOfFile.Contains(":"), "locationOfFile must be relative to the library's base directory and not a global directory");


            FilterIBelongTo = filterIBelongTo;
            //if name does not have a .h, include it
            if (Path.GetExtension(name) == "")
            {
                name += ".h";
            }
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
        //public string FullName { get { return FilterIBelongTo.GetFullAddress() + Name + ".h"; } }
    }
}



namespace ExtensionMethods
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CodeGenerator.IDESettingXMLs;

    public static class EXTCInc
    {

        public static MyCLIncludeFile GetCIncWithName(this List<MyCLIncludeFile> listinc, string nameOfCcompile)
        {
            return listinc.Where((MyCLIncludeFile ccomp) =>
            {
                return (ccomp.Name) == nameOfCcompile;
            }).First();
        }
    }
}

