#pragma once

#include "ModuleBconf.h"
#include "GlobalBuildConfig.h"   

int main()
{  

	GlobalBuildConfig glob;
	glob.Init();
	glob.BUILD->SetValue(BuildEnum::DEBUG);
	glob.PLATFORM->SetValue(PlatformEnum::VS);


	ModuleBConf0  ModuleB;
	ModuleB.Init(); //dont forget to init() every library


	Config::PrintDefines(ModuleB);
	 return 1;
}