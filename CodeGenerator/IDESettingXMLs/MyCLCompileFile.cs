using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.IDESettingXMLs
{
    public class MyCLCompileFile
    {
        public MyCLCompileFile(MyFilter filterIBelongTo, string name, string locationOfFile = "")
        {
            //assert that the locationOfFile does not have a ':' as this would mean a global path
            Debug.Assert(!locationOfFile.Contains(":"), "locationOfFile must be relative to the library's base directory and not a global directory");

            FilterIBelongTo = filterIBelongTo;

            //if name does not have a .cpp, include it
            if (Path.GetExtension(name) == "")
            {
                name += ".cpp";
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

    public static class EXTCComp
    {

        public static MyCLCompileFile GetCCompileWithName(this List<MyCLCompileFile> listCcomp, string nameOfCcompile)
        {
            return listCcomp.Where((MyCLCompileFile ccomp) =>
            {
                return (ccomp.Name) == nameOfCcompile;
            }).First();
        }

        public static List<MyCLCompileFile> GetCCompilesFromFilter(this List<MyCLCompileFile> listCcomp, string filterFullAddress)
        {
            return listCcomp.Where((MyCLCompileFile ccomp) =>
            {
                return (ccomp.FilterIBelongTo.GetFullAddress()) == filterFullAddress;
            }).ToList();
        }

        public static List<MyCLCompileFile> GetCCompilesFromLocationOfFile(this List<MyCLCompileFile> listCcomp, string Location)
        {
            return listCcomp.Where((MyCLCompileFile ccomp) =>
            {
                return (ccomp.LocationOfFile) == Location;
            }).ToList();
        }
    }
}
