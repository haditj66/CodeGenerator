using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CodeGenerator.cgenXMLSaves.SaveFiles
{
    public class SaveFilecgenProject : SaveFileBase
    {
        
        public SaveFilecgenProject() : base()
        {
            
        }

        public SaveFilecgenProject(string fileLocation) : base(fileLocation)
        { 
        }

        public cgenProjects CgenProjects { get { return (cgenXMLSaves.cgenProjects)XMLClassToSave; } }

        protected override string FileNameDefault => this.BaseDirectoryForProjectSaveFiles + "cgenProjs.cgx";

        public override dynamic GetDefaultXMLClass()
        {
            cgenProjects returncgen = new cgenProjects();
            returncgen.Projects = new List<cgenProject>();
            return returncgen;
        }

        protected override XmlSerializer CreateSerializer()
        {
            return new XmlSerializer(typeof(cgenXMLSaves.cgenProjects));
        }


    }
}
