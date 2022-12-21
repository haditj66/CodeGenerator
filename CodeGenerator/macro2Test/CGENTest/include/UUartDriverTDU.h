//generated file: C:\CodeGenerator\CodeGenerator\macro2Test\CGENTest\include\UUartDriverTDU.h
//**********************************************************************
//this is an auto-generated file using the template file located in the directory of C:\CodeGenerator\CodeGenerator\bin\Debug\..\..\FileTemplates\Files
//ONLY WRITE CODE IN THE UserCode_Section BLOCKS
//If you write code anywhere else,  it will be overwritten. modify the actual template file if needing to modify code outside usersection blocks.

//############################################### 
//this is an autogenerated file using cgen's macro2 command. Dont modify this file here unless it is in user sections 
//###############################################


#pragma once

#include "AEUtilityAsService.h" 

#include "AEPublishSubscribeManager.h" 

#include "ActionRequestObjectArgTDU.h"
 
#include "AE_TDUAsService.h" 

#include "UUartDriverTDU_ServiceGen.h"


//UserCode_Sectionheader
//UserCode_Sectionheader_end

class UUartDriverTDU :   
	 public AEService< 10 , ActionRequestObjectArg1<char const* , bool, 10, UUartDriverTDU>, AENullActionRequest, AENullActionRequest, ActionRequestObjectArgTDU1<char* , int8_t, 10, UUartDriverTDU>, AENullActionRequest, AENullActionRequest >
{
public: 

	//UserCode_Sectionpublic
//UserCode_Sectionpublic_end
	 
	UUartDriverTDU ()
	{ 
		ActionReq1.ServiceName = "Transmit"; //ActionReq1.ServiceName = "Transmit";
		
		
		ActionReq4.ServiceName = "TransmitTDU";
		
		 
		 
		UUartDriverTDU_CTOR
		
		//UserCode_Sectionctor
//UserCode_Sectionctor_end
		 	
	}
	UUartDriverTDU_Service
	
	
	
	
protected: 
	
	//UserCode_Sectionprot
//UserCode_Sectionprot_end
	
	
	//example for waiting on an event
	//actReq->WaitForEvent<SomeOther_Sig>(
	//				[](void* s, AEEventDiscriminator_t* evt)->void {
	//					//SomeOther_Sig* sssc = (SomeOther_Sig*)evt->evt;
	//					AEPrint("Event SomeOther_Sig recieved"); 
	//					return;
	//				}
	//				, 500);
	
	//dont forget to return the service value when finished
	//actReq->SetReturnArg(1); 
	
	//return true in the update function for tdus when the service is done.
	
	//=====================================
	//Normal services
	//=====================================
	void _Transmit(char const* msg) 
   {

         //UserCode_SectionTransmit
//UserCode_SectionTransmit_end 
   }
  
	  
	  
	
	
	//=====================================
	//TDU services
	//=====================================
	void _TransmitTDU(char* msg) 
   {

         //UserCode_SectionTransmitTDU
//UserCode_SectionTransmitTDU_end 
   }

    bool _TransmitTDUUpdate(ActionRequestObjectArgTDU1<char* , int8_t, 10, UUartDriverTDU>* request, char* msg) 
   {

         //UserCode_SectionTransmitTDUu
	   AEPrint("entered tdu with msg: %s", msg);
//UserCode_SectionTransmitTDUu_end 
    return true; 
    }
  
	  
	  
	 
	  

};




//UserCode_Sectionfunc
//UserCode_Sectionfunc_end