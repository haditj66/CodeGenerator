#pragma once
#include "Configuration4.h"
//#include "ClassToInterfaceWith.h"

class SomeLibraryClass
{
public:
	int buffer[BUFFERSIZE];
#ifdef BUFFERSIZE_1
	int buffer_1[BUFFERSIZE_1];
#endif // BUFFERSIZE_1
#ifdef BUFFERSIZE_2
	int buffer_2[BUFFERSIZE_2];
#endif // BUFFERSIZE_1

	//ClassToInterfaceWith l;
	 
	void Foo(int a, int b);

	SomeLibraryClass();

};

