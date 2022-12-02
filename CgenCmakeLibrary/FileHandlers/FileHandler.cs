using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace CgenCmakeLibrary.FileHandlers
{
    public class FileHandler
    {

        public DirectoryInfo DirectoryOfFile { get; protected set; }
        protected string FullFilePath;
        protected string FullFileName;

        public FileHandler(DirectoryInfo dir, string fileName)
        {
            _init(dir, fileName);
        }

        protected FileHandler()
        { 

        }

        protected void _init(DirectoryInfo dir, string fileName)
        {
            FullFileName = fileName;
            DirectoryOfFile = dir;
            FullFilePath = Path.Combine(DirectoryOfFile.FullName, fileName);

            //if the file does not exist, create it.
            if (File.Exists(FullFilePath) == false)
            {
                RemoveContents();
            }
        }

        public bool IsFileContentsFilled()
        {
            string contents = GetContents();
            return contents.Length > 0;
        }

        public string GetContents()
        {
            string ret = "";
            bool worked = false;
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    ret =  File.ReadAllText(FullFilePath);
                    worked = true;
                }
                catch (IOException e)
                {
                    Thread.Sleep(800);
                    worked = false;
                }

                if (worked)
                {
                    break;
                }
            }

            return ret;
        }

        public void RemoveContents()
        {

            bool worked = false;
            for (int i = 0; i < 5; i++)
            { 
                try
                {
                    File.WriteAllText(FullFilePath, "");
                    worked = true;
                }
                catch (IOException)
                {
                    Thread.Sleep(800);
                    worked = false; 
                }

                if (worked)
                {
                    break;
                }
            }
             
        }
    }
}
