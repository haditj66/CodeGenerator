################################################ 
#this is an autogenerated file using cgen's macro2 command. Dont modify this file here unless it is in user sections 
################################################



#Cgen_Start(CGEN_PROJECT_DIRECTORY "C:/CodeGenerator/CodeGenerator/macro2Test/CGENTest2")

MATH(EXPR _arg_ONLY_CREATE_LIBRARY "${_arg_ONLY_CREATE_LIBRARY}+1")
include("C:/CodeGenerator/CodeGenerator/macro2Test/CGENTest2/AEConfigProject.cmake")
MATH(EXPR _arg_ONLY_CREATE_LIBRARY "${_arg_ONLY_CREATE_LIBRARY}-1")

#CREATE_TARGET_INTEGRATIONEXE(NAME_OF_TARGET CGENTest2
#LOCATION_OF_TARGET "C:/CodeGenerator/CodeGenerator/macro2Test/CGENTest2"
#LibrariesToLinkTo AECoreLib CGENTest_lib
#ONLY_CREATE_LIBRARY TRUE
#)  
#Cgen_End_Session()
 

CREATE_TARGET_INTEGRATIONEXE(NAME_OF_TARGET ${INTEGRATION_TARGET_NAME}
LOCATION_OF_TARGET ${INTEGRATION_TARGET_DIRECTORY}
LibrariesToLinkTo AECoreLib CGENTest2_lib
LIST_OF_TESTS defaultTest
) 