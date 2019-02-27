using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.IDESettingXMLs
{
    public interface ISynchronizableProject
    { 
         

        void AddFilter(MyFilter filterToAdd);
        void AddAdditionalInclude(string AdditionalInclude);
        void AddCLIncludeFile(MyCLIncludeFile CLIncludeFile);
        void AddCLCompileFile(MyCLCompileFile CLCompileFile);
         

        /*
          void AddMyFilterAsXMLFilter(MyFilter myfilters);

          void AddSTRINGIncludeAsAdditionalIncludes(string additionalIncludeDir);
          bool DoesAdditionalIncludesAlreadyExist(string additionalIncludeDir);
         
          void AddMyClIncludesAsCIncludes(MyCLIncludeFile CLIncludeFile);
          bool DoesClIncludeExist(MyCLIncludeFile CLIncludeFile);

        void AddMyCLCompileFileAsCLCompileFile(MyCLCompileFile CLCompileFile);
        bool DoesCCompileExist(MyCLCompileFile clCompileToCheck);
        */
    }
}
