#pragma once


#include "GlobalBuildConfig.h"
#include "Config.h"
#include "Defines.h"
#include "IDefine.h"
#include <iostream> 
#include <string>
#include <algorithm>
#include "ModAAConf1.h" 
#include <sstream>
#include <string>
#include <fstream>
//#include "Pre_ClassToInterfaceWith.h"
//#include "SomaLibraryClassM1AA.h"
/*
#define PRINTER(var) printer(#var, ((int)var))
 

void printer(char* name, int value) {
	
	std::string namestr(name);
	auto yy = namestr.find('.');

	std::string varName = namestr.substr(yy+1);
	printf("#define %s %d\n", varName.c_str() , value);
}*/
 


int main()
{ 



	//std::cout << prin << std::endl;
	//LOG("fsf", "asd");

	 /*
	it seems that creating a namespace is the way to go. Just make sure to be careful for if 
		there is already a namespace being used. Also prefixing all FILES NOT CLASSES with some prefix type of 
		library would be best as well also what about hiding all files that are NOT supposed
		to be revealed as the library interface??
		*/
	 
	/*
	PRE::ClassToInterfaceWith ci;
	ci.fooFast("hello");

	PRE::SomaLibraryClassM1AA d(2);
	*/

	
	GlobalBuildConfig glob;
	glob.Init();
	glob.BUILD->SetValue(BuildEnum::DEBUG);
	glob.PLATFORM->SetValue(PlatformEnum::IAR);

	ModAAConf1<ModeEnum::Fast> m;
	m.Init(); 
	m.SetPublicDefineValue<int>("BUFFERSIZE",4);// change back to vector and just iterate through to find name
	m.BUFFERSIZE->SetValue(500);
	m.MSG2->SetValue("hello bla"); 
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
 
 







	//SomaLibraryClassM1AA sss;
	//sss.Goo(8, "sc");
	

	//m.publicDefineList[0].  make this a vector or something instead

	//always initialize the configs that are static first.
	//GlobalBuildConfig buildConf;
	//buildConf.BUILD = BuildEnum::RELEASE;
	//buildConf.PLATFORM = PlatformEnum::VS;

	//initialize the config with no associations next


	//initialize the configs with associations next


	//finally initialize this app or libraries config 
	//Module1AAConfig0<modeEnum::FAST> mAA; //AA0_BUFFERSIZE1
	//ISFEATURE1ENABLE = true; //initialize static members here.


	//PRINTER(buildConf.BUILD);
}

