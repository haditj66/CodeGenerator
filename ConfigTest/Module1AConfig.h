#pragma once

#include "Module1AAConfig.h"

//this config is being utilized by module 1 
//can it be utilized by other modules though?

class Module1AConfig : Module1AAConfig
{
public:
	VIRTUAL BuildEnum BUILD = BuildEnum::VS OVERRIDE;
	VIRTUAL int MAJOR = 2 OVERRIDE;
	VIRTUAL int MINOR = 2 OVERRIDE;
	VIRTUAL int PATCH = 3 OVERRIDE;

	//Module1AConfig specific stuff
	VIRTUAL int BUFFERSIZE1A = 4000;

	//Module1AAConfig specific stuff
	VIRTUAL int BUFFERSIZE2 = 3000 OVERRIDE; 
	 

	Module1AConfig()
	{ 
		 
		if (BASE BUFFERSIZE2 > BUFFERSIZE2)
		{
			BUFFERSIZE2 = BASE BUFFERSIZE2;
		}
		else
		{
			BUFFERSIZE2 = BUFFERSIZE2;
		}
	}
	  
};

