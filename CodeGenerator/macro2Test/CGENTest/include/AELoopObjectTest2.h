//generated file: C:/CodeGenerator/CodeGenerator/macro2Test/CGENTest\include\AELoopObjectTest2.h
//**********************************************************************
//this is an auto-generated file using the template file located in the directory of C:\CodeGenerator\CodeGenerator\bin\Debug\..\..\FileTemplates\Files
//ONLY WRITE CODE IN THE UserCode_Section BLOCKS
//If you write code anywhere else,  it will be overwritten. modify the actual template file if needing to modify code outside usersection blocks.

//############################################### 
//this is an autogenerated file using cgen's macro2 command. Dont modify this file here unless it is in user sections 
//###############################################

#pragma once
 
#include "AELoopObject.h" 
#include  "AEObjects.h"   

//UserCode_Sectionheader

#include "SomeUtility.h"
extern int StateTracker;
//UserCode_Sectionheader_end

class AELoopObjectTest2 : public AELoopObject
{
	
public:
	AELoopObjectTest2() { 
		  //UserCode_Sectionctor
//UserCode_Sectionctor_end
	} 
	
 //UserCode_Sectionpublic
//UserCode_Sectionpublic_end
	


protected: 
	
		void StartAOLoopObject()  override
	{
//		AELoopSubscribe(Button1, AELoopObject1Test, Button1_Callback);
		//UserCode_Sectionstartae
//UserCode_Sectionstartae_end
	}
	
	void Update() override {
		
		//UserCode_Sectionupdate
		AEPrint("tttttttttttttt\n");
		
		
		
		
		if (swapperind == 0)
		{
			//AEITEST_AECoreTestEXE("FirstState", StateTracker == 0, "initially StateTracker is 0")
			AEITEST_CGENTest("FirstState", StateTracker == 10, "initially BlindsUI FSM is in idle state")
			PublishEvt(&Button1_Instance);
		}
		else if (swapperind == 1)
		{
			AEITEST_CGENTest("SecondState", StateTracker == 20, "ConfigBlindsUI FSM is in idle state")
			PublishEvt(&Button3_Instance);	
		}
		else if (swapperind == 2)
		{
			AEITEST_CGENTest("ThirdState", StateTracker == 30, "ConfigBlindsUI FSM is in SettingTopLimit state")
			PublishEvt(&Button3_Instance);	
		}		
		else if (swapperind == 3)
		{
			AEITEST_CGENTest("FourthState", StateTracker == 40, "ConfigBlindsUI FSM is in SettingBottomLimit state")
			PublishEvt(&Button3_Instance);	 
		}
		else if (swapperind == 4)
		{ 
			PublishEvt(&Button3_Instance);	 
		}
		else if (swapperind == 5)
		{
			AEITEST_CGENTest("fifthState", StateTracker == 10, "ConfigBlindsUI FSM Has left and entered BlindsUI FSM idle state")
			PublishEvt(&Button2_Instance);	
			//PublishEvt(&Button2_Sig_Instance);	
		}
		else if (swapperind == 6)
		{
			AEITEST_CGENTest("sixthState", StateTracker == 50, "ConfigBlindsUI FSM Has left and entered BlindsUI FSM idle state")
			
			//PublishEvt(&Button2_Sig_Instance);	
		}
		
		//swapperind = swapperind > 5 ?  0 : (swapperind +1);
		swapperind++;
//UserCode_Sectionupdate_end
		 
	} 
	
	//UserCode_Sectionprotect
	int swapperind = 0;
//UserCode_Sectionprotect_end
	
};


//UserCode_Sectionfuncs
//UserCode_Sectionfuncs_end