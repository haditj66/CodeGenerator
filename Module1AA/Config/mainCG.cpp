#pragma once

#include "modaaconf.h"
#include "GlobalBuildConfig.h"   

int mainCG()
{  

	GlobalBuildConfig glob;
	glob.Init();
	glob.BUILD->SetValue(BuildEnum::DEBUG);
	glob.PLATFORM->SetValue(PlatformEnum::IAR);

	modaaConf1<ModeEnum::Fast> m;
	m.Init();
	m.SetPublicDefineValue<int>("BUFFERSIZE", 4);// change back to vector and just iterate through to find name
	m.BUFFERSIZE->SetValue(570);
	m.MSG2->SetValue("hello again!!!");

	int y = 90;
	y = y > 35 ? 35 : y;
	y += 24;
	

	m.BUFFERSIZE->SetValue(y);

	/*
	ModAAConf1<ModeEnum::Fast> m11;
	m11.Init();
	m11.BUFFERSIZE->SetValue(455);

	ModAAConf1<ModeEnum::Slow> m2;
	m2.Init();
	m2.BUFFERSIZE->SetValue(1000);
	m2.MSG2->SetValue("wf bla");
	*/
	Config::PrintDefines(m);

	return 1;
}