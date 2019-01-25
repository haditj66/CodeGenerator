using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGenerator.IDESettingXMLs;
using System.IO;
using CodeGenerator.IDESettingXMLs.VisualStudioXMLs;

namespace CodeGenerator.ProjectBuilders
{
    public class ProjectBuilderVS : ProjectBuilderBase
    {
        public ProjectBuilderVS(XMLSetting configSettings) : base(configSettings)
        {
        }

        public override MySettingsBase CreateMySettingsBase(Config configClass)
        {
            XMLSettingVSProj settingProj = new XMLSettingVSProj(Path.GetDirectoryName(configClass.FileNameString), ".vcxproj", typeof(IDESettingXMLs.VisualStudioXMLs.Project));

            XMLSetting settingFilter = new XMLSetting(Path.GetDirectoryName(configClass.FileNameString), ".filters", typeof(IDESettingXMLs.VisualStudioXMLs.Filters.Project));

            //create the settings
            return new MySettingsVS(settingFilter, settingProj);
        } 

    }
}
