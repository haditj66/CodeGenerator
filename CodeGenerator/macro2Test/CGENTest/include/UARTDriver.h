//generated file: C:\CodeGenerator\CodeGenerator\macro2Test\CGENTest\include\UARTDriver.h
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

#include "UARTDriver_ServiceGen.h"


//UserCode_Sectionheader
//UserCode_Sectionheader_end

class UARTDriver :   
	 public AEService< 10 , ActionRequestObjectArg1<char const* , bool, 10, UARTDriver>, AENullActionRequest, AENullActionRequest, AENullActionRequest, AENullActionRequest, AENullActionRequest >
{
public: 

	//UserCode_Sectionpublic
//UserCode_Sectionpublic_end
	 
	UARTDriver ()
	{ 
		ActionReq1.ServiceName = "Transmit"; //ActionReq1.ServiceName = "Transmit";
		
		
		
		
		 
		 
		UARTDriver_CTOR
		
		//UserCode_Sectionctor
//UserCode_Sectionctor_end
		 	
	}
	UARTDriver_Service
	
	
	protected: float SomeVar1; 
public: float  GetSomeVar1()   const {return SomeVar1; }
public:  void SetSomeVar1(float  _SomeVar1){SomeVar1 = _SomeVar1; }

protected: int32_t somePer; 
public: int32_t  GetsomePer()   const {return somePer; }
public:  void SetsomePer(int32_t  _somePer){somePer = _somePer; }


public: void UserInitialize(float _SomeVar1 , int32_t _somePer )
	{
		SomeVar1 =  _SomeVar1;
somePer =  _somePer;

		
		//UserCode_SectionuserInited
//UserCode_SectionuserInited_end
		
		userInitialized = true;
	}
private: bool userInitialized = false;
protected: void CheckIfConfiguredProperly() const override  
	{
		//UARTDriver::CheckIfConfiguredProperly();
		if (userInitialized == false)
		{
			// you did not initialize one of your AEobjects
			AEAssertRuntime(userInitialized == true, "user did not call the UserInitailize function for  UARTDriver"); 
		} 
		 
	}
	
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
	  
	  
	  
	 
	  

};




//UserCode_Sectionfunc
//UserCode_Sectionfunc_end