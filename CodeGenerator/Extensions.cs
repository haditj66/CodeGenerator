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
