//generated file: C:/CodeGenerator/CodeGenerator/macro2Test/CGENTest\include\BlindsUITOPFSM.h
//**********************************************************************
//this is an auto-generated file using the template file located in the directory of C:\CodeGenerator\CodeGenerator\bin\Debug\..\..\FileTemplates\Files
//ONLY WRITE CODE IN THE UserCode_Section BLOCKS
//If you write code anywhere else,  it will be overwritten. modify the actual template file if needing to modify code outside usersection blocks.

//############################################### 
//this is an autogenerated file using cgen's macro2 command. Dont modify this file here unless it is in user sections 
//###############################################

#pragma once
 
#include "AEClock.h"

#include "AESimpleFSM.h"
 
#include "AEEventBase.h" 

#include "ConfiguringFSM.h"
#include "NormalOperationFSM.h"


 

//UserCode_Sectionheader
extern int StateTracker;
//UserCode_Sectionheader_end
 

class BlindsUITOPFSM : public AESimpleFSM<BlindsUITOPFSM, ConfiguringFSM, NormalOperationFSM>
{
public:
	BlindsUITOPFSM() 
	{
	//UserCode_Sectionctor
//UserCode_Sectionctor_end
	};

	//UserCode_Sectionpublic
//UserCode_Sectionpublic_end

	static AETransitionType Idle(TypeOfThisFSM* const thisFSM, AEEventDiscriminator_t const * const evt);
static AETransitionType Configuring(TypeOfThisFSM* const thisFSM, AEEventDiscriminator_t const * const evt);
static AETransitionType NormalOperating(TypeOfThisFSM* const thisFSM, AEEventDiscriminator_t const * const evt);
 


	// Inherited via AESimpleFSM
	StateFuncPtr InitialState() override;
	
protected:

	//UserCode_Sectionprot
//UserCode_Sectionprot_end

};


//UserCode_Sectionfunc
//UserCode_Sectionfunc_end
 



//state: Idle ======================================================
inline AETransitionType BlindsUITOPFSM::Idle(TypeOfThisFSM * const thisFSM, AEEventDiscriminator_t const * const evt)
{
	AETransitionType transitionType = UNHANDLED;

	//UserCode_SectionIdlea
//UserCode_SectionIdlea_end
	
	
	auto evtId = evt->GetEvtID();
	switch (evtId)
	{
	case Enter_Sig:
	{
         thisFSM->Subscribe<Button1>();
         thisFSM->Subscribe<Button2>();
         thisFSM->Subscribe<Button3>();
 
		
	//UserCode_SectionIdleenter
		StateTracker = 10;
//UserCode_SectionIdleenter_end
		 
		 
		break;
	}
	case Exit_Sig:
	{
          thisFSM->UnSubscribe<Button1>();
          thisFSM->UnSubscribe<Button2>();
          thisFSM->UnSubscribe<Button3>();
 
		
	//UserCode_SectionIdleexit
//UserCode_SectionIdleexit_end
		
		break;
	}
	

// Button1 ------------------------------------------------------------
    case Button1:
     {
    //UserCode_SectionIdleButton1
	     AEPrint("\nBlindsUITopFSM:  Idle:Button1\n");
//UserCode_SectionIdleButton1_end
        TRANSITION_TOSTATE(&Configuring)
        break;
     }


// Button2 ------------------------------------------------------------
    case Button2:
     {
    //UserCode_SectionIdleButton2
	     AEPrint("\nBlindsUITopFSM:  Idle:Button2\n");
//UserCode_SectionIdleButton2_end
        TRANSITION_TOSTATE(&NormalOperating)
        break;
     }


// Button3 ------------------------------------------------------------
    case Button3:
     {
    //UserCode_SectionIdleButton3
	     AEPrint("\nBlindsUITopFSM:  Idle:Button3\n");
//UserCode_SectionIdleButton3_end
        break;
     }

 
	
	default:
		break;
	}

	return transitionType;
}


