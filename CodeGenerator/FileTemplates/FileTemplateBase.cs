using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeGenerator.FileTemplates
{
    public abstract class FileTemplateBase
    {
        private string v1;
        private string v2;

        protected List<Macro> Macros { get; set; }
        protected string PathTOFileTemplate { get; set; }
        protected string TemplateOutputDestination { get; set; }
        public string NameOfTemplateFile { get; }
        public string NameOfOutputTemplateFile { get; }

        public FileTemplateBase(string templateOutputDestination, string nameOfTemplateFile, string  nameOfOutputTemplateFile)
        {
            PathTOFileTemplate = @"C:\Users\Hadi\OneDrive\Documents\VisualStudioprojects\Projects\cSharp\CodeGenerator\CodeGenerator\CodeGenerator\FileTemplates\Files";
            TemplateOutputDestination = templateOutputDestination;
            Debug.Assert(nameOfTemplateFile.Contains("."), "name of file needs an extension");
            NameOfTemplateFile = nameOfTemplateFile;
            Debug.Assert(nameOfOutputTemplateFile.Contains("."), "name of file needs an extension");
            NameOfOutputTemplateFile = nameOfOutputTemplateFile;
        }
         

        public void CreateTemplate()
        {
            //read from the template file
            string templateFileStr = "";
            string FullPathToTemplate = Path.Combine(PathTOFileTemplate, NameOfTemplateFile);
            using (StreamReader sr = new StreamReader(FullPathToTemplate))
            {
                templateFileStr = sr.ReadToEnd();
            }

            //get that and replace all macros with their approprieate values
            foreach (var macro  in Macros)
            {
                templateFileStr = Regex.Replace(templateFileStr,@"<#\s*"+ macro.NameInFile+ @"\s*#>", macro.ValueToReplace);
            }


            //now write this into the destination File
            //this will overwrite the file itf it exists file does not exist
            string FullFilePathDestination = Path.Combine(TemplateOutputDestination, NameOfOutputTemplateFile); 
            File.WriteAllText(FullFilePathDestination, templateFileStr); 
        }

            
        

    }


    public class Macro
    {
    public string NameInFile;
    public string ValueToReplace;
    public Macro(string NameInFile, string ValueToReplace)
    {
        this.NameInFile = NameInFile;
        this.ValueToReplace = ValueToReplace;
    }

    }
}
