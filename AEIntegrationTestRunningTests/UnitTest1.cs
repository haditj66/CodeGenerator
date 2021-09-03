
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSHHandler;

namespace AEIntegrationTestRunningTests
{
    [TestClass]
    public class UnitTest1
    {

        string outTest1 = ""; 
        public void sshOutCallback(string blas) {
            outTest1 = blas;
        }

        [TestMethod]
        public void SSHTest()
        {

            using (SSH_Handler ssh = new SSH_Handler("~/Documents/tut_ws", "192.168.1.177", "ubuntu", "new11111"))
            { 
                ssh.OutputUpdated += sshOutCallback;
                ssh.AddCommand("echo hi");

                ssh.ExecuteAllAddedCommands();
             
                System.Threading.Thread.Sleep(2000);

                bool c = outTest1.Contains("hi");
                Assert.IsTrue(c);

                ssh.ClearCommand();
                ssh.AddCommand("echo bye");
                ssh.ExecuteAllAddedCommands();

                System.Threading.Thread.Sleep(2000);

                Assert.IsTrue(outTest1.Contains("bye"));
            }
        }
    }
}