//state: Configuring ======================================================
inline AETransitionType BlindsUITOPFSM::Configuring(TypeOfThisFSM * const thisFSM, AEEventDiscriminator_t const * const evt)
{
	AETransitionType transitionType = UNHANDLED;

	//UserCode_SectionConfiguringa
//UserCode_SectionConfiguringa_end
	
	
	auto evtId = evt->GetEvtID();
	switch (evtId)
	{
	case Enter_Sig:
	{
         thisFSM->Subscribe<Button1>();
         thisFSM->Subscribe<Button2>();
         thisFSM->Subscribe<Button3>();
         thisFSM->StartUpdateTimer(100);
         thisFSM->ActivateSubmachine(thisFSM->SubMachine1);
 
		
	//UserCode_SectionConfiguringenter
//UserCode_SectionConfiguringenter_end
		 
		 
		break;
	}
	case Exit_Sig:
	{
          thisFSM->UnSubscribe<Button1>();
          thisFSM->UnSubscribe<Button2>();
          thisFSM->UnSubscribe<Button3>();
          thisFSM->StopUpdateTimer();
         thisFSM->DeActivateSubmachine(thisFSM->SubMachine1);
 
		
	//UserCode_SectionConfiguringexit
//UserCode_SectionConfiguringexit_end
		
		break;
	}
	

// Button1 ------------------------------------------------------------
    case Button1:
     {
    //UserCode_SectionConfiguringButton1
	     AEPrint("\nBlindsUITopFSM:  Configuring:Button1\n");
//UserCode_SectionConfiguringButton1_end
        break;
     }


// Button2 ------------------------------------------------------------
    case Button2:
     {
    //UserCode_SectionConfiguringButton2
	     AEPrint("\nBlindsUITopFSM:  Configuring:Button2\n");
//UserCode_SectionConfiguringButton2_end
        break;
     }


// Button3 ------------------------------------------------------------
    case Button3:
     {
    //UserCode_SectionConfiguringButton3
	     AEPrint("\nBlindsUITopFSM:  Configuring:Button3\n");
//UserCode_SectionConfiguringButton3_end
        TRANSITION_TOSTATE(&Idle)
        break;
     }

 
	
	default:
		break;
	}

	return transitionType;
}


//state: NormalOperating ======================================================
inline AETransitionType BlindsUITOPFSM::NormalOperating(TypeOfThisFSM * const thisFSM, AEEventDiscriminator_t const * const evt)
{
	AETransitionType transitionType = UNHANDLED;

	//UserCode_SectionNormalOperatinga
//UserCode_SectionNormalOperatinga_end
	
	
	auto evtId = evt->GetEvtID();
	switch (evtId)
	{
	case Enter_Sig:
	{
         thisFSM->Subscribe<Button1>();
         thisFSM->Subscribe<Button2>();
         thisFSM->Subscribe<Button3>();
         thisFSM->StartUpdateTimer(100);
         thisFSM->ActivateSubmachine(thisFSM->SubMachine2);
 
		
	//UserCode_SectionNormalOperatingenter
//UserCode_SectionNormalOperatingenter_end
		 
		 
		break;
	}
	case Exit_Sig:
	{
          thisFSM->UnSubscribe<Button1>();
          thisFSM->UnSubscribe<Button2>();
          thisFSM->UnSubscribe<Button3>();
          thisFSM->StopUpdateTimer();
         thisFSM->DeActivateSubmachine(thisFSM->SubMachine2);
 
		
	//UserCode_SectionNormalOperatingexit
//UserCode_SectionNormalOperatingexit_end
		
		break;
	}
	

// Button1 ------------------------------------------------------------
    case Button1:
     {
    //UserCode_SectionNormalOperatingButton1
	     AEPrint("\nBlindsUITopFSM:  NormalOperating:Button1\n");
//UserCode_SectionNormalOperatingButton1_end
        break;
     }


// Button2 ------------------------------------------------------------
    case Button2:
     {
    //UserCode_SectionNormalOperatingButton2
	     AEPrint("\nBlindsUITopFSM:  NormalOperating:Button2\n");
//UserCode_SectionNormalOperatingButton2_end
        break;
     }


// Button3 ------------------------------------------------------------
    case Button3:
     {
    //UserCode_SectionNormalOperatingButton3
	     AEPrint("\nBlindsUITopFSM:  NormalOperating:Button3\n");
//UserCode_SectionNormalOperatingButton3_end
        TRANSITION_TOSTATE(&Idle)
        break;
     }

 
	
	default:
		break;
	}

	return transitionType;
} 


inline BlindsUITOPFSM::StateFuncPtr BlindsUITOPFSM::InitialState()
{
	return CastToStateFuncPtr(&Idle);
}
