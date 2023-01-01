//generated file: C:/CodeGenerator/CodeGenerator/macro2Test/CGENTest\include\AdderSPB.h
//**********************************************************************
//this is an auto-generated file using the template file located in the directory of C:\CodeGenerator\CodeGenerator\bin\Debug\..\..\FileTemplates\Files
//ONLY WRITE CODE IN THE UserCode_Section BLOCKS
//If you write code anywhere else,  it will be overwritten. modify the actual template file if needing to modify code outside usersection blocks.

//############################################### 
//this is an autogenerated file using cgen's macro2 command. Dont modify this file here unless it is in user sections 
//###############################################

#pragma once


#include "AESPBObservorOutputType.h"
#include "AEUtils.h"
#include "FreeRTOS.h"
#include "AEFilter.h"

#include "AEIntegrationTesting.h"

//UserCode_Sectionheader
//UserCode_Sectionheader_end

template< uint16_t NUMOFINPUTSIGNALS, bool isSubscribable, uint32_t CHANNELCOUNTBUFFER, TEMPLATESPB_Filters>
	class AdderSPB :
		public AESPBObservorOutputType <1, NUMOFINPUTSIGNALS, isSubscribable,
TEMPLATESPB_FilterParams,
CHANNELCOUNTBUFFER, false,
CHANNELCOUNTBUFFER, false,
CHANNELCOUNTBUFFER, false,
CHANNELCOUNTBUFFER, false,
CHANNELCOUNTBUFFER, false,
CHANNELCOUNTBUFFER, false,
CHANNELCOUNTBUFFER, false,
CHANNELCOUNTBUFFER, false,
CHANNELCOUNTBUFFER, false,
CHANNELCOUNTBUFFER, false,
CHANNELCOUNTBUFFER, false,
CHANNELCOUNTBUFFER, false>
	{
	public:
		AdderSPB();
		
		//UserCode_Sectionpublic
//UserCode_Sectionpublic_end


	
	
	private:
	//UserCode_Sectionprivate
//UserCode_Sectionprivate_end

		void RefreshOVERRIDE(float OutputSignal[1]) override;

	};


//UserCode_Sectionfunc
//UserCode_Sectionfunc_end


template< uint16_t NUMOFINPUTSIGNALS, bool isSubscribable, uint32_t CHANNELCOUNTBUFFER, TEMPLATESPB_FiltersFunctionParams>
	inline AdderSPB<  NUMOFINPUTSIGNALS,  isSubscribable,  CHANNELCOUNTBUFFER, TEMPLATESPB_FilterParams>::AdderSPB()
	{
		//UserCode_Sectionctor
//UserCode_Sectionctor_end
	}

template< uint16_t NUMOFINPUTSIGNALS, bool isSubscribable, uint32_t CHANNELCOUNTBUFFER, TEMPLATESPB_FiltersFunctionParams>
	inline void AdderSPB<  NUMOFINPUTSIGNALS,  isSubscribable,  CHANNELCOUNTBUFFER, TEMPLATESPB_FilterParams>::RefreshOVERRIDE(float OutputSignal[1])
	{

		float* AllChannels[NUMOFINPUTSIGNALS]; 
       for (int i = 0; i < NUMOFINPUTSIGNALS; i++) 
        {        
           AllChannels[i] = this->InputChannels[i]->ChannelSignalBufferSingle; 
        }       

 
		//UserCode_Sectionrefresh
		
		for (float* aa : AllChannels) 
		{        
			OutputSignal[0] = aa[0]; 
		}   
		
//UserCode_Sectionrefresh_end
	}

