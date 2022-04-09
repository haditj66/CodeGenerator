#include "ClassToInterfaceWith.h"
#include <iostream>

#if MODE == MODE_Fast
ClassToInterfaceWith::ClassToInterfaceWith()
{

	soms.push_back(new SomaLibraryClassM1AA(3));
	soms.push_back(new SomaLibraryClassM1AA(4)); 

	 
}

void ClassToInterfaceWith::fooFast(std::string msg)
{

	for (int i = 0; i < soms.size(); i++)
	{
		soms[i]->Goo(3,"sdg");
	}
	std::cout << msg;

} 
#endif


#if MODE == MODE_Slow 
ClassToInterfaceWith::ClassToInterfaceWith()
{

	soms.push_back(new SomaLibraryClassM1AA());
	soms.push_back(new SomaLibraryClassM1AA());
	 
}

void ClassToInterfaceWith::foo()
{

	for (int i = 0; i < soms.size(); i++)
	{
		soms[i]->Goo(3, "sdg");
	}
	std::cout << "default";

}

#endif
