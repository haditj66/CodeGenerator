using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeGenerator.CMD_Handler
{

    public enum CMDTYPE
    {
        Normal,
        VS
    }
    public class CMDHandler
    {

        protected ProcessStartInfo processInfo { get; set; }
        protected Process process { get; set; } 

        public CMDHandler(CMDTYPE CMDType)
        {
            processInfo = new ProcessStartInfo("cmd.exe");

            if (CMDType == CMDTYPE.VS)
            {
                //first check if a GetVSEnironmentVariables.bat file exists
                if (!File.Exists("GetVSEnironmentVariables.bat"))
                {
                    //if not, create it
                    File.WriteAllText("GetVSEnironmentVariables.bat", @"CALL ""C:\Program Files(x86)\Microsoft Visual Studio 14.0\VC\vcvarsall.bat"" x86 \n SET > EnironVariables.txt");
                }

                //run that batch file
                ExecuteCommand("GetVSEnironmentVariables.bat");

                //now get the environment variables that the batch file produced 
                using (StreamReader sr = new StreamReader("EnironVariables.txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        //grab the thing before the = and after    key = value
                        Match match = Regex.Match(line, @"([\w\d()]*)=(.*)");
                        string key = match.Groups[1].Value;
                        string value = match.Groups[2].Value;

                        //check if key already exists
                        if (processInfo.Environment.ContainsKey(key))
                        {
                            //than replace the key's value
                            processInfo.Environment[key] = value;
                            //processInfo.EnvironmentVariables[key] = value;
                        }
                        else
                        {
                            //else add the key value
                            processInfo.Environment.Add(key, value);
                            //processInfo.EnvironmentVariables.Add(key, value);
                        }

                    }
                }
            }

        } 


        public void ExecuteCommand(string command)
        {
            //if this is a cd command, handle it differently
            if (Regex.IsMatch(command, @"^\s*cd\s*"))
            {
                //take out the cd part
                Regex.Replace(command, @"^\s*cd\s*","");
                SetWorkingDirectory(command);
                return;
            }

            int exitCode;
            //processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
            processInfo.Arguments = "/c " + command;
            //GetEnvironmentVariables(processInfo);
            //processInfo = new ProcessStartInfo(@"""C:\Program Files (x86)\Microsoft Visual Studio 14.0\VC\vcvarsall.bat"" x86", "/c " + "cl");
            //processInfo.WorkingDirectory = @"C:\Program Files (x86)\Microsoft Visual Studio 14.0\VC";
            //processInfo.WorkingDirectory = @"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\Module1A";
            //processInfo.WorkingDirectory =@"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\CodeGenerator\bin\Debug";
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            // *** Redirect the output ***
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            process = Process.Start(processInfo);
            process.WaitForExit();

            // *** Read the streams ***
            // Warning: This approach can lead to deadlocks, see Edit #2
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            exitCode = process.ExitCode;
#if DEBUG
            Console.WriteLine("output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output));
            Console.WriteLine("error>>" + (String.IsNullOrEmpty(error) ? "(none)" : error));
            Console.WriteLine("ExitCode: " + exitCode.ToString(), "ExecuteCommand");
#endif
            process.Close();
        }

        public void SetWorkingDirectory(string workingDirectory)
        {
            //check if it contains ":" as this would mean to replace a global directory
            if (workingDirectory.Contains(":"))
            {
                processInfo.WorkingDirectory = workingDirectory;
            }
            else
            {
                //it must be a relative path, add to current path
                if (workingDirectory == "cd ..")
                {
                    processInfo.WorkingDirectory = Directory.GetParent(workingDirectory).FullName;
                }
                else
                {
                    processInfo.WorkingDirectory = Path.Combine(processInfo.WorkingDirectory, workingDirectory);
                }
            }
            
        }
    }  
}
