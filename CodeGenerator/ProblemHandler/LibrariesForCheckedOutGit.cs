using System.Collections.Generic;
using CodeGenerator.CMD_Handler;

namespace CodeGenerator.ProblemHandler
{
    public class  LibrariesForCheckedOutGit : ICleanUp
    {
        protected CMDHandler Cmd { get; set; }

        protected List<Library> LibrariesCheckedOutSoFar { get; set; }

        public LibrariesForCheckedOutGit(CMDHandler cmd)
        {
            Cmd = cmd;
        }

        public void AddLibraryCheckedOutSoFar(Library libraryCheckedout)
        {
            LibrariesCheckedOutSoFar.Add(libraryCheckedout);
        }


        public void UncheckoutLibraryCheckedOutSoFar(Library libraryCheckedout)
        {
            UncheckoutLibrary(libraryCheckedout); 
            LibrariesCheckedOutSoFar.Remove(libraryCheckedout);
        }

        private void UncheckoutLibrary(Library libraryToUncheckout)
        {
            Cmd.SetWorkingDirectory(libraryToUncheckout.settings.PATHOfProject);
            Cmd.ExecuteCommand("git checkout -"); //go back to the commit I was on earlier
            Cmd.ExecuteCommand("git stash pop"); //pop the stash back in
        }

        public void CleanUp()
        { 
            foreach (var library in LibrariesCheckedOutSoFar)
            {
                UncheckoutLibrary(library); 
            }
        }
    }
}