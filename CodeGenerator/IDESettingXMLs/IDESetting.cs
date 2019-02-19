using CodeGenerator.IDESettingXMLs.VisualStudioXMLs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace CodeGenerator.IDESettingXMLs
{
    public class IDESetting
    {
        public dynamic RootOfSetting { get; set; }
        public string PathWithoutFileNameOfXmlSetting { get; }
        public string ProjectExtension { get; }
        public XmlSerializer Serializer { get; }
        public string SettingFileFullPath { get; private set; }
        public Type TypeOfRootSetting { get; private set; }


        public IDESetting(string PathWithoutFileNameOfXmlSetting, string projectExtension, Type typeOfRootSetting)
        {
            this.PathWithoutFileNameOfXmlSetting = PathWithoutFileNameOfXmlSetting;
            ProjectExtension = projectExtension;
            TypeOfRootSetting = typeOfRootSetting;
            Serializer = new XmlSerializer(TypeOfRootSetting);

            Debug.Assert(IsIDEProjectExistHere(PathWithoutFileNameOfXmlSetting), "There is no project setting here of extension "+ProjectExtension+". Make sure you create cgen project at same directory of your IDE project settings.");

            DirectoryInfo libraryDir = new DirectoryInfo(PathWithoutFileNameOfXmlSetting);
            SettingFileFullPath = libraryDir.GetFiles()
                .Where((FileInfo file) => { return file.Extension == projectExtension; }).First().FullName;


            LoadSettings( );

        }
         


        public IDESetting(string projFileFullPath, Type typeOfRootSetting)
        {
            LoadSettings( );

        }

        public bool IsIDEProjectExistHere(string PathWithoutFileNameOfXmlSetting)
        {
            var t = new DirectoryInfo(PathWithoutFileNameOfXmlSetting);

            bool isfileExtensionThere = false;

            foreach (var file in t.GetFiles())
            {
                if (file.Extension == ProjectExtension)
                {
                    isfileExtensionThere = true;
                    break;
                }
            }

            if (!isfileExtensionThere)
            {
                return false;
            }


            return true;
        }


        public void LoadSettings()
        {
            //get library project xml path  
            using (StreamReader reader = new StreamReader(SettingFileFullPath))
            {
                RootOfSetting = Convert.ChangeType(Serializer.Deserialize(reader), TypeOfRootSetting);
            }
        }

        protected string GetFullFilePathFromPathWithoutFile(string FullPathToPutGeneratedXMLFileWITHOUTFILENAME)
        {
            //get the name of the file I am going to replace
            DirectoryInfo libraryDir = new DirectoryInfo(FullPathToPutGeneratedXMLFileWITHOUTFILENAME);
            string projFileFullPath = libraryDir.GetFiles()
                .Where((FileInfo file) => { return file.Extension == ProjectExtension; }).First().FullName;
            return projFileFullPath;
        }

        public virtual void GenerateXMLSetting(string FullPathToPutGeneratedXMLFileWITHOUTFILENAME)
        {

            using (StreamWriter sw = new StreamWriter(GetFullFilePathFromPathWithoutFile(FullPathToPutGeneratedXMLFileWITHOUTFILENAME)))
            {
                Serializer.Serialize(sw, RootOfSetting);
            }
        }
    }
}
