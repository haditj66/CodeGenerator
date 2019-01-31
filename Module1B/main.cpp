#pragma once

#include "Config/ModuleBconf.h"
#include "GlobalBuildConfig.h"   

int main()
{

	GlobalBuildConfig glob;
	glob.Init();
	glob.BUILD->SetValue(BuildEnum::DEBUG);
	glob.PLATFORM->SetValue(PlatformEnum::VS);


	ModuleBConf0  ModuleB;
	ModuleB.Init(); //dont forget to init() every library
	int p = 35;
	p += 7;
	ModuleB.BUFFERSIZE->SetValue(p);

	Config::PrintDefines(ModuleB);
	return 1;
}