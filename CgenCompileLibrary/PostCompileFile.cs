using System;
using System.IO;

namespace CgenCompileLibrary
{
    public class PostCompileFile
    { 
        public FileInfo IntermediaryFile { get; private set; }
        public FileInfo SourceFile { get; private set; }

        public string[] SourceFileContents { get; private set; }

        private string[] _IntermediaryFileContents;
        public string[] IntermediaryFileContents
        {
            get
            {
                if (IsIntermediaryContentsSet == true)
                {
                    return _IntermediaryFileContents;
                }
                else
                {
                    _IntermediaryFileContents = File.ReadAllLines(IntermediaryFile.FullName);
                    IsIntermediaryContentsSet = true;
                    return _IntermediaryFileContents;
                }
            }
            private set {
                if (IsIntermediaryContentsSet == false)
                { 
                    _IntermediaryFileContents = File.ReadAllLines(IntermediaryFile.FullName);
                    IsIntermediaryContentsSet = true;
                } 

            }
        
        }
        public string FileName { get; private set; } 
        public bool IsIntermediaryContentsSet { get; protected set; } 

        public PostCompileFile(FileInfo intermediaryFile)
        {
            this.IsIntermediaryContentsSet = false;
            this.IntermediaryFile = intermediaryFile;

            //the first line of the intermediary file gives the relative path to the source file
            string firstline = ""; 
            using (StreamReader reader = new StreamReader(intermediaryFile.FullName))
            {
                firstline = reader.ReadLine() ?? "";
            }
            firstline = firstline.Substring(firstline.IndexOf("\"")+1, firstline.Length - firstline.IndexOf("\"")-2);
             
            string sourcePath = Path.GetFullPath(firstline, intermediaryFile.DirectoryName);
            SourceFile = new FileInfo(sourcePath);


            FileName = intermediaryFile.Name;



            SourceFileContents = File.ReadAllLines(SourceFile.FullName);
 

        }

    }
}
