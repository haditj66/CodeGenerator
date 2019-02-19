using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CodeGenerator.CMD_Handler;
using CodeGenerator.ProblemHandler;

namespace CodeGenerator.GitHandlerForLibraries
{
    public class GitHandlerForLibrary
    {
        public CMDHandler Cmd { get; }

        public GitHandlerForLibrary(CMDHandler cmd)
        {
            Cmd = cmd;
        }


        public bool DoesLibraryContainsGitRepoAndTagForMajor(Library Library, out string tag)
        {

            Cmd.SetWorkingDirectory(Library.settings.PATHOfProject);
            Cmd.ExecuteCommand("git rev-parse --git-dir"); //this will check if .git exists here. if so, it should read ".git" exactly
            Cmd.Output = Cmd.Output.Replace("\n", "");
            Cmd.Output = Cmd.Output.Trim();
            if (Cmd.Output != ".git")
            {
                tag = "";
                return false;
            }
            //get all tags in this git. make sure you are in master branch first
            //Cmd.ExecuteCommand("git stash");
            //Cmd.ExecuteCommand("git checkout master");
            Cmd.ExecuteCommand("git tag");
            //go through the output and look for a tag that has a v\d.\d.\d pattern 
            List<string> lines = Cmd.Output.Split('\n').ToList();
            lines.Reverse();
            foreach (var line in lines)
            {
                if (Regex.IsMatch(line, @"v\d+\.\d+\.\d+"))
                {
                    //check if the major matches the major of this tag
                    if (Regex.Match(line.ToLower(), @"v(\d+)\.\d+\.\d+").Groups[1].Value == Library.config.Major)
                    {
                        tag = line;
                        return true;
                    }
                }
            } 

            tag = "";
            return false;
        }

        public void StashAndCheckoutTag(Library Library, string tagToCheckoutTo, ProblemHandle problemHandler)
        {
            //get to that working directory
            Cmd.SetWorkingDirectory((Library.settings.PATHOfProject));
            //stash all changes
            Cmd.ExecuteCommand("git stash");
            if (Cmd.Error.Contains("error:") || Cmd.Error.Contains("fatal:"))
            {
                problemHandler.ThereisAProblem(@"could not ""git stash"" current changes to library " + Library.config.ClassName + ". Resolve these issues Git in that library before trying again");
            }

            //checkout that tag
            Cmd.ExecuteCommand("git checkout " + tagToCheckoutTo);

        }
    }
}
