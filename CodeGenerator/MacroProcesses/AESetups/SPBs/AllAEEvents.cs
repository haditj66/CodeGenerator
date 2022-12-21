using CgenMin.MacroProcesses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.MacroProcesses.AESetups.SPBs
{
    public class I2C_RXCpltEVT : AEEventEVT<I2C_RXCpltEVT>
    { 
        public I2C_RXCpltEVT() 
            : base( "I2C_RXCpltEVT", 
            "uint8_t forI2C;"
            )
        {
        }
         
    }


    public class Button1 : AEEventSignal<Button1>
    {
        public Button1(   ) :
            base( "Button1", 
            ""
            )
        {

        } 
    }

    public class Button2 : AEEventSignal<Button2>
    {
        public Button2(   ) :
            base( "Button2",  
            ""
            )
        {

        } 
    }
    public class Button3 : AEEventSignal<Button3>
    {
        public Button3(   ) :
            base( "Button3",  
            ""
            )
        {

        }
         
    }

    public class UpdateEVT : AEEventSignal<UpdateEVT>
    {
        public UpdateEVT(   ) :
            base( "UpdateEVT",  
            ""
            )
        {

        }
         
    }
     
}
