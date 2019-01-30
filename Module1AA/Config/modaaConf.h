#pragma once
#include "Config.h"
#include "Defines.h"
#include "IDefine.h"



enum ModeEnum
{
	Fast,
	Slow
};


template<ModeEnum MODE>
class modaaConf1 :
	public Config
{
public:
	//ModConf1()(char* prefix, int major) : Config(prefix, major) {}
	modaaConf1() : Config()
	{
	}
	 

protected: 

	PUBLIC_DEF_DECLARE(BUFFERSIZE, int)
		PUBLIC_DEF_DECLARE(MSG1, std::string)
		PUBLIC_DEF_DECLARE(MSG2, std::string)
		//STATIC_DEF_DECLARE(BLAFEATURE,int)
		//static Defines<int>* BLAFEATURE;

		PUBLIC_DEF_START
		PUBLIC_DEF_CREATION1(BUFFERSIZE, int, 1000, false)
		PUBLIC_DEF_CREATION1(MSG1, std::string, "kihi", true)
		PUBLIC_DEF_CREATION2(MSG2, std::string)

		END


		PRIVATE_DEF_START
		PRIVATE_DEF_CREATION1(BUFFERSIZELOCAL, int, 50,true)
		END


		TEMPLATE_DEF_START
		TEMPLATE_DEF_CREATION(MODE, ModeEnum)
		END


		//STATIC_DEF_START
		//STATIC_DEF_CREATION(BLAFEATURE,int,3)
		//DEF_END


};

//template <int MODE>
//Defines<int>* ModConf1<MODE>::BLAFEATURE;
