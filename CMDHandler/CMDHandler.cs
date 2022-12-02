using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

public class CMDHandler
{

    public string Output;
    public string Error;

    protected ProcessStartInfo processInfo { get; set; }
    protected Process process { get; set; }

    public CMDHandler(string StartingWorkingDirtory)
    {
        processInfo = new ProcessStartInfo("cmd.exe");
        SetWorkingDirectory(StartingWorkingDirtory);


    }


    public async void ExecuteCommand(string command, bool SupressErrorMsg = false)
    {

        Output = "";
        Error = "";

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
        //GetEnvironmentVariables(processInfo);
        //processInfo = new ProcessStartInfo(@"""C:\Program Files (x86)\Microsoft Visual Studio 14.0\VC\vcvarsall.bat"" x86", "/c " + "cl");
        //processInfo.WorkingDirectory = @"C:\Program Files (x86)\Microsoft Visual Studio 14.0\VC";
        //processInfo.WorkingDirectory = @"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\Module1A";
        //processInfo.WorkingDirectory =@"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\CodeGenerator\bin\Debug";
        processInfo.CreateNoWindow = true;
        processInfo.UseShellExecute = true;
        // *** Redirect the output ***
        processInfo.RedirectStandardError = false;
        processInfo.RedirectStandardOutput = false;

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
        if (false)//(!SupressErrorMsg)
        {
            var task1 = Task.Run(() => GetAndWaitOutput());
            
            int thisManyTries = 0;
            while (Output == "")
            { 
                Thread.Sleep(300);
                if (thisManyTries >= 25)
                {
                    break;
                }
                thisManyTries++;
            } 

        }
        else
        {
            Output = "";
            Error = "";
        }
        process.WaitForExit();

        // *** Read the streams ***
        // Warning: This approach can lead to deadlocks, see Edit #2
        //Output = process.StandardOutput.ReadToEnd();




        while (Output == null)
        {

        }

        exitCode = process.ExitCode;
#if DEBUG
        //Console.WriteLine("output>>" + (String.IsNullOrEmpty(Output) ? "(none)" : Output));
        //Console.WriteLine("error>>" + (String.IsNullOrEmpty(Error) ? "(none)" : Error));
        //Console.WriteLine("ExitCode: " + exitCode.ToString(), "ExecuteCommand");
#endif
        process.Close();
    }


    public async Task GetAndWaitOutput()
    {
        Output = process.StandardOutput.ReadToEnd();
        Error = process.StandardError.ReadToEnd();

        //return Task.CompletedTask.IsCompleted;

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
