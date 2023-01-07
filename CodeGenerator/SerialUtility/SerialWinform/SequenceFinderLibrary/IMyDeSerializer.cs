 
using MatFileHandler;
using System.Collections.Generic;

namespace HolterMonitorGui
{
    public interface IMyDeSerializer
    {
        IMatFile ConvertMyDeserializeVarsToMatFile();
        bool Deserialize(List<byte> data);
        int NumOfVars { get; set; }
    }
     

}