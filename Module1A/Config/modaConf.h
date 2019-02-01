#pragma once


#include "Config.h"
#include "Defines.h"
#include "IDefine.h"

#include "CGKeywordDefine.h"    
#define USING_ModAAConf1_H true
#include "AllLibraryIncludes.h"



enum FeatureEnum
{
	FEATURE1,
	FEATURE2,
	FEATURE3
};


class modaConf0 : public Config
{
public:
	//ModConf1()(char* prefix, int major) : Config(prefix, major) {}

	modaConf0 DEPENDS_ON1(modaaConf1<ModeEnum::Fast>*, maa)
	//modaConf0 DEPENDS_ON0()
		//dependency is Mod1AA  
		
		maa->Init();
	maa->BUFFERSIZE->SetValue(1800);
	maa->MSG1->SetValue("from ModA using ModAA");
	maa->MSG2->SetValue("ascavde");
	END


protected:

	PUBLIC_DEF_DECLARE(BUFFERSIZE, int)
		PUBLIC_DEF_DECLARE(FEATURE, FeatureEnum)
		PUBLIC_DEF_DECLARE(MSG1, std::string)

		PUBLIC_DEF_START
		PUBLIC_DEF_CREATION1(BUFFERSIZE, int, 2000, false)
		PUBLIC_DEF_CREATION1(MSG1, std::string, "from modA", true)
		PUBLIC_DEF_CREATION1(FEATURE, FeatureEnum, FeatureEnum::FEATURE2, true)
		END


		PRIVATE_DEF_START
		PRIVATE_DEF_CREATION1(BUFFERSIZEL, int, 80, true)
		END


		TEMPLATE_DEF_START
		END


};