using System.IO;
using System.Text.RegularExpressions;

namespace CodeGenerator.CMD_Handler
{
    public class CMDHandlerVSDev : CMDHandler
    {
        public CMDHandlerVSDev(string StartingWorkingDirtory, string DirectoryToPutBatchFile) : base(StartingWorkingDirtory)
        {

            //first check if a GetVSEnironmentVariables.bat file exists
            if (!File.Exists(Path.Combine(DirectoryToPutBatchFile, "GetVSEnironmentVariables.bat")))
            {
                //also create environmental.txt whether it exists or not
                //File.Create(Path.Combine(DirectoryToPutBatchFile, "EnironVariables.txt"));

                //if not, create it 
                File.WriteAllText(Path.Combine(DirectoryToPutBatchFile, "GetVSEnironmentVariables.bat"), @"CALL ""C:\Program Files (x86)\Microsoft Visual Studio 14.0\VC\vcvarsall.bat"" x86");
                File.AppendAllText(Path.Combine(DirectoryToPutBatchFile, "GetVSEnironmentVariables.bat"), "\n SET > " + Path.Combine(DirectoryToPutBatchFile, "EnironVariables.txt"));
            }
            /*
            //first check if a GetVSEnironmentVariables.bat file exists
            if (!File.Exists(Path.Combine(DirectoryToPutBatchFile, "GetVSEnironmentVariables.bat")))
            {
                //if not, create it
                File.WriteAllText(Path.Combine(DirectoryToPutBatchFile, "GetVSEnironmentVariables.bat"), @"CALL ""C:\Program Files(x86)\Microsoft Visual Studio 14.0\VC\vcvarsall.bat"" x86 \n SET > EnironVariables.txt");
            }*/

            //run that batch file 
            ExecuteCommand("GetVSEnironmentVariables.bat");

            //now get the environment variables that the batch file produced 
            using (StreamReader sr = new StreamReader(Path.Combine(DirectoryToPutBatchFile, "EnironVariables.txt")))
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
}