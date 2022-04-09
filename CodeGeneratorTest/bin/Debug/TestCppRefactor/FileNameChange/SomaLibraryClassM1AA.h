#pragma once 
#include <string>

class SomaLibraryClassM1AA
{
public:

	char bufferForBla[BUFFERSIZELOCAL];

	char buffer1[BUFFERSIZE]; 
#ifdef BUFFERSIZE_1
	char buffer2[BUFFERSIZE_1];
#endif  
#ifdef BUFFERSIZE_2
	char buffer3[BUFFERSIZE_2];
#endif  


	SomaLibraryClassM1AA();
	SomaLibraryClassM1AA(int howFast); 
 
	int Goo(int a, std::string wd);  

};

