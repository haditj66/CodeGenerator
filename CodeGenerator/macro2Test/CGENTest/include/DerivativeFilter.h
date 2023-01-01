//generated file: C:/CodeGenerator/CodeGenerator/macro2Test/CGENTest\include\DerivativeFilter.h
//**********************************************************************
//this is an auto-generated file using the template file located in the directory of C:\CodeGenerator\CodeGenerator\bin\Debug\..\..\FileTemplates\Files
//ONLY WRITE CODE IN THE UserCode_Section BLOCKS
//If you write code anywhere else,  it will be overwritten. modify the actual template file if needing to modify code outside usersection blocks.

//############################################### 
//this is an autogenerated file using cgen's macro2 command. Dont modify this file here unless it is in user sections 
//###############################################

#pragma once 

#include <cstdint>
#include "AEFilter.h"


//UserCode_Sectionheader
//UserCode_Sectionheader_end


class DerivativeFilter : public Filter<DerivativeFilter, 2>
{
  //template<class TFilterDerived, uint16_t PastDataBufferSize>
	friend class Filter;

public:

//UserCode_Sectionpubl
//UserCode_Sectionpubl_end

	DerivativeFilter();

protected:

//UserCode_Sectionprot
//UserCode_Sectionprot_end
	
	void InitializeImpl(float samplingPeriodOfObservorInSeconds);

private:
	float RunFilter(float newestInput);
	
	//UserCode_Sectionpriv
//UserCode_Sectionpriv_end
	

};


inline DerivativeFilter::DerivativeFilter()
{
	//UserCode_Sectionctor
//UserCode_Sectionctor_end
}
 

inline void DerivativeFilter::InitializeImpl(float samplingPeriodOfObservorInSeconds)
{
	//UserCode_Sectioninit
//UserCode_Sectioninit_end
}


inline float DerivativeFilter::RunFilter(float newestInput)
{
//PastDataCircularBuffer[0] is the newest input
//example: float der = (PastDataCircularBuffer[0] - PastDataCircularBuffer[1]) / this->SamplingPeriodOfObservorInSeconds;
//return the output of the filter
//return der;

//UserCode_Sectionimpl 
float der = (PastDataCircularBuffer[0] - PastDataCircularBuffer[1]) / this->SamplingPeriodOfObservorInSeconds;
return der;

//UserCode_Sectionimpl_end
 
}


//UserCode_Sectionfunc
//UserCode_Sectionfunc_end