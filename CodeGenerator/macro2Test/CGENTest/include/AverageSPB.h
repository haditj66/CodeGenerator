//generated file: C:\CodeGenerator\CodeGenerator\macro2Test\CGENTest\include\AverageSPB.h
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

template<bool isSubscribable, uint32_t CHANNELCOUNTBUFFER1, TEMPLATESPB_Filters>
	class AverageSPB :
		public AESPBObservorOutputType <1, 1, isSubscribable,
TEMPLATESPB_FilterParams,
CHANNELCOUNTBUFFER1, false>
	{
	public:
		AverageSPB();
		
		//UserCode_Sectionpublic
//UserCode_Sectionpublic_end

	private:
	//UserCode_Sectionprivate
//UserCode_Sectionprivate_end

		void RefreshOVERRIDE(float OutputSignal[1]) override;

	};


//UserCode_Sectionfunc
//UserCode_Sectionfunc_end


template<bool isSubscribable, uint32_t CHANNELCOUNTBUFFER1, TEMPLATESPB_FiltersFunctionParams>
	inline AverageSPB< isSubscribable,  CHANNELCOUNTBUFFER1, TEMPLATESPB_FilterParams>::AverageSPB()
	{
		//UserCode_Sectionctor
//UserCode_Sectionctor_end
	}

template<bool isSubscribable, uint32_t CHANNELCOUNTBUFFER1, TEMPLATESPB_FiltersFunctionParams>
	inline void AverageSPB< isSubscribable,  CHANNELCOUNTBUFFER1, TEMPLATESPB_FilterParams>::RefreshOVERRIDE(float OutputSignal[1])
	{

		float* ch1 = this->InputChannels[0]->ChannelSignalBufferSingle; 

 
		//UserCode_Sectionrefresh
		
		float average = 0;
		for (int i = 0; i < CHANNELCOUNTBUFFER1; i++)
		{
			average +=  ch1[i];
		}
		average  = average / CHANNELCOUNTBUFFER1;
		
		OutputSignal[0] = average;
		
		//UserCode_Sectionrefresh_end
	}

