 
#pragma once

#include "Config/modaConf.h"//"modaConf.h"
#include "GlobalBuildConfig.h" 
//#include "SomeLibraryClass.h"
#include <iostream> 
#include <thread>

int main() 
{
	/*
	//update 3
	std::cout << "a5sd" << std::endl;
	std::this_thread::sleep_for(std::chrono::milliseconds(1000));
	
	SomeLibraryClass s;
	s.Foo(3, 2);*/

	
	GlobalBuildConfig glob;
	glob.Init();
	glob.BUILD->SetValue(BuildEnum::DEBUG);
	glob.PLATFORM->SetValue(PlatformEnum::VS);

	/*
	modaaConf1<ModeEnum::Fast> maa;
	maa.Init();
	modaConf0 m(&maa);
	
	modaConf0 m;
	m.Init();
	m.FEATURE->SetValue(FeatureEnum::FEATURE3);
	*m.BUFFERSIZE = 676;

	//ModAConf0 m2(&maa);
	//m2.Init();

	Config::PrintDefines(m); 
	 */
	 
} 