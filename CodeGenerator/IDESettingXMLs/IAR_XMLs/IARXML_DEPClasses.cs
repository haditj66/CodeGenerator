using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CodeGenerator.IDESettingXMLs.IAR_XMLs.DEP
{

    [XmlRoot(ElementName = "outputs")]
    public class Outputs
    {
        [XmlElement(ElementName = "file")]
        public List<string> File { get; set; }
        [XmlElement(ElementName = "tool")]
        public List<Tool> Tool { get; set; }
    }

    [XmlRoot(ElementName = "tool")]
    public class Tool
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "file")]
        public string File { get; set; }
    }

    [XmlRoot(ElementName = "file")]
    public class File
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "outputs")]
        public Outputs Outputs { get; set; }
        [XmlElement(ElementName = "inputs")]
        public Inputs Inputs { get; set; }
    }

    [XmlRoot(ElementName = "inputs")]
    public class Inputs
    {
        [XmlElement(ElementName = "tool")]
        public List<Tool> Tool { get; set; }
    }

    [XmlRoot(ElementName = "forcedrebuild")]
    public class Forcedrebuild
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "tool")]
        public string Tool { get; set; }
    }

    [XmlRoot(ElementName = "configuration")]
    public class Configuration
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "outputs")]
        public Outputs Outputs { get; set; }
        [XmlElement(ElementName = "file")]
        public List<File> File { get; set; }
        [XmlElement(ElementName = "forcedrebuild")]
        public Forcedrebuild Forcedrebuild { get; set; }
    }

    [XmlRoot(ElementName = "project")]
    public class ProjectDEP
    {
        [XmlElement(ElementName = "fileVersion")]
        public string FileVersion { get; set; }
        [XmlElement(ElementName = "fileChecksum")]
        public string FileChecksum { get; set; }
        [XmlElement(ElementName = "configuration")]
        public Configuration Configuration { get; set; }
    }

}
