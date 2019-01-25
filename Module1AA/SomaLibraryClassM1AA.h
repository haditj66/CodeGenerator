#pragma once
#include "Configuration4.h"
#include <string>

class SomaLibraryClassM1AA
{
public:

	char bufferForBla[BUFFERSIZELOCAL];

	char buffer1[BUFFERSIZE]; 
#ifdef BUFFERSIZE_1
	char buffer2[BUFFERSIZE_1];
#endif // BUFFERSIZE1
#ifdef BUFFERSIZE_2
	char buffer3[BUFFERSIZE_2];
#endif // BUFFERSIZE1


	

#if MODE == MODE_Fast
	int Goo(int a, std::string wd);
	SomaLibraryClassM1AA(int howFast);
#endif 

#if MODE == MODE_Slow
	SomaLibraryClassM1AA();
	char* Goo(int u);
#endif  

};

