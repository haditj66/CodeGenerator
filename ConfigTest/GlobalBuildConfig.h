#pragma once
#include "CGKeywordDefines.h"
#include "Config.h"

//this can be the global config that all derives from

enum PlatformEnum
{
	QP,
	IAR,
	VS
};

enum BuildEnum
{
	DEBUG,
	RELEASE,
	TESTING
};

//MAJOR version when you make incompatible API changes,
//MINOR version when you add functionality in a backwards - compatible manner, and (you added a new feature for the app)
//PATCH version when you make backwards - compatible bug fixes. (you fixed a bug)

class GlobalBuildConfig : public Config
{
public:

	GlobalBuildConfig DEPENDS_ON0()
		END

	

		PUBLIC_DEF_DECLARE(PLATFORM, PlatformEnum)
		PUBLIC_DEF_DECLARE(BUILD, BuildEnum)
		

		PUBLIC_DEF_START
		PUBLIC_DEF_CREATION1(PLATFORM, PlatformEnum, PlatformEnum::VS, true)
		PUBLIC_DEF_CREATION1(BUILD, BuildEnum, BuildEnum::DEBUG, true) 
		END


		PRIVATE_DEF_START
		
		END


		TEMPLATE_DEF_START

		END
};
