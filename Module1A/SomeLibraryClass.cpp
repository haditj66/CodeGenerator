#include "SomeLibraryClass.h"
#include <iostream>


void SomeLibraryClass::Foo(int a, int b)
{
#if FEATURE == FEATURE_FEATURE2
	++a;
#endif  
#if FEATURE == FEATURE_FEATURE3
	a++;
	a++;
#endif 


	for (int i = 0; i < BUFFERSIZE; i++)
	{
		buffer[i] = i;
	}
	std::cout << MSG1;
	
}

SomeLibraryClass::SomeLibraryClass()
{
}
 