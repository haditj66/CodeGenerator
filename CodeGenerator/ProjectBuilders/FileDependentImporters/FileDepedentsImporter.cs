using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGenerator.IDESettingXMLs;

namespace CodeGenerator.ProjectBuilders.FileDependentImporters
{
    public class FileDepedentsImporter
    {
        protected string PrefixToAdd { get; }
        protected List<MyCLCompileFile> ClCompFilesToImport { get; }
        protected List<MyCLIncludeFile> ClIncFilesToImport { get; }

        public FileDepedentsImporter(string prefixToAdd, List<MyCLCompileFile> clCompFilesToImport, List<MyCLIncludeFile> clIncFilesToImport)
        {
            PrefixToAdd = prefixToAdd;
            ClCompFilesToImport = clCompFilesToImport;
            ClIncFilesToImport = clIncFilesToImport;
        }

        public void ImportFilesToPath(string Path)
        {

        }
    }
}
