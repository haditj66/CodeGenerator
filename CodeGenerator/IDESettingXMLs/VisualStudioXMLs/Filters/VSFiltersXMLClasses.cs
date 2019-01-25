using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CodeGenerator.IDESettingXMLs.VisualStudioXMLs.Filters
{
    [XmlRoot(ElementName = "Filter", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class Filter
    { 

        [XmlElement(ElementName = "UniqueIdentifier", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public string UniqueIdentifier { get; set; }
        [XmlElement(ElementName = "Extensions", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public string Extensions { get; set; }
        [XmlAttribute(AttributeName = "Include")]
        public string Include { get; set; }
    }

    [XmlRoot(ElementName = "ItemGroup", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ItemGroup
    {
        [XmlElement(ElementName = "Filter", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public List<Filter> Filter { get; set; }
        [XmlElement(ElementName = "ClInclude", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public List<ClInclude> ClInclude { get; set; }
        [XmlElement(ElementName = "ClCompile", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public List<ClCompile> ClCompile { get; set; }
    }

    [XmlRoot(ElementName = "ClInclude", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ClInclude
    {
        [XmlElement(ElementName = "Filter", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public string Filter { get; set; }
        [XmlAttribute(AttributeName = "Include")]
        public string Include { get; set; }
    }

    [XmlRoot(ElementName = "ClCompile", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ClCompile
    {
        [XmlElement(ElementName = "Filter", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public string Filter { get; set; }
        [XmlAttribute(AttributeName = "Include")]
        public string Include { get; set; }
    }

    [XmlRoot(ElementName = "Project", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class Project 
    {
        [XmlElement(ElementName = "ItemGroup", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public List<ItemGroup> ItemGroup { get; set; }
        [XmlAttribute(AttributeName = "ToolsVersion")]
        public string ToolsVersion { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }

}
