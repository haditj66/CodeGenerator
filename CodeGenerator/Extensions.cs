using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using CgenMin.MacroProcesses;
using CodeGenerator.CMD_Handler;
using CodeGenerator.ProblemHandler;

namespace Extensions
{
    public static class restOfExtensions
    {

        public static string ToString(this PinEnum pinEnum)
        {
            string ret = "";

            switch (pinEnum)
            {
                case PinEnum.PIN0:
                    return "PIN0";
                    break;
                case PinEnum.PIN1:
                    return "PIN1";
                    break;
                case PinEnum.PIN2:
                    return "PIN2";
                    break;
                case PinEnum.PIN3:
                    return "PIN3";
                    break;
                case PinEnum.PIN4:
                    return "PIN4";
                    break;
                case PinEnum.PIN5:
                    return "PIN5";
                    break;
                case PinEnum.PIN6:
                    return "PIN6";
                    break;
                case PinEnum.PIN7:
                    return "PIN7";
                    break;
                case PinEnum.PIN8:
                    return "PIN8";
                    break;
                case PinEnum.PIN9:
                    return "PIN9";
                    break;
                case PinEnum.PIN10:
                    return "PIN10";
                    break;
                case PinEnum.PIN11:
                    return "PIN11";
                    break;
                case PinEnum.PIN12:
                    return "PIN12";
                    break;
                case PinEnum.PIN13:
                    return "PIN13";
                    break;
                case PinEnum.PIN14:
                    return "PIN14";
                    break;
                case PinEnum.PIN15:
                    return "PIN15";
                    break;
                case PinEnum.PINMaxNum:
                    break;
                default:
                    break;
            }

            return ret;
        }

        public static string ToString(this Portenum pinEnum)
        {
            string ret = "";

            switch (pinEnum)
            {
                case Portenum.PortA:
                    return "PortA";
                    break;
                case Portenum.PortB:
                    return "PortB";
                    break;
                case Portenum.PortC:
                    return "PortC";
                    break;
                case Portenum.PortD:
                    return "PortD";
                    break;
                case Portenum.PortE:
                    return "PortE";
                    break;
                case Portenum.PortF:
                    return "PortF";
                    break;
                case Portenum.PortG:
                    return "PortG";
                    break;
                case Portenum.PortH:
                    return "PortH";
                    break;
                case Portenum.PortI:
                    return "PortI";
                    break;
                case Portenum.PortsMaxNum:
                    break;
                default:
                    break;
            }

            return ret;
        }


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
