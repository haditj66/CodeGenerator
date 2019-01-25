
#pragma once

#include "Module1AConfig.h"
#include "CGKeywordDefines.h"
 


class Module1Config 
{
	VIRTUAL BuildEnum BUILD = BuildEnum::VS OVERRIDE;
	VIRTUAL int MAJOR = 2 OVERRIDE;
	VIRTUAL int MINOR = 2 OVERRIDE;
	VIRTUAL int PATCH = 3 OVERRIDE;
	 
	//Module1Config specific stuff
	VIRTUAL VARTYPE ARG1TYPE = "int";
	VIRTUAL VARNAME ARG1 = "myThingToPrint";

	//Module1AConfig specific stuff
	VIRTUAL int BUFFERSIZE1A  OVERRIDE;

	//Module1AAConfig specific stuff
	VIRTUAL int BUFFERSIZE2 = 2000 OVERRIDE;

	Module1Config()
	{
		if (BUILD == BuildEnum::VS)
		{
			BUFFERSIZE1A = 2000;
		}
		else
		{
			BUFFERSIZE1A = 1000;
		}
	}

};

