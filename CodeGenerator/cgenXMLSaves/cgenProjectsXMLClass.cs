using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CodeGenerator.cgenXMLSaves
{
    [XmlRoot(ElementName = "Project")]
    public class cgenProject
    {
        [XmlAttribute(AttributeName = "NameOfProject")]
        public string NameOfProject { get; set; }

        [XmlAttribute(AttributeName = "PathOfProject")]
        public string PathOfProject { get; set; }

        [XmlAttribute(AttributeName = "UniqueIdentifier")]
        public string UniqueIdentifier { get; set; }

    }

    public static class cgenXMLMemeberCreationHelper
    {
        static Random rng = new Random();
        public static string UniqueIdentifierCreator()
        {
            //9999999-9999-9999999
            int first = rng.Next(1000000, 9999999);
            int second = rng.Next(1000, 9999);
            int third = rng.Next(1000000, 9999999);
            return first.ToString() + second.ToString() + third.ToString();
        }
    }


    [XmlRoot(ElementName = "Projects")]
    public class cgenProjects
    {
        [XmlElement(ElementName = "Projects")]
        public List<cgenProject> Projects { get; set; }

    }


    #region Config

    [XmlRoot(ElementName = "Config")]
    public class cgenConfig
    {
        [XmlAttribute(AttributeName = "DirectoryOfConfig")]
        public string DirectoryOfConfig { get; set; }

    }

    #endregion








}
