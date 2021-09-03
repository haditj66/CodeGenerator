using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SSHHandler
{
    public class SSH_Handler : IDisposable
    {
        public string WorkingDirectory { get; }

        public string RemoteIPAddress { get; }
        public string RemoteUserName { get; }
        public string RemotePassword { get; }

        private string _enironmentOfCommands;


        private List<string> EnvironmentsCmd;
        private List<string> Cmds;
        private CancellationTokenSource ts;
        private CancellationToken ct;
        private ConnectionInfo connectionInfo;
        SshClient Client;

        private SshCommand _RunningCmdHandler;
        private IAsyncResult _AsyncExecute;
        private string _ShellOutput;
        private MemoryStream memoryStream;
        private Task T1;

        public delegate void OutputUpdated_t(string output);
        public event OutputUpdated_t OutputUpdated;

        public SSH_Handler(string workingDirectory, string remoteIPAddress, string remoteUserName, string remotePassword)
        {
            WorkingDirectory = workingDirectory;
            RemoteIPAddress = remoteIPAddress;
            RemoteUserName = remoteUserName;
            RemotePassword = remotePassword;
            _ShellOutput = "";

            connectionInfo = new ConnectionInfo(remoteIPAddress,
                                        remoteUserName,
                                        new PasswordAuthenticationMethod(remoteUserName, remotePassword)
                                        //, new PrivateKeyAuthenticationMethod("rsa.key")
                                        );
            memoryStream = new MemoryStream();
            EnvironmentsCmd = new List<string>();
            Cmds = new List<string>();


            ts = new CancellationTokenSource();
            ct = ts.Token;
            T1 = Task.Factory.StartNew(() =>
            {

                while (true)
                {
                    System.Threading.Thread.Sleep(300);

                    string newOut; 

                    memoryStream.Position = 0;
                    var reader = new StreamReader(memoryStream);
                    
                        newOut = reader.ReadToEnd();
                    

                    if (newOut != _ShellOutput)
                    {
                        _ShellOutput = newOut;
                        OutputUpdated.Invoke(_ShellOutput);
                    }

                    if (ct.IsCancellationRequested)
                    { 
                        break;
                    }
                }

            });
            


            Client = new SshClient(connectionInfo);

            Client.Connect();

            //Client = new SshClient(connectionInfo);
        }



        private string GetEnironmentOfCommands()
        {
            _enironmentOfCommands = "cd " + WorkingDirectory + "";

            foreach (var envcmd in EnvironmentsCmd)
            {
                _enironmentOfCommands += " && " + envcmd;
            }

            return _enironmentOfCommands;
        }

        private string GetSSHCommand()
        {
            string sshcmd = GetEnironmentOfCommands();

            foreach (var cmd in Cmds)
            {
                sshcmd += " && " + cmd;
            }

            return sshcmd;
        }

        public void AddENVCommand(string cmd)
        {
            EnvironmentsCmd.Add(cmd);
        }

        public void ClearENVCommand()
        {
            EnvironmentsCmd.Clear();
        }
        public void AddCommand(string cmd)
        {
            Cmds.Add(cmd);
        }
        public void ClearCommand()
        {
            Cmds.Clear();
        }



        public void ExecuteAllAddedCommands()
        {
           

            _RunningCmdHandler = Client.CreateCommand(GetSSHCommand());
            _AsyncExecute = _RunningCmdHandler.BeginExecute();
            _RunningCmdHandler.OutputStream.CopyTo(memoryStream);// (Console.OpenStandardOutput()); 
        }

        public void CancelPreviousCommand()
        {
            _RunningCmdHandler.CancelAsync();
            _RunningCmdHandler.EndExecute(_AsyncExecute);
        }

        public void Dispose()
        {
            ts.Cancel();
            T1.Wait();
            T1.Dispose();
            Client.Disconnect();
            Client.Dispose();
            memoryStream.Dispose();
        }
    }
}
