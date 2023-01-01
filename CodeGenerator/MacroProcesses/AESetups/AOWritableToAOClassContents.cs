﻿using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace CgenMin.MacroProcesses
{

    public interface IWriteAOClassContents
    {
        string ProjectDirectory { get; set; }

        void WriteTheContentedToFiles();
    }

    public abstract class AOWritableToAOClassContents : AO, IWriteAOClassContents , IPartOfAEDefines
    {


        public static List<AOWritableToAOClassContents> AllAOWritableToAOClassContents = new List<AOWritableToAOClassContents>();
        protected class RelativeDirPathWrite
        {
            public string FileNameWithExt { get; protected set; }
            public string FileExtension { get; protected set; }
            public string RelativePath { get; protected set; }
            public string ConentsToWrite { get; protected set; }
            public bool UseMacro1 { get; }

            public RelativeDirPathWrite(string fileNameWithExt, string fileExtension, string relativePath, string conentsToWrite, bool useMacro1 )
            {
                FileNameWithExt = fileNameWithExt;
                FileExtension = fileExtension;
                RelativePath = relativePath;
                ConentsToWrite = conentsToWrite;
                UseMacro1 = useMacro1;
            }
        }
         

        public AOWritableToAOClassContents(string fromLibraryName, string instanceName, AOTypeEnum aOType) : 
            base(  instanceName, aOType)
        {
            FromLibraryName =  fromLibraryName;
            //// from this library name, I need to get the directory that it belongs to.
            ////first grab all the contents of the cmake file in C:/AERTOS/AERTOS/CMakeLists.txt .
            //string cmakeCont = File.ReadAllText(@"C:/AERTOS/AERTOS/CMakeLists.txt");

            ////    STREQUAL "exeHalTest")
            ////set(INTEGRATION_TARGET_DIRECTORY "C:/AERTOS/AERTOS/src/AE/hal/exeHalTest")
            //Regex re = new Regex(@"STREQUAL\s*\""" + FromLibraryName + @"\""\s*\)\s*set\s*\(\s*INTEGRATION_TARGET_DIRECTORY\s*\""(?<ArgReqContents>.*)\""");
            //_ProjectDirectory = re.Match(cmakeCont).Value;

            //_ProjectDirectory = FromLibraryName == "CGENTest" ? @"C:\CodeGenerator\CodeGenerator\macro2Test\CGENTest" : _ProjectDirectory; //for debugging
             

            _ProjectDirectory = AEInitializing.GetRunningDirectoryFromProjectName(fromLibraryName);

            AllAOWritableToAOClassContents.Add(this);
        }

        public string ProjectDirectory { get => _ProjectDirectory; set => _ProjectDirectory = value; }
        public string FromLibraryName { get; }

        private string _ProjectDirectory;

        public void WriteTheContentedToFiles()
        {
            //???if this AO is not apart of the current project, DONT write any files. This might mess with build 
            var tt = _WriteTheContentedToFiles();
            if (tt != null)
            {
                foreach (var contentesToW in tt)
                { 

                    string FullPath = Path.Combine(_ProjectDirectory, contentesToW.RelativePath, contentesToW.FileNameWithExt);

                    AEInitializing.TheMacro2Session.WriteFileContents(contentesToW.ConentsToWrite, FullPath, contentesToW.FileExtension, contentesToW.UseMacro1);
                }
            }

        }

        public static void WriteAllFileContents()
        {
            foreach (var item in AllAOWritableToAOClassContents)
            {
                item.WriteTheContentedToFiles();
            }
        }

        protected abstract List<RelativeDirPathWrite> _WriteTheContentedToFiles();

        public abstract string GetFullTemplateType();

        public abstract string GetFullTemplateArgsValues();
        public abstract string GetFullTemplateArgs();
    }

}
