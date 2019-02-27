using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CodeGenerator.IDESettingXMLs.IAR_XMLs.EWD
{
    [XmlRoot(ElementName = "toolchain")]
    public class Toolchain
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
    }

    [XmlRoot(ElementName = "option")]
    public class Option
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "state")]
        public List<string> State { get; set; }
        [XmlElement(ElementName = "version")]
        public string Version { get; set; }
    }

    [XmlRoot(ElementName = "data")]
    public class Data
    {
        [XmlElement(ElementName = "version")]
        public string Version { get; set; }
        [XmlElement(ElementName = "wantNonLocal")]
        public string WantNonLocal { get; set; }
        [XmlElement(ElementName = "debug")]
        public string Debug { get; set; }
        [XmlElement(ElementName = "option")]
        public List<Option> Option { get; set; }
        [XmlElement(ElementName = "extensions")]
        public string Extensions { get; set; }
        [XmlElement(ElementName = "cmdline")]
        public string Cmdline { get; set; }
        [XmlElement(ElementName = "hasPrio")]
        public string HasPrio { get; set; }
        [XmlElement(ElementName = "prebuild")]
        public string Prebuild { get; set; }
        [XmlElement(ElementName = "postbuild")]
        public string Postbuild { get; set; }
    }

    [XmlRoot(ElementName = "settings")]
    public class Settings
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "archiveVersion")]
        public string ArchiveVersion { get; set; }
        [XmlElement(ElementName = "data")]
        public Data Data { get; set; }
    }

    [XmlRoot(ElementName = "configuration")]
    public class Configuration
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "toolchain")]
        public Toolchain Toolchain { get; set; }
        [XmlElement(ElementName = "debug")]
        public string Debug { get; set; }
        [XmlElement(ElementName = "settings")]
        public List<Settings> Settings { get; set; }
    }

    [XmlRoot(ElementName = "file")]
    public class File
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
    }

    [XmlRoot(ElementName = "group")]
    public class Group
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "file")]
        public List<File> File { get; set; }
        [XmlElement(ElementName = "group")]
        public List<Group> GroupNested { get; set; }
    }

    [XmlRoot(ElementName = "project")]
    public class ProjectEWD
    {
        [XmlElement(ElementName = "fileVersion")]
        public string FileVersion { get; set; }
        [XmlElement(ElementName = "configuration")]
        public Configuration Configuration { get; set; }
        [XmlElement(ElementName = "group")]
        public List<Group> Group { get; set; }
    }
}
