using CgenMin.MacroProcesses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.MacroProcesses.AESetups
{

    public class IntegrationMacroFileHandler
    {
        public IntegrationMacroFileHandler(AEProject projChosen, MacroProcess macroProcess)
        {
            MacroProcess = macroProcess;
            DirectoriesOfIntegrationMacroFile = new List<string>();
            NameOfProject = projChosen.Name;

            foreach (var board in AEProject.ListOfBoardTargets)
            {
                DirectoriesOfIntegrationMacroFile.Add(Path.Combine(projChosen.DirectoryOfLibrary, "CGensaveFiles", $"${projChosen.Name}_${board}", "DEBUG"));
            }
             
             
            if (projChosen != null)
            {
                tdependsStr = projChosen.LibrariesIDependOnStr;
            }

        }

        public IntegrationMacroFileHandler(string nameOfProjectChosen, List<string> projChosenDepends, string projectChosenDir, MacroProcess macroProcess)
        {
            MacroProcess = macroProcess;
            DirectoriesOfIntegrationMacroFile = new List<string>();
            NameOfProject = nameOfProjectChosen;

            foreach (var board in AEProject.ListOfBoardTargets)
            {
                DirectoriesOfIntegrationMacroFile.Add(Path.Combine(projectChosenDir, "CGensaveFiles", "cmakeGui", $"{nameOfProjectChosen}_{board}", "DEBUG"));
            }

            tdependsStr = projChosenDepends;

        }


        List<string> tdependsStr = new List<string>();
        public string NameOfProject { get; }
        public MacroProcess MacroProcess { get; }
        public List<string> DirectoriesOfIntegrationMacroFile { get; }

        public void CreateTheFile(string directoryOfFile_noFileName)
        {
            if (Directory.Exists(directoryOfFile_noFileName) == false)
            {
                Directory.CreateDirectory(directoryOfFile_noFileName);
            }

            //get all depends and create empty macrotestdefines
            string dependsEmpty = "";
            foreach (var d in tdependsStr)
            {
                dependsEmpty += MacroProcess.GenerateFileOut("AERTOS\\IntegrationTestMacroEmpty",
                    new MacroVar() { MacroName = "TestEmpty", VariableValue = d }); dependsEmpty += "\n";
            }

            string chosenEmpty = MacroProcess.GenerateFileOut("AERTOS\\IntegrationTestMacroEmpty",
                    new MacroVar() { MacroName = "TestEmpty", VariableValue = NameOfProject }); chosenEmpty += "\n";

            string chosenFull = MacroProcess.GenerateFileOut("AERTOS\\IntegrationTestMacroFull",
                    new MacroVar() { MacroName = "TestChosen", VariableValue = NameOfProject }); chosenFull += "\n";

            MacroProcess.WriteFileContents_FromCGENMMFile_ToFullPath(
                "AERTOS\\IntegrationTestMacros",
                Path.Combine(directoryOfFile_noFileName, "IntegrationTestMacros.h"),
                true, false,
                 new MacroVar() { MacroName = "AllDependsEmpty", VariableValue = dependsEmpty },
                 new MacroVar() { MacroName = "ChosenDefines", VariableValue = chosenFull },
                 new MacroVar() { MacroName = "ChosenDefinesEmpty", VariableValue = chosenEmpty }
                ); 
        }

        public  void CreateAllIntegrationFilesTheFiles()
        { 
            foreach (var d in DirectoriesOfIntegrationMacroFile)
            {
                CreateTheFile(d);
            } 

        }

    }
}
