using CodeGenerator.IDESettingXMLs.VisualStudioXMLs;
using System;
using System.Collections.Generic;
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

        public IDESetting(string PathWithoutFileNameOfXmlSetting, string projectExtension, Type typeOfRootSetting)
        {
            this.PathWithoutFileNameOfXmlSetting = PathWithoutFileNameOfXmlSetting;
            ProjectExtension = projectExtension;
            Serializer = new XmlSerializer(typeOfRootSetting);

            DirectoryInfo libraryDir = new DirectoryInfo(PathWithoutFileNameOfXmlSetting);
            string projFileFullPath = libraryDir.GetFiles()
                .Where((FileInfo file) => { return file.Extension == projectExtension; }).First().FullName;


            InitRootSetting(projFileFullPath, typeOfRootSetting);
        }


        public IDESetting(string projFileFullPath, Type typeOfRootSetting)
        {
            InitRootSetting(projFileFullPath, typeOfRootSetting);

        }

        private void InitRootSetting(string projFileFullPath, Type typeOfRootSetting)
        {
            //get library project xml path  
            using (StreamReader reader = new StreamReader(projFileFullPath))
            {
                RootOfSetting = Convert.ChangeType(Serializer.Deserialize(reader), typeOfRootSetting);
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
