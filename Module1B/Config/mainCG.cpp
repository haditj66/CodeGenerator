#pragma once

#include "ModuleBconf.h"
#include "GlobalBuildConfig.h"   
#include <ctime> 
#undef RAND_MAX
#define RAND_MAX 100

int mainCG()
{  

	GlobalBuildConfig glob;
	glob.Init();
	glob.BUILD->SetValue(BuildEnum::DEBUG);
	glob.PLATFORM->SetValue(PlatformEnum::VS);


	ModuleBConf0  ModuleB;
	ModuleB.Init(); //dont forget to init() every library 
	std::srand((unsigned)std::time(0));
	int p; 
	p = std::rand()%329;//random from 0 to 329

	ModuleB.BUFFERSIZE->SetValue(p);

	Config::PrintDefines(ModuleB);
	 return 1;
}