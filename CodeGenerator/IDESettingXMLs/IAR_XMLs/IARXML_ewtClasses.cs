using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CodeGenerator.IDESettingXMLs.IAR_XMLs.EWT
{
    [XmlRoot(ElementName = "toolchain")]
    public class Toolchain
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
    }

    [XmlRoot(ElementName = "cstatargs")]
    public class Cstatargs
    {
        [XmlElement(ElementName = "useExtraArgs")]
        public string UseExtraArgs { get; set; }
        [XmlElement(ElementName = "extraArgs")]
        public string ExtraArgs { get; set; }
        [XmlElement(ElementName = "analyzeTimeoutEnabled")]
        public string AnalyzeTimeoutEnabled { get; set; }
        [XmlElement(ElementName = "analyzeTimeout")]
        public string AnalyzeTimeout { get; set; }
        [XmlElement(ElementName = "enableParallel")]
        public string EnableParallel { get; set; }
        [XmlElement(ElementName = "parallelThreads")]
        public string ParallelThreads { get; set; }
        [XmlElement(ElementName = "enableFalsePositives")]
        public string EnableFalsePositives { get; set; }
        [XmlElement(ElementName = "messagesLimitEnabled")]
        public string MessagesLimitEnabled { get; set; }
        [XmlElement(ElementName = "messagesLimit")]
        public string MessagesLimit { get; set; }
    }

    [XmlRoot(ElementName = "check")]
    public class Check
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "enabled")]
        public string Enabled { get; set; }
    }

    [XmlRoot(ElementName = "group")]
    public class Group //: IEnumerable
    {
        [XmlElement(ElementName = "check")]
        public List<Check> Check { get; set; }
        [XmlAttribute(AttributeName = "enabled")]
        public string Enabled { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string _Name { get; set; }
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "group")]
        public List<Group> GroupNested { get; set; }
        [XmlElement(ElementName = "file")]
        public List<File> File { get; set; }

        public static Group GetDefaultGroup()
        {
            Group group = new Group();
            group.Check = new List<Check>();
            group.GroupNested = new List<Group>();
            group.File = new List<File>();
            return group;
        }

        /*
        public IEnumerator<Group> GetEnumerator()
        {
            yield return this;
            //now go through each childFilter and return that
            foreach (var child in GroupNested)
            {
                foreach (var ch in child)
                {
                    yield return ch;
                }
            }

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }*/
    }

    [XmlRoot(ElementName = "package")]
    public class Package
    {
        [XmlElement(ElementName = "group")]
        public List<Group> Group { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "enabled")]
        public string Enabled { get; set; }
    }

    [XmlRoot(ElementName = "checks_tree")]
    public class Checks_tree
    {
        [XmlElement(ElementName = "package")]
        public List<Package> Package { get; set; }
    }

    [XmlRoot(ElementName = "cstat_settings")]
    public class Cstat_settings
    {
        [XmlElement(ElementName = "cstat_version")]
        public string Cstat_version { get; set; }
        [XmlElement(ElementName = "checks_tree")]
        public Checks_tree Checks_tree { get; set; }
    }

    [XmlRoot(ElementName = "data")]
    public class Data
    {
        [XmlElement(ElementName = "version")]
        public string Version { get; set; }
        [XmlElement(ElementName = "cstatargs")]
        public Cstatargs Cstatargs { get; set; }
        [XmlElement(ElementName = "cstat_settings")]
        public Cstat_settings Cstat_settings { get; set; }
        [XmlElement(ElementName = "wantNonLocal")]
        public string WantNonLocal { get; set; }
        [XmlElement(ElementName = "debug")]
        public string Debug { get; set; }
        [XmlElement(ElementName = "option")]
        public List<Option> Option { get; set; }
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

    [XmlRoot(ElementName = "option")]
    public class Option
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "state")]
        public string State { get; set; }
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

    [XmlRoot(ElementName = "project")]
    public class ProjectEWT
    {
        [XmlElement(ElementName = "fileVersion")]
        public string FileVersion { get; set; }
        [XmlElement(ElementName = "configuration")]
        public Configuration Configuration { get; set; }
        [XmlElement(ElementName = "group")]
        public List<Group> Group { get; set; }
    }
}
