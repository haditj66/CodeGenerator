
//*************************************
//these are the defines that should be generated.
//names of defines will be
//prefixName_prefixInstance_defineName+suffixMajor_instanceOfconfig
//***********************************************************************

//for GlobalBuildConfig. ----------------------------------------------
//prefix_name:
//prefix_instance: because this is a static class, this does not get any prefix. 
//suffixMajor: (not sure about suffix)?
//instanceOfconfig: is static, cant have one
//types defined: 
#define QP 0
#define IAR 1
#define VS 2
 
#define DEBUG 0
#define RELEASE 1

//defines
#define BUILD 0
#define PLATFORM 2



//for Module1AAConfig0<modeEnum::FAST> ----------------------------------
//prefix_name: M1AA
//prefix_instance: FAST
//suffixMajor: 0
//instanceOfconfig: 0 a zero for a instance of config does not show. only 1 and up will show.
//types defined:  
#define MODE_Fast 0
#define MODE_Slow 1

//defines
//prefixName_prefixInstance_defineName_suffixMajor_instanceOfconfig
#define ISFEATURE1ENABLE true
#define BUFFERSIZE 400
#define BUFFERSIZE2 1000 
#define MODE 0 
#define MINOR 10
#define PATCH 15
#define BUFFERSIZELOCAL 5

 