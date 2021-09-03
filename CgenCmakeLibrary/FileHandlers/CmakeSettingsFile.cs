using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace CgenCmakeLibrary.FileHandlers
{


    [XmlRoot(ElementName = "CmakeSettings")]
    public class CmakeSettings
    {
        [XmlElement(ElementName = "CmakeLocation")]
        public string CmakeLocation;

        [XmlElement(ElementName = "Generator")]
        public string Generator;

        [XmlElement(ElementName = "CmakeOptions")]
        public string CmakeOptions;

        [XmlElement(ElementName = "IsRemotePC")]
        public bool IsRemotePC;

        [XmlElement(ElementName = "RemoteWorkingDirectory")]
        public string RemoteWorkingDirectory;

        [XmlElement(ElementName = "RemoteIPAddress")]
        public string RemoteIPAddress;

        [XmlElement(ElementName = "RemoteUsername")]
        public string RemoteUsername;

        [XmlElement(ElementName = "RemotePassword")]
        public string RemotePassword;


    }


    public class CmakeSettingsFile : FileHandler
    {

        //from base
        //public bool IsFileContentsFilled(); 
        //public string GetContents();
        //public void RemoveContents();
        public CmakeSettings CmakeSettingsData;

        public CmakeSettingsFile(DirectoryInfo dir, string cMAKE_CURRENT_SOURCE_DIR, string cMAKE_CURRENT_BINARY_DIR) : base(dir, "UserCmakeCommand.xml")
        {
            CMAKE_CURRENT_SOURCE_DIR = cMAKE_CURRENT_SOURCE_DIR;
            CMAKE_CURRENT_BINARY_DIR = cMAKE_CURRENT_BINARY_DIR;
            IsDataLoaded = false;
        }

        public void LoadData()
        {

            if (IsFileContentsFilled() == false)
            {
                IsDataLoaded = IsDataLoaded == true ? true : false;
                return;
            }

            XmlSerializer xmlser = new XmlSerializer(typeof(CmakeSettings));
            using (FileStream fs = new FileStream(this.FullFilePath, FileMode.Open))
            {
                try
                {
                    CmakeSettingsData = (CmakeSettings)xmlser.Deserialize(fs);
                    IsDataLoaded = true;
                }
                catch (System.InvalidOperationException)
                {
                    IsDataLoaded = IsDataLoaded == true ? true : false;
                    return;
                }

            }

        }


        public void SaveData(CmakeSettings dataToSave)
        {
            CmakeSettingsData = dataToSave;

            XmlSerializer xmlser = new XmlSerializer(typeof(CmakeSettings));
            using (FileStream fs = new FileStream(this.FullFilePath, FileMode.Create))
            {
                xmlser.Serialize(fs, CmakeSettingsData);
                IsDataLoaded = true;
            }
        }

        public string getCmakeCmd()
        {

            string cmd;

            if (IsDataLoaded == false)
            {
                cmd = "";
                return cmd;
            }
            cmd =  CmakeSettingsData.CmakeLocation
            + " -G \"" + CmakeSettingsData.Generator + "\""
            + " -S " + CMAKE_CURRENT_SOURCE_DIR
            + " -B " + CMAKE_CURRENT_BINARY_DIR
            + "  " + CmakeSettingsData.CmakeOptions; //" -DCGEN_GUI_SET=FALSE" + 

            return cmd;
        }

        public string CMAKE_CURRENT_SOURCE_DIR { get; }
        public string CMAKE_CURRENT_BINARY_DIR { get; }


        public bool IsDataLoaded { get; protected set; }
        /*
CMAKE_LOCATION: C:\Users\Hadi\AppData\Local\VisualGDB\CMake\bin\cmake.exe
GENERATOR: -G "Ninja" 
CMAKE_OPTIONS: -DCMAKE_BUILD_TYPE=DEBUG -DTOOLCHAIN_ROOT=C:/SysGCC/mingw32 -DCMAKE_MAKE_PROGRAM="C:/Program Files (x86)/Sysprogs/VisualGDB/ninja.exe" -DCMAKE_TOOLCHAIN_FILE=C:\visualgdb_projects\AERTOSCopy/build/Simulation/Debug/toolchain.cmake -DPLATFORM=Simulation
        */

    }
}
