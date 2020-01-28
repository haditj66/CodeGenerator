using CodeGenerator.IDESettingXMLs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.CMD_Handler
{
    public class CLCommandBuilder
    {
        public List<string> AdditionalIncludes { get; set; }
        public List<MyStaticLib> Libs { get; set; }
        public List<MyCLCompileFile> FilesCComp { get; set; }
        public string OutputLocation { get; set; }
        public string Name { get; set; }

        public CLCommandBuilder(string name, params MyCLCompileFile[] filesCComp)
        {
            Name = name;
            AdditionalIncludes = new List<string>();
            Libs = new List<MyStaticLib>();
            FilesCComp = new List<MyCLCompileFile>();
            foreach (var ccomp in filesCComp)
            {
                FilesCComp.Add(ccomp);
            } 
            OutputLocation = "";
        }

        public string GetCompileCommand()
        {
            string command = @"cl    /EHsc /MDd";

            command += " ";

           //set the output for .obj files
           command += @"/Fo""" + OutputLocation + @"""\";

            command += " ";


            //add all the std library stuff
            command += @"/std:c++17";
            //command += @"/ZI /Gm- /Od /sdl /Fd""Debug\vc141.pdb"" /Zc:inline /fp:precise /D ""_MBCS"" /errorReport:prompt /WX- /Zc:forScope /RTC1 /Gd /Oy- /MDd /FC /Fa""Debug\"" /EHsc /nologo /Fo""Debug\""";
            command += " ";

            AdditionalIncludes.Add(@"C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\VC\Tools\MSVC\14.16.27023\include\map");
            AdditionalIncludes.Add(@"C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\VC\Tools\MSVC\14.16.27023\include\vector");

            //set additional include 
            foreach (var additoinalInclude in AdditionalIncludes)
            {
                command += @"/I " + @""""+additoinalInclude+@"""" + " ";
            }

            //put all .cpp files involved in this compile
            foreach (var ccomp in FilesCComp)
            {
                command += ccomp.FullLocationName + " ";
            }

            //set library .lib links
            foreach (var lib in Libs)
            {
                command += @"/link " + lib.Name + " ";
            }


            //set lib paths
            foreach (var lib in Libs)
            {
                command += @"/libpath:"+@""""+ lib.FullLocation+@"""" + " ";
            }


            //set output
            string fileFullPath = Path.Combine(OutputLocation,Name);
            fileFullPath =Path.ChangeExtension(fileFullPath, ".exe");
            //change all \ to /
            fileFullPath = fileFullPath.Replace(@"\", @"/");
            command += @"/out:" + fileFullPath;

            //command = @"/JMC /GS /analyze- /W3 /Zc:wchar_t /I""LibraryDependencies\stmFlashConf0"" /I""LibraryDependencies\SPISlaveConf0"" /I""LibraryDependencies\WaveletConf0"" /I""C:\Users\Hadi\Documents\Visual Studio 2017\Projects\AO Projects\WaveletTransformSPB\WaveletTransformSPB\Config"" /I""LibraryDependencies"" /I""C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\AEActiveObjectsRTOS\AERTOS\freertosTemplate"" /I""C:\Users\Hadi\Documents\Visual Studio 2017\Projects\AO Projects\UVariableSaver\UVariableSaver\"" /I""C:\Users\Hadi\Documents\Visual Studio 2017\Projects\AO Projects\UVariableSaver\UVariableSaver\\Config"" /I""C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\ConfigTest"" /I"".\FreertosFilesForMSVC\Source\include"" /I"".\FreertosFilesForMSVC\Common"" /I"".\FreertosFilesForMSVC\Trace_Recorder_Configuration"" /I"".\FreertosFilesForMSVC"" /I"".\FreertosFilesForMSVC\FreeRTOS-Plus-Trace\Include"" /I"".\FreertosFilesForMSVC\Source\portable\AE"" /I"".\FreertosFilesForMSVC\FreeRTOS-Plus-Trace"" /I""C:\Users\Hadi\Documents\Visual Studio 2017\Projects\AO Projects\UVariableSaver\UVariableSaver\FreertosFilesForMSVC\CMSIS_RTOS"" /I""."" /I""LibraryDependencies\UUartDriverConf0"" /I""LibraryDependencies\UUartDriverConf0\SizeOfTransmitBuffer100PoolSize10"" /I""C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\AEActiveObjectsRTOS\AO Projects\UUartDriver\UUartDriver\FreertosFilesForMSVC\Source\portable\AE"" /I""C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\AEActiveObjectsRTOS\AO Projects\UUartDriver\UUartDriver\FreertosFilesForMSVC\Trace_Recorder_Configuration"" /I""C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\AEActiveObjectsRTOS\AO Projects\UUartDriver\UUartDriver\FreertosFilesForMSVC\Source\portable"" /I""C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\AEActiveObjectsRTOS\AO Projects\UUartDriver\UUartDriver\FreertosFilesForMSVC\Source\include"" /I""C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\AEActiveObjectsRTOS\AO Projects\UUartDriver\UUartDriver\FreertosFilesForMSVC\FreeRTOS-Plus-Trace\Include"" /I""C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\AEActiveObjectsRTOS\AO Projects\UUartDriver\UUartDriver\FreertosFilesForMSVC\Common\include"" /I""C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\AEActiveObjectsRTOS\AO Projects\UUartDriver\UUartDriver\FreertosFilesForMSVC\CMSIS_RTOS"" /ZI /Gm- /Od /sdl /Fd""Debug\vc141.pdb"" /Zc:inline /fp:precise /D ""_MBCS"" /errorReport:prompt /WX- /Zc:forScope /RTC1 /Gd /Oy- /MDd /FC /Fa""Debug\"" /EHsc /nologo /Fo""Debug\"" /Fp""Debug\UUartDriver.pch"" /diagnostics:classic ";

            return command ;
        }
    }
}
