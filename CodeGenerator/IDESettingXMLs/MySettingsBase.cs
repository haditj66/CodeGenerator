using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGenerator.ProblemHandler;

namespace CodeGenerator.IDESettingXMLs
{
    public abstract class MySettingsBase : ISynchronizableProject
    {
        public List<MyFilter> myFilters { get; protected set; }
        public List<string> StringIncludes { get; protected set; }
        public List<MyCLCompileFile> CLCompileFiles { get; protected set; }
        public List<MyCLIncludeFile> CLIncludeFiles { get; protected set; }

        public List<IDESetting> XmlSettings { get; protected set; }
        public string PATHOfProject { get; private set; }

        public ProblemHandle ProblemHandle { get; set; }

     

        public MySettingsBase(params IDESetting[] xmlSettings)
        {
            ProblemHandle = new ProblemHandle();
            PATHOfProject = xmlSettings[0].PathWithoutFileNameOfXmlSetting;

            XmlSettings = xmlSettings.ToList();
             
            myFilters = new List<MyFilter>(); 

        }


        protected bool IsAlreadyInitiated = false;
        public abstract void Initiate();

        protected abstract void AddMyFilterAsXMLFilter(MyFilter myfilters);
        protected abstract List<MyFilter> ConvertAllCurrentXMLFiltersAsMyFilters();
        public abstract void AddFilter(MyFilter filterToAdd);

        protected abstract void AddSTRINGIncludeAsAdditionalIncludes(string additionalIncludeDir);
        //protected abstract bool DoesAdditionalIncludesAlreadyExist(string additionalIncludeDir);
        protected abstract List<string> ConvertAllCurrentAdditionalIncludesAsStrings();
        public abstract void AddAdditionalInclude(string AdditionalInclude);

        protected abstract void AddMyClIncludesAsCIncludes(MyCLIncludeFile CLIncludeFile);
        //protected abstract bool DoesClIncludeExist(MyCLIncludeFile CLIncludeFile); 
        public abstract void AddCLIncludeFile(MyCLIncludeFile CLIncludeFile);


        protected abstract void AddMyCLCompileFileAsCLCompileFile(MyCLCompileFile CLCompileFile);
        //protected abstract bool DoesCCompileExist(MyCLCompileFile clCompileToCheck);
        //protected abstract List<MyCLCompileFile> ConvertAllCurrentCCompilessAsMyClCompiles();
        public abstract void AddCLCompileFile(MyCLCompileFile CLCompileFile);

          

        public void Save(string baseDirectoryForProject)
        {
            foreach (var setting in XmlSettings)
            {
                setting.GenerateXMLSetting(baseDirectoryForProject);//(Path.GetDirectoryName(config.ConfigFileFullPath));
            }
        }


    }
}

