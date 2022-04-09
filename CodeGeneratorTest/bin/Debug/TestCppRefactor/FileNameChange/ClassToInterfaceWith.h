#pragma once

//hello bla
#include "SomaLibraryClassM1AA.h"
#include <vector>

class BlaBla;

class ClassToInterfaceWith
{
public:

	std::vector<SomaLibraryClassM1AA*> soms;

	ClassToInterfaceWith(); 

#if MODE == MODE_Fast
	void fooFast(std::string msg);
#endif

#if MODE == MODE_Slow
	void foo();
#endif
};

