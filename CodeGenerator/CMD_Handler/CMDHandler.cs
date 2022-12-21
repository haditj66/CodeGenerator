using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace CodeGenerator.CMD_Handler
{
    public class CMDHandler
    {

        public string Output;
        public string Error;

        protected ProcessStartInfo processInfo { get; set; }
        protected Process process { get; set; }

        protected List<string> MultipleCommands;

        public CMDHandler(string StartingWorkingDirtory)
        {
            processInfo = new ProcessStartInfo("cmd.exe");
            SetWorkingDirectory(StartingWorkingDirtory);

            MultipleCommands = new List<string>();


        }


        public void SetMultipleCommands(string command)
        {
            MultipleCommands.Add(command);
        }



        public void ExecuteMultipleCommands_InSeperateProcess()
        {
            Process p = ExecuteMultipleCommands(true, false, "", "", false);
            p.WaitForExit();
        }

        public void ExecuteMultipleCommands(bool SupressErrorMsg = false)
        {
            ExecuteMultipleCommands(false, false, "", "", SupressErrorMsg);
        }


        public void ExecuteMultipleCommands_InItsOwnBatch(string toBatchFileDir, string nameOfBatch)
        {
            ExecuteMultipleCommands(false, true, toBatchFileDir, nameOfBatch);
        }


        private Process ExecuteMultipleCommands(bool RunSeperateProcess = false, bool justCreateBatchFile = false, string toBatchFileDir = "", string nameOfBatch = "", bool SupressErrorMsg = false)
        {
            string NameOfBatch;
            string pathToBatch;
            if (justCreateBatchFile)
            {
                NameOfBatch = nameOfBatch + ".bat";
                pathToBatch = toBatchFileDir + @"\" + NameOfBatch;
            }
            else
            {
                NameOfBatch = "batch_" + this.GetHashCode() + ".bat";
                pathToBatch = @"C:\CodeGenerator\CodeGenerator\bin\Debug\" + NameOfBatch;
            }




            //File.Create(pathToBatch);


            bool keeptrying = true;
            while (keeptrying)
            {
                try
                {
                    File.WriteAllText(pathToBatch, "");
                    //send commands to a batch file that will run
                    File.WriteAllLines(pathToBatch, MultipleCommands);

                    keeptrying = false;
                }
                catch (Exception e)
                {

                }
            }


            //get the old workingdirectory
            string oldwd = processInfo.WorkingDirectory;

            //change to the directory where the batch file is in
            //SetWorkingDirectory(".");

            //execute the bash file
            Process p = null;
            if (RunSeperateProcess == true)
            {
                p = System.Diagnostics.Process.Start(@"C:\CodeGenerator\CodeGenerator\bin\Debug\" + "batch_" + this.GetHashCode() + ".bat");

            }
            else if (justCreateBatchFile == false)
            {
                ExecuteCommand(@"call C:\CodeGenerator\CodeGenerator\bin\Debug\" + "batch_" + this.GetHashCode());
            }

            MultipleCommands.Clear();

            return p;
            //change back to the previous working directory
            //SetWorkingDirectory(oldwd);
        }

        //public void ExecuteCommand(string command, bool SupressErrorMsg = false)
        //{
        //    _ExecuteCommand(command, SupressErrorMsg, false);
        //}
        //public void ExecuteCommand(string command, bool SupressErrorMsg = false)
        //{
        //    _ExecuteCommand(command, SupressErrorMsg, false);
        //}

        public TimeSpan GetTotalTimeSinceStarted()
        {
            return process.TotalProcessorTime;
        }
        public bool IsProcessFinished()
        {
            bool ret = false;

            try
            {
                ret  = process.HasExited; 
            }
            catch (System.InvalidOperationException e)
            {

                return true;
            }


            if (process == null)
            {
                return true;
            }

            return ret;
        }
        private void Process_Exited(object sender, EventArgs e)
        {
            process.Exited += Process_Exited;

            Output = process.StandardOutput.ReadToEnd();
            Error = process.StandardError.ReadToEnd();
            while (Output == null)
            {

            }
             

            process.Close();

        }

        public void KillProcess()
        {
            process.Kill();
            process.Close();
        }

        public void ExecuteCommand(string command, bool SupressErrorMsg = false, bool waitProcessToFinish = true)
        {
            //if this is a cd command, handle it differently
            if (Regex.IsMatch(command, @"^\s*cd\s*"))
            {
                //take out the cd part
                Regex.Replace(command, @"^\s*cd\s*", "");
                SetWorkingDirectory(command);
                return;
            }

            int exitCode;
            //processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
            processInfo.Arguments = "/c " + command;
             

            //processInfo.Arguments = "/c " + "oursource" + " /c " + "cd ../testmod" + " /c " + "ourcolcon";

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

            //string error = "";
            //string output = "";

            process = Process.Start(processInfo);
            
            /*
            process.OutputDataReceived += (sender, args) =>
            {
                string output = args.Data;
                // ...
            };
            process.ErrorDataReceived += (sender, args) =>
            {
                string error = args.Data;
                // ...
            };
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            */
            if (waitProcessToFinish)
            {
                if (!SupressErrorMsg)
                {
                    Output = process.StandardOutput.ReadToEnd();
                    Error = process.StandardError.ReadToEnd();
                }
                else
                {
                    Output = "";
                    Error = "";
                }
                process.WaitForExit();    // *** Read the streams ***
                                          // Warning: This approach can lead to deadlocks, see Edit #2
                                          //Output = process.StandardOutput.ReadToEnd();




                while (Output == null)
                {

                }

                exitCode = process.ExitCode;

                process.Close();
            }
            else
            {
                process.Exited += Process_Exited;
            }


#if DEBUG
            //Console.WriteLine("output>>" + (String.IsNullOrEmpty(Output) ? "(none)" : Output));
            //Console.WriteLine("error>>" + (String.IsNullOrEmpty(Error) ? "(none)" : Error));
            //Console.WriteLine("ExitCode: " + exitCode.ToString(), "ExecuteCommand");
#endif

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
