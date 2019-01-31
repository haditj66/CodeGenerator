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

        public string Output;
        public string Error;

        protected ProcessStartInfo processInfo { get; set; }
        protected Process process { get; set; } 

        public CMDHandler(CMDTYPE CMDType, string StartingWorkingDirtory)
        {
            processInfo = new ProcessStartInfo("cmd.exe");
            SetWorkingDirectory(StartingWorkingDirtory);
            if (CMDType == CMDTYPE.VS)
            {
                //first check if a GetVSEnironmentVariables.bat file exists
                if (!File.Exists(Path.Combine(Program.DIRECTORYOFTHISCG, "GetVSEnironmentVariables.bat")))
                {
                    //also create environmental.txt whether it exists or not
                    File.Create(Path.Combine(Program.DIRECTORYOFTHISCG, "EnironVariables.txt"));

                    //if not, create it 
                    File.WriteAllText(Path.Combine(Program.DIRECTORYOFTHISCG, "GetVSEnironmentVariables.bat"), @"CALL ""C:\Program Files(x86)\Microsoft Visual Studio 14.0\VC\vcvarsall.bat"" x86");
                    File.AppendAllText(Path.Combine(Program.DIRECTORYOFTHISCG, "GetVSEnironmentVariables.bat"), "\n SET > " + Path.Combine(Program.DIRECTORYOFTHISCG, "EnironVariables.txt"));
                }

                //run that batch file 
                ExecuteCommand("GetVSEnironmentVariables.bat");

                //now get the environment variables that the batch file produced 
                using (StreamReader sr = new StreamReader( Path.Combine(Program.DIRECTORYOFTHISCG, "EnironVariables.txt")))
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


        public void ExecuteCommand(string command, bool SupressErrorMsg = false)
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
            Output = process.StandardOutput.ReadToEnd();
            Error = process.StandardError.ReadToEnd();
            if (SupressErrorMsg)
            {
                Output = "";
                Error = "";
            }
            

            exitCode = process.ExitCode;
#if DEBUG
            Console.WriteLine("output>>" + (String.IsNullOrEmpty(Output) ? "(none)" : Output));
            Console.WriteLine("error>>" + (String.IsNullOrEmpty(Error) ? "(none)" : Error));
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
