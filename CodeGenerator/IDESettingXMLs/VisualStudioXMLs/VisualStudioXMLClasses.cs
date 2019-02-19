using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CodeGenerator.IDESettingXMLs.VisualStudioXMLs
{
    [Serializable]
    [XmlRoot(ElementName = "ProjectConfiguration", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ProjectConfiguration
    {
        [XmlElement(ElementName = "Configuration", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public string Configuration { get; set; }
        [XmlElement(ElementName = "Platform", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public string Platform { get; set; }
        [XmlAttribute(AttributeName = "Include")]
        public string Include { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "ItemGroup", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ItemGroup
    {
        [XmlElement(ElementName = "ProjectConfiguration", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003" ,Order = 1)]
        public List<ProjectConfiguration> ProjectConfiguration { get; set; }
        [XmlAttribute(AttributeName = "Label")]
        public string Label { get; set; }
        [XmlElement(ElementName = "ClInclude", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003", Order = 2)]
        public List<ClInclude> ClInclude { get; set; }
        [XmlElement(ElementName = "ClCompile", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003", Order = 3)]
        public List<ClCompile> ClCompile { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "PropertyGroup", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class PropertyGroup
    {
        [XmlElement(ElementName = "VCProjectVersion", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public string VCProjectVersion { get; set; }
        [XmlElement(ElementName = "ProjectGuid", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public string ProjectGuid { get; set; }
        [XmlElement(ElementName = "RootNamespace", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public string RootNamespace { get; set; }
        [XmlElement(ElementName = "WindowsTargetPlatformVersion", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public string WindowsTargetPlatformVersion { get; set; }
        [XmlAttribute(AttributeName = "Label")]
        public string Label { get; set; }
        [XmlElement(ElementName = "ConfigurationType", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public string ConfigurationType { get; set; }
        [XmlElement(ElementName = "UseDebugLibraries", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public string UseDebugLibraries { get; set; }
        [XmlElement(ElementName = "PlatformToolset", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public string PlatformToolset { get; set; }
        [XmlElement(ElementName = "CharacterSet", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public string CharacterSet { get; set; }
        [XmlAttribute(AttributeName = "Condition")]
        public string Condition { get; set; }
        [XmlElement(ElementName = "WholeProgramOptimization", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public string WholeProgramOptimization { get; set; }
        [XmlElement(ElementName = "IncludePath", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public string IncludePath { get; set; }
        [XmlElement(ElementName = "LibraryPath", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public string LibraryPath { get; set; }
        [XmlElement(ElementName = "SourcePath", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public string SourcePath { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "Import", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class Import
    {
        [XmlAttribute(AttributeName = "Project")]
        public string Project { get; set; }
        [XmlAttribute(AttributeName = "Condition")]
        public string Condition { get; set; }
        [XmlAttribute(AttributeName = "Label")]
        public string Label { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "ImportGroup", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ImportGroup
    {
        [XmlAttribute(AttributeName = "Label")]
        public string Label { get; set; }
        [XmlElement(ElementName = "Import", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public Import Import { get; set; }
        [XmlAttribute(AttributeName = "Condition")]
        public string Condition { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "ClCompile", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ClCompile
    {
        [XmlElement(ElementName = "WarningLevel", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public string WarningLevel { get; set; }
        [XmlElement(ElementName = "Optimization", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public string Optimization { get; set; }
        [XmlElement(ElementName = "SDLCheck", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public string SDLCheck { get; set; }
        [XmlElement(ElementName = "AdditionalIncludeDirectories", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public string AdditionalIncludeDirectories { get; set; }
        [XmlElement(ElementName = "FunctionLevelLinking", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public string FunctionLevelLinking { get; set; }
        [XmlElement(ElementName = "IntrinsicFunctions", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public string IntrinsicFunctions { get; set; }
        [XmlAttribute(AttributeName = "Include")]
        public string Include { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "Link", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class Link
    {
        [XmlElement(ElementName = "AdditionalDependencies", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public string AdditionalDependencies { get; set; }
        [XmlElement(ElementName = "AdditionalLibraryDirectories", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public string AdditionalLibraryDirectories { get; set; }
        [XmlElement(ElementName = "EnableCOMDATFolding", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public string EnableCOMDATFolding { get; set; }
        [XmlElement(ElementName = "OptimizeReferences", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public string OptimizeReferences { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "ItemDefinitionGroup", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ItemDefinitionGroup
    {
        [XmlElement(ElementName = "ClCompile", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public ClCompile ClCompile { get; set; }
        [XmlElement(ElementName = "Link", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public Link Link { get; set; }
        [XmlAttribute(AttributeName = "Condition")]
        public string Condition { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "ClInclude", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class ClInclude
    {
        [XmlAttribute(AttributeName = "Include")]
        public string Include { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "Project", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
    public class Project
    {
        [XmlElement(ElementName = "ItemGroup", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public List<ItemGroup> ItemGroup { get; set; }
        [XmlElement(ElementName = "PropertyGroup", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public List<PropertyGroup> PropertyGroup { get; set; }
        [XmlElement(ElementName = "Import", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public List<Import> Import { get; set; }
        [XmlElement(ElementName = "ImportGroup", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public List<ImportGroup> ImportGroup { get; set; }
        [XmlElement(ElementName = "ItemDefinitionGroup", Namespace = "http://schemas.microsoft.com/developer/msbuild/2003")]
        public List<ItemDefinitionGroup> ItemDefinitionGroup { get; set; }
        [XmlAttribute(AttributeName = "DefaultTargets")]
        public string DefaultTargets { get; set; }
        [XmlAttribute(AttributeName = "ToolsVersion")]
        public string ToolsVersion { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }
}
