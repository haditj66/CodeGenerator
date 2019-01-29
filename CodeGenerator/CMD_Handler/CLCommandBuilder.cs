using CodeGenerator.IDESettingXMLs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.CMD_Handler
{
    public class CLCommandBuilder
    {
        public List<string> AdditionalIncludes { get; set; }
        public List<MyStaticLib> Libs { get; set; }
        public List<MyCLCompileFile> FilesCComp { get; set; }
        public string OutputLocation { get; set; }
        public string Name { get; set; }

        public CLCommandBuilder(string name, params MyCLCompileFile[] filesCComp)
        {
            Name = name;
            AdditionalIncludes = new List<string>();
            Libs = new List<MyStaticLib>();
            FilesCComp = new List<MyCLCompileFile>();
            foreach (var ccomp in filesCComp)
            {
                FilesCComp.Add(ccomp);
            } 
            OutputLocation = "";
        }

        public string GetCompileCommand()
        {
            string command = @"cl    /EHsc /MDd";

            command += " ";

           //set the output for .obj files
           command += @"/Fo""" + OutputLocation + @"""\";

            command += " ";

            //set additional include 
            foreach (var additoinalInclude in AdditionalIncludes)
            {
                command += @"/I " + @""""+additoinalInclude+@"""" + " ";
            }

            //put all .cpp files involved in this compile
            foreach (var ccomp in FilesCComp)
            {
                command += ccomp.FullLocationName + " ";
            }

            //set library .lib links
            foreach (var lib in Libs)
            {
                command += @"/link " + lib.Name + " ";
            }


            //set lib paths
            foreach (var lib in Libs)
            {
                command += @"/libpath:"+@""""+ lib.FullLocation+@"""" + " ";
            }


            //set output
            string fileFullPath = Path.Combine(OutputLocation,Name);
            fileFullPath =Path.ChangeExtension(fileFullPath, ".exe");
            command += @"/out:" + fileFullPath;


            return command ;
        }
    }
}
