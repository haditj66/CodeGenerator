using System.IO;
using ClangSharp;

namespace ConsoleApp2.MyClangWrapperClasses
{
    public class MyCXFile
    {
        public CXFile CxFile { get; }

        public string FileName { get; private set; }
        public string FileFullName { get; private set; }

        public MyCXFile(CXFile CXFile)
        {
            CxFile = CXFile;

            //----get file location
            FileFullName = clang.getFileName(CXFile).ToString();
            FileName = Path.GetFileName(FileFullName);
        }

        public MyCXFile(CXFile CXFile, string fileFullName)
        {
            CxFile = CXFile;

            //----get file location
            FileFullName = fileFullName;
            FileName = Path.GetFileName(FileFullName);
        }

    }
}