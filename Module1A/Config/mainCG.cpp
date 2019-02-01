#pragma once

#include "modaconf.h"
#include "GlobalBuildConfig.h"   

int mainCG()
{  
	
	GlobalBuildConfig glob;
	glob.Init();
	glob.BUILD->SetValue(BuildEnum::DEBUG);
	glob.PLATFORM->SetValue(PlatformEnum::VS);

	//over here. time to get dependencies to work. folders and filters should be created first as I have already figured out. then files should be put in those respective folders
	
	modaaConf1<ModeEnum::Fast> maa;
	maa.Init();

	modaConf0 moda(&maa); 
	moda.Init(); //dont forget to init() every library


	Config::PrintDefines(moda);
	 return 1;
}