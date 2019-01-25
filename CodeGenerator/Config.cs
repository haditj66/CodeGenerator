using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CodeGenerator
{
    [XmlRoot(ElementName = "EnumDefinition")]
    public class EnumDefinition
    {
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "Value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "EnumsDefinintions")]
    public class EnumsDefinintions
    {
        [XmlElement(ElementName = "EnumDefinition")]
        public List<EnumDefinition> EnumDefinition { get; set; }
    }

    [XmlRoot(ElementName = "Define")]
    public class Define
    {
        [XmlAttribute(AttributeName = "DefineName")]
        public string DefineName { get; set; }
        [XmlAttribute(AttributeName = "Type")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "Value")]
        public string Value { get; set; }
        [XmlAttribute(AttributeName = "MyPrivacy")]
        public string MyPrivacy { get; set; }
        [XmlAttribute(AttributeName = "PrefixName")]
        public string PrefixName { get; set; }
        [XmlAttribute(AttributeName = "IsStatic")]
        public string IsStatic { get; set; }
    }

    [XmlRoot(ElementName = "Defines")]
    public class Defines
    {
        [XmlElement(ElementName = "Define")]
        public List<Define> Define { get; set; }
    }

    [XmlRoot(ElementName = "Config")]
    public class Config
    {
        [XmlElement(ElementName = "EnumsDefinintions")]
        public EnumsDefinintions EnumsDefinintions { get; set; }
        [XmlElement(ElementName = "Defines")]
        public Defines Defines { get; set; }
        [XmlAttribute(AttributeName = "Prefix")]
        public string Prefix { get; set; }
        [XmlAttribute(AttributeName = "myInstanceNum")]
        public string MyInstanceNum { get; set; }
        [XmlAttribute(AttributeName = "Major")]
        public string Major { get; set; }
        [XmlAttribute(AttributeName = "ConfTypePrefix")]
        public string ConfTypePrefix { get; set; }
        [XmlAttribute(AttributeName = "isTopLevel")]
        public string IsTopLevel { get; set; }
        [XmlAttribute(AttributeName = "FileNameString")]
        public string FileNameString { get; set; }
        [XmlAttribute(AttributeName = "ClassName")]
        public string ClassName { get; set; }
        [XmlElement(ElementName = "Depends")]
        public Depends Depends { get; set; }
    }


    [XmlRoot(ElementName = "Depend")]
    public class Depend
    {
        [XmlAttribute(AttributeName = "NameOfDepend")]
        public string NameOfDepend { get; set; }
        [XmlAttribute(AttributeName = "TypePrefOfDepend")]
        public string TypePrefOfDepend { get; set; }
        [XmlAttribute(AttributeName = "ModeOfDepend")]
        public string ModeOfDepend { get; set; } 
    }

    [XmlRoot(ElementName = "Depends")]
    public class Depends
    {
        [XmlElement(ElementName = "Depend")]
        public List<Depend> Depend { get; set; }
    }

    [XmlRoot(ElementName = "Configs")]
    public class Configs
    {
        [XmlElement(ElementName = "Config")]
        public List<Config> Config { get; set; }
    }

    [XmlRoot(ElementName = "Root")]
    public class Root
    {
        [XmlElement(ElementName = "Configs")]
        public Configs Configs { get; set; }
    }


    /*
    [Serializable()]
    [XmlRoot(ElementName = "Config")]
    public class Config
    {
        //<Config Prefix="GlobalConfigBuild" myInstanceNum="0" Major="0" ConfTypePrefix="" isTopLevel="false" FileNameString="c:\users\hadi\onedrive\documents\visualstudioprojects\projects\c# apps\codegenerator\codegenerator\configtest\globalbuildconfig.h" ClassName="GlobalBuildConfig">
        [XmlAttribute]
        string Prefix { get; set; }
        [XmlAttribute]
        int Major { get; set; }
        [XmlAttribute]
        string ConfTypePrefix { get; set; }
        [XmlAttribute]
        bool isTopLevel { get; set; }
        [XmlAttribute]
        string FileNameString { get; set; }
        [XmlAttribute]
        string ClassName { get; set; }
    }

    */

}
