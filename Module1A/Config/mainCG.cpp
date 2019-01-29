#pragma once

#include "modaconf.h"
#include "GlobalBuildConfig.h"   

int mainCG()
{  

	GlobalBuildConfig glob;
	glob.Init();
	glob.BUILD->SetValue(BuildEnum::DEBUG);
	glob.PLATFORM->SetValue(PlatformEnum::VS);


	//modaConf0  moda;
	//moda.Init(); //dont forget to init() every library


	//Config::PrintDefines(moda);
	 return 1;
}