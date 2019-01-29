using CodeGenerator.IDESettingXMLs.VisualStudioXMLs.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.IDESettingXMLs
{
    public abstract class MySettingsBase
    {
        public List<IDESetting> XmlSettings { get; } 
        public object XmlFilterClass { get; }
        public object XmlProjectClass { get; }
        public List<MyFilter> myFilters { get; private set; }
        public List<string> StringIncludes { get; private set; }
        public List<MyCLCompileFile> CLCompileFiles { get; private set; }
        public List<MyCLIncludeFile> CLIncludeFiles { get; private set; }

        public MySettingsBase(IDESetting xmlFilterSetting, IDESetting xmlProjectsetting)
        {
            XmlSettings = new List<IDESetting>();
            XmlSettings.Add(xmlFilterSetting);
            XmlSettings.Add(xmlProjectsetting);
            XmlFilterClass = xmlFilterSetting.RootOfSetting;
            XmlProjectClass = xmlProjectsetting.RootOfSetting;

        }

        public void Initiate()
        {

            RemoveAllMentionsOfLibraryDependencyFilters();
            RemoveAllMentionsOfLibraryDependencyCLCompiles();
            RemoveAllMentionsOfLibraryDependencyCLIncludes();

            myFilters = ConvertAllCurrentXMLFiltersAsMyFilters();
            StringIncludes = ConvertAllCurrentAdditionalIncludesAsStrings();
            CLCompileFiles = ConvertAllCurrentCCompilessAsMyClCompiles();
            CLIncludeFiles = ConvertAllCurrentCIncludesAsMyClIncludes();


            //take out all ccompile, cincludes, and myfilters that have mention of LibraryDependency
            CLCompileFiles.RemoveAll((MyCLCompileFile cc) => { return cc.FullFilterName.Contains("LibraryDependencies"); });
            CLIncludeFiles.RemoveAll((MyCLIncludeFile cc) => { return cc.FullFilterName.Contains("LibraryDependencies"); });
            myFilters.RemoveAll((MyFilter cc) => { return cc.GetFullAddress().Contains("LibraryDependencies"); });
        }

        protected abstract void RemoveAllMentionsOfLibraryDependencyFilters();
        protected abstract List<MyFilter> ConvertAllCurrentXMLFiltersAsMyFilters();
        protected abstract void AddMyFilterAsXMLFilter(MyFilter myfilters);

        protected abstract List<string> ConvertAllCurrentAdditionalIncludesAsStrings();
        protected abstract void AddSTRINGIncludeAsAdditionalIncludes(string additionalIncludeDir);
        protected abstract bool DoesAdditionalIncludesAlreadyExist(string additionalIncludeDir);
         
        protected abstract void RemoveAllMentionsOfLibraryDependencyCLIncludes();
        protected abstract List<MyCLIncludeFile> ConvertAllCurrentCIncludesAsMyClIncludes();
        protected abstract void AddMyClIncludesAsCIncludes(MyCLIncludeFile CLIncludeFile);
        protected abstract bool DoesClIncludeExist(MyCLIncludeFile CLIncludeFile);

        protected abstract void RemoveAllMentionsOfLibraryDependencyCLCompiles();
        protected abstract List<MyCLCompileFile> ConvertAllCurrentCCompilessAsMyClCompiles();
        protected abstract void AddMyCLCompileFileAsCLCompileFile(MyCLCompileFile CLCompileFile);
        protected abstract bool DoesCCompileExist(MyCLCompileFile clCompileToCheck);
         
        //public abstract void GenerateXmlSettings();



        public void AddFilter(MyFilter filterToAdd)
        {
            //add to filters but also convert it to a XMLFilter and add it there to the settings as needed.
            myFilters.Add(filterToAdd);

            //convert that filter to an xmlClassFilter
            AddMyFilterAsXMLFilter(filterToAdd);
        }

        public void AddAdditionalInclude(string AdditionalInclude)
        {
            if (!DoesAdditionalIncludesAlreadyExist(AdditionalInclude))
            {
                StringIncludes.Add(AdditionalInclude);

                AddSTRINGIncludeAsAdditionalIncludes(AdditionalInclude);
            }
        }

        public void AddCLIncludeFile(MyCLIncludeFile CLIncludeFile)
        {
            if (!DoesClIncludeExist(CLIncludeFile))
            { 
                CLIncludeFiles.Add(CLIncludeFile);

                AddMyClIncludesAsCIncludes(CLIncludeFile);
            }
        }

        public void AddCLCompileFile(MyCLCompileFile CLCompileFile)
        {
            if (!DoesCCompileExist(CLCompileFile))
            {
                CLCompileFiles.Add(CLCompileFile);

                AddMyCLCompileFileAsCLCompileFile(CLCompileFile);
            }

        }




    }
}
