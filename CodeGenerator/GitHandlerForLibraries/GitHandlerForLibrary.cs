using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using CodeGenerator.CMD_Handler;
using CodeGenerator.ProblemHandler;
using Extensions;

namespace CodeGenerator.GitHandlerForLibraries
{
    public class GitHandlerForLibrary
    {
        public CMDHandler Cmd { get; }

        public GitHandlerForLibrary(CMDHandler cmd)
        {
            Cmd = cmd;
        }


        public bool IsPathHaveGit(string path)
        {
            Cmd.SetWorkingDirectory(path);
            Cmd.ExecuteCommand("git rev-parse --git-dir"); //this will check if .git exists here. if so, it should read ".git" exactly
            Cmd.Output = Cmd.Output.Replace("\n", "");
            Cmd.Output = Cmd.Output.Trim();
            if (Cmd.Output != ".git")
            {
                if (Directory.Exists(Path.Combine(path,".git")))
                {
                    throw new Exception("Im not sure anymore");
                }
                return false;
            }

            return true;
        }


        public bool DoesLibraryContainsGitRepoAndTagForMajor(string pathOfLibrary, string libraryMajor, out string tag)
        {

            if (!IsPathHaveGit(pathOfLibrary))
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
                    if (Regex.Match(line.ToLower(), @"v(\d+)\.\d+\.\d+").Groups[1].Value == libraryMajor)
                    {
                        tag = line;
                        return true;
                    }
                }
            }

            tag = "";
            return false;
        }

        public bool DoesLibraryContainsGitRepoAndTagForMajor(Library Library, out string tag)
        {
            return DoesLibraryContainsGitRepoAndTagForMajor(Library.settings.PATHOfProject, Library.config.Major, out tag);
             
        }


        public void StashAndCheckoutTag(Library Library, string tagToCheckoutTo, ProblemHandle problemHandler)
        {
            //get to that working directory
            Cmd.SetWorkingDirectory((Library.settings.PATHOfProject));
            //stash all changes
            Cmd.ExecuteCommandWithProblemCheck("git stash", problemHandler, @"could not ""git stash"" current changes to library " + Library.config.ClassName + ". Resolve these issues Git in that library before trying again");
 

            //checkout that tag
            Cmd.ExecuteCommand("git checkout " + tagToCheckoutTo);
            Cmd.ExecuteCommandWithProblemCheck("git checkout " + tagToCheckoutTo, problemHandler, @"could not ""git checkout -"" for library " + Library.config.ClassName + ". the changes that were stashed as well. you need to check that library and checkout - and restore any stashed changes ");
        }

        public void InitGitHere(string envIronDirectory)
        {
            Cmd.SetWorkingDirectory(envIronDirectory);
            Cmd.ExecuteCommand("git init");
        }

        public void CommitAll(string envIronDirectory, bool suppressErrorMsg)
        {
            Cmd.SetWorkingDirectory(envIronDirectory);
            //Cmd.ExecuteCommand("git add .gitignore .gitattributes");
            //Cmd.ExecuteCommand(@"git commit -m ""added gitignore""");
            //Cmd.ExecuteCommand("git add --all");
            Cmd.ExecuteCommand(@"git add -A && git commit -m ""added all files""", suppressErrorMsg);

        }

        public void AddVersionTag(string envIronDirectory, int major, int minor, int patch, string msg = "")
        {
            Cmd.SetWorkingDirectory(envIronDirectory);

            Cmd.ExecuteCommand(@"git tag v" + major.ToString() + @"." + minor.ToString() + @"." + patch.ToString() + @" -m """ + msg + @""" ");
        }
    }
}
