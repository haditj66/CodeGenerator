using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using CodeGenerator.CMD_Handler;
using CodeGenerator.ProblemHandler;

namespace Extensions
{
    public static class restOfExtensions
    { 
         

        public static List<T> InitListIfNull<T>(this  List<T>  list)
        {
            if (list == null)
            {
                return list = new List<T>();
            }

            return list;
        }

        public static T DeepCopy<T>(T other)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, other);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }


        public static string SetEscapesOnString(this string strToEscapeEscapes)
        {
            List<char> cc = new List<char>();
            //create regex pattern by putting escapes
            string pat = "";
            foreach (char c in strToEscapeEscapes)
            {
                pat += c.ToString();
                if (c.ToString() == "\\")
                {
                    pat += "\\";
                }
            }

            return pat;
        }



        //directory ---------------------------------------------------------
        public static void CopyAllContentsInDirectory(string source_dir, string destination_dir)
        {
            //string source_dir = source_dir;//@"E:\";
            //string destination_dir = destination_dir;//@"C:\";

            // substring is to remove destination_dir absolute path (E:\).

            // Create subdirectory structure in destination    
            foreach (string dir in System.IO.Directory.GetDirectories(source_dir, "*", System.IO.SearchOption.AllDirectories))
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.Combine(destination_dir, dir.Substring(source_dir.Length + 1)));
                // Example:
                //     > C:\sources (and not C:\E:\sources)
            }

            foreach (string file_name in System.IO.Directory.GetFiles(source_dir, "*", System.IO.SearchOption.AllDirectories))
            {
                System.IO.File.Copy(file_name, System.IO.Path.Combine(destination_dir, file_name.Substring(source_dir.Length + 1)));
            }
        }


        //cmdhandler ---------------------------------------------------------

        public static void ExecuteCommandWithProblemCheck(this CMDHandler cmd, string command, ProblemHandle problemHandle, string problemMSG)
        {
            cmd.ExecuteCommand(command);
            if (cmd.Output.Contains(": error") || cmd.Output.Contains(": fatal") || cmd.Error.Contains(": error") || cmd.Error.Contains(": fatal"))
            {
                problemMSG = cmd.Output + "/n" + cmd.Error + "/n" + problemMSG;
                problemHandle.ThereisAProblem(problemMSG);
            }
        }
    }
}
