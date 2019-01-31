#pragma once


#include "Config.h"
#include "Defines.h"
#include "IDefine.h"

#include "CGKeywordDefine.h"    
//#define USING_CgenLibraryConf1_H true  //just an example if you want to use another library
#include "AllLibraryIncludes.h"

  

class ModuleBConf0 : public Config
{
public:

	//Put any dependency libraries here and define them --------------------------
	ModuleBConf0 DEPENDS_ON0() 
	END


protected:

		// declare your public defines here --------------------------
		//these defines are meant to be changes from outside libraries and can have mutliple instances if static = false.
		PUBLIC_DEF_DECLARE(BUFFERSIZE, int) //Example
			PUBLIC_DEF_DECLARE(BUFFERSIZE2, int) //Example
		//PUBLIC_DEF_DECLARE(FEATURE, FeatureEnum) //Example
		 
		//define public defines here --------------------------
		PUBLIC_DEF_START
		PUBLIC_DEF_CREATION1(BUFFERSIZE, int, 1814, false)  //Example
		PUBLIC_DEF_CREATION1(BUFFERSIZE2, int, 24, false)  //Example
		//PUBLIC_DEF_CREATION1(FEATURE, FeatureEnum, FeatureEnum::FEATURE2, true) //Example
		END


		//define private defines here --------------------------
		//these defines are only meant to be changed locally and are hidden from other libraries
		PRIVATE_DEF_START
		//PRIVATE_DEF_CREATION1(BUFFERSIZEL, int, 80, true) //Example
		END

		//define template defines here --------------------------
		//these are defines that will change the library interface entirely if changed. they need to match this classes template
		TEMPLATE_DEF_START
		//TEMPLATE_DEF_CREATION(MODE, ModeEnum) // example for a class of template template<ModeEnum MODE>
		END


};