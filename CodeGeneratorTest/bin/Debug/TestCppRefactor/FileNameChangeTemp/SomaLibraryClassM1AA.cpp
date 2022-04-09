#include "SomaLibraryClassM1AA.h"
#include "ConfigurationCG.h"
 

#if MODE == MODE_Fast
int SomaLibraryClassM1AA::Goo(int a, std::string wd)
{
	a++; 
	#if PLATFORM == PLATFORM_VS
	memset(bufferForBla, 'd', BUFFERSIZELOCAL);

	a++;

	#elif PLATFORM == PLATFORM_IAR
	memset(bufferForBla, 'q', BUFFERSIZELOCAL);
	#endif
  

	return 0;
}
SomaLibraryClassM1AA::SomaLibraryClassM1AA(int howFast)
{
	howFast++;
}
#endif 


#if MODE == MODE_Slow
SomaLibraryClassM1AA::SomaLibraryClassM1AA()
{

}


char* SomaLibraryClassM1AA::Goo(int u)
{
	u++;

#if PLATFORM == PLATFORM_VS
	return "VS";  
#elif PLATFORM == PLATFORM_QP
	return "QP"; 
#else
	return "IAR";
#endif 
}
#endif 

 
